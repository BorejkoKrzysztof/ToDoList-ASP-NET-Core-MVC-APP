﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;

namespace ToDoListInfrastructure.Models.Repositories
{
    public interface IToDoListRepository
    {
        IEnumerable<ToDoList> ReadAllNotHiddenToDoLists(string accountId, int listPage, int pageSize);
        ToDoList ReadToDoList(Guid id);
        void CreateToDoList(ToDoList toDoList);
        void UpdateToDoList(ToDoList toDoList);
        void DeleteToDoList(ToDoList toDoList);
        //ToDoList CopyToDoList(ToDoList toDoList);
        IEnumerable<ToDoList> ReadAllHiddenToDoLists(string currentUserID, int listPage, int pageSize);
        int CountNotHiddenUserToDoLists(string accountId);
        int CountHiddenUserToDoLists(string accountId);
        //List<ToDoEntry> ReadAllToDoEntriesByToDoListId(Guid id);
        //List<ToDoEntry> CopyToDoEntries(List<ToDoEntry> toDoEntriesToCopy);
        //void AddRangeToDoEntries(List<ToDoEntry> toDoEntriesToCopy);
        int CountNumberOfCopiesForToDoList(string toDoListTitle, string accountId);
    }
}
