using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListCore.Enums;

namespace ToDoListInfrastructure.DTOs
{
    public class ToDoEntryDetailsDto
    {

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        // todoentry Id
        public Guid Id { get; set; }
        public string ToDoListTitle { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public DateTime DueDate { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ProgressStatus Progress { get; set; }
    }
}
