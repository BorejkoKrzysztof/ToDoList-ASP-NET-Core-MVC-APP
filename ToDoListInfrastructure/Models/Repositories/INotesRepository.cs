using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;

namespace ToDoListInfrastructure.Models.Repositories
{
    public interface INotesRepository
    {
        void CreateNote(NotesTde newNote);
        NotesTde ReadNoteById(Guid noteId);
        void DeleteNote(NotesTde noteToDelete);
        IEnumerable<NotesTde> GetNotesByToDoEntryId(Guid toDoEntryId, int listPage, int pageSize);
        int CountNotes(Guid toDoEntryId);
    }
}
