using Microsoft.AspNetCore.Identity;
using NoteLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLog.Interfaces
{
    public interface IAuthenticationService
    {
        Task<SignInResult> LoginAsync(string userName, string password);

        Task<ResultModel> RegisterAsync(RegisterUserModel registerUser);

        Task LogoutAsync();
    }
}
