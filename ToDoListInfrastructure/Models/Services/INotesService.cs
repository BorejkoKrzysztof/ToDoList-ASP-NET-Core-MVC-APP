using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListInfrastructure.Models.ViewModels.Notes;

namespace ToDoListInfrastructure.Models.Services
{
    public interface INotesService
    {
        void CreateNote(CreateNotesViewModel model);
        void DeleteNote(DeleteNotesViewModel model);
        NotesCollectionViewModel GetNotesByToDoEntryId(Guid toDoEntryId, int listPage, int pageSize);
    }
}
