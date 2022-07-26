using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListCore.Enums;
using ToDoListInfrastructure.Database;
using ToDoListInfrastructure.Models.Repositories;

namespace ToDoListTests.Repositories
{
    [TestFixture]
    internal class NoteRepositoryTests
    {
        private readonly ToDoListAppDbContext context;
        private NotesRepository notesRepository;
        private ToDoList toDoListExample;
        private ToDoEntry toDoEntryExample;
        private NotesTde noteExample;
        private List<NotesTde> collectionOfNotes;

        public NoteRepositoryTests()
        {
            DbContextOptionsBuilder<ToDoListAppDbContext> dbOptions =
                new DbContextOptionsBuilder<ToDoListAppDbContext>()
                                                .UseInMemoryDatabase(
                                                    Guid.NewGuid().ToString());

            this.context = new ToDoListAppDbContext(dbOptions.Options);
        }

        [SetUp]
        public void Setup()
        {
            this.notesRepository = new NotesRepository(this.context);
            this.toDoListExample = new ToDoList()
            {
                AccountId = Guid.NewGuid().ToString(),
                Title = "Todo list title",
            };

            this.toDoEntryExample = new ToDoEntry()
            {
                Title = "New Title",
                Description = "New Description",
                DueDate = DateTime.Now.AddDays(5),
            };

            this.noteExample = new NotesTde()
            {
                Note = "Note",
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
        public void TestCounstructor_IncorrectParameters_DbContextIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new NotesRepository(null!));
        }

