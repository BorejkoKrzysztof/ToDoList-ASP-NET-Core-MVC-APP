using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListCore.Enums;
using ToDoListInfrastructure.Database;
using ToDoListInfrastructure.DTOs;
using ToDoListInfrastructure.Models.Repositories;

namespace ToDoListTests.Repositories
{
    [TestFixture]
    internal class ToDoEntryRepositoryTests
    {
        private readonly ToDoListAppDbContext context;
        private ToDoEntryRepository toDoEntryRepository;
        private List<ToDoList> collectionOfToDoListOwners;
        private List<ToDoEntry> collectionOfToDoEntry;

        public ToDoEntryRepositoryTests()
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
            this.toDoEntryRepository = new ToDoEntryRepository(this.context);
            this.collectionOfToDoListOwners = new List<ToDoList>()
            {
                new ToDoList()
                {
                    AccountId = Guid.NewGuid().ToString(),
                    Title = "My ToDoList",
                },
                new ToDoList()
                {
                    AccountId = Guid.NewGuid().ToString(),
                    Title = "My ToDoList2",
                },
            };

            this.collectionOfToDoEntry = new List<ToDoEntry>()
            {
                new ToDoEntry()
                {
                    Title = "First ToDoEntry",
                    Description = "Description of my TODO Entry",
                    DueDate = DateTime.Now.AddDays(32),
                },
                new ToDoEntry()
                {
                    Title = "Second ToDoEntry",
                    Description = "Description of my SECOND TODO Entry",
                    DueDate = DateTime.Now.AddDays(54),
                },
                new ToDoEntry()
                {
                    Title = "Third ToDoEntry",
                    Description = "Description of my THIRD TODO Entry",
                    DueDate = DateTime.Now.AddDays(32),
                },
                new ToDoEntry()
                {
                    Title = "ToDoEntry no 1",
                    Description = string.Empty,
                    DueDate = DateTime.Now.AddDays(3),
                },
                new ToDoEntry()
                {
                    Title = "ToDoEntry no 2",
                    Description = "Description of my TODO Entry no 2",
                    DueDate = DateTime.Now.AddDays(12),
                },
            };
        }

