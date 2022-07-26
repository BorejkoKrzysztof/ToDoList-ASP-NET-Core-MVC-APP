using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfrastructure.DTOs
{
    public class ToDoEntryReminderDto
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string ToDoEntryTitle { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DateTime ToDoEntryDueDate { get; set; }
    }
}
