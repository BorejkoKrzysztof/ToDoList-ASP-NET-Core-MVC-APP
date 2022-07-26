using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListInfrastructure.Models.ViewModels.ToDoEntry
{
    public class DeleteToDoEntryViewModel
    {
        public Guid ToDoEntryId { get; set; }
        public Guid ToDoListId { get; set; }
    }
}
