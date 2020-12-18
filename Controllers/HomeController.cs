using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LoginReg.Models;
using LoginReg.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace LoginReg.Controllers
{
    public class HomeController : Controller
    {

        private HomeContext _context;

        private User GetUserFromDB()
        {
            return _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
        }

        public HomeController(HomeContext context)
        {
            _context = context;
        }

        //Index.cshtml
        public IActionResult Index()
        {
            return View();
        }





        [HttpGet("Signin")]
        public IActionResult Signin()
        {
            return View();
        }


        [HttpPost("Login")]
        public IActionResult Login(LoginUser log)
        {
            if (ModelState.IsValid)
            {
                User userInDB = _context.Users.FirstOrDefault(u => u.Email == log.LoginEmail);
                if (userInDB == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    ModelState.AddModelError("LoginPassword", "Invalid Email/Password");
                    return View("Signin");
                }
                PasswordHasher<LoginUser> hash = new PasswordHasher<LoginUser>();
                var result = hash.VerifyHashedPassword(log, userInDB.Password, log.LoginPassword);

                if (result == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    ModelState.AddModelError("LoginPassword", "Invalid Email/Password");
                    return View("Signin");
                }

                HttpContext.Session.SetInt32("UserId", userInDB.UserId);
                return RedirectToAction("Dashboard");
            }
            return View("Signin");
        }




        [HttpPost("Signup")]
        public IActionResult SignUp(User reg)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Email == reg.Email))
                {
                    ModelState.AddModelError("Email", "That email is already taken.");
                    return View("Index");
                }
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                reg.Password = hasher.HashPassword(reg, reg.Password);
                _context.Users.Add(reg);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", reg.UserId);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }






        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            User userInDB = GetUserFromDB();
            if (userInDB == null)
            {
                Console.WriteLine("Got here.");
                return RedirectToAction("Logout");
            }
            return View(userInDB);
        }




        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Signin");
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