        [Test]
        public void ReadNoteById_IncorrectData_NoteIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.notesRepository.ReadNoteById(Guid.Empty));
        }

        [Test]
        public void ReadNoteById_CorrectData_ShouldReturnNoteWithSpecificId()
        {
            // Arrange
            var toDoList = this.toDoListExample;
            var toDoEntry = this.toDoEntryExample;

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoListFromDb = this.context.ToDoLists.First();
            toDoEntry.ToDoList = toDoListFromDb;

            this.context.ToDoEntries.Add(toDoEntry);
            this.context.SaveChanges();

            var toDoEntryFromDb = this.context.ToDoEntries.First();

            var note = this.noteExample;
            note.ToDoEntry = toDoEntryFromDb;

            this.context.Notes_ToDoEntry.Add(note);
            this.context.SaveChanges();

            var noteFromDatabase = this.context.Notes_ToDoEntry.First();

            // Act
            var result = notesRepository.ReadNoteById(noteFromDatabase.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<NotesTde>(result);
            Assert.That(result.Id, Is.EqualTo(noteFromDatabase.Id));
            Assert.That(result.Note, Is.EqualTo(noteFromDatabase.Note));
            Assert.That(result.ToDoEntry.Id, Is.EqualTo(noteFromDatabase.ToDoEntry.Id));
        }

        [Test]
        public void ReadNoteById_CorrectData_NoteDoesNotExist_ShouldReturnNull()
        {
            // Act
            var result = this.notesRepository.ReadNoteById(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
            Assert.IsNotInstanceOf<NotesTde>(result);
        }

        [Test]
        public void CreateNote_IncorrectData_GivenNoteIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.notesRepository.CreateNote(null!));
        }

        [Test]
        public void CreateNote_IncorrectData_ToDoEntryIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoList = this.toDoListExample;

            var toDoEntry = this.toDoEntryExample;
            toDoEntry.ToDoList = toDoList;
            toDoEntry.Id = Guid.Empty;

            var note = this.noteExample;
            note.ToDoEntry = toDoEntry;

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.notesRepository.CreateNote(note));
        }

        [Test]
        public void CreateNote_IncorrectData_NotePropertyIsNullOrStringEmpty_ShouldthrowArgumentNullException()
        {
            // Arrange
            var toDoList = this.toDoListExample;
            toDoList.Id = Guid.NewGuid();

            var toDoEntry = this.toDoEntryExample;
            toDoEntry.Id = Guid.NewGuid();
            toDoEntry.ToDoList = toDoList;

            var note = this.noteExample;
            note.ToDoEntry = toDoEntry;
            note.Note = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.notesRepository.CreateNote(note));
            note.Note = null!;
            Assert.Throws<ArgumentNullException>(() => this.notesRepository.CreateNote(note));
        }

        [Test]
        public void CreateNote_IncorrectData_NotePropertyIsTooLong_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoList = this.toDoListExample;
            toDoList.Id = Guid.NewGuid();

            var toDoEntry = this.toDoEntryExample;
            toDoEntry.Id = Guid.NewGuid();
            toDoEntry.ToDoList = toDoList;

            var note = this.noteExample;
            note.ToDoEntry = toDoEntry;
            note.Note = new string('N', 151);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.notesRepository.CreateNote(note));
        }

        [Test]
        public void CreateNote_CorrectData_ShouldCreateNewNote()
        {
            // Arrange
            var toDoList = this.toDoListExample;
            var toDoEntry = this.toDoEntryExample;

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoListFromDb = this.context.ToDoLists.First();
            toDoEntry.ToDoList = toDoListFromDb;

            this.context.ToDoEntries.Add(toDoEntry);
            this.context.SaveChanges();

            var toDoEntryFromDb = this.context.ToDoEntries.First();

            var note = this.noteExample;
            note.ToDoEntry = toDoEntryFromDb;

            // Act
            this.notesRepository.CreateNote(note);

            var noteResult = this.context.Notes_ToDoEntry.First();
            var toDoEntryResult = this.context.ToDoEntries.ToList();

            // Assert
            Assert.NotNull(noteResult);
            Assert.IsInstanceOf<NotesTde>(noteResult);
            Assert.That(noteResult.Note, Is.EqualTo(note.Note));
            Assert.That(toDoEntryResult.Count, Is.EqualTo(1));
        }

        [Test]
        public void DeleteNote_IncorrectData_NoteToDeleteIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.notesRepository.DeleteNote(null!));
        }

        [Test]
        public void DeleteNote_IncorrectData_ToDoEntryOfNoteIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ToDoEntry toDoEntry = null!;

            var note = this.noteExample;
            note.ToDoEntry = toDoEntry;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.notesRepository.DeleteNote(note));
        }

        [Test]
        public void DeleteNote_CorrectData_ShouldDeleteNoteFromDatabase()
        {
            // Arrange
            var toDoList = this.toDoListExample;
            var toDoEntry = this.toDoEntryExample;
            var note = this.noteExample;

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoListFromDb = this.context.ToDoLists.First();

            toDoEntry.ToDoList = toDoListFromDb;
            this.context.ToDoEntries.Add(toDoEntry);
            this.context.SaveChanges();

            var toDoEntryFromDb = this.context.ToDoEntries.First();
            note.ToDoEntry = toDoEntryFromDb;
            this.context.Notes_ToDoEntry.Add(note);
            this.context.SaveChanges();

            var noteFromDb = this.context.Notes_ToDoEntry.First();

            // Act
            this.notesRepository.DeleteNote(noteFromDb);
            var result = this.context.Notes_ToDoEntry.ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetNotesByToDoEntryId_IncorrectData_ToDoEntryIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                        this.notesRepository.GetNotesByToDoEntryId(Guid.Empty, 1, 4));
        }

        [Test]
        public void GetNotesByToDoEntryId_IncorrectData_listPageIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                        this.notesRepository.GetNotesByToDoEntryId(Guid.NewGuid(), -1, 4));
        }

        [Test]
        public void GetNotesByToDoEntryId_IncorrectData_PageSizeIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                        this.notesRepository.GetNotesByToDoEntryId(Guid.NewGuid(), 1, -4));
        }

        [Test]
        public void GetNotesByToDoEntryId_CorrectData_ShouldReturnCollectionOfNotes()
        {
            // Arrange
            var toDoList = this.toDoListExample;
            
            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoListFromDb = this.context.ToDoLists.First();

            var toDoEntry = this.toDoEntryExample;
            toDoEntryExample.ToDoList = toDoListFromDb;

            var notesCollection = this.collectionOfNotes;

            this.context.ToDoEntries.Add(toDoEntry);
            this.context.SaveChanges();

            var toDoEntryFromDb = this.context.ToDoEntries.First();

            notesCollection.ForEach(x => x.ToDoEntry = toDoEntryFromDb);

            this.context.Notes_ToDoEntry.AddRange(notesCollection);
            this.context.SaveChanges();

            // Act
            var result = this.notesRepository.GetNotesByToDoEntryId(toDoEntryFromDb.Id, 1, 4);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<NotesTde>>(result);
            Assert.That(result.Count(), Is.EqualTo(4));
        }

        [Test]
        public void CountNotes_IncorrectData_ToDoEntryIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.notesRepository.CountNotes(Guid.Empty));
        }

        [Test]
        public void CountNotes_CorrectData_NoNotesForToDoEntry_ShouldReturnZERO()
        {
            // Arrange
            var toDoList = this.toDoListExample;

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoListFromDb = this.context.ToDoLists.First();

            var toDoEntry = this.toDoEntryExample;
            toDoEntryExample.ToDoList = toDoListFromDb;

            this.context.ToDoEntries.Add(toDoEntry);
            this.context.SaveChanges();

            var toDoEntryFromDb = this.context.ToDoEntries.First();

            // Act
            var result = this.notesRepository.CountNotes(toDoEntryFromDb.Id);

            // Assert
            Assert.IsInstanceOf<int>(result);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CountNotes_CorrectData_ShouldReturnAmountOfNotesForToDoEntry()
        {
            // Arrange
            var toDoList = this.toDoListExample;

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoListFromDb = this.context.ToDoLists.First();

            var toDoEntry = this.toDoEntryExample;
            toDoEntryExample.ToDoList = toDoListFromDb;

            var notesCollection = this.collectionOfNotes;

            this.context.ToDoEntries.Add(toDoEntry);
            this.context.SaveChanges();

            var toDoEntryFromDb = this.context.ToDoEntries.First();

            notesCollection.ForEach(x => x.ToDoEntry = toDoEntryFromDb);

            this.context.Notes_ToDoEntry.AddRange(notesCollection);
            this.context.SaveChanges();

            // Act
            var result = this.notesRepository.CountNotes(toDoEntryFromDb.Id);

            // Assert
            Assert.IsInstanceOf<int>(result);
            Assert.That(result, Is.EqualTo(5));
        }


        [TearDown]
        public void CleanContext()
        {
            this.context.Database.EnsureDeleted();
        }
    }
}
