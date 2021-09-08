using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLog.Models
{
    public class ApplicationUserModel : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName {  get; set; }
    }
}
