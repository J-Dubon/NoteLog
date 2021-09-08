using Microsoft.AspNetCore.Mvc;
using NoteLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLog.Interfaces
{
    public interface INotesService
    {
        Task<bool> SaveNoteAsync(NotesModel notes);
        Task<bool> UpdateNoteAsync(NotesModel notes);
    }
}
