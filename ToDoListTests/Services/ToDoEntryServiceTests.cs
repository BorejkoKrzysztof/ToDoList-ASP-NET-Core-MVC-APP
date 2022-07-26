using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListCore.Enums;
using ToDoListInfrastructure.DTOs;
using ToDoListInfrastructure.Mappers;
using ToDoListInfrastructure.Models.Repositories;
using ToDoListInfrastructure.Models.Services;
using ToDoListInfrastructure.Models.ViewModels.ToDoEntry;

namespace ToDoListTests.Services
{
    [TestFixture]
    internal class ToDoEntryServiceTests
    {
        private Mock<IToDoEntryRepository> mockToDoEntryRepository;
        private Mock<IToDoListRepository> mockToDoListRepository;
        private IMapper mapper;
        private ToDoEntryService toDoEntryService;
        private List<ToDoEntry> collectionOfToDoEntries;
        private CreateToDoEntryViewModel createViewModelExample;
        private ToDoList toDoListExample;
        private EditToDoEntryViewModel EditoDoEntryViewModel;

        [SetUp]
        public void Setup()
        {
            this.mockToDoEntryRepository = new Mock<IToDoEntryRepository>();
            this.mockToDoListRepository = new Mock<IToDoListRepository>();
            this.mapper = AutoMapperConfig.Initialize();
            this.toDoEntryService = new ToDoEntryService(this.mockToDoEntryRepository.Object,
                                                this.mockToDoListRepository.Object,
                                                this.mapper);

            this.collectionOfToDoEntries = new List<ToDoEntry>()
            {
                new ToDoEntry()
                {
                    Title = "Todo 1",
                    Description = "Description 1",
                    DueDate = DateTime.Now.AddDays(2),
                },
                new ToDoEntry()
                {
                    Title = "Todo 2",
                    Description = "Description 2",
                    DueDate = DateTime.Now.AddDays(4),
                },
                new ToDoEntry()
                {
                    Title = "Todo 3",
                    Description = "Description 3",
                    DueDate = DateTime.Now.AddDays(6),
                },
            };

            this.createViewModelExample = new CreateToDoEntryViewModel()
            {
                Title = "Title",
                Description = "Description",
                DueDate = DateTime.Now.AddDays(15)
            };

            this.toDoListExample = new ToDoList()
            {
                AccountId = Guid.NewGuid().ToString(),
                ToDoEntries = new List<ToDoEntry>(),
                Title = "My ToDoList"
            };

            this.EditoDoEntryViewModel = new EditToDoEntryViewModel()
            {
                Description = "Description",
                Title = "Title",
                DueDate = DateTime.Now.AddDays(50)
            };
        }

