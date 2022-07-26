using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListInfrastructure.DTOs;

namespace ToDoListInfrastructure.Models.ViewModels.ToDoEntry
{
    public class ToDoEntryCollectionViewModel
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public List<ToDoEntryDto> ToDoEntries { get; set; }
        public PagingInfo pagingInfo { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public bool hideCompleted { get; set; }
    }
}
