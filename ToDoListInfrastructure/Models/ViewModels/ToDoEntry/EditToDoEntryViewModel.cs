using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListInfrastructure.Utilitites;

namespace ToDoListInfrastructure.Models.ViewModels.ToDoEntry
{
    public class EditToDoEntryViewModel
    {
        public Guid ToDoEntryId { get; set; }
        public Guid ToDoListId { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string? Title { get; set; }
        public string? Description { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Required(ErrorMessage = "The DueDate Field is required.")]
        [DateTimeMustBeLaterThanNowAttribute(ErrorMessage = "DueDate must be later than now")]
        public DateTime DueDate { get; set; }

    }
}
