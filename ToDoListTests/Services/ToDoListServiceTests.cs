using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListInfrastructure.DTOs;
using ToDoListInfrastructure.Mappers;
using ToDoListInfrastructure.Models.Repositories;
using ToDoListInfrastructure.Models.Services;
using ToDoListInfrastructure.Models.ViewModels;
using ToDoListInfrastructure.Models.ViewModels.ToDoList;

namespace ToDoListTests.Services
{
    internal class ToDoListServiceTests
    {
        private string accountId;
        private Mock<IToDoListRepository> mockToDoListRepository;
        private Mock<IToDoEntryRepository> mockToDoEntryRepository;
        private ToDoListService toDoListService;
        private IMapper mapper;
        private List<ToDoListDto> toDoListsDTOsCollection;
        private List<ToDoList> toDoListCollection;

        [SetUp]
        public void Setup()
        {
            this.accountId = Guid.NewGuid().ToString();
            this.mockToDoListRepository = new Mock<IToDoListRepository>();
            this.mockToDoEntryRepository = new Mock<IToDoEntryRepository>();
            this.mapper = AutoMapperConfig.Initialize();
            this.toDoListService = new ToDoListService(this.mockToDoListRepository.Object,
                                    this.mockToDoEntryRepository.Object,
                                    this.mapper);

            this.toDoListsDTOsCollection = new List<ToDoListDto>()
            {
                new ToDoListDto()
                {
                    Title = "Tdl DTO 1",
                    ToDoEntries = new List<ToDoEntryDto>()
                },
                new ToDoListDto()
                {
                    Title = "Tdl DTO 2",
                    ToDoEntries = new List<ToDoEntryDto>()
                },
                new ToDoListDto()
                {
                    Title = "Tdl DTO 3",
                    ToDoEntries = new List<ToDoEntryDto>()
                },
                new ToDoListDto()
                {
                    Title = "Tdl DTO 4",
                    ToDoEntries = new List<ToDoEntryDto>()
                },
                new ToDoListDto()
                {
                    Title = "Tdl DTO 5",
                    ToDoEntries = new List<ToDoEntryDto>()
                },
            };

            this.toDoListCollection = new List<ToDoList>()
            {
                new ToDoList()
                {
                    Title = "Tdl DTO 1",
                    ToDoEntries = new List<ToDoEntry>(),
                    AccountId = this.accountId
                },
                new ToDoList()
                {
                    Title = "Tdl DTO 2",
                    ToDoEntries = new List<ToDoEntry>(),
                    AccountId = this.accountId
                },
                new ToDoList()
                {
                    Title = "Tdl DTO 3",
                    ToDoEntries = new List<ToDoEntry>(),
                    AccountId = this.accountId
                },
                                new ToDoList()
                {
                    Title = "Tdl DTO 4",
                    ToDoEntries = new List<ToDoEntry>(),
                    AccountId = this.accountId
                },
                new ToDoList()
                {
                    Title = "Tdl DTO 5",
                    ToDoEntries = new List<ToDoEntry>(),
                    AccountId = this.accountId
                },
            };
        }

        [Test]
        public void TestConstructor_ToDoListRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ToDoListService(null!,
                                                                        this.mockToDoEntryRepository.Object,
                                                                        this.mapper));
        }

        [Test]
        public void TestConstructor_ToDoEntryRepositoryIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ToDoListService(this.mockToDoListRepository.Object,
                                                                        null!,
                                                                        this.mapper));
        }

        [Test]
        public void TestConstructor_MapperIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ToDoListService(this.mockToDoListRepository.Object,
                                                                        this.mockToDoEntryRepository.Object,
                                                                        null!));
        }

        [Test]
        public void TestConstructor_AllParametersAreNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ToDoListService(null!, null!, null!));
        }

        [Test]
        public void ReadAllLists_IncorrectDatas_AccountIdIsNullOrEmptyString_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListService.ReadAllLists(string.Empty, 1, 4));
            Assert.Throws<ArgumentNullException>(() => this.toDoListService.ReadAllLists(null!, 1, 4));
        }

        [Test]
        public void ReadAllLists_IncorrectDatas_AccountIdIsNoRepresentationOfGuid_ShouldThrowArgumentException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => this.toDoListService.ReadAllLists("Bad id", 1, 4));
        }

        [Test]
        public void ReadAllLists_IncorrectDatas_listPageIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListService.ReadAllLists(this.accountId, 0, 4));
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListService.ReadAllLists(this.accountId, -1, 4));
        }

        [Test]
        public void ReadAllLists_IncorrectDatas_PageSizeIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListService.ReadAllLists(this.accountId, 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListService.ReadAllLists(this.accountId, 1, -4));
        }

        [Test]
        public void ReadAllLists_CorrectData_ShouldReturnToDoListCollectionViewModelWith1Page_OnlyFourToDoListsUserHas()
        {
            // Arrange
            this.mockToDoListRepository.Setup(x => x.ReadAllNotHiddenToDoLists(this.accountId, 1, 4))
                                            .Returns(this.toDoListCollection.Take(4));
            this.mockToDoListRepository.Setup(x => x.CountNotHiddenUserToDoLists(this.accountId))
                                            .Returns(4);


            // Act
            var results = toDoListService.ReadAllLists(this.accountId, 1, 4);

            // Assert
            Assert.NotNull(results);
            Assert.IsInstanceOf<ToDoListCollectionViewModel>(results);
            Assert.IsInstanceOf<IEnumerable<ToDoListDto>>(results.ToDoListCollection);
            Assert.IsInstanceOf<PagingInfo>(results.PagingInfo);
            Assert.That(results.ToDoListCollection.Count(), Is.EqualTo(4));
            Assert.That(results.PagingInfo.CurrentPage, Is.EqualTo(1));
            Assert.That(results.PagingInfo.ItemsPerPage, Is.EqualTo(4));
            Assert.That(results.PagingInfo.TotalPages, Is.EqualTo(1));

            this.mockToDoListRepository.Verify(x => x.ReadAllNotHiddenToDoLists(this.accountId, 1, 4), Times.Once());
            this.mockToDoListRepository.Verify(x => x.CountNotHiddenUserToDoLists(this.accountId), Times.Once());
        }

        [Test]
        public void ReadAllLists_CorrectData_ShouldReturnToDoListCollectionViewModelWith2Pages_UserHas5ToDoLists()
        {
            // Arrange
            this.mockToDoListRepository.Setup(x => x.ReadAllNotHiddenToDoLists(this.accountId, 1, 4))
                                            .Returns(this.toDoListCollection.Take(4));
            this.mockToDoListRepository.Setup(x => x.CountNotHiddenUserToDoLists(this.accountId))
                                            .Returns(5);


            // Act
            var results = toDoListService.ReadAllLists(this.accountId, 1, 4);

            // Assert
            Assert.NotNull(results);
            Assert.IsInstanceOf<ToDoListCollectionViewModel>(results);
            Assert.IsInstanceOf<IEnumerable<ToDoListDto>>(results.ToDoListCollection);
            Assert.IsInstanceOf<PagingInfo>(results.PagingInfo);
            Assert.That(results.ToDoListCollection.Count(), Is.EqualTo(4));
            Assert.That(results.PagingInfo.CurrentPage, Is.EqualTo(1));
            Assert.That(results.PagingInfo.ItemsPerPage, Is.EqualTo(4));
            Assert.That(results.PagingInfo.TotalPages, Is.EqualTo(2));

            this.mockToDoListRepository.Verify(x => x.ReadAllNotHiddenToDoLists(this.accountId, 1, 4), Times.Once());
            this.mockToDoListRepository.Verify(x => x.CountNotHiddenUserToDoLists(this.accountId), Times.Once());
        }

        [Test]
        public void ReadAllLists_CorrectData_ShouldReturnToDoListCollectionViewModelWith0Pages_UserHasNoToDoLists()
        {
            // Arrange
            this.mockToDoListRepository.Setup(x => x.ReadAllNotHiddenToDoLists(this.accountId, 1, 4))
                                            .Returns(Enumerable.Empty<ToDoList>());
            this.mockToDoListRepository.Setup(x => x.CountNotHiddenUserToDoLists(this.accountId))
                                            .Returns(0);


            // Act
            var results = toDoListService.ReadAllLists(this.accountId, 1, 4);

            // Assert
            Assert.NotNull(results);
            Assert.IsInstanceOf<ToDoListCollectionViewModel>(results);
            Assert.IsInstanceOf<IEnumerable<ToDoListDto>>(results.ToDoListCollection);
            Assert.IsInstanceOf<PagingInfo>(results.PagingInfo);
            Assert.That(results.ToDoListCollection.Count(), Is.EqualTo(0));
            Assert.That(results.PagingInfo.CurrentPage, Is.EqualTo(1));
            Assert.That(results.PagingInfo.ItemsPerPage, Is.EqualTo(4));
            Assert.That(results.PagingInfo.TotalPages, Is.EqualTo(0));

            this.mockToDoListRepository.Verify(x => x.ReadAllNotHiddenToDoLists(this.accountId, 1, 4), Times.Once());
            this.mockToDoListRepository.Verify(x => x.CountNotHiddenUserToDoLists(this.accountId), Times.Once());
        }

        [Test]
        public void ReadAllLists_CorrectData_ShouldReturnToDoListCollectionViewModelWithCurrentPage2_UserHas8ToDoLists()
        {
            // Arrange
            this.mockToDoListRepository.Setup(x => x.ReadAllNotHiddenToDoLists(this.accountId, 2, 4))
                                            .Returns(this.toDoListCollection.Take(4));
            this.mockToDoListRepository.Setup(x => x.CountNotHiddenUserToDoLists(this.accountId))
                                            .Returns(8);


            // Act
            var results = toDoListService.ReadAllLists(this.accountId, 2, 4);

            // Assert
            Assert.NotNull(results);
            Assert.IsInstanceOf<ToDoListCollectionViewModel>(results);
            Assert.IsInstanceOf<IEnumerable<ToDoListDto>>(results.ToDoListCollection);
            Assert.IsInstanceOf<PagingInfo>(results.PagingInfo);
            Assert.That(results.ToDoListCollection.Count(), Is.EqualTo(4));
            Assert.That(results.PagingInfo.CurrentPage, Is.EqualTo(2));
            Assert.That(results.PagingInfo.ItemsPerPage, Is.EqualTo(4));
            Assert.That(results.PagingInfo.TotalPages, Is.EqualTo(2));

            this.mockToDoListRepository.Verify(x => x.ReadAllNotHiddenToDoLists(this.accountId, 2, 4), Times.Once());
            this.mockToDoListRepository.Verify(x => x.CountNotHiddenUserToDoLists(this.accountId), Times.Once());
        }

        [Test]
        public void CreateToDoList_IncorrectData_AccountIdIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var model = new CreateToDoListViewModel()
            {
                Title = "title"
            };

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListService.CreateToDoList(model, string.Empty));
            Assert.Throws<ArgumentNullException>(() => this.toDoListService.CreateToDoList(model, null!));
        }

        [Test]
        public void CreateToDoList_IncorrectData_AccountIdIsNoRepresentationOfGuid_ShouldThrowArgumentException()
        {
            // Arrange
            var model = new CreateToDoListViewModel()
            {
                Title = "title"
            };

            // Assert
            Assert.Throws<ArgumentException>(() => this.toDoListService.CreateToDoList(model, "Bad id"));
        }

        [Test]
        public void CreateToDoList_IncorrectData_ModelIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListService.CreateToDoList(null!, this.accountId));
        }

        [Test]
        public void CreateToDoList_IncorrectData_ModelTitleIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var model = new CreateToDoListViewModel()
            {
                Title = string.Empty
            };

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListService.CreateToDoList(model, this.accountId));
            model.Title = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoListService.CreateToDoList(model, this.accountId));
        }

        [Test]
        public void CreateToDoList_IncorrectData_ToDoListTitleIsTooLong_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var model = new CreateToDoListViewModel()
            {
                Title = new string('A', 101)
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => 
                                                    this.toDoListService.CreateToDoList(model, this.accountId));
        }

        [Test]
        public void CreateToDoList_IncorrectData_AllParametersAreNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListService.CreateToDoList(null!, null!));
        }

        [Test]
        public void CreateToDoList_CorrectData_ShouldRunCreateMethodFromRepositoryOnceWithSpecificParameters()
        {
            // Arrange
            var model = new CreateToDoListViewModel()
            {
                Title = "New Title"
            };

            // Act
            this.toDoListService.CreateToDoList(model, this.accountId);

            // Assert
            this.mockToDoListRepository.Verify(x => x.CreateToDoList(It.Is<ToDoList>(x => x.Title == model.Title &&
                                                                                       x.AccountId == this.accountId)), 
                                                                                        Times.Once);
        }

        [Test]
        public void UpdateToDoList_IncorrectData_ViewModelIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListService.UpdateToDoList(null!));
        }

        [Test]
        public void UpdateToDoList_IncorrectData_ViewModelTitleIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var model = new UpdateToDoListViewModel()
            {
                ToDoListId = Guid.NewGuid(),
                Title = string.Empty
            };

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListService.UpdateToDoList(model));
            model.Title = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoListService.UpdateToDoList(model));
        }

        [Test]
        public void UpdateToDoList_IncorrectData_ViewModelToDoListIdIsEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var model = new UpdateToDoListViewModel()
            {
                ToDoListId = Guid.Empty,
                Title = "Title"
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListService.UpdateToDoList(model));
        }

        [Test]
        public void UpdateToDoList_IncorrectData_ToDoListTitleIsTooLong_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var model = new UpdateToDoListViewModel()
            {
                Title = new string('A', 101),
                ToDoListId = Guid.NewGuid()
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                                                    this.toDoListService.UpdateToDoList(model));
        }

        [Test]
        public void UpdateToDoList_IncorrectData_AllDataAreInvalid()
        {
            // Arrange
            var model = new UpdateToDoListViewModel()
            {
                Title = string.Empty,
                ToDoListId = Guid.Empty
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoListService.UpdateToDoList(model));
        }

        [Test]
        public void UpdateToDoList_CorrectData_ShouldRunUpdateMethodFromRepositoryOnlyOnce()
        {
            // Arrange
            var toDoListID = Guid.NewGuid();

            var mockedToDoListInstance = new ToDoList()
            {
                Id = toDoListID,
                AccountId = this.accountId,
                Title = "Title",
                ToDoEntries = new List<ToDoEntry>(),
            };


            this.mockToDoListRepository.Setup(x => x.ReadToDoList(toDoListID))
                                        .Returns(mockedToDoListInstance);

            var model = new UpdateToDoListViewModel()
            {
                Title = "New Title",
                ToDoListId = toDoListID
            };

            // Act
            toDoListService.UpdateToDoList(model);

            // Assert
            this.mockToDoListRepository.Verify(x => x.ReadToDoList(toDoListID), Times.Once());
            this.mockToDoListRepository.Verify(x => x.UpdateToDoList(It.Is<ToDoList>(x => x.Title == model.Title &&
                                                                        x.Id == toDoListID)),
                                                                            Times.Once);
        }

        [Test]
        public void DeleteToDoList_IncorrectData_ViewModleIsNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListService.DeleteToDoList(null!));
        }

        [Test]
        public void DeleteToDoList_IncorrectData_ToDoListIdIsGuidEmpty()
        {
            // Arrange
            var model = new DeleteToDoListViewModel()
            {
                Id = Guid.Empty
            };

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoListService.DeleteToDoList(model));
        }

        [Test]
        public void DeleteToDoList_CorrectData_ShouldRunTwoMethodsFromRepositoryOneTime()
        {
            // Arrange
            var toDoListId = Guid.NewGuid();

            var viewModel = new DeleteToDoListViewModel()
            {
                Id = toDoListId
            };

            var returnedToDoList = this.toDoListCollection[0];

            this.mockToDoListRepository.Setup(x => x.ReadToDoList(toDoListId))
                                            .Returns(returnedToDoList);


            // Act
            toDoListService.DeleteToDoList(viewModel);

            // Assert
            this.mockToDoListRepository.Verify(x => x.ReadToDoList(toDoListId), Times.Once());
            this.mockToDoListRepository.Verify(x => x.DeleteToDoList(returnedToDoList), Times.Once());
        }

        [Test]
        public void CopyToDoList_IncorrectData_GivenIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoListService.CopyToDoList(Guid.Empty));
        }

        [Test]
        public void CopyToDoList_CorrectParameters_ButToDoListToCopyDoesNotExist_ShouldRunOnlyReadToDoListFromRepository()
        {
            // Arrange
            var toDoListId = Guid.NewGuid();

            this.mockToDoListRepository.Setup(x => x.ReadToDoList(toDoListId)).Returns((ToDoList)null!);

            // Act
            this.toDoListService.CopyToDoList(toDoListId);

            // Assert
            this.mockToDoListRepository.Verify(x => x.CountNumberOfCopiesForToDoList(It.IsAny<string>(),
                                                                                        It.IsAny<string>()),
                                                                                        Times.Never());
            this.mockToDoEntryRepository.Verify(x => x.ReadAllToDoEntriesByToDoListId(toDoListId), Times.Never());
            this.mockToDoEntryRepository.Verify(x => x.AddRangeToDoEntries(It.IsAny<List<ToDoEntry>>()), Times.Never());
        }

        [Test]
        public void CopyToDoList_CorrectParameters_ShouldRunMethodFromRepositoryOnlyOnce()
        {
            // Arrange
            var toDoListId = Guid.NewGuid();
            var toDoList = this.toDoListCollection[0];
            toDoList.Id = toDoListId;

            this.mockToDoListRepository.Setup(x => x.ReadToDoList(toDoListId)).Returns(toDoList);

            string expectedTitleAfterUpdate = $"{toDoList.Title} -Copy 1";

            this.mockToDoListRepository.Setup(x => x.CountNumberOfCopiesForToDoList(toDoList.Title,
                                                                                        toDoList.AccountId))
                                                                                            .Returns(1);

            this.mockToDoEntryRepository.Setup(x => x.ReadAllToDoEntriesByToDoListId(toDoListId))
                                                            .Returns(Enumerable.Empty<ToDoEntry>());



            // Act
            this.toDoListService.CopyToDoList(toDoListId);

            // Assert
            this.mockToDoListRepository.Verify(x => x.ReadToDoList(toDoListId), Times.Once());
            this.mockToDoListRepository.Verify(x => x.CountNumberOfCopiesForToDoList(toDoList.Title,
                                                                                        toDoList.AccountId), Times.Once());
            this.mockToDoListRepository.Verify(x => x.CreateToDoList(It.Is<ToDoList>(x => x.Title == expectedTitleAfterUpdate)),
                                                                                    Times.Once());
            this.mockToDoEntryRepository.Verify(x => x.ReadAllToDoEntriesByToDoListId(toDoListId), Times.Once());
            this.mockToDoEntryRepository.Verify(x => x.AddRangeToDoEntries(It.IsAny<List<ToDoEntry>>()),
                                                                                    Times.Once());
        }

        [Test]
        public void CopyToDoList_CorrectParameters_TitleIsToLongToCopy_ShouldRunMethodFromRepositoryOnlyOnceAndMakeTitleShorter()
        {
            // Arrange
            var toDoListId = Guid.NewGuid();
            var toDoList = this.toDoListCollection[0];
            toDoList.Id = toDoListId;
            toDoList.Title = new string('A', 100);

            this.mockToDoListRepository.Setup(x => x.ReadToDoList(toDoListId)).Returns(toDoList);

            this.mockToDoListRepository.Setup(x => x.CountNumberOfCopiesForToDoList(toDoList.Title,
                                                                                        toDoList.AccountId))
                                                                                            .Returns(1);

            this.mockToDoEntryRepository.Setup(x => x.ReadAllToDoEntriesByToDoListId(toDoListId))
                                                            .Returns(Enumerable.Empty<ToDoEntry>());


            // Act
            this.toDoListService.CopyToDoList(toDoListId);

            // Assert
            this.mockToDoListRepository.Verify(x => x.ReadToDoList(toDoListId), Times.Once());
            this.mockToDoListRepository.Verify(x => x.CountNumberOfCopiesForToDoList(toDoList.Title,
                                                                                        toDoList.AccountId), Times.Once());
            this.mockToDoListRepository.Verify(x => x.CreateToDoList(It.Is<ToDoList>(x => x.Title.Length < 90)),
                                                                                    Times.Once());
            this.mockToDoEntryRepository.Verify(x => x.ReadAllToDoEntriesByToDoListId(toDoListId), Times.Once());
            this.mockToDoEntryRepository.Verify(x => x.AddRangeToDoEntries(It.IsAny<List<ToDoEntry>>()),
                                                                                    Times.Once());
        }

        [Test]
        public void SwitchHideToDoList_IncorrectData_ToDoListIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListService.SwitchHideToDoList(Guid.Empty));
        }

        [Test]
        public void SwitchHideToDoList_CorrectData_StartWithHiddenFalse_ShouldChangeHiddenPropertyAndRunMethodsFromRepositoryOneTime()
        {
            // Arrange
            var toDoListId = Guid.NewGuid();
            var toDoList = this.toDoListCollection[0];
            toDoList.Id = toDoListId;
            toDoList.UpdatedAt = new DateTime(2000, 12, 2, 14, 44, 32);

            this.mockToDoListRepository.Setup(x => x.ReadToDoList(toDoListId)).Returns(toDoList);

            // Act
            this.toDoListService.SwitchHideToDoList(toDoListId);

            // Assert
            this.mockToDoListRepository.Verify(x => x.ReadToDoList(toDoListId), Times.Once());
            this.mockToDoListRepository.Verify(x => x.UpdateToDoList(It.Is<ToDoList>(x => x.Hidden)), Times.Once());
            this.mockToDoListRepository.Verify(x =>
                                                x.UpdateToDoList(It.Is<ToDoList>(x => x.UpdatedAt.Year == DateTime.Now.Year)),
                                            Times.Once());
        }

        [Test]
        public void SwitchHideToDoList_CorrectData_StartWithHiddenTrue_ShouldChangeHiddenPropertyAndRunMethodsFromRepositoryOneTime()
        {
            // Arrange
            var toDoListId = Guid.NewGuid();
            var toDoList = this.toDoListCollection[0];
            toDoList.Id = toDoListId;
            toDoList.UpdatedAt = new DateTime(2000, 12, 2, 14, 44, 32);
            toDoList.Hidden = true;

            this.mockToDoListRepository.Setup(x => x.ReadToDoList(toDoListId)).Returns(toDoList);

            // Act
            this.toDoListService.SwitchHideToDoList(toDoListId);

            // Assert
            this.mockToDoListRepository.Verify(x => x.ReadToDoList(toDoListId), Times.Once());
            this.mockToDoListRepository.Verify(x => x.UpdateToDoList(It.Is<ToDoList>(x => !x.Hidden)), Times.Once());
            this.mockToDoListRepository.Verify(x =>
                                                x.UpdateToDoList(It.Is<ToDoList>(x => x.UpdatedAt.Year == DateTime.Now.Year)),
                                            Times.Once());
        }

        [Test]
        public void ReadAllHiddenLists_IncorrectData_AccountIdIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => toDoListService.ReadAllHiddenLists(string.Empty, 1, 4));
            Assert.Throws<ArgumentNullException>(() => toDoListService.ReadAllHiddenLists(null!, 1, 4));
        }

        [Test]
        public void ReadAllHiddenLists_IncorrectData_AccountIdIsNoRepresentationOfGuid_ShouldThrowArgumentException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => toDoListService.ReadAllHiddenLists("Bad id", 1, 4));
        }

        [Test]
        public void ReadAllHiddenLists_IncorrectData_ListPageIsLessThanOne_ShouldThrowArgumentNullException()
        {
            // Arrange
            var toDoListId = Guid.NewGuid().ToString();

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoListService.ReadAllHiddenLists(toDoListId, 0, 4));
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoListService.ReadAllHiddenLists(toDoListId, -1, 4));
        }

        [Test]
        public void ReadAllHiddenLists_IncorrectData_PageSizeIsLessThanOne_ShouldThrowArgumentNullException()
        {
            // Arrange
            var toDoListId = Guid.NewGuid().ToString();

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoListService.ReadAllHiddenLists(toDoListId, 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => toDoListService.ReadAllHiddenLists(toDoListId, 1, -4));
        }

        [Test]
        public void ReadAllHiddenLists_IncorrectData_AllParametersAreWrong_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => toDoListService.ReadAllHiddenLists(string.Empty, 0, 0));
            Assert.Throws<ArgumentNullException>(() => toDoListService.ReadAllHiddenLists(null!, -1, -4));
        }

        [Test]
        public void ReadAllHiddenLists_CorrectData_ShouldReturnCollectionOfHiddenToDoListsDTO()
        {
            // Arrange
            var collection = this.toDoListCollection.Take(3).ToList();
            collection.ForEach(x => x.Hidden = true);

            this.mockToDoListRepository.Setup(x => x.ReadAllHiddenToDoLists(this.accountId, 1, 4)).Returns(collection);

            // Act
            var results = toDoListService.ReadAllHiddenLists(this.accountId, 1, 4);

            // Assert
            Assert.NotNull(results);
            Assert.That(results.ToDoListCollection.Count(), Is.EqualTo(3));
            Assert.IsInstanceOf<ToDoListCollectionViewModel>(results);
            this.mockToDoListRepository.Verify(x => x.ReadAllHiddenToDoLists(this.accountId, 1, 4), Times.Once());
        }
    }
}
