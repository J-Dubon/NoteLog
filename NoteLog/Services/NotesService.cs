using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NoteLog.Interfaces;
using NoteLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NoteLog.Services
{
    public class NotesService : INotesService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<NotesService> _logger;

        public NotesService(ApplicationDbContext applicationDbContext, ILogger<NotesService> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        /// <summary>
        /// Crea una nota
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        public Task<bool> SaveNoteAsync(NotesModel notes)
        {
            try
            {
                notes.CreatedDate = DateTime.Now;

                _applicationDbContext.Notes.Add(notes);
                _applicationDbContext.SaveChanges();

                return Task.Run(() => true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Actualiza una nota
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        public Task<bool> UpdateNoteAsync(NotesModel notes)
        {
            try
            {
                _applicationDbContext.Notes.Update(notes);
                _applicationDbContext.SaveChanges();

                return Task.Run(() => true);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new NotImplementedException();
            }
        }
    }
}
