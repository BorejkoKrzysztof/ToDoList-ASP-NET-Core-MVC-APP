using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListInfrastructure.DTOs;

namespace ToDoListInfrastructure.Models.ViewModels.Notes
{
    public class NotesCollectionViewModel
    {
        public Guid ToDoEntryId { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public List<NoteDto> Notes { get; set; }
        public PagingInfo PagingInfo { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
