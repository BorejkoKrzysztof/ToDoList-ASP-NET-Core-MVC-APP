using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListInfrastructure.DTOs;
using ToDoListInfrastructure.Extensions;
using ToDoListInfrastructure.Models.Repositories;
using ToDoListInfrastructure.Models.ViewModels;
using ToDoListInfrastructure.Models.ViewModels.ToDoList;

namespace ToDoListInfrastructure.Models.Services
{
    public class ToDoListService : IToDoListService
    {
        private readonly IToDoListRepository toDoListRepository;
        private readonly IToDoEntryRepository toDoEntryRepository;
        private readonly IMapper mapper;

        public ToDoListService(IToDoListRepository toDoListRepository, IToDoEntryRepository toDoEntryRepository,
            IMapper mapper)
        {
            this.toDoListRepository = toDoListRepository ?? throw new ArgumentNullException(nameof(toDoListRepository), "Given repository is null.");
            this.toDoEntryRepository = toDoEntryRepository ?? throw new ArgumentNullException(nameof(toDoEntryRepository), "Given repository is null.");
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), "Given mapper is null.");
        }

        public ToDoListCollectionViewModel ReadAllLists(string accountId, int listPage, int pageSize)
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

            var toDoListsCollection = this.toDoListRepository.ReadAllNotHiddenToDoLists(accountId, listPage, pageSize);
            int amountOfToDoList = this.toDoListRepository.CountNotHiddenUserToDoLists(accountId);

            var dtoCollection = this.mapper.Map<IEnumerable<ToDoListDto>>(toDoListsCollection);


            var toDoListViewModel = new ToDoListCollectionViewModel()
            {
                ToDoListCollection = dtoCollection,
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = listPage,
                    ItemsPerPage = pageSize,
                    TotalItems = amountOfToDoList
                }
            };

            return toDoListViewModel;
        }

        public void CreateToDoList(CreateToDoListViewModel model, string accountId)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "Given View Model is null.");
            }

            accountId.CheckExceptions();
            accountId.IsStringRepresentationOfGuid();
#pragma warning disable CS8604 // Possible null reference argument.
            model.Title.CheckExceptions();
            model.Title.CheckMaxLengthExceptions(100);
#pragma warning restore CS8604 // Possible null reference argument.

            var newToDoList = new ToDoList()
            {
                AccountId = accountId,
                Title = model.Title,
            };

            this.toDoListRepository.CreateToDoList(newToDoList);
        }

        public void UpdateToDoList(UpdateToDoListViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "Given model is null.");
            }

            model.ToDoListId.CheckExceptions();
#pragma warning disable CS8604 // Possible null reference argument.
            model.Title.CheckExceptions();
            model.Title.CheckMaxLengthExceptions(100);
#pragma warning restore CS8604 // Possible null reference argument.

            var toDoListToUpdate = this.toDoListRepository.ReadToDoList(model.ToDoListId);
            toDoListToUpdate.Title = model.Title;
            toDoListToUpdate.UpdatedAt = DateTime.Now;
            this.toDoListRepository.UpdateToDoList(toDoListToUpdate);
        }

        public void DeleteToDoList(DeleteToDoListViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "Given model is null.");
            }

            model.Id.CheckExceptions();

            var toDoListToRemove = this.toDoListRepository.ReadToDoList(model.Id);
            this.toDoListRepository.DeleteToDoList(toDoListToRemove);
        }

        public void CopyToDoList(Guid id)
        {
            id.CheckExceptions();

            var toDoListToCopy = this.toDoListRepository.ReadToDoList(id);

            if (toDoListToCopy is not null)
            {
                var amountOfCopies = this.toDoListRepository.CountNumberOfCopiesForToDoList(toDoListToCopy.Title,
                                                                                       toDoListToCopy.AccountId);

                var copiedToDoList = new ToDoList()
                {
                    AccountId = toDoListToCopy.AccountId,
                    Title = $"{toDoListToCopy.Title.Split(" -Copy").First()} -Copy {amountOfCopies}",
                    Hidden = toDoListToCopy.Hidden,
                };

                if (copiedToDoList.Title.Length > 100)
                {
                    var shortedTitle = $"{copiedToDoList.Title.Substring(0, 60)}..." +
                                                            $" -Copy {amountOfCopies}";

                    copiedToDoList.Title = shortedTitle;
                }

                this.toDoListRepository.CreateToDoList(copiedToDoList);


                var toDoEntriesToCopy = this.toDoEntryRepository.ReadAllToDoEntriesByToDoListId(id).ToList();

                var copyOfToDoEntriesCollection = new List<ToDoEntry>(toDoEntriesToCopy.Count);

                toDoEntriesToCopy.ForEach(x =>
                {
                    copyOfToDoEntriesCollection.Add(new ToDoEntry()
                    {
                        Title = x.Title,
                        Description = x.Description,
                        DueDate = x.DueDate,
                        CreationDate = x.CreationDate,
                        Progress = x.Progress,
                        ToDoList = copiedToDoList
                    });
                });

                this.toDoEntryRepository.AddRangeToDoEntries(copyOfToDoEntriesCollection);
            }
        }

        public void SwitchHideToDoList(Guid id)
        {
            id.CheckExceptions();

            var toDoListToHide = this.toDoListRepository.ReadToDoList(id);
            toDoListToHide.Hidden = !toDoListToHide.Hidden;
            toDoListToHide.UpdatedAt = DateTime.Now;
            this.toDoListRepository.UpdateToDoList(toDoListToHide);
        }

        public ToDoListCollectionViewModel ReadAllHiddenLists(string currentUserID, int listPage, int pageSize)
        {
            currentUserID.CheckExceptions();
            currentUserID.IsStringRepresentationOfGuid();

            if (listPage < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(listPage), "Given list page value is wrong.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Given page size value is wrong.");
            }

            var collection = this.toDoListRepository.ReadAllHiddenToDoLists(currentUserID, listPage, pageSize);
            int amountOfToDoList = this.toDoListRepository.CountHiddenUserToDoLists(currentUserID);

            var dtoCollection = this.mapper.Map<IEnumerable<ToDoListDto>>(collection);

            var toDoListViewModel = new ToDoListCollectionViewModel()
            {
                ToDoListCollection = dtoCollection,
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = listPage,
                    ItemsPerPage = pageSize,
                    TotalItems = amountOfToDoList
                }
            };

            return toDoListViewModel;
        }
    }
}
