using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Enums;

namespace ToDoListInfrastructure.DTOs
{
    public class ToDoEntryDto
    {
        public Guid Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Title { get; set; }
        public string Description { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public DateTime DueDate { get; set; }
        public ProgressStatus Progress { get; set; }
    }
}
