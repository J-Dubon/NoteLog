using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteLog.Interfaces;
using NoteLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLog.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;


        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Vista del Login e iniciar sesión
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Vista para registrar un usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Método para iniciar sesión
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> LoginUser(string userName, string password) {

            var loginResult = await _signInManager.PasswordSignInAsync(userName, password, false, false);

            if (loginResult.Succeeded)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }

        /// <summary>
        /// Método para registrar un usuario
        /// </summary>
        /// <param name="registerUser"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> RegisterUser(RegisterUser registerUser)
        {
            var response = await _authenticationService.RegisterAsync(registerUser);

            return Json(response);
        }

        /// <summary>
        /// Método para cerrar sesión
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
