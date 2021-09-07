using Microsoft.AspNetCore.Identity;
using NoteLog.Interfaces;
using NoteLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLog.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<string> RegisterAsync(RegisterUser registerUser)
        {
            if (string.IsNullOrEmpty(registerUser.FirstName) && string.IsNullOrEmpty(registerUser.LastName) &&
                string.IsNullOrEmpty(registerUser.Password) && string.IsNullOrEmpty(registerUser.Email))
            {
                return await Task.Run(() => "Revise que todos los campos estén llenos");
            }

            var existedUser = await _userManager.FindByEmailAsync(registerUser.Email);

            if (existedUser is null)
            {
                existedUser = await _userManager.FindByNameAsync(registerUser.UserName);
            }

            if (existedUser is not null && existedUser.PasswordHash is null)
            {
                var resultPassword = await _userManager.AddPasswordAsync(existedUser, registerUser.Password);

                if (resultPassword.Succeeded)
                {
                    return await Task.Run(() => "Usuario creado con éxito");
                }
                else
                {
                    return await Task.Run(() => "Hubo algún problema");
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

                    var resultPassword = await _userManager.AddPasswordAsync(user, registerUser.Password);

                    if (resultPassword.Succeeded)
                    {
                        //return Json(new { Url = Url.Action("Login") });
                        return await Task.Run(() => "Bien");
                    }
                    else
                    {
                        return await Task.Run(() => "Hubo algún problema");
                    }

                }
                else
                {
                    return await Task.Run(() => "Hubo algún problema");
                }
            }
        }
    }
}
