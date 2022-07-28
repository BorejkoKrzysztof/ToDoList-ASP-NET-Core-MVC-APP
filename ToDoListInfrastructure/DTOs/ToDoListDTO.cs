using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfrastructure.DTOs
{
    public class ToDoListDto
    {
        // toDoList Id
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public bool Hidden { get; set; } = false;
        public List<ToDoEntryDto>? ToDoEntries { get; set; }
    }
}
