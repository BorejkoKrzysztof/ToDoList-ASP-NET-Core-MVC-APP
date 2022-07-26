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
using ToDoListInfrastructure.Models.ViewModels.Notes;

namespace ToDoListInfrastructure.Models.Services
{
    public class NotesService : INotesService
    {
        private readonly INotesRepository notesRepository;
        private readonly IToDoEntryRepository toDoEntryRepository;
        private readonly IMapper mapper;

        public NotesService(INotesRepository notesRepository, IToDoEntryRepository toDoEntryRepository, IMapper mapper)
        {
            this.notesRepository = notesRepository ?? throw new ArgumentNullException(nameof(notesRepository), "Given repository is null.");
            this.toDoEntryRepository = toDoEntryRepository ?? throw new ArgumentNullException(nameof(toDoEntryRepository), "Given repository is null.");
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), "Given mapper is null.");
        }

        public void CreateNote(CreateNotesViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "Give ViewModel is null");
            }

            model.ToDoListId.CheckExceptions();
            model.ToDoEntryId.CheckExceptions();
            model.Note.CheckExceptions();

            var toDoEntry = this.toDoEntryRepository.ReadToDoEntry(model.ToDoEntryId);
            var newNote = new NotesTde()
            {
                ToDoEntry = toDoEntry,
                Note = model.Note
            };

            this.notesRepository.CreateNote(newNote);
        }

        public void DeleteNote(DeleteNotesViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model), "Given ViewModel is null.");
            }

            model.ToDoListId.CheckExceptions();
            model.ToDoEntryId.CheckExceptions();
            model.NoteId.CheckExceptions();

            var noteToDelete = this.notesRepository.ReadNoteById(model.NoteId);
            noteToDelete.ToDoEntry = this.toDoEntryRepository.ReadToDoEntry(model.ToDoEntryId);
            this.notesRepository.DeleteNote(noteToDelete);
        }

        public NotesCollectionViewModel GetNotesByToDoEntryId(Guid toDoEntryId, int listPage, int pageSize)
        {
            toDoEntryId.CheckExceptions();

            if (listPage < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(listPage), "Given list page is less than 1.");
            }

            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Given page size is less than 1.");
            }

            IEnumerable<NotesTde> notesCollection = this.notesRepository.GetNotesByToDoEntryId(toDoEntryId, listPage, pageSize);
            var dtoCollection = this.mapper.Map<IEnumerable<NoteDto>>(notesCollection);
            int amountOfNotes = this.notesRepository.CountNotes(toDoEntryId);

            var model = new NotesCollectionViewModel()
            {
                Notes = dtoCollection.ToList(),
                ToDoEntryId = toDoEntryId,
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = listPage,
                    ItemsPerPage = pageSize,
                    TotalItems = amountOfNotes
                }
            };

            return model;
        }
    }
}
