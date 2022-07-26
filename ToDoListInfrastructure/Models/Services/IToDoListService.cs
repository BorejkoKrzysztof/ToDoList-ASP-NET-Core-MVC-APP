using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListInfrastructure.DTOs;
using ToDoListInfrastructure.Models.ViewModels.ToDoList;

namespace ToDoListInfrastructure.Models.Services
{
    public interface IToDoListService
    {
        ToDoListCollectionViewModel ReadAllLists(string accountId, int listPage, int pageSize);
        void CreateToDoList(CreateToDoListViewModel model, string accountId);
        void UpdateToDoList(UpdateToDoListViewModel model);
        void DeleteToDoList(DeleteToDoListViewModel model);
        void CopyToDoList(Guid id);
        void SwitchHideToDoList(Guid id);
        ToDoListCollectionViewModel ReadAllHiddenLists(string currentUserID, int listPage, int pageSize);
    }
}
