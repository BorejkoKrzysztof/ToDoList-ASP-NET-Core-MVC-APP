using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfrastructure.Models.ViewModels.Notes
{
    public class DeleteNotesViewModel
    {
        public Guid NoteId { get; set; }
        public Guid ToDoEntryId { get; set; }
        public Guid ToDoListId { get; set; }
    }
}
