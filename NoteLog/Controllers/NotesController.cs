using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NoteLog.Interfaces;
using NoteLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NoteLog.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly INotesService _notesService;
        private readonly ILogger<AccountController> _logger;

        public NotesController(ApplicationDbContext applicationDbContext, INotesService notesService, ILogger<AccountController> logger)
        {
            _applicationDbContext = applicationDbContext;
            _notesService = notesService;
            _logger = logger;
        }

        /// <summary>
        /// Vista general de las notas y todas sus opciones
        /// </summary>
        /// <returns></returns>
        public IActionResult Notes()
        {
            return View();
        }

        /// <summary>
        /// Vista parcial para el listado de notas
        /// </summary>
        /// <returns></returns>
        public IActionResult NotesList()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notes = _applicationDbContext.Notes.Include(x => x.User).Where(x => x.UserId == userId).OrderBy(x => x.CreatedDate)
                .Select(x => new NotesModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    CreatedDate = x.CreatedDate,
                    Subject = x.Subject
                }).ToList();

            return PartialView("_NotesList", notes);
        }

        /// <summary>
        /// Vista parcial para crear notas
        /// </summary>
        /// <returns></returns>
        public IActionResult NotesNew()
        {
            return PartialView("_NotesNew");
        }

        /// <summary>
        /// Vista parcial para visualizar / editar notas creadas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult NotesEdit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var note = _applicationDbContext.Notes.Where(x => x.UserId == userId && x.Id == id)
                .Select(x => new NotesModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Subject = x.Subject,
                    CreatedDate = x.CreatedDate,
                    Body = x.Body
                }).FirstOrDefault();

            return PartialView("_NotesEdit", note);
        }

        /// <summary>
        /// Método para crear una nota
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> SaveNote(NotesModel notes)
        {
            try
            {
                notes.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var resultSaveNote = await _notesService.SaveNoteAsync(notes);

                return Json(true);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(false);
            }
        }

        /// <summary>
        /// Método para actualizar una nota
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UpdateNote(NotesModel notes)
        {
            try
            {
                notes.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var resultUpdateNote = await _notesService.UpdateNoteAsync(notes);

                return Json(true);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return Json(false);
            }
        }
    }
}