        [Test]
        public void ConstructorTest_ToDoEntryRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ToDoEntryService(null!,
                                                                        this.mockToDoListRepository.Object,
                                                                        this.mapper));
        }

        [Test]
        public void ConstructorTest_ToDoListRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ToDoEntryService(this.mockToDoEntryRepository.Object,
                                                                        null!,
                                                                        this.mapper));
        }

        [Test]
        public void ConstructorTest_MapperIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ToDoEntryService(this.mockToDoEntryRepository.Object,
                                                                        this.mockToDoListRepository.Object,
                                                                        null!));
        }

        [Test]
        public void ReadToDoEntriesByToDoListId_IncorrectData_ToDoListIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                this.toDoEntryService.ReadToDoEntriesByToDoListId(Guid.Empty, 1, 4, true));
        }

        [Test]
        public void ReadToDoEntriesByToDoListId_IncorrectData_ListPageIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                this.toDoEntryService.ReadToDoEntriesByToDoListId(Guid.NewGuid(), -1, 4, true));
        }

        [Test]
        public void ReadToDoEntriesByToDoListId_IncorrectData_PageSizeIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                this.toDoEntryService.ReadToDoEntriesByToDoListId(Guid.NewGuid(), 1, -4, true));
        }

        [Test]
        public void ReadToDoEntriesByToDoListId_CorrectData_ShouldReturnToDoEntryCollectionViewModel()
        {
            // Arrange
            var toDoListId = Guid.NewGuid();
            var toDoEntryCollection = this.collectionOfToDoEntries.AsEnumerable();

            this.mockToDoEntryRepository.Setup(x => x.ReadAllToDoEntriesByToDoListId(toDoListId, 1, 4))
                                                                .Returns(toDoEntryCollection);
            this.mockToDoEntryRepository.Setup(x => x.CountToDoEntriesByToDoListId(toDoListId))
                                                                .Returns(toDoEntryCollection.Count());
            // Act
            var result = this.toDoEntryService.ReadToDoEntriesByToDoListId(toDoListId, 1, 4, true);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ToDoEntryCollectionViewModel>(result);
            Assert.That(result.ToDoEntries.Count, Is.EqualTo(toDoEntryCollection.Count()));
            Assert.That(result.pagingInfo.CurrentPage, Is.EqualTo(1));
            Assert.That(result.pagingInfo.ItemsPerPage, Is.EqualTo(4));
            Assert.That(result.pagingInfo.TotalPages, Is.EqualTo(1));
        }

        [Test]
        public void CreateToDoEntry_IncorrectData_ViewModelIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            CreateToDoEntryViewModel model = null!;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryService.CreateToDoEntry(model));
        }

        [Test]
        public void CreateToDoEntry_IncorrectData_ToDoListOwnerIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var model = this.createViewModelExample;
            model.ToDoListId = Guid.Empty;

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryService.CreateToDoEntry(model));
        }

        [Test]
        public void CreateToDoEntry_IncorrectData_TitleInViewModelIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var model = this.createViewModelExample;
            model.Title = string.Empty;
            model.ToDoListId = Guid.NewGuid();

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryService.CreateToDoEntry(model));
            model.Title = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryService.CreateToDoEntry(model));
        }

        [Test]
        public void CreateToDoEntry_IncorrectData_DescriptionInViewModelIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var model = this.createViewModelExample;
            model.ToDoListId = Guid.NewGuid();
            model.Title = "Title";
            model.Description = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryService.CreateToDoEntry(model));
            model.Description = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryService.CreateToDoEntry(model));
        }

        [Test]
        public void CreateToDoEntry_CorrectData_ShouldReturnToDoEntryDto()
        {
            // Assert
            var toDoListId = Guid.NewGuid();
            var model = this.createViewModelExample;
            model.ToDoListId = Guid.NewGuid();

            var toDoListOwner = this.toDoListExample;
            toDoListOwner.Id = toDoListId;

            var createdToDoEntry = new ToDoEntry()
            {
                ToDoList = toDoListOwner,
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate
            };

            var expectedDtoToDoEntry = this.mapper.Map<ToDoEntryDto>(createdToDoEntry);

            this.mockToDoListRepository.Setup(x => x.ReadToDoList(model.ToDoListId)).Returns(toDoListOwner);

            // Act
            var result = this.toDoEntryService.CreateToDoEntry(model);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ToDoEntryDto>(result);
            Assert.That(result.Title, Is.EqualTo(expectedDtoToDoEntry.Title));
            Assert.That(result.Description, Is.EqualTo(expectedDtoToDoEntry.Description));
            Assert.That(result.Progress, Is.EqualTo(expectedDtoToDoEntry.Progress));
            Assert.That(result.DueDate, Is.EqualTo(expectedDtoToDoEntry.DueDate));
            this.mockToDoListRepository.Verify(x => x.ReadToDoList(model.ToDoListId), Times.Once);
        }

        [Test]
        public void ReadToDoEntry_IncorrectData_GivenToDoEntryIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryService.ReadToDoEntry(Guid.Empty));
        }

        [Test]
        public void ReadToDoEntry_CorrectData_ShouldReturnToDoEntryDetailsDto()
        {
            // Arrange
            var toDoListOwner = this.toDoListExample;
            toDoListOwner.Id = Guid.NewGuid();

            var createdToDoEntry = new ToDoEntry()
            {
                Id = Guid.NewGuid(),
                ToDoList = toDoListOwner,
                Title = "My ToDo Entry",
                Description = "Description",
                DueDate = DateTime.Now.AddDays(15)
            };

            this.mockToDoEntryRepository.Setup(x => x.ReadToDoEntry(createdToDoEntry.Id)).Returns(createdToDoEntry);
            var toDoEntryDetailsDto = this.mapper.Map<ToDoEntryDetailsDto>(createdToDoEntry);

            // Act
            var result = this.toDoEntryService.ReadToDoEntry(createdToDoEntry.Id);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ToDoEntryDetailsDto>(result);
            Assert.That(result.Title, Is.EqualTo(toDoEntryDetailsDto.Title));
            Assert.That(result.Description, Is.EqualTo(toDoEntryDetailsDto.Description));
            Assert.That(result.DueDate, Is.EqualTo(toDoEntryDetailsDto.DueDate));
        }

        [Test]
        public void EditToDoEntry_IncorrectData_EditToDoEntryViewModelIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryService.EditToDoEntry(null!));
        }

        [Test]
        public void EditToDoEntry_IncorrectData_ToDoEntryIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var model = this.EditoDoEntryViewModel;
            model.ToDoEntryId = Guid.Empty;

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryService.EditToDoEntry(model));
        }

        [Test]
        public void EditToDoEntry_IncorrectData_DueDateIsEarlierOrEqualNow_ShouldThrowArgumentOuOfRangeException()
        {
            // Arrange
            var model = this.EditoDoEntryViewModel;
            model.ToDoEntryId = Guid.NewGuid();
            model.DueDate = DateTime.Now.AddDays(-50);

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryService.EditToDoEntry(model));
        }

        [Test]
        public void EditToDoEntry_CorrectData_ButToDoEntryWithThatSpecificIdDoesNotExist_ShouldThrowArgumentNullException()
        {
            // Arrange
            var model = this.EditoDoEntryViewModel;
            model.ToDoEntryId = Guid.NewGuid();
            model.DueDate = DateTime.Now.AddDays(50);

            ToDoEntry returnedToDoEntry = null!;

            this.mockToDoEntryRepository.Setup(x => x.ReadToDoEntry(model.ToDoEntryId))
                                                .Returns(returnedToDoEntry);

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryService.EditToDoEntry(model));
        }

        [Test]
        public void EditToDoEntry_CorrectData_ShouldEditToDoEntry_ChangeOnePropertyAtTheSameTime()
        {
            // Arrange
            var toDoEntryId = Guid.NewGuid();

            var newTitle = "New title Updated";

            var toDoListOwner = this.toDoListExample;
            toDoListOwner.Id = Guid.NewGuid();

            var returnedToDoEntryFromRepository = this.collectionOfToDoEntries[0];
            returnedToDoEntryFromRepository.Id = toDoEntryId;
            returnedToDoEntryFromRepository.ToDoList = toDoListOwner;

            var model = this.EditoDoEntryViewModel;
            model.ToDoEntryId = toDoEntryId;
            model.Title = newTitle;

            this.mockToDoEntryRepository.Setup(x => x.ReadToDoEntry(toDoEntryId))
                                            .Returns(returnedToDoEntryFromRepository);

            // Act
            var result = toDoEntryService.EditToDoEntry(model);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ToDoEntryDetailsDto>(result);
            Assert.That(result.Title, Is.EqualTo(newTitle));
            this.mockToDoEntryRepository.Verify(x => x.UpdateToDoEntry
                                                            (It.Is<ToDoEntry>(y => y.Title == newTitle)),
                                                                        Times.Once());
        }

        [Test]
        public void EditToDoEntry_CorrectData_ShouldEditToDoEntry_AllPossiblePropertyAtTheSameTime()
        {
            // Arrange
            var toDoEntryId = Guid.NewGuid();

            var newTitle = "New title Updated";
            var newDescription = "New Description Updated";
            var newDueDate = DateTime.Now.AddDays(15);

            var toDoListOwner = this.toDoListExample;
            toDoListOwner.Id = Guid.NewGuid();

            var returnedToDoEntryFromRepository = this.collectionOfToDoEntries[0];
            returnedToDoEntryFromRepository.Id = toDoEntryId;
            returnedToDoEntryFromRepository.ToDoList = toDoListOwner;

            var model = this.EditoDoEntryViewModel;
            model.ToDoEntryId = toDoEntryId;
            model.Title = newTitle;
            model.Description = newDescription;
            model.DueDate = newDueDate;

            this.mockToDoEntryRepository.Setup(x => x.ReadToDoEntry(toDoEntryId))
                                            .Returns(returnedToDoEntryFromRepository);

            // Act
            var result = this.toDoEntryService.EditToDoEntry(model);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ToDoEntryDetailsDto>(result);
            Assert.That(result.Title, Is.EqualTo(newTitle));
            Assert.That(result.Description, Is.EqualTo(newDescription));
            Assert.That(result.DueDate, Is.EqualTo(newDueDate));
        }

        [Test]
        public void DeleteToDoEntry_IncorrectData_ViewModelIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryService.DeleteToDoEntryAsync(null!));
        }

        [Test]
        public void DeleteToDoEntry_IncorrectData_ToDoEntryIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var model = new DeleteToDoEntryViewModel()
            {
                ToDoEntryId = Guid.Empty
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryService.DeleteToDoEntryAsync(model));
        }

        [Test]
        public void DeleteToDoEntry_CorrectData_ShouldRunDeleteMethodFromRepository()
        {
            // Arrange
            var toDoEntryId = Guid.NewGuid();
            var model = new DeleteToDoEntryViewModel()
            {
                ToDoEntryId = toDoEntryId
            };

            var toDoListOwner = this.toDoListExample;
            toDoListOwner.Id = Guid.NewGuid();

            var returnedToDoEntryFromRepository = this.collectionOfToDoEntries[0];
            returnedToDoEntryFromRepository.Id = toDoEntryId;
            returnedToDoEntryFromRepository.ToDoList = toDoListOwner;

            this.mockToDoEntryRepository.Setup(x => x.ReadToDoEntry(toDoEntryId))
                                                .Returns(returnedToDoEntryFromRepository);

            // Act
            this.toDoEntryService.DeleteToDoEntryAsync(model);

            // Assert
            this.mockToDoEntryRepository.Verify(x => x.DeleteToDoEntry(It.Is<ToDoEntry>(y =>
                                                        y.Id == returnedToDoEntryFromRepository.Id &&
                                                        y.Title == returnedToDoEntryFromRepository.Title)),
                                                                                    Times.Once());
        }

        [Test]
        public void ChangeProgressValue_IncorrectData_GivenViewModelIsNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryService
                                                            .ChangeProgressValue(null!));
        }

        [Test]
        public void ChangeProgressValue_IncorrectData_ToDoEntryIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var model = new ChangeProgressStatusViewModel()
            {
                ToDoEntryId = Guid.Empty,
                ProgressValue = ProgressStatus.Completed
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryService.ChangeProgressValue(model));
        }

        [Test]
        public void ChangeProgressValue_IncorrectData_GivenProgressValueIsOutOfRange_ShouldThrowOutOfRangeException()
        {
           // Arrange
           var model = new ChangeProgressStatusViewModel()
           {
               ToDoEntryId = Guid.NewGuid(),
               ProgressValue = (ProgressStatus)int.MaxValue
           };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryService.ChangeProgressValue(model));
            model.ProgressValue = (ProgressStatus)int.MinValue;
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryService.ChangeProgressValue(model));
        }

        [Test]
        public void ChangeProgressValue_CorrectData_ShouldRunUpdateMethodWithToDoEntryWhichHasUpdatedProgressStatus()
        {
            // Arrange
            var toDoEntryId = Guid.NewGuid();

            var model = new ChangeProgressStatusViewModel()
            {
                ToDoEntryId = toDoEntryId,
                ProgressValue = ProgressStatus.In_Progress
            };

            var toDoListOwner = this.toDoListExample;
            toDoListOwner.Id = Guid.NewGuid();

            var toDoEntry = this.collectionOfToDoEntries[1];
            toDoEntry.Id = toDoEntryId;
            toDoEntry.ToDoList = toDoListOwner;

            this.mockToDoEntryRepository.Setup(x => x.ReadToDoEntry(toDoEntryId)).Returns(toDoEntry);

            // Act
            this.toDoEntryService.ChangeProgressValue(model);

            // Assert
            this.mockToDoEntryRepository.Verify(x => x.ReadToDoEntry(toDoEntryId), Times.Once());
            this.mockToDoEntryRepository.Verify(x =>
                                        x.UpdateToDoEntry(It.Is<ToDoEntry>(y =>
                                                             y.Progress == model.ProgressValue)),
                                                                Times.Once());
        }

        [Test]
        public void ReadTodaysItemsByUserId_IncorrectData_GivenAccountIdIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => 
                                        this.toDoEntryService.ReadTodaysItemsByUserId(string.Empty, 1, 4));
            Assert.Throws<ArgumentNullException>(() => 
                                        this.toDoEntryService.ReadTodaysItemsByUserId(null!, 1, 4));
        }

        [Test]
        public void ReadTodaysItemsByUserId_IncorrectData_GivenAccountIdIsNotRepresentationOfGuid_ShouldThrowArgumentException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() =>
                                        this.toDoEntryService.ReadTodaysItemsByUserId("Bad id", 1, 4));
        }

        [Test]
        public void ReadTodaysItemsByUserId_CorrectData_ShouldReturnCollectionOfToDoEntryDtoWithDueDatesToday()
        {
            // Arrange
            var randomAccountId = Guid.NewGuid().ToString();
            var toDoEntryCollection = this.collectionOfToDoEntries;
            toDoEntryCollection.ForEach(x => x.DueDate = DateTime.Today);

            this.mockToDoEntryRepository.Setup(x => x.ReadAllDueDateToday(randomAccountId, 1, 4))
                                                        .Returns(toDoEntryCollection);

            // Act
            var results = this.toDoEntryService.ReadTodaysItemsByUserId(randomAccountId, 1, 4);

            // Assert
            Assert.NotNull(results);
            Assert.IsInstanceOf<ToDoEntryCollectionViewModel>(results);
            Assert.That(results.ToDoEntries.Count, Is.EqualTo(3));
        }

        [Test]
        public void GetToDoEntryForReminder_IncorrectData_AccountIdIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryService.GetToDoEntryForReminder(string.Empty));
            Assert.Throws<ArgumentNullException>(() => this.toDoEntryService.GetToDoEntryForReminder(null!));
        }

        [Test]
        public void GetToDoEntryForReminder_IncorrectData_AccountIdIsNotRepresentationOfString_ShouldThrowArgumentException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => this.toDoEntryService.GetToDoEntryForReminder("Bad Id"));
        }

        [Test]
        public void GetToDoEntryForReminder_CorrectData_ShouldReturnToDoEntryReminderDTO()
        {
            // Arrange

            var accountId = Guid.NewGuid().ToString();
            var reminderModel = new ToDoEntryReminderDto()
            {
                ToDoEntryDueDate = new DateTime(2022, 8, 22, 20, 22, 0),
                ToDoEntryTitle = "The closest task!"
            };

            this.mockToDoEntryRepository.Setup(x => x.GetInfoForReminder(accountId)).Returns(reminderModel);

            // Act
            var result = this.toDoEntryService.GetToDoEntryForReminder(accountId);

            // Assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ToDoEntryReminderDto>(result);
            Assert.That(result.ToDoEntryTitle, Is.EqualTo(reminderModel.ToDoEntryTitle));
            Assert.That(result.ToDoEntryDueDate, Is.EqualTo(reminderModel.ToDoEntryDueDate));
        }

        [Test]
        public void CompleteToDoEntry_IncorrectData_ToDoEntryIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoEntryService.CompleteToDoEntry(Guid.Empty));
        }

        [Test]
        public void CompleteToDoEntry_CorrectData_ShouldRunMethodUpdateWithProgressStatusCompleted()
        {
            // Arrange
            var toDoEntryId = Guid.NewGuid();
            var toDoEntry = this.collectionOfToDoEntries[0];
            toDoEntry.Id = toDoEntryId;
            toDoEntry.Progress = ProgressStatus.Not_Started;

            this.mockToDoEntryRepository.Setup(x => x.ReadToDoEntry(toDoEntryId)).Returns(toDoEntry);

            // Act
            this.toDoEntryService.CompleteToDoEntry(toDoEntryId);

            // Assert
            this.mockToDoEntryRepository.Verify(x => x.ReadToDoEntry(toDoEntryId), Times.Once());
            this.mockToDoEntryRepository.Verify(x => x.UpdateToDoEntry(It.Is<ToDoEntry>(y =>
                                                                            y.Progress == ProgressStatus.Completed)),
                                                                            Times.Once());
        }
    }
}