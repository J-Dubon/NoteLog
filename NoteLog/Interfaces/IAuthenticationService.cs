using NoteLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLog.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> RegisterAsync(RegisterUser registerUser);
    }
}
