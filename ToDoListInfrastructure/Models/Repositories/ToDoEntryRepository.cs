using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListInfrastructure.Database;
using ToDoListInfrastructure.DTOs;
using ToDoListInfrastructure.Extensions;

namespace ToDoListInfrastructure.Models.Repositories
{
    public class ToDoEntryRepository : IToDoEntryRepository
    {
        private readonly ToDoListAppDbContext dbContext;

        public ToDoEntryRepository(ToDoListAppDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Given dbContext is null.");
        }

        public void CreateToDoEntry(ToDoEntry toDoEntry)
        {
            if (toDoEntry is null)
            {
                throw new ArgumentNullException(nameof(toDoEntry), "Given ToDoEntry is null.");
            }

            toDoEntry.ToDoList.CheckExceptions();
            toDoEntry.Title.CheckExceptions();
            toDoEntry.Title.CheckMaxLengthExceptions(75);
            toDoEntry.Description.CheckExceptions();
            toDoEntry.Description.CheckMaxLengthExceptions(250);
            toDoEntry.Progress.ValidateProgressStatus();

            this.dbContext.Add(toDoEntry);
            this.dbContext.SaveChanges();
        }

        public ToDoEntry ReadToDoEntry(Guid toDoEntryId)
        {
            toDoEntryId.CheckExceptions();

            var result = this.dbContext.ToDoEntries
                                            .FirstOrDefault(x => x.Id == toDoEntryId);

            return result!;
        }

        public void UpdateToDoEntry(ToDoEntry toDoEntry)
        {
            toDoEntry.CheckExceptions();
            toDoEntry.Id.CheckExceptions();
            toDoEntry.Title.CheckExceptions();
            toDoEntry.Title.CheckMaxLengthExceptions(75);
            toDoEntry.Description.CheckExceptions();
            toDoEntry.Description.CheckMaxLengthExceptions(250);
            toDoEntry.DueDate.ValidateEdgeDateTime();
            toDoEntry.Progress.ValidateProgressStatus();

            this.dbContext.Entry(this.dbContext.ToDoEntries.First(x => x.Id == toDoEntry.Id))
                                                            .CurrentValues.SetValues(toDoEntry);
            this.dbContext.SaveChanges();
        }

        public void DeleteToDoEntry(ToDoEntry toDoEntryToRemove)
        {
            toDoEntryToRemove.CheckExceptions();
            toDoEntryToRemove.Id.CheckExceptions();
            toDoEntryToRemove.Title.CheckExceptions();
            toDoEntryToRemove.Title.CheckMaxLengthExceptions(75);
            toDoEntryToRemove.Description.CheckExceptions();
            toDoEntryToRemove.Description.CheckMaxLengthExceptions(250);
            toDoEntryToRemove.DueDate.ValidateEdgeDateTime();
            toDoEntryToRemove.Progress.ValidateProgressStatus();

            this.dbContext.ToDoEntries.Remove(toDoEntryToRemove);
            this.dbContext.SaveChanges();
        }
        public IEnumerable<ToDoEntry> ReadAllToDoEntriesByToDoListId(Guid id)
        {
            id.CheckExceptions();

            return this.dbContext.ToDoEntries.Where(x => x.ToDoList.Id == id).ToList();
        }

        public IEnumerable<ToDoEntry> ReadAllToDoEntriesByToDoListId(Guid toDoListId, int listPage, int pageSize)
        {
            toDoListId.CheckExceptions();

            if (listPage < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(listPage), "List Page is less than 1.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "List Page is less than 1.");
            }

            return this.dbContext.ToDoEntries.Where(x => x.ToDoList.Id == toDoListId)
                                                    .Skip((listPage - 1) * pageSize)
                                                    .Take(pageSize)
                                                    .AsEnumerable();
        }

        public IEnumerable<ToDoEntry> ReadAllDueDateToday(string currentUserID, int listPage, int pageSize)
        {
            currentUserID.CheckExceptions();
            currentUserID.IsStringRepresentationOfGuid();

            if (listPage < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(listPage), "Given list page is less than 1.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Given page size is less than 1.");
            }

            var toDoEntriesToday = this.dbContext.ToDoEntries.Where(x => 
                                                            string.Equals(x.ToDoList.AccountId, currentUserID) &&
                                                            x.DueDate.Date == DateTime.Today)
                                                                                .OrderBy(x => x.DueDate)
                                                                                .Skip((listPage - 1) * pageSize)
                                                                                .Take(pageSize)
                                                                                .AsEnumerable();

            return toDoEntriesToday;
        }

        public int CountToDoEntriesByToDoListId(Guid toDoListId)
        {
            toDoListId.CheckExceptions();

            return this.dbContext.ToDoEntries.Where(x => x.ToDoList.Id == toDoListId).Count();
        }

        public int CountTodayToDoEntriesByToDoAccountId(string currentUserID)
        {
            currentUserID.CheckExceptions();
            currentUserID.IsStringRepresentationOfGuid();

            return this.dbContext.ToDoEntries.Where(x => x.ToDoList.AccountId == currentUserID 
                                                          && x.DueDate.Date == DateTime.Today).Count();
        }

        public ToDoEntryReminderDto GetInfoForReminder(string currentUserID)
        {
            currentUserID.CheckExceptions();
            currentUserID.IsStringRepresentationOfGuid();

            var toDoEntry = this.dbContext.ToDoEntries.Where(x => x.ToDoList.AccountId == currentUserID
                                                                                && DateTime.Compare(x.DueDate, DateTime.Now) > 0 )
                                                                                .OrderBy(x => x.DueDate)
                                                                                .FirstOrDefault()!;

            if (toDoEntry is null)
            {
                return null!;
            }

            return new ToDoEntryReminderDto()
            {
                ToDoEntryDueDate = toDoEntry.DueDate,
                ToDoEntryTitle = toDoEntry.Title
            };
        }

        public void AddRangeToDoEntries(List<ToDoEntry> toDoEntriesToCopy)
        {
            if (toDoEntriesToCopy is null)
            {
                throw new ArgumentNullException(nameof(toDoEntriesToCopy), "Given collection is null");
            }

            if (toDoEntriesToCopy.Count == 0)
            {
                return;
            }

            this.dbContext.AddRange(toDoEntriesToCopy);
            this.dbContext.SaveChanges();
        }
    }
}
