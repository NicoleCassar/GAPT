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
    { // Setting up global variables for controller
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStudentRepository _studentRepository;
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IAreaRepository _areaRepository;
        private readonly IPreferenceRepository _preferenceRepository;
        private readonly IAllocationRepository _allocationRepository;
        bool ismanual; // Will be used to identify if an allocation has been done manually
        string stud; 
        string sup;
        string ar;
        string intro, introExtra, contact, submit, formB, sendoff; // Variables to be used with email templating

        public AccountsController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IStudentRepository studentRepository, ISupervisorRepository supervisorRepository, IAreaRepository areaRepository, IPreferenceRepository preferenceRepository, IAllocationRepository allocationRepository)
        { // Contructor for AccountsController, it is important to note that since templating requires complex searching, each of the database tables has been referenced
            _signinManager = signInManager;
            _userManager = userManager;
            _studentRepository = studentRepository;
            _supervisorRepository = supervisorRepository;
            _areaRepository = areaRepository;
            _preferenceRepository = preferenceRepository;
            _allocationRepository = allocationRepository;
        }

        public IActionResult Login(string returnUrl) // Loads login page
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl) // Handles processes involved with logging in users
        {
            if (!ModelState.IsValid) // If login form has not been filled in properly
                return View(loginViewModel); // Return the form and prompt appropriate validations

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName); // Find the details for the user who is logging in
            if (user != null) // If the user exists
            {
                var result = await _signinManager.PasswordSignInAsync(user, loginViewModel.Password, false, false); // Call method the sign in user using credentials
                if (result.Succeeded) // if login is successful 
                {
                        return RedirectToAction("Index", "Home"); // Redirect to home page
                }
            }

            ModelState.AddModelError("", "Email or password not correct"); // Inform user of incorrect details entered
            return View(loginViewModel);
        }

        

        [HttpPost]
        public async Task<IActionResult> Logout() // Perform logout operation to sign user out of the system
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }

        [Authorize(Roles = "Supervisor")] // Restrict Staff profile page to supervisors
        public IActionResult StaffProfile()
        {
            var model = new Supervisor_AreaViewModel(); // Making use of the Supervisor_AreaViewModel
            model.supervisor = _supervisorRepository.GetAllData(); // Getting all supervisor data
            model.area = _areaRepository.GetAllData(); // Getting all area data
            ViewBag.deleted = TempData["success"]; // Prepare notification in the case of a successfully added area
            return View(model);
        }

        public IActionResult Delete_Area(int id) // Delete an area that a supervisor wishes to remove
        {
            _areaRepository.Delete(id); // Call repository to delete area from database
            TempData["success"] = "Area has been deleted"; // Inform user of area deletion
            return RedirectToAction("StaffProfile", "Accounts"); // Refresh profile page
        }

        [Authorize(Roles = "Administrator")] // Restrict Mailer to administrators
        public IActionResult Mailer() // Used for emailing all students upon publishing allocation results
        { // Retrieve all data to be used in retrieving email details to be applied to template
            var students = _studentRepository.GetAllData();
            var supervisors = _supervisorRepository.GetAllData();
            var areas = _areaRepository.GetAllData();
            var allocations = _allocationRepository.GetAllData();
            var preferences = _preferenceRepository.GetAllData();

            GMailer.GmailUsername = "uom.secretary.ict@gmail.com"; // Set username
            GMailer.GmailPassword = "FYPMaster!"; // Set password

            GMailer mailer = new GMailer(); // Create new GMailer object

            foreach (var allocation in allocations) // For each allocation
            {
                foreach (var student in students) // Go through all students
                {
                    if (allocation.student_id == student.student_id) // Once the student who matches the allocation student_id is found
                    {
                        if (!allocation.manual) // If student has been allocated automatically
                        {
                            ismanual = allocation.manual; // Set whether or not the student was manually allocated
                            stud = student.name; // Set the student name
                            mailer.ToEmail = student.email; // Set the target address as the student's email address
                            foreach (var preference in preferences) // Go through their preferences
                            {
                                if (preference.student_id == allocation.student_id) // Once the student's preferences are found
                                {
                                    foreach (var area in areas) // Go through the list of areas for that student
                                    {
                                        if (preference.area_id == area.area_id) // When the student's assigned preference is found
                                        {
                                            foreach (var supervisor in supervisors) // Go through the list of supervisor
                                            {
                                                if (area.supervisor_id == supervisor.supervisor_id) // Once the supervisor who is in charge of the area is found
                                                {
                                                    if (allocation.supervisor_id == area.supervisor_id) // Find the supervisor who is allocated to the student
                                                    {
                                                        sup = supervisor.name + " " + supervisor.surname; // Set the supervisor full name
                                                        ar = area.title; // Set the area title
                                                    }

                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else // If student has been allocated manually
                        {
                            ismanual = allocation.manual; // Set whether or not the student was manually allocated
                            stud = student.name; // Set the student name
                            mailer.ToEmail = student.email; // Set the target address as the student's email address
                            foreach (var supervisor in supervisors) // Go through the list of supervisor
                            {
                                if (allocation.supervisor_id == supervisor.supervisor_id) // Once the allocated supervisor is found
                                {
                                        sup = supervisor.name + " " + supervisor.surname; // Set the supervisor full name 
                                }
                            }
                        }                        
                    }
                }
                if (!ismanual) // If the student has been automatically assigned, then send a set template, extracted from the faculty of ICT
                {
                    intro = "Dear " + stud + ",<br/><br/>You have been assigned <strong>" + sup + "</strong> as your principal FYP supervisor in the area of <strong>" + ar + "</strong>. You may also be assigned a co - supervisor, ";
                    introExtra = "in which case this will be communicated to you by your principal supervisor in due course.";
                    contact = "<br/></br>You are to contact your principal supervisor in order to start the preparation of the proposal for Part 1 of the FYP.This proposal is to be entered into Form B(blank form attached) ";
                    submit = "and submitted to your principal supervisor, via email.Your principal supervisor will then forward it via email to the Department of CIS Administration on cis.ict@um.edu.mt to the attention of ";
                    formB = "Lilian Ali and Shirley Borg.<br/><br/><br/>The completed Form B is to be submitted to the Department of CIS Administration by not later than noon, <strong>Friday, 12th June.</strong>";
                    sendoff = "<br/><br/><br/> Should you have any queries in this regard, please contact the Department office via email. <br/><br/><br/>Yours sincerely,<br/>FYP Bot";
                }
                else // Otherwise, inform the student that they had to be manually assigned due to a lack of availability
                {
                    intro = "Dear " + stud + ",<br/><br/>Due to a lack of availability for your submitted preferences, you have been assigned <strong>" + sup + "</strong> as your principal FYP supervisor. You may also be assigned a co - supervisor, ";
                    introExtra = "in which case this will be communicated to you by your principal supervisor in due course.";
                    contact = "<br/></br>You are to contact your principal supervisor in order to start the preparation of the proposal for Part 1 of the FYP.This proposal is to be entered into Form B(blank form attached) ";
                    submit = "and submitted to your principal supervisor, via email.Your principal supervisor will then forward it via email to the Department of CIS Administration on cis.ict@um.edu.mt to the attention of ";
                    formB = "Lilian Ali and Shirley Borg.<br/><br/><br/>The completed Form B is to be submitted to the Department of CIS Administration by not later than noon, <strong>Friday, 12th June.</strong>";
                    sendoff = "<br/><br/><br/> Should you have any queries in this regard, please contact the Department office via email. <br/><br/><br/>Yours sincerely,<br/>FYP Bot";
                }
                mailer.Subject = "FYP Area Selection"; // Set the subject for the email
                mailer.Body = intro + introExtra + contact + submit + formB + sendoff; // Copy in the template from above into the email body
                mailer.IsHtml = true; // Give the email html format
                mailer.Send(); // Send the email
            }
            TempData["Success"] = "Success! Results have been sent out to all enrolled students"; // Return a success message
            return RedirectToAction("Allocation", "Allocation"); // Redirect back to allocations page
        }
    }
}