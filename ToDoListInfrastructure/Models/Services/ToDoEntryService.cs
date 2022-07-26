using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListCore.Enums;
using ToDoListInfrastructure.DTOs;
using ToDoListInfrastructure.Extensions;
using ToDoListInfrastructure.Models.Repositories;
using ToDoListInfrastructure.Models.ViewModels;
using ToDoListInfrastructure.Models.ViewModels.ToDoEntry;

namespace ToDoListInfrastructure.Models.Services
{
    public class ToDoEntryService : IToDoEntryService
    {
        private readonly IToDoEntryRepository toDoEntryRepository;
        private readonly IToDoListRepository toDoListRepository;
        private readonly IMapper mapper;

        public ToDoEntryService(IToDoEntryRepository toDoEntryRepository,
                                  IToDoListRepository toDoListRepository,
                                  IMapper mapper)
        {
            this.toDoEntryRepository = toDoEntryRepository ?? throw new ArgumentNullException(nameof(toDoEntryRepository), "Given repository is null.");
            this.toDoListRepository = toDoListRepository ?? throw new ArgumentNullException(nameof(toDoListRepository), "Given repository is null.");
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), "Given mapper is null.");
        }

        public ToDoEntryCollectionViewModel ReadToDoEntriesByToDoListId(Guid toDoListId, int listPage, int pageSize, bool hideCompleted)
        {
            toDoListId.CheckExceptions();

            if (listPage < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(listPage), "Given list Page is less than 1.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Given Page Size is less than 1.");
            }

            IEnumerable<ToDoEntry> toDoEntriesCollection = this.toDoEntryRepository.ReadAllToDoEntriesByToDoListId(toDoListId, listPage, pageSize);
            int amountOfToDoEntries = this.toDoEntryRepository.CountToDoEntriesByToDoListId(toDoListId);

            var dtoCollection = this.mapper.Map<IEnumerable<ToDoEntryDto>>(toDoEntriesCollection);

            var model = new ToDoEntryCollectionViewModel()
            {
                ToDoEntries = hideCompleted ? dtoCollection.Where(x => x.Progress != ProgressStatus.Completed).ToList() : dtoCollection.ToList(),
                pagingInfo = new PagingInfo()
                {
                    CurrentPage = listPage,
                    ItemsPerPage = pageSize,
                    TotalItems = amountOfToDoEntries
                },
                hideCompleted = hideCompleted
            };

            return model;
        }

        public ToDoEntryDto CreateToDoEntry(CreateToDoEntryViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "Given ViewModel is null.");
            }

            model.ToDoListId.CheckExceptions();
            model.Title.CheckExceptions();
            model.Description.CheckExceptions();

            var toDoListOwner = toDoListRepository.ReadToDoList(model.ToDoListId);

            var newToDoEntry = new ToDoEntry()
            {
                ToDoList = toDoListOwner,
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                CreationDate = model.CreationDate
            };

            this.toDoEntryRepository.CreateToDoEntry(newToDoEntry);

            var toDoEntryDto = this.mapper.Map<ToDoEntryDto>(newToDoEntry);

            return toDoEntryDto;
        }

        public ToDoEntryDetailsDto ReadToDoEntry(Guid toDoEntryId)
        {
            toDoEntryId.CheckExceptions();

            var toDoEntry = this.toDoEntryRepository.ReadToDoEntry(toDoEntryId);
            var toDoEntryDetailsDto = this.mapper.Map<ToDoEntryDetailsDto>(toDoEntry);

            return toDoEntryDetailsDto;
        }

        public ToDoEntryDetailsDto EditToDoEntry(EditToDoEntryViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "Given model is null.");
            }

            model.ToDoEntryId.CheckExceptions();
            model.DueDate.DateTimeValidatorLaterThanNow();

            var toDoEntryFromDb = this.toDoEntryRepository.ReadToDoEntry(model.ToDoEntryId);

            if (toDoEntryFromDb is null)
            {
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
                throw new ArgumentNullException(nameof(toDoEntryFromDb), "No ToDoEntry match.");
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            }
            
            toDoEntryFromDb.Title = model.Title ?? toDoEntryFromDb.Title;
            toDoEntryFromDb.Description = model.Description ?? toDoEntryFromDb.Description;
            toDoEntryFromDb.DueDate = DateTime.Compare(toDoEntryFromDb.DueDate, model.DueDate) != 0 ? 
                                                                                    model.DueDate
                                                                                    :
                                                                                    toDoEntryFromDb.DueDate;

            this.toDoEntryRepository.UpdateToDoEntry(toDoEntryFromDb);

            var dto = this.mapper.Map<ToDoEntryDetailsDto>(toDoEntryFromDb);

            return dto;
        }

        public void DeleteToDoEntryAsync(DeleteToDoEntryViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "Given ViewModel is null.");
            }

            model.ToDoEntryId.CheckExceptions();

            var toDoEntryToRemove = this.toDoEntryRepository.ReadToDoEntry(model.ToDoEntryId);
            toDoEntryToRemove.ToDoList = this.toDoListRepository.ReadToDoList(model.ToDoListId);
            this.toDoEntryRepository.DeleteToDoEntry(toDoEntryToRemove);
        }

        public void ChangeProgressValue(ChangeProgressStatusViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "Given ViewModel is null.");
            }

            model.ToDoEntryId.CheckExceptions();
            var progress = model.ProgressValue;
            progress.ValidateProgressStatus();

            var toDoEntry = this.toDoEntryRepository.ReadToDoEntry(model.ToDoEntryId);
            toDoEntry.Progress = progress;


            this.toDoEntryRepository.UpdateToDoEntry(toDoEntry);
        }

        public ToDoEntryCollectionViewModel ReadTodaysItemsByUserId(string currentUserID, int listPage, int pageSize)
        {
            currentUserID.CheckExceptions();
            currentUserID.IsStringRepresentationOfGuid();

            var toDoEntries = this.toDoEntryRepository.ReadAllDueDateToday(currentUserID, listPage, pageSize);
            int amountOfToDoEntries = this.toDoEntryRepository.CountTodayToDoEntriesByToDoAccountId(currentUserID);
            var dtoDueDateToday = this.mapper.Map<IEnumerable<ToDoEntryDto>>(toDoEntries);


            var model = new ToDoEntryCollectionViewModel()
            {
                ToDoEntries = dtoDueDateToday.ToList(),
                pagingInfo = new PagingInfo()
                {
                    CurrentPage = listPage,
                    ItemsPerPage = pageSize,
                    TotalItems = amountOfToDoEntries
                },

            };

            return model;
        }

        public ToDoEntryReminderDto GetToDoEntryForReminder(string currentUserID)
        {
            currentUserID.CheckExceptions();
            currentUserID.IsStringRepresentationOfGuid();

            var toDoEntryInfoForReminder = this.toDoEntryRepository.GetInfoForReminder(currentUserID);

            return toDoEntryInfoForReminder;
        }

        public void CompleteToDoEntry(Guid id)
        {
            id.CheckExceptions();

            var toDoEntryToComplete = this.toDoEntryRepository.ReadToDoEntry(id);
            toDoEntryToComplete.Progress = ProgressStatus.Completed;

            this.toDoEntryRepository.UpdateToDoEntry(toDoEntryToComplete);
        }
    }
}
