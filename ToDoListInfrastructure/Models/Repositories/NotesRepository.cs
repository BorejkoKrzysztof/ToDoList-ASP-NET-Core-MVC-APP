using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListInfrastructure.Database;
using ToDoListInfrastructure.Extensions;

namespace ToDoListInfrastructure.Models.Repositories
{
    public class NotesRepository : INotesRepository
    {
        private readonly ToDoListAppDbContext dbContext;

        public NotesRepository(ToDoListAppDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Given dbContext is null.");
        }

        public NotesTde ReadNoteById(Guid noteId)
        {
            noteId.CheckExceptions();

            return this.dbContext.Notes_ToDoEntry.FirstOrDefault(x => x.Id == noteId)!;
        }

        public void CreateNote(NotesTde newNote)
        {
            newNote.CheckExceptions();
            newNote.ToDoEntry.CheckExceptions();
            newNote.ToDoEntry.Id.CheckExceptions();           
            newNote.Note.CheckExceptions();
            newNote.Note.CheckMaxLengthExceptions(150);

            this.dbContext.Notes_ToDoEntry.Add(newNote);
            this.dbContext.SaveChanges();
        }

        public void DeleteNote(NotesTde noteToDelete)
        {
            noteToDelete.CheckExceptions();
            noteToDelete.ToDoEntry.CheckExceptions();

            this.dbContext.Notes_ToDoEntry.Remove(noteToDelete);
            this.dbContext.SaveChanges();
        }

        public IEnumerable<NotesTde> GetNotesByToDoEntryId(Guid toDoEntryId, int listPage, int pageSize)
        {
            toDoEntryId.CheckExceptions();

            if (listPage < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(listPage), "Given list page is less than one.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Given page size is less than one.");
            }

            return this.dbContext.Notes_ToDoEntry.Where(x => x.ToDoEntry.Id == toDoEntryId)
                                                    .Skip((listPage - 1) * pageSize)
                                                    .Take(pageSize)
                                                    .AsEnumerable();
        }

        public int CountNotes(Guid toDoEntryId)
        {
            toDoEntryId.CheckExceptions();

            return this.dbContext.Notes_ToDoEntry.Where(x => x.ToDoEntry.Id == toDoEntryId).Count();
        }
    }
}
