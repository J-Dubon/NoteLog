using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoteLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLog.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            if (!ModelState.IsValid)
            {
                return Json(false);
            }

            if (string.IsNullOrEmpty(registerUser.FirstName) && string.IsNullOrEmpty(registerUser.LastName)&& 
                string.IsNullOrEmpty(registerUser.Password) && string.IsNullOrEmpty(registerUser.Email))
            {
                return Json("Revise que todos los campos estén llenos");
            }
            
            var existedUser = await _userManager.FindByEmailAsync(registerUser.Email);

            if(existedUser is null)
            {
                existedUser = await _userManager.FindByNameAsync(registerUser.UserName);
            }

            if (existedUser is not null && existedUser.PasswordHash is null)
            {
                var resultPassword = await _userManager.AddPasswordAsync(existedUser, registerUser.Password);

                if (resultPassword.Succeeded)
                {
                    return Json("Usuario creado con éxito");
                }
                else
                {
                    return Json("Hubo algún problema");
                }
            }
            else
            {
                ApplicationUser user = new()
                {
                    FirstName = registerUser.FirstName,
                    LastName = registerUser.LastName,
                    UserName = registerUser.UserName,
                    Email = registerUser.Email
                };

                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    await _userManager.SetLockoutEnabledAsync(user, true);

                    var resulPassword = await _userManager.AddPasswordAsync(user, registerUser.Password);

                    if (resulPassword.Succeeded)
                    {
                        return Json(new { Url = Url.Action("Login") });
                    }
                    else
                    {
                        return Json("Hubo algún problema");
                    }

                }
                else
                {
                    return Json("Hubo algún problema");
                }
            }
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
