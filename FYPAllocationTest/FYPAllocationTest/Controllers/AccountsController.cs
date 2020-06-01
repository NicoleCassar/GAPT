﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IAreaRepository _areaRepository;

        public AccountsController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ISupervisorRepository supervisorRepository, IAreaRepository areaRepository)
        {
            _signinManager = signInManager;
            _userManager = userManager;
            _supervisorRepository = supervisorRepository;
            _areaRepository = areaRepository;
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
            SmtpClient smtpClient = new SmtpClient("mail.connorsf.com", 25);

            smtpClient.Credentials = new System.Net.NetworkCredential("info@connorsf.com", "Banana1234");
            // smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress("info@connorsf.com", "Mail Tester");
            mail.To.Add(new MailAddress("info@connorsf.com"));
            mail.CC.Add(new MailAddress("connorsantdls@gmail.com"));

            smtpClient.Send(mail);

            return RedirectToAction("Index", "Home");
        }
    }
}