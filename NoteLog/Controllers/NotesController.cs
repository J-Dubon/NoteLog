using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private ApplicationDbContext _applicationDbContext;

        public NotesController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
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
        public IActionResult _NotesList()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notes = _applicationDbContext.Notes.Include(x => x.User).Where(x => x.UserId == userId).OrderBy(x => x.CreatedDate)
                .Select(x => new Notes
                {
                    Id = x.Id,
                    Title = x.Title,
                    CreatedDate = x.CreatedDate,
                    Subject = x.Subject
                }).ToList();

            return PartialView(notes);
        }

        /// <summary>
        /// Vista parcial para crear notas
        /// </summary>
        /// <returns></returns>
        public IActionResult _NotesNew()
        {
            return PartialView();
        }

        /// <summary>
        /// Vista parcial para visualizar / editar notas creadas
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult _NotesEdit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var note = _applicationDbContext.Notes.Where(x => x.UserId == userId && x.Id == id)
                .Select(x => new Notes
                {
                    Id = x.Id,
                    Title = x.Title,
                    Subject = x.Subject,
                    CreatedDate = x.CreatedDate,
                    Body = x.Body
                }).FirstOrDefault();

            return PartialView(note);
        }

        /// <summary>
        /// Método para crear o actualizar una Nota
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveOrUpdateNote(Notes notes)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (notes.Id == 0)
                {
                    notes.CreatedDate = DateTime.Now;
                    notes.UserId = userId;

                    _applicationDbContext.Notes.Add(notes);
                    _applicationDbContext.SaveChanges();

                    return Json(true);
                }
                else
                {
                    notes.UserId = userId;

                    _applicationDbContext.Notes.Update(notes);
                    _applicationDbContext.SaveChanges();
                    return Json(true);
                }
            }
            catch(Exception ex)
            {
                return Json(false);
            }
        }
    }
}
