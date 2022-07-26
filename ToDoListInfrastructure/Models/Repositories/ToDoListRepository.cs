using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListInfrastructure.Database;
using ToDoListInfrastructure.Extensions;

namespace ToDoListInfrastructure.Models.Repositories
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly ToDoListAppDbContext dbContext;

        public ToDoListRepository(ToDoListAppDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Given dbContext is null.");
        }

        public IEnumerable<ToDoList> ReadAllNotHiddenToDoLists(string accountId, int listPage, int pageSize)
        {
            accountId.CheckExceptions();
            accountId.IsStringRepresentationOfGuid();

            if (listPage < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(listPage), "Given list page value is wrong.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Given page size value is wrong.");
            }

            var result = this.dbContext.ToDoLists
                                .Where(x => x.AccountId == accountId && !x.Hidden)
                                .Skip((listPage - 1) * pageSize)
                                .Take(pageSize)
                                .AsEnumerable();

            return result;
        }

        public ToDoList ReadToDoList(Guid id)
        {
            id.CheckExceptions();

            var result = this.dbContext.ToDoLists.FirstOrDefault(x => x.Id == id);

            return result!;
        }

        public void CreateToDoList(ToDoList toDoList)
        {
            toDoList.CheckExceptions();
            toDoList.Title.CheckMaxLengthExceptions(100);

            this.dbContext.ToDoLists.Add(toDoList);
            this.dbContext.SaveChanges();
        }

        public void UpdateToDoList(ToDoList toDoList)
        {
            toDoList.CheckExceptions();
            toDoList.Title.CheckMaxLengthExceptions(100);

            this.dbContext.Entry(this.dbContext.ToDoLists.First(x => x.Id == toDoList.Id))
                                                            .CurrentValues.SetValues(toDoList);
            this.dbContext.SaveChanges();
        }

        public void DeleteToDoList(ToDoList toDoList)
        {
            toDoList.CheckExceptions();

            this.dbContext.ToDoLists.Remove(toDoList);
            this.dbContext.SaveChanges();
        }

        public int CountNumberOfCopiesForToDoList(string toDoListTitle, string accountId)
        {
            toDoListTitle.CheckExceptions();
            accountId.CheckExceptions();
            accountId.IsStringRepresentationOfGuid();

            string? mainPartOfTitle = toDoListTitle.Split(" -Copy").First();

            var numberOfCopies = this.dbContext.ToDoLists
                                        .Where(x => x.Title.StartsWith(mainPartOfTitle) &&
                                                        x.AccountId == accountId)
                                        .Count();

            return numberOfCopies;
        }

        public IEnumerable<ToDoList> ReadAllHiddenToDoLists(string currentUserID, int listPage, int pageSize)
        {
            currentUserID.CheckExceptions();
            currentUserID.IsStringRepresentationOfGuid();

            if (listPage < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(listPage), "List Page value is less than one.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page Size value is less than one.");
            }

            return this.dbContext.ToDoLists.Where(x => x.AccountId == currentUserID && x.Hidden)
                                                .Skip((listPage - 1) * pageSize)
                                                .Take(pageSize)
                                                .AsEnumerable();
        }

        public int CountNotHiddenUserToDoLists(string accountId)
        {
            accountId.CheckExceptions();
            accountId.IsStringRepresentationOfGuid();

            return this.dbContext.ToDoLists.Where(x => x.AccountId == accountId && !x.Hidden).Count();
        }

        public int CountHiddenUserToDoLists(string accountId)
        {
            accountId.CheckExceptions();
            accountId.IsStringRepresentationOfGuid();

            return this.dbContext.ToDoLists.Where(x => x.AccountId == accountId && x.Hidden).Count();
        }
    }
}
