using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListInfrastructure.DTOs;
using ToDoListInfrastructure.Models.ViewModels.ToDoEntry;

namespace ToDoListInfrastructure.Models.Services
{
    public interface IToDoEntryService
    {
        ToDoEntryDto CreateToDoEntry(CreateToDoEntryViewModel model);
        ToDoEntryDetailsDto ReadToDoEntry(Guid toDoEntryId);
        ToDoEntryDetailsDto EditToDoEntry(EditToDoEntryViewModel model);
        void DeleteToDoEntryAsync(DeleteToDoEntryViewModel model);
        ToDoEntryCollectionViewModel ReadToDoEntriesByToDoListId(Guid toDoListId, int listPage, int pageSize, bool hideCompleted);
        void ChangeProgressValue(ChangeProgressStatusViewModel model);
        ToDoEntryCollectionViewModel ReadTodaysItemsByUserId(string currentUserID, int listPage, int pageSize);
        ToDoEntryReminderDto GetToDoEntryForReminder(string currentUserID);
        void CompleteToDoEntry(Guid id);
    }
}
