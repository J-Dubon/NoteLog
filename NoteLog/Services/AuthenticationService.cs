using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
        private readonly UserManager<ApplicationUserModel> _userManager;
        private readonly SignInManager<ApplicationUserModel> _signInManager;
        private readonly ILogger<NotesService> _logger;

        public AuthenticationService(UserManager<ApplicationUserModel> userManager, SignInManager<ApplicationUserModel> signInManager, ILogger<NotesService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Método para iniciar sesión
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<SignInResult> LoginAsync(string userName, string password)
        {
            try
            {
                return await _signInManager.PasswordSignInAsync(userName, password, false, false);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Método para cerrar sesión
        /// </summary>
        /// <returns></returns>
        public async Task LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Método para registrar un usuario
        /// </summary>
        /// <param name="registerUser"></param>
        /// <returns></returns>
        public async Task<string> RegisterAsync(RegisterUserModel registerUser)
        {
            try
            {
                if (IsRegisterUserModelValid(registerUser))
                {
                    var existingUser = await FindUser(registerUser);

                    if (existingUser is not null)
                    {
                        return await Task.Run(() => "Usuario existente");
                    }
                    else
                    {
                        return await CreateUser(registerUser);
                    }
                }
                else
                {
                    return await Task.Run(() => "Revise que todos los campos estén correctamente llenados");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Evalua las entradas del usuario
        /// </summary>
        /// <param name="registerUser"></param>
        /// <returns></returns>
        bool IsRegisterUserModelValid(RegisterUserModel registerUser)
        {
            try
            {
                if (string.IsNullOrEmpty(registerUser.FirstName) || string.IsNullOrEmpty(registerUser.LastName) ||
                string.IsNullOrEmpty(registerUser.Password) || string.IsNullOrEmpty(registerUser.Email))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Busca si existe un usuario
        /// </summary>
        /// <param name="registerUser"></param>
        /// <returns></returns>
        async Task<ApplicationUserModel> FindUser(RegisterUserModel registerUser)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(registerUser.Email);

                if (user is null)
                {
                    user = await _userManager.FindByNameAsync(registerUser.UserName);
                }

                return user;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Crea un usuario en la BD
        /// </summary>
        /// <param name="registerUser"></param>
        /// <returns></returns>
        async Task<string> CreateUser(RegisterUserModel registerUser)
        {
            try
            {
                ApplicationUserModel user = new()
                {
                    FirstName = registerUser.FirstName,
                    LastName = registerUser.LastName,
                    UserName = registerUser.UserName,
                    Email = registerUser.Email
                };

                var resultRegisterUser = await _userManager.CreateAsync(user);

                if (resultRegisterUser.Succeeded)
                {
                    await _userManager.SetLockoutEnabledAsync(user, true);
                    var resultPassword = await _userManager.AddPasswordAsync(user, registerUser.Password);

                    if (resultPassword.Succeeded)
                    {
                        return await Task.Run(() => "Usuario creado exitosamente");
                    }
                    else
                    {
                        return await Task.Run(() => "Hubo algún problema");
                    }
                }
                else
                {
                    return await Task.Run(() => "Hubo un problema al intentar crear el usuario");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
