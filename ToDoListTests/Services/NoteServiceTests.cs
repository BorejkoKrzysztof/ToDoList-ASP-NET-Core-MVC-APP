using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListInfrastructure.Mappers;
using ToDoListInfrastructure.Models.Repositories;
using ToDoListInfrastructure.Models.Services;
using ToDoListInfrastructure.Models.ViewModels.Notes;

namespace ToDoListTests.Services
{
    [TestFixture]
    internal class NoteServiceTests
    {
        private Mock<INotesRepository> mockNoteRepository;
        private Mock<IToDoEntryRepository> mockToDeEntryRepository;
        private IMapper mapper;
        private NotesService noteService;
        private Guid toDoEntryId;
        private ToDoEntry toDoEntryExample;
        private NotesTde noteExample;
        private List<NotesTde> collectionOfNotes;

        [SetUp]
        public void Setup()
        {
            this.mockNoteRepository = new Mock<INotesRepository>();
            this.mockToDeEntryRepository = new Mock<IToDoEntryRepository>();
            this.mapper = AutoMapperConfig.Initialize();

            this.noteService = new NotesService(this.mockNoteRepository.Object,
                                                this.mockToDeEntryRepository.Object,
                                                this.mapper);

            this.toDoEntryId = Guid.NewGuid();

            this.toDoEntryExample = new ToDoEntry()
            {
                Id = toDoEntryId,
                Title = "ToDo Entry Title",
                Description = "ToDo Entry Description",
                DueDate = DateTime.Now.AddDays(14),
                ToDoList = new ToDoList()
            };

            this.noteExample = new NotesTde()
            {
                ToDoEntry = new ToDoEntry(),
                Note = "note"
            };

            this.collectionOfNotes = new List<NotesTde>()
            {
                new NotesTde()
                {
                    Note = "Note 1"
                },
                new NotesTde()
                {
                    Note = "Note 2"
                },
                new NotesTde()
                {
                    Note = "Note 3"
                },
                new NotesTde()
                {
                    Note = "Note 4"
                },
                new NotesTde()
                {
                    Note = "Note 5"
                },
            };
        }

        [Test]
        public void CreateNote_IncorrectData_GivenViewModelIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.noteService.CreateNote(null!));
        }


        [Test]
        public void CreateNote_IncorrectData_ToDoEntryIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var model = new CreateNotesViewModel()
            {
                ToDoEntryId = Guid.Empty,
                Note = "My new note"
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.noteService.CreateNote(model));
        }

        [Test]
        public void CreateNote_IncorrectData_NoteIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var model = new CreateNotesViewModel()
            {
                ToDoListId = Guid.NewGuid(),
                ToDoEntryId = Guid.NewGuid(),
                Note = string.Empty
            };

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.noteService.CreateNote(model));
        }

        [Test]
        public void CreateNote_CorrectData_ShouldAddNewNoteToDb()
        {
            // Arrange
            var model = new CreateNotesViewModel()
            {
                ToDoEntryId = this.toDoEntryId,
                Note = "My note",
                ToDoListId = Guid.NewGuid()
            };

            var toDoEntry = this.toDoEntryExample;

            this.mockToDeEntryRepository.Setup(x => x.ReadToDoEntry(toDoEntry.Id)).Returns(toDoEntry);

            // Act
            this.noteService.CreateNote(model);

            // Assert
            this.mockToDeEntryRepository.Verify(x => x.ReadToDoEntry(toDoEntry.Id), Times.Once());
            this.mockNoteRepository.Verify(x => x.CreateNote
                                                (It.Is<NotesTde>
                                                            (y => y.ToDoEntry.Id == toDoEntry.Id &&
                                                                    y.Note == model.Note)),
                                                                            Times.Once());
        }

        [Test]
        public void DeleteNote_IncorrectData_GivenViewModelIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.noteService.DeleteNote(null!));
        }

        [Test]
        public void DeleteNote_IncorrectData_ToDoListIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            //Arrange
            var model = new DeleteNotesViewModel()
            {
                ToDoListId = Guid.Empty,
                ToDoEntryId = Guid.NewGuid(),
                NoteId = Guid.NewGuid(),
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.noteService.DeleteNote(model));
        }

        [Test]
        public void DeleteNote_IncorrectData_ToDoEntryIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            //Arrange
            var model = new DeleteNotesViewModel()
            {
                ToDoListId = Guid.NewGuid(),
                ToDoEntryId = Guid.Empty,
                NoteId = Guid.NewGuid(),
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.noteService.DeleteNote(model));
        }

        [Test]
        public void DeleteNote_IncorrectData_NotesIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            //Arrange
            var model = new DeleteNotesViewModel()
            {
                ToDoListId = Guid.NewGuid(),
                ToDoEntryId = Guid.NewGuid(),
                NoteId = Guid.Empty,
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.noteService.DeleteNote(model));
        }

        [Test]
        public void DeleteNote_IncorrectData_GivenNoteIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var model = new DeleteNotesViewModel()
            {
                NoteId = Guid.Empty
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.noteService.DeleteNote(model));
        }

        [Test]
        public void DeleteNote_CorrectData_ShouldRunDeleteMethodFromRepositoryWithSpecificNoteIdAsArgument()
        {
            // Arrange
            var noteId = Guid.NewGuid();

            var model = new DeleteNotesViewModel()
            {
                NoteId = noteId,
                ToDoEntryId = Guid.NewGuid(),
                ToDoListId = Guid.NewGuid()
            };

            var returnedNote = this.noteExample;
            returnedNote.Id = noteId;

            this.mockNoteRepository.Setup(x => x.ReadNoteById(noteId)).Returns(returnedNote);

            // Act
            this.noteService.DeleteNote(model);

            // Assert
            this.mockNoteRepository.Verify(x => x.ReadNoteById(noteId), Times.Once());
            this.mockNoteRepository.Verify(x => x.DeleteNote
                                                (It.Is<NotesTde>(y => y.Id == noteId && y.Note == "note")),
                                                                                Times.Once());
        }

        [Test]
        public void GetNotesByToDoEntryId_IncorrectData_ToDoEntryIDIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                            this.noteService.GetNotesByToDoEntryId(Guid.Empty, 1, 4));
        }

        [Test]
        public void GetNotesByToDoEntryId_IncorrectData_ListPageIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                            this.noteService.GetNotesByToDoEntryId(Guid.NewGuid(), -1, 4));
        }

        [Test]
        public void GetNotesByToDoEntryId_IncorrectData_PageSizeIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                            this.noteService.GetNotesByToDoEntryId(Guid.NewGuid(), 1, -4));
        }

        [Test]
        public void GetNotesByToDoEntryId_CorrectData_ShouldReturnNotesCollectionViewModel()
        {
            // Arrange
            this.mockNoteRepository.Setup(x => x.GetNotesByToDoEntryId(this.toDoEntryId, 1, 4))
                                                            .Returns(this.collectionOfNotes.Take(4));

            this.mockNoteRepository.Setup(x => x.CountNotes(this.toDoEntryId))
                                                            .Returns(this.collectionOfNotes.Count);

            // Act
            var result = this.noteService.GetNotesByToDoEntryId(this.toDoEntryId, 1, 4);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotesCollectionViewModel>(result);
            Assert.That(result.Notes.Count, Is.EqualTo(4));
            Assert.That(result.PagingInfo.CurrentPage, Is.EqualTo(1));
            Assert.That(result.PagingInfo.TotalPages, Is.EqualTo(2));
        }

        [Test]
        public void GetNotesByToDoEntryId_CorrectData_NoNotesForToDoEntry_ShouldReturnEmptyNotesCollectionViewModel()
        {
            // Arrange
            this.mockNoteRepository.Setup(x => x.GetNotesByToDoEntryId(this.toDoEntryId, 1, 4))
                                                            .Returns(Enumerable.Empty<NotesTde>());

            this.mockNoteRepository.Setup(x => x.CountNotes(this.toDoEntryId))
                                                            .Returns(0);

            // Act
            var result = this.noteService.GetNotesByToDoEntryId(this.toDoEntryId, 1, 4);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotesCollectionViewModel>(result);
            Assert.That(result.Notes.Count, Is.EqualTo(0));
            Assert.That(result.PagingInfo.CurrentPage, Is.EqualTo(1));
            Assert.That(result.PagingInfo.TotalPages, Is.EqualTo(0));
        }
    }
}
