using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Enums;

namespace ToDoListCore.Domain_Models
{
    public class ToDoEntry
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Required]
        public Guid Id { get; set; }

        [Required]
        public virtual ToDoList ToDoList { get; set; }

        [Required]
        [StringLength(75)]
        public string Title { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Required]
        public ProgressStatus Progress { get; set; } = ProgressStatus.Not_Started;

        [Required]
        public List<NotesTde> AdditionalNotes { get; set; } = new List<NotesTde>();

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
