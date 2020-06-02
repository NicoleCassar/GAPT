using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using FYPAllocationTest.Models;
using FYPAllocationTest.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FYPAllocationTest.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStudentRepository _studentRepository;
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly IAllocationRepository _allocationRepository;
        bool ismanual;
        string stud;
        string sup;
        string ar;
        string zero, one, two, three, four, five;

        public AccountsController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IStudentRepository studentRepository, ISupervisorRepository supervisorRepository, IAreaRepository areaRepository, IPreferenceRepository preferenceRepository, IAllocationRepository allocationRepository)
        {
            _signinManager = signInManager;
            _userManager = userManager;
            _studentRepository = studentRepository;
            _supervisorRepository = supervisorRepository;
            _areaRepository = areaRepository;
            _preferenceRepository = preferenceRepository;
            _allocationRepository = allocationRepository;
        }

        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName);

            if (user != null)
            {
                var result = await _signinManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                if (result.Succeeded)
                {
                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return LocalRedirect(returnUrl);
                    else
                        return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Email or password not correct");
            return View(loginViewModel);
        }

        public IActionResult Register(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = registerViewModel.UserName,
                };

                var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("Index", "Home");
                }

                //If registration errors exist, show the first error one on the list
                ModelState.AddModelError("", result.Errors.First().Description);
            }

            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }

        [Authorize(Roles = "Supervisor")]
        public IActionResult StaffProfile()
        {
            var model = new Supervisor_AreaViewModel();
            model.supervisor = _supervisorRepository.GetAllData();
            model.area = _areaRepository.GetAllData();
            ViewBag.deleted = TempData["success"];
            return View(model);
        }

        public IActionResult Delete_Area(int id)
        {
            _areaRepository.Delete(id);
            TempData["success"] = "Area has been deleted";
            return RedirectToAction("StaffProfile", "Accounts");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Mailer()
        {
            var students = _studentRepository.GetAllData();
            var supervisors = _supervisorRepository.GetAllData();
            var areas = _areaRepository.GetAllData();
            var allocations = _allocationRepository.GetAllData();
            var preferences = _preferenceRepository.GetAllData();

            GMailer.GmailUsername = "uom.secretary.ict@gmail.com"; //Set username
            GMailer.GmailPassword = "FYPMaster!"; //Set password

            GMailer mailer = new GMailer(); //create new GMailer object

            foreach (var allocation in allocations) //for each allocation
            {
                foreach (var student in students) //Go through all students
                {
                    if (allocation.student_id == student.student_id) //Once the student who matches the allocation student_id is found
                    {
                        ismanual = allocation.manual; //Set whether or not the student was manually allocated
                        stud = student.name; //Set the student name
                        mailer.ToEmail = student.email; //set the target address as the student's email address
                        foreach (var preference in preferences) //Go through their preferences
                        {
                            if (preference.student_id == allocation.student_id) //Once the student's preferences are found
                            {
                                foreach (var area in areas) //Go through the list of areas for that student
                                {
                                    if (preference.area_id == area.area_id) //When the student's assigned preference is found
                                    {
                                        foreach (var supervisor in supervisors) //Go through the list of supervisor
                                        {
                                            if (area.supervisor_id == supervisor.supervisor_id) //Once the supervisor who is in charge of the area is found
                                            {
                                                if (allocation.supervisor_id == area.supervisor_id) //Find the supervisor who is allocated to the student
                                                {
                                                    sup = supervisor.name + " " + supervisor.surname; //Set the supervisor full name
                                                    ar = area.title; //Set the area title
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                if (!ismanual) //if the student has been automatically assigned, then send a set template, extracted from the faculty of ICT
                {
                    zero = "Dear " + stud + ",<br/><br/>You have been assigned <strong>" + sup + "</strong> as your principal FYP supervisor in the area of <strong>" + ar + "</strong>. You may also be assigned a co - supervisor, ";
                    one = "in which case this will be communicated to you by your principal supervisor in due course.";
                    two = "<br/></br>You are to contact your principal supervisor in order to start the preparation of the proposal for Part 1 of the FYP.This proposal is to be entered into Form B(blank form attached) ";
                    three = "and submitted to your principal supervisor, via email.Your principal supervisor will then forward it via email to the Department of CIS Administration on cis.ict@um.edu.mt to the attention of ";
                    four = "Lilian Ali and Shirley Borg.<br/><br/><br/>The completed Form B is to be submitted to the Department of CIS Administration by not later than noon, <strong>Friday, 12th June.</strong>";
                    five = "<br/><br/><br/> Should you have any queries in this regard, please contact the Department office via email. <br/><br/><br/>Yours sincerely,<br/>FYP Bot";
                }
                else //Otherwise, inform the student that they had to be manually assigned due to a lack of availability
                {
                    zero = "Dear " + stud + ",<br/><br/>Due to a lack of availability for your submitted preferences, you have been assigned <strong>" + sup + "</strong> as your principal FYP supervisor. You may also be assigned a co - supervisor, ";
                    one = "in which case this will be communicated to you by your principal supervisor in due course.";
                    two = "<br/></br>You are to contact your principal supervisor in order to start the preparation of the proposal for Part 1 of the FYP.This proposal is to be entered into Form B(blank form attached) ";
                    three = "and submitted to your principal supervisor, via email.Your principal supervisor will then forward it via email to the Department of CIS Administration on cis.ict@um.edu.mt to the attention of ";
                    four = "Lilian Ali and Shirley Borg.<br/><br/><br/>The completed Form B is to be submitted to the Department of CIS Administration by not later than noon, <strong>Friday, 12th June.</strong>";
                    five = "<br/><br/><br/> Should you have any queries in this regard, please contact the Department office via email. <br/><br/><br/>Yours sincerely,<br/>FYP Bot";
                }
                mailer.Subject = "FYP Area Selection"; //Set the subject for the email
                mailer.Body = zero + one + two + three + four + five; //Copy in the template from above into the email body
                mailer.IsHtml = true; //Give the email html format
                mailer.Send(); //Send the email
            }
            TempData["Success"] = "Success! Results have been sent out to all enrolled students"; //Return a success message
            return RedirectToAction("Allocation", "Allocation"); //Redirect back to allocations page
        }
    }
}