using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthenticationService authenticationService, ILogger<AccountController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
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

            var loginResult = await _authenticationService.LoginAsync(userName, password);

            if (loginResult.Succeeded)
            {
                _logger.LogInformation("Inicio de sesión para usuario {@UserName}", userName);
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
        public async Task<JsonResult> RegisterUser(RegisterUserModel registerUser)
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
            await _authenticationService.LogoutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
