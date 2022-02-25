﻿using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System.Diagnostics;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly IAccountRepository _accRepo;
        private readonly ITrailRepository _trailRepo;
        public HomeController(ILogger<HomeController> logger, INationalParkRepository npRepo,
            ITrailRepository trailRepo, IAccountRepository accRepo)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
            _logger = logger;
            _accRepo = accRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            User obj = new User();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User obj)
        {
            User objUser = await _accRepo.LoginAsync(SD.AccountAPIPath + "authenticate/", obj);
            if (objUser.Token == null)
            {
                return View();
            }

            HttpContext.Session.SetString("JWToken", objUser.Token);

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            bool result = await _accRepo.RegisterAsync(SD.AccountAPIPath + "register/", obj);
            if (result == false)
            {
                return View();
            }

            return RedirectToAction("Login");

        }
        public IActionResult Logout()
        {
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Index");
        }
    }
}