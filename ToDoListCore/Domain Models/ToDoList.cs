using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListCore.Domain_Models
{
    public class ToDoList
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string AccountId { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public bool Hidden { get; set; } = false;

        public List<ToDoEntry> ToDoEntries { get; set; } = new List<ToDoEntry>();

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
