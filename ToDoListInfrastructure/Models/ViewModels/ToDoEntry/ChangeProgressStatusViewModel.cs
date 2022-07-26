using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Enums;

namespace ToDoListInfrastructure.Models.ViewModels.ToDoEntry
{
    public class ChangeProgressStatusViewModel
    {
        public Guid ToDoEntryId { get; set; }
        public Guid ToDoListId { get; set; }
        public ProgressStatus ProgressValue { get; set; }
    }
}
