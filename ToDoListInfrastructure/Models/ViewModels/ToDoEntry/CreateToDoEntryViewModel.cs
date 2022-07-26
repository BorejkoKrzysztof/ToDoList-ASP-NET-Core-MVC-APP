using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListInfrastructure.Utilitites;

namespace ToDoListInfrastructure.Models.ViewModels.ToDoEntry
{
    public class CreateToDoEntryViewModel
    {
        [Required]
        public Guid ToDoListId { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required]
        [MaxLength(75)]
        public string Title { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [Required]
        [DateTimeMustBeLaterThanNowAttribute(ErrorMessage = "DueDate must be later than now")]
        public DateTime DueDate { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }
    }
}
