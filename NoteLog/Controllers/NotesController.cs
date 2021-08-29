using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLog.Controllers
{
    public class NotesController : Controller
    {
        public IActionResult Notes()
        {
            return View();
        }
    }
}