        [Test]
        public void TestConstructor_IncorrectData_DbContextIsNull_ShouldThrowArgumentNullExceptions()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ToDoEntryRepository(null!));
        }

        [Test]
        public void CreateToDoEntry_IncorrectData_ToDoEntryIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.CreateToDoEntry(null!));
        }

        [Test]
        public void CreateToDoEntry_IncorrectData_OwnerOfToDoEntryIsNull_ShouldthrowArgumentNullException()
        {
            // Arrange
            var toDoEntryToAdd = this.collectionOfToDoEntry[0];
            toDoEntryToAdd.ToDoList = null!;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.CreateToDoEntry(toDoEntryToAdd));
        }

        [Test]
        public void CreateToDoEntry_IncorrectData_ToDoEntryTitleIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var toDoEntryToAdd = this.collectionOfToDoEntry[0];
            toDoEntryToAdd.Title = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.CreateToDoEntry(toDoEntryToAdd));
            toDoEntryToAdd.Title = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.CreateToDoEntry(toDoEntryToAdd));
        }

        [Test]
        public void CreateToDoEntry_IncorrectData_ToDoEntryDescriptionIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var toDoEntryToAdd = this.collectionOfToDoEntry[0];
            toDoEntryToAdd.ToDoList = new ToDoList()
            {
                AccountId = Guid.NewGuid().ToString(),
                Title = "TDL"
            };
            toDoEntryToAdd.Description = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.CreateToDoEntry(toDoEntryToAdd));
            toDoEntryToAdd.Description = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.CreateToDoEntry(toDoEntryToAdd));
        }

        [Test]
        public void CreateToDoEntry_IncorrectData_ToDoListOwnerTitleIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var toDoEntryToAdd = this.collectionOfToDoEntry[0];
            toDoEntryToAdd.ToDoList = new ToDoList()
            {
                AccountId = Guid.NewGuid().ToString(),
                Title = string.Empty
            };

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.CreateToDoEntry(toDoEntryToAdd));
            toDoEntryToAdd.ToDoList.Title = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.CreateToDoEntry(toDoEntryToAdd));
        }

        [Test]
        public void CreateToDoEntry_IncorrectData_ToDoListOwnerAccountIdIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var toDoEntryToAdd = this.collectionOfToDoEntry[0];
            toDoEntryToAdd.ToDoList = new ToDoList()
            {
                AccountId = string.Empty,
                Title = "TDL"
            };

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.CreateToDoEntry(toDoEntryToAdd));
            toDoEntryToAdd.ToDoList.AccountId = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.CreateToDoEntry(toDoEntryToAdd));
        }

        [Test]
        public void CreateToDoEntry_IncorrectData_ToDoEntryProgressValueIsWrong_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoEntryToAdd = this.collectionOfToDoEntry[0];
            toDoEntryToAdd.ToDoList = new ToDoList()
            {
                AccountId = Guid.NewGuid().ToString(),
                Title = "TDL"
            };

            toDoEntryToAdd.Progress = (ProgressStatus)(-1);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryRepository.CreateToDoEntry(toDoEntryToAdd));
            toDoEntryToAdd.Progress = toDoEntryToAdd.Progress = (ProgressStatus)4;
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryRepository.CreateToDoEntry(toDoEntryToAdd));
        }

        [Test]
        public void CreateToDoEntry_IncorrectData_TitleIsTooLong_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoEntryToAdd = this.collectionOfToDoEntry[0];
            toDoEntryToAdd.ToDoList = new ToDoList()
            {
                AccountId = Guid.NewGuid().ToString(),
                Title = "TDL"
            };

            toDoEntryToAdd.Title = new string('T', 76); // one character too much.

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryRepository.CreateToDoEntry(toDoEntryToAdd));
        }

        [Test]
        public void CreateToDoEntry_IncorrectData_DescriptionIsTooLong_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoEntryToAdd = this.collectionOfToDoEntry[0];
            toDoEntryToAdd.ToDoList = new ToDoList()
            {
                AccountId = Guid.NewGuid().ToString(),
                Title = "TDL"
            };

            toDoEntryToAdd.Description = new string('T', 251); // one character too much.

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryRepository.CreateToDoEntry(toDoEntryToAdd));
        }

        [Test]
        public void CreateToDoEntry_CorrectData_ShouldAddNewToDoEntryToDatabase()
        {
            // Arrange
            var toDoListToAdd = this.collectionOfToDoListOwners[0];
            var toDoEntryToAdd = this.collectionOfToDoEntry[0];

            this.context.ToDoLists.Add(toDoListToAdd);
            this.context.SaveChanges();

            var addedToDoListFromDb = this.context.ToDoLists.First();

            toDoEntryToAdd.ToDoList = addedToDoListFromDb;

            // Act
            this.toDoEntryRepository.CreateToDoEntry(toDoEntryToAdd);
            var result = this.context.ToDoEntries.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(toDoEntryToAdd.Title, Is.EqualTo(result.First().Title));
            Assert.That(result.First().ToDoList.Id, Is.EqualTo(addedToDoListFromDb.Id));
        }

        [Test]
        public void ReadToDoEntry_IncorrectData_GivenToDoEntryIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryRepository.ReadToDoEntry(Guid.Empty));
        }

        [Test]
        public void ReadToDoEntry_CorrectData_ShouldReturnToDoEntryFromDatabase()
        {
            // Arrange
            var toDoListToAdd = this.collectionOfToDoListOwners[0];
            var toDoEntryToAdd = this.collectionOfToDoEntry[0];

            this.context.ToDoLists.Add(toDoListToAdd);
            this.context.SaveChanges();

            var addedToDoListFromDb = this.context.ToDoLists.First();

            toDoEntryToAdd.ToDoList = addedToDoListFromDb;

            this.context.ToDoEntries.Add(toDoEntryToAdd);
            this.context.SaveChanges();

            var expectedToDoEntry = this.context.ToDoEntries.First();

            // Act
            var result = this.toDoEntryRepository.ReadToDoEntry(expectedToDoEntry.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ToDoEntry>(result);
            Assert.That(result.Id, Is.EqualTo(expectedToDoEntry.Id));
            Assert.That(result.Title, Is.EqualTo(expectedToDoEntry.Title));
        }

        [Test]
        public void ReadtoDoEntry_CorrectData_ToDoEntryWithThatIdDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var toDoEntryId = Guid.NewGuid();

            // Act
            var result = this.toDoEntryRepository.ReadToDoEntry(toDoEntryId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void UpdateToDoEntry_IncorrectData_ToDoEntryIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.UpdateToDoEntry(null!));
        }

        [Test]
        public void UpdateToDoEntry_IncorrectData_ToDoEntryIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoEntry = this.collectionOfToDoEntry[1];
            toDoEntry.Id = Guid.Empty;

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryRepository.UpdateToDoEntry(toDoEntry));
        }

        [Test]
        public void UpdateToDoEntry_IncorrectData_TitleOfToDoEntryIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var toDoEntry = this.collectionOfToDoEntry[1];
            toDoEntry.Id = Guid.NewGuid();
            toDoEntry.Title = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.UpdateToDoEntry(toDoEntry));
            toDoEntry.Title = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.UpdateToDoEntry(toDoEntry));
        }

        [Test]
        public void UpdateToDoEntry_IncorrectData_DescriptionOfToDoEntryIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var toDoEntry = this.collectionOfToDoEntry[1];
            toDoEntry.Id = Guid.NewGuid();
            toDoEntry.Description = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.UpdateToDoEntry(toDoEntry));
            toDoEntry.Description = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.UpdateToDoEntry(toDoEntry));
        }

        [Test]
        public void UpdateToDoEntry_IncorrectData_ProgressStatusIsOutOfRange_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoEntry = this.collectionOfToDoEntry[1];
            toDoEntry.Id = Guid.NewGuid();
            toDoEntry.Progress = (ProgressStatus)(-1);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository.UpdateToDoEntry(toDoEntry));
            toDoEntry.Progress = (ProgressStatus)int.MaxValue;
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository.UpdateToDoEntry(toDoEntry));
        }

        [Test]
        public void UpdateToDoEntry_InCorrectData_ToDoEntryTitleIsTooLong_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoEntry = this.collectionOfToDoEntry[1];
            toDoEntry.Id = Guid.NewGuid();
            toDoEntry.Title = new string('T', 76);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository.UpdateToDoEntry(toDoEntry));
        }

        [Test]
        public void UpdateToDoEntry_InCorrectData_ToDoEntryDescriptionIsTooLong_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoEntry = this.collectionOfToDoEntry[1];
            toDoEntry.Id = Guid.NewGuid();
            toDoEntry.Description = new string('T', 251);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository.UpdateToDoEntry(toDoEntry));
        }

        [Test]
        public void UpdateToDoEntry_InCorrectData_ToDoEntryDueDateIsDateTimeMinOrMaxValue_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoEntry = this.collectionOfToDoEntry[1];
            toDoEntry.Id = Guid.NewGuid();
            toDoEntry.DueDate = DateTime.MinValue;

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository.UpdateToDoEntry(toDoEntry));
            toDoEntry.DueDate = DateTime.MaxValue;
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository.UpdateToDoEntry(toDoEntry));
        }

        [Test]
        public void UpdateToDoEntry_CorrectData_ShouldUpdateToDoEntry_UpdateSinglePropertyAtOneTime()
        {
            // Arrange
            var newDueDate = DateTime.Now.AddMonths(2);
            var newTitle = "New Title Updated";
            var newDescription = "New Description Updated";

            var toDoList = this.collectionOfToDoListOwners[0];
            var toDoEntry = this.collectionOfToDoEntry[1];

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoListFromDatabase = this.context.ToDoLists.First();

            toDoEntry.ToDoList = toDoListFromDatabase;

            this.context.ToDoEntries.Add(toDoEntry);
            this.context.SaveChanges();


            // Act
            toDoEntry.DueDate = newDueDate;
            this.toDoEntryRepository.UpdateToDoEntry(toDoEntry);

            var result = this.context.ToDoEntries.First();

            // Assert
            Assert.That(DateTime.Compare(newDueDate, result.DueDate), Is.EqualTo(0));

            // Act 2
            result.Title = newTitle;
            this.toDoEntryRepository.UpdateToDoEntry(result);

            result = this.context.ToDoEntries.First();

            // Assert 2
            Assert.That(result.Title, Is.EqualTo(newTitle));

            // Act 3
            result.Description = newDescription;
            this.toDoEntryRepository.UpdateToDoEntry(result);

            result = this.context.ToDoEntries.First();

            // Assert 3
            Assert.That(result.Description, Is.EqualTo(newDescription));
        }

        [Test]
        public void UpdateToDoEntry_CorrectData_ShouldUpdateToDoEntry_UpdateAllPossiblePropertiesAtOneTime()
        {
            // Arrange
            var newDueDate = DateTime.Now.AddMonths(2);
            var newTitle = "New Title Updated";
            var newDescription = "New Description Updated";

            var toDoList = this.collectionOfToDoListOwners[0];
            var toDoEntry = this.collectionOfToDoEntry[1];

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoListFromDatabase = this.context.ToDoLists.First();

            toDoEntry.ToDoList = toDoListFromDatabase;

            this.context.ToDoEntries.Add(toDoEntry);
            this.context.SaveChanges();

            var toDoEntryFromDatabase = this.context.ToDoEntries.First();
            toDoEntryFromDatabase.Title = newTitle;
            toDoEntryFromDatabase.Description = newDescription;
            toDoEntryFromDatabase.DueDate = newDueDate;


            // Act
            this.toDoEntryRepository.UpdateToDoEntry(toDoEntryFromDatabase);
            var result = this.context.ToDoEntries.First();

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ToDoEntry>(result);
            Assert.That(newTitle, Is.EqualTo(result.Title));
            Assert.That(newDescription, Is.EqualTo(result.Description));
            Assert.That(newDueDate, Is.EqualTo(result.DueDate));
        }

        [Test]
        public void DeleteToDoEntry_IncorrectData_ToDoEntryIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.DeleteToDoEntry(null!));
        }
        

        [Test]
        public void DeleteToDoEntry_IncorrectData_ToDoEntryIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoEntry = this.collectionOfToDoEntry[1];
            toDoEntry.Id = Guid.Empty;

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryRepository.DeleteToDoEntry(toDoEntry));
        }

        [Test]
        public void DeleteToDoEntry_IncorrectData_TitleOfToDoEntryIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var toDoEntry = this.collectionOfToDoEntry[1];
            toDoEntry.Id = Guid.NewGuid();
            toDoEntry.Title = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.DeleteToDoEntry(toDoEntry));
            toDoEntry.Title = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.DeleteToDoEntry(toDoEntry));
        }

        [Test]
        public void DeleteToDoEntry_IncorrectData_DescriptionOfToDoEntryIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var toDoEntry = this.collectionOfToDoEntry[1];
            toDoEntry.Id = Guid.NewGuid();
            toDoEntry.Description = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.DeleteToDoEntry(toDoEntry));
            toDoEntry.Description = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.DeleteToDoEntry(toDoEntry));
        }

        [Test]
        public void DeleteToDoEntry_IncorrectData_ProgressStatusIsOutOfRange_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoEntry = this.collectionOfToDoEntry[1];
            toDoEntry.Id = Guid.NewGuid();
            toDoEntry.Progress = (ProgressStatus)(-1);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository.DeleteToDoEntry(toDoEntry));
            toDoEntry.Progress = (ProgressStatus)int.MaxValue;
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository.DeleteToDoEntry(toDoEntry));
        }

        [Test]
        public void DeleteToDoEntry_InCorrectData_ToDoEntryTitleIsTooLong_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoEntry = this.collectionOfToDoEntry[1];
            toDoEntry.Id = Guid.NewGuid();
            toDoEntry.Title = new string('T', 76);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository.DeleteToDoEntry(toDoEntry));
        }

        [Test]
        public void DeleteToDoEntry_InCorrectData_ToDoEntryDescriptionIsTooLong_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoEntry = this.collectionOfToDoEntry[1];
            toDoEntry.Id = Guid.NewGuid();
            toDoEntry.Description = new string('T', 251);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository.DeleteToDoEntry(toDoEntry));
        }

        [Test]
        public void DeleteToDoEntry_InCorrectData_ToDoEntryDueDateIsDateTimeMinOrMaxValue_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var toDoEntry = this.collectionOfToDoEntry[1];
            toDoEntry.Id = Guid.NewGuid();
            toDoEntry.DueDate = DateTime.MinValue;

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository.DeleteToDoEntry(toDoEntry));
            toDoEntry.DueDate = DateTime.MaxValue;
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository.DeleteToDoEntry(toDoEntry));
        }

        [Test]
        public void DeleteToDoEntry_CorrectData_ShouldRemoveToDoEntryFromDatabase()
        {
            // Arrange
            var toDoEntryToDelete = this.collectionOfToDoEntry[0];
            toDoEntryToDelete.ToDoList = this.collectionOfToDoListOwners[1];
            toDoEntryToDelete.ToDoList.Id = Guid.NewGuid();

            this.context.ToDoLists.Add(toDoEntryToDelete.ToDoList);
            this.context.SaveChanges();

            this.context.ToDoEntries.Add(toDoEntryToDelete);
            this.context.SaveChanges();

            // Act
            this.toDoEntryRepository.DeleteToDoEntry(toDoEntryToDelete);
            var result = this.context.ToDoEntries.ToList();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void ReadAllToDoEntriesById_IncorrectData_ToDoListIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => 
                                            this.toDoEntryRepository.ReadAllToDoEntriesByToDoListId(Guid.Empty));
        }

        [Test]
        public void ReadAllToDoEntriesByToDoListId_CorrectData_ShouldReturnCollectionOfToDoEntry()
        {
            // Arrange
            var newToDoList = this.collectionOfToDoListOwners[0];

            var collectionOfToDoEntries = this.collectionOfToDoEntry;

            this.context.ToDoLists.Add(newToDoList);
            this.context.SaveChanges();

            var toDoListFromDatabase = this.context.ToDoLists.First();
            collectionOfToDoEntries.ForEach(x => x.ToDoList = toDoListFromDatabase);

            this.context.ToDoEntries.AddRange(collectionOfToDoEntries);
            this.context.SaveChanges();

            // Act
            var results = this.toDoEntryRepository.ReadAllToDoEntriesByToDoListId(toDoListFromDatabase.Id);

            // Assert
            Assert.NotNull(results);
            Assert.IsInstanceOf<IEnumerable<ToDoEntry>>(results);
            Assert.That(results.Count(), Is.EqualTo(collectionOfToDoEntries.Count));
        }

        [Test]
        public void ReadAllToDoEntriesByToDoListId_CorrectData_ToDoEntriesDontExist_ShouldReturnEmptyCollection()
        {
            // Act
            var result = this.toDoEntryRepository.ReadAllToDoEntriesByToDoListId(Guid.NewGuid());

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<ToDoEntry>>(result);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ReadAllToDoEntriesByToDoListId_IncorrectData_ToDoListIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                      this.toDoEntryRepository.ReadAllToDoEntriesByToDoListId(Guid.Empty, 1, 4));
        }

        [Test]
        public void ReadAllToDoEntriesByToDoListId_IncorrectData_ListPageIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                      this.toDoEntryRepository.ReadAllToDoEntriesByToDoListId(Guid.NewGuid(), 0, 4));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                      this.toDoEntryRepository.ReadAllToDoEntriesByToDoListId(Guid.NewGuid(), -1, 4));
        }

        [Test]
        public void ReadAllToDoEntriesByToDoListId_IncorrectData_PageSizeIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                      this.toDoEntryRepository.ReadAllToDoEntriesByToDoListId(Guid.NewGuid(), 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                      this.toDoEntryRepository.ReadAllToDoEntriesByToDoListId(Guid.NewGuid(), 2, -4));
        }

        [Test]
        public void ReadAllToDoEntriesByToDoListId_IncorrectData_AllParametersAreWrong_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                      this.toDoEntryRepository.ReadAllToDoEntriesByToDoListId(Guid.Empty, -1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                      this.toDoEntryRepository.ReadAllToDoEntriesByToDoListId(Guid.Empty, 0, -4));
        }

        [Test]
        public void ReadAllToDoEntriesByToDoListId_CorrectData_ShouldReturnCollectionOfToDoEntries()
        {
            // Arrange
            var newToDoList = this.collectionOfToDoListOwners[0];

            var collectionOfToDoEntries = this.collectionOfToDoEntry;

            this.context.ToDoLists.Add(newToDoList);
            this.context.SaveChanges();

            var toDoListFromDatabase = this.context.ToDoLists.First();
            collectionOfToDoEntries.ForEach(x => x.ToDoList = toDoListFromDatabase);

            this.context.ToDoEntries.AddRange(collectionOfToDoEntries);
            this.context.SaveChanges();

            // Act
            var result = this.toDoEntryRepository.ReadAllToDoEntriesByToDoListId(toDoListFromDatabase.Id, 1, 4);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<ToDoEntry>>(result);
            Assert.That(result.Count(), Is.EqualTo(4));
        }

        [Test]
        public void ReadAllToDoEntriesByToDoListId_CorrectData_ToDoEntriesForThatToDoListDontExist_ShouldReturnEmptyCollection()
        {
            // Arrange
            var newToDoList = this.collectionOfToDoListOwners[0];

            this.context.ToDoLists.Add(newToDoList);
            this.context.SaveChanges();

            var toDoListFromDatabase = this.context.ToDoLists.First();

            // Act
            var result = this.toDoEntryRepository.ReadAllToDoEntriesByToDoListId(toDoListFromDatabase.Id, 1, 4);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<ToDoEntry>>(result);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ReadAllDueDateToday_IncorrectData_GivenAccountIdIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => toDoEntryRepository
                                                            .ReadAllDueDateToday(string.Empty!, 1, 4));
            Assert.Throws<ArgumentNullException>(() => toDoEntryRepository
                                                            .ReadAllDueDateToday(null!, 1, 4));
        }

        [Test]
        public void ReadAllDueDateToday_IncorrectData_GivenAccountIdIsNotRepresentationOfGuid_ShouldThrowArgumentException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => toDoEntryRepository
                                                            .ReadAllDueDateToday("normal string", 1, 4));
        }

        [Test]
        public void ReadAllDueDateToday_IncorrectData_ListPageIsLessThanOne_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository
                                                            .ReadAllDueDateToday(Guid.NewGuid().ToString(), 0, 4));
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository
                                                            .ReadAllDueDateToday(Guid.NewGuid().ToString(), -1, 4));
        }

        [Test]
        public void ReadAllDueDateToday_IncorrectData_PageSizeIsLessThanOne_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository
                                                            .ReadAllDueDateToday(Guid.NewGuid().ToString(), 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoEntryRepository
                                                            .ReadAllDueDateToday(Guid.NewGuid().ToString(), 1, -4));
        }

        [Test]
        public void ReadAllDueDateToday_CorrectData_ShouldReturnCollectionOfToDoEntriesForSpecificUserWithDueToday()
        {
            // Arrange
            var AccountIdOne = Guid.NewGuid().ToString();
            var AccountIdTwo = Guid.NewGuid().ToString();

            var toDoList1 = this.collectionOfToDoListOwners[0];
            toDoList1.AccountId = AccountIdOne;

            var toDoList2 = this.collectionOfToDoListOwners[1];
            toDoList2.AccountId = AccountIdTwo;

            var toDoEntries1 = this.collectionOfToDoEntry;

            toDoEntries1[0].DueDate = DateTime.Today;

            var ToDoEntries2 = new List<ToDoEntry>()
            {
                new ToDoEntry()
                {
                    Title = "First for second account",
                    Description = "Description 1",
                    DueDate = DateTime.Today.AddHours(14)
                },
                new ToDoEntry()
                {
                    Title = "Second ToDoEntry account 2",
                    Description = "Description of my SECOND TODO Entry for second account",
                    DueDate = DateTime.Now.AddDays(54),
                },
                new ToDoEntry()
                {
                    Title = "Just ToDoEntry",
                    Description = "Description 3 account 2",
                    DueDate = DateTime.Now.AddDays(32),
                },
            };

            this.context.ToDoLists.AddRange(toDoList1, toDoList2);
            this.context.SaveChanges();

            var toDoListsCollectionFromDb = this.context.ToDoLists.ToList();

            toDoEntries1.ForEach(x => x.ToDoList = toDoListsCollectionFromDb[0]);
            ToDoEntries2.ForEach(x => x.ToDoList = toDoListsCollectionFromDb[1]);

            this.context.ToDoEntries.AddRange(toDoEntries1);
            this.context.ToDoEntries.AddRange(ToDoEntries2);
            this.context.SaveChanges();

            // Act
            var results = this.toDoEntryRepository.ReadAllDueDateToday(AccountIdOne, 1, 4);

            // Assert
            Assert.NotNull(results);
            Assert.IsInstanceOf<IEnumerable<ToDoEntry>>(results);
            Assert.That(() => results.Any());
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results, Is.Ordered.By("DueDate"));
        }

        [Test]
        public void ReadAllDueDateToday_CorrectData_NoToDoEntiresShouldReturnEmptyCollection()
        {
            // Arrange
            var accountId = Guid.NewGuid().ToString();

            var toDoList = this.collectionOfToDoListOwners[0];
            toDoList.AccountId = accountId;


            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoListFromDb = this.context.ToDoLists.First();

            // Act
            var results = toDoEntryRepository.ReadAllDueDateToday(toDoListFromDb.AccountId, 1, 4);

            // Assert
            Assert.NotNull(results);
            Assert.IsInstanceOf<IEnumerable<ToDoEntry>>(results);
            Assert.That(results.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CountToDoEntriesByToDoListId_IncorrecData_ToDoListIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                            this.toDoEntryRepository.CountToDoEntriesByToDoListId(Guid.Empty));
        }

        [Test]
        public void CountToDoEntriesByToDoListId_CorrectData_ShouldReturnAmountOfToDoEntriesForSpecificToDoList()
        {
            // Arrange
            var toDoList = this.collectionOfToDoListOwners[0];

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var collectionOfToDoEntries = this.collectionOfToDoEntry;

            var toDoListFromDatabase = this.context.ToDoLists.First();
            collectionOfToDoEntries.ForEach(x => x.ToDoList = toDoListFromDatabase);

            this.context.ToDoEntries.AddRange(collectionOfToDoEntries);
            this.context.SaveChanges();

            // Act
            var result = this.toDoEntryRepository.CountToDoEntriesByToDoListId(toDoListFromDatabase.Id);

            // Assert
            Assert.That(result, Is.EqualTo(collectionOfToDoEntries.Count));
            Assert.IsInstanceOf<int>(result);
        }

        [Test]
        public void CountToDoEntriesByToDoListId_CorrectData_ToDoListHasNoToDoEntries_ShouldReturnZERO()
        {
            // Arrange
            var toDoList = this.collectionOfToDoListOwners[0];

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoListFromDatabase = this.context.ToDoLists.First();

            // Act
            var result = this.toDoEntryRepository.CountToDoEntriesByToDoListId(toDoListFromDatabase.Id);

            // Assert
            Assert.That(result, Is.EqualTo(0));
            Assert.IsInstanceOf<int>(result);
        }

        [Test]
        public void CountTodayToDoEntriesByToDoAccountId_IncorrectData_AccountIdIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                                        this.toDoEntryRepository.CountTodayToDoEntriesByToDoAccountId(string.Empty));
            Assert.Throws<ArgumentNullException>(() =>
                                        this.toDoEntryRepository.CountTodayToDoEntriesByToDoAccountId(null!));
        }

        [Test]
        public void CountTodayToDoEntriesByToDoAccountId_IncorrectData_AccountIdIsNotRepresentationOfGuid_ShouldThrowArgumentException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() =>
                                 this.toDoEntryRepository.CountTodayToDoEntriesByToDoAccountId("text"));
        }

        [Test]
        public void CountTodayToDoEntriesByToDoAccountId_CorrectData_ShouldReturnAmountOfToDoEntriesForToday()
        {
            // Arrange
            var toDoList = this.collectionOfToDoListOwners[0];

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var collectionOfToDoEntries = this.collectionOfToDoEntry;

            var toDoListFromDatabase = this.context.ToDoLists.First();
            
            collectionOfToDoEntries.ForEach(x => x.ToDoList = toDoListFromDatabase);
            collectionOfToDoEntries[0].DueDate = DateTime.Today.AddHours(10);
            collectionOfToDoEntries[1].DueDate = DateTime.Today.AddHours(7);
            collectionOfToDoEntries[2].DueDate = DateTime.Today.AddHours(5);

            this.context.ToDoEntries.AddRange(collectionOfToDoEntries);
            this.context.SaveChanges();

            // Act
            var result = this.toDoEntryRepository.CountTodayToDoEntriesByToDoAccountId(toDoListFromDatabase.AccountId);

            // Assert
            Assert.That(result, Is.EqualTo(3));
            Assert.IsInstanceOf<int>(result);
        }

        [Test]
        public void CountTodayToDoEntriesByToDoAccountId_CorrectData_NoToDoEntriesToday_ShouldReturnZERO()
        {
            // Arrange
            var toDoList = this.collectionOfToDoListOwners[0];

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoListFromDatabase = this.context.ToDoLists.First();

            // Act
            var result = this.toDoEntryRepository.CountTodayToDoEntriesByToDoAccountId(toDoListFromDatabase.AccountId);

            // Assert
            Assert.That(result, Is.EqualTo(0));
            Assert.IsInstanceOf<int>(result);
        }

        [Test]
        public void GetInfoForReminder_IncorrectData_AccountIdIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.GetInfoForReminder(string.Empty));
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.GetInfoForReminder(null!));
        }

        [Test]
        public void GetInfoForReminder_IncorrectData_AccountIdIsNotRepresentationOfGuid_ShouldThrowArgumentException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => this.toDoEntryRepository.GetInfoForReminder("text"));
        }

        [Test]
        public void GetInfoForReminder_OneToDoEntry_ShouldReturnInfoOfToDoEntry()
        {
            // Arrange
            var toDoList = this.collectionOfToDoListOwners[0];

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoEntry = this.collectionOfToDoEntry[0];
            toDoEntry.ToDoList = toDoList;

            this.context.ToDoEntries.Add(toDoEntry);
            this.context.SaveChanges();

            // Act
            var result = this.toDoEntryRepository.GetInfoForReminder(toDoList.AccountId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ToDoEntryReminderDto>(result);
            Assert.NotNull(result.ToDoEntryTitle);
            Assert.That(result.ToDoEntryTitle, Is.EqualTo(toDoEntry.Title));
            Assert.That(result.ToDoEntryDueDate, Is.EqualTo(toDoEntry.DueDate));
        }

        [Test]
        public void GetInfoForReminder_TwoToDoEntries_ShouldReturnInfoWithTheClosestByDueDate()
        {
            // Arrange
            var toDoList = this.collectionOfToDoListOwners[0];

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoEntry1 = this.collectionOfToDoEntry[0];
            toDoEntry1.ToDoList = toDoList;
            toDoEntry1.DueDate = new DateTime(2022, 8, 15, 20, 22, 0);

            var toDoEntry2 = this.collectionOfToDoEntry[1];
            toDoEntry2.ToDoList = toDoList;
            toDoEntry2.DueDate = new DateTime(2022, 8, 16, 20, 22, 0);

            this.context.ToDoEntries.Add(toDoEntry1);
            this.context.ToDoEntries.Add(toDoEntry2);
            this.context.SaveChanges();

            // Act
            var result = this.toDoEntryRepository.GetInfoForReminder(toDoList.AccountId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ToDoEntryReminderDto>(result);
            Assert.NotNull(result.ToDoEntryTitle);
            Assert.That(result.ToDoEntryTitle, Is.EqualTo(toDoEntry1.Title));
            Assert.That(result.ToDoEntryDueDate, Is.EqualTo(toDoEntry1.DueDate));
        }

        [Test]
        public void GetInfoForReminder_NoToDoEntries_ShouldReturnNull()
        {
            // Arrange
            var toDoList = this.collectionOfToDoListOwners[0];

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();


            // Act
            var result = this.toDoEntryRepository.GetInfoForReminder(toDoList.AccountId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void AddRangeToDoEntries_IncorrectData_CollectionIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryRepository.AddRangeToDoEntries(null!));
        }

        [Test]
        public void AddRangeToDoEntries_CorrectData_ShouldAddCollectionOfToDoEntriesToDatabase()
        {
            // Arrange
            var toDoList = this.collectionOfToDoListOwners[0];

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoEntryCollection = this.collectionOfToDoEntry;
            toDoEntryCollection.ForEach(x => x.ToDoList = toDoList);

            // Act
            this.toDoEntryRepository.AddRangeToDoEntries(toDoEntryCollection);
            var result = this.context.ToDoEntries.Where(x => x.ToDoList == toDoList).AsEnumerable();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(toDoEntryCollection.Count));
        }

        [TearDown]
        public void CleanContext()
        {
            this.context.Database.EnsureDeleted();
        }
    }
}
