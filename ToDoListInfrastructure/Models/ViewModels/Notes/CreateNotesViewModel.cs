using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfrastructure.Models.ViewModels.Notes
{
    public class CreateNotesViewModel
    {
        [Required]
        public Guid ToDoEntryId { get; set; }

        public Guid ToDoListId { get; set; }

        [Required]
        [MaxLength(150)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Note { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
