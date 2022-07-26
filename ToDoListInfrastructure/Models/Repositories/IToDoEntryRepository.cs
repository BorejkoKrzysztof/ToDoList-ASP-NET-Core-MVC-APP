using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListInfrastructure.DTOs;

namespace ToDoListInfrastructure.Models.Repositories
{
    public interface IToDoEntryRepository
    {
        void CreateToDoEntry(ToDoEntry toDoEntry);
        ToDoEntry ReadToDoEntry(Guid toDoEntryId);
        void UpdateToDoEntry(ToDoEntry toDoEntry);
        void DeleteToDoEntry(ToDoEntry toDoEntryToRemove);
        IEnumerable<ToDoEntry> ReadAllToDoEntriesByToDoListId(Guid id);
        IEnumerable<ToDoEntry> ReadAllToDoEntriesByToDoListId(Guid toDoListId, int listPage, int pageSize);
        IEnumerable<ToDoEntry> ReadAllDueDateToday(string currentUserID, int listPage, int pageSize);
        int CountToDoEntriesByToDoListId(Guid toDoListId);
        int CountTodayToDoEntriesByToDoAccountId(string currentUserID);
        ToDoEntryReminderDto GetInfoForReminder(string currentUserID);
        void AddRangeToDoEntries(List<ToDoEntry> toDoEntriesToCopy);
    }
}
