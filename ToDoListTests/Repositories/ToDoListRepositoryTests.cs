using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListCore.Domain_Models;
using ToDoListInfrastructure.Database;
using ToDoListInfrastructure.Models.Repositories;

namespace ToDoListTests.Repositories
{
    [TestFixture]
    internal class ToDoListRepositoryTests
    {
        private readonly ToDoListAppDbContext context;
        private ToDoListRepository toDoListRepository;
        private string accountId;
        private List<ToDoList> toDoListCollection;

        public ToDoListRepositoryTests()
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
            this.toDoListRepository = new ToDoListRepository(this.context);
            this.accountId = Guid.NewGuid().ToString();
            this.toDoListCollection = new List<ToDoList>()
            {
                new ToDoList()
                {
                    Title = "Tdl 1",
                    ToDoEntries = new List<ToDoEntry>(),
                    AccountId = this.accountId
                },
                new ToDoList()
                {
                    Title = "Tdl 2",
                    ToDoEntries = new List<ToDoEntry>(),
                    AccountId = this.accountId
                },
                new ToDoList()
                {
                    Title = "Tdl 3",
                    ToDoEntries = new List<ToDoEntry>(),
                    AccountId = this.accountId
                },
                new ToDoList()
                {
                    Title = "Tdl 4",
                    ToDoEntries = new List<ToDoEntry>(),
                    AccountId = this.accountId
                },
                new ToDoList()
                {
                    Title = "Tdl 5",
                    ToDoEntries = new List<ToDoEntry>(),
                    AccountId = Guid.NewGuid().ToString()
                },
            };

        }

        [Test]
        public void TestConstructor_dbContextIsNull_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new ToDoListRepository(null!));
        }

        [Test]
        public void ReadAllNotHiddenToDoLists_IncorrectData_AccountIdIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.ReadAllNotHiddenToDoLists(string.Empty, 1, 4));
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.ReadAllNotHiddenToDoLists(null!, 1, 4));
        }

        [Test]
        public void ReadAllNotHiddenToDoLists_IncorrectData_AccountIdIsNotRepresentationOfGuid_ShouldThrowArgumentException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => this.toDoListRepository.ReadAllNotHiddenToDoLists("text", 1, 4));        
        }

        [Test]
        public void ReadAllNotHiddenToDoLists_IncorrectData_listPageIsLessThan1_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListRepository.ReadAllNotHiddenToDoLists(this.accountId, 0, 4));
        }

        [Test]
        public void ReadAllNotHiddenToDoLists_IncorrectData_pageSizeIsLessThan1_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListRepository.ReadAllNotHiddenToDoLists(this.accountId, 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListRepository.ReadAllNotHiddenToDoLists(this.accountId, 1, -3));
        }

        [Test]
        public void ReadAllNotHiddenToDoLists_IncorrectData_AllDataAreWrong_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.ReadAllNotHiddenToDoLists(string.Empty, 0, 0));
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.ReadAllNotHiddenToDoLists(null!, 0, 0));
        }

        [Test]
        public void ReadAllNotHiddenToDoLists_IncorrectData_CorrectAccountID_IncorrectPageSizeAndListPage_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListRepository.ReadAllNotHiddenToDoLists(this.accountId, 0, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListRepository.ReadAllNotHiddenToDoLists(this.accountId, 0, 0));
        }

        [Test]
        public void ReadAllNotHiddenToDoLists_CorrectData_ShouldReturnCollectionOfToDoLists()
        {
            // Arrange
            this.context.ToDoLists.AddRange(this.toDoListCollection);
            this.context.SaveChanges();

            var expected = this.toDoListCollection.Where(x => x.AccountId == this.accountId).ToList();

            // Act
            var result = this.toDoListRepository.ReadAllNotHiddenToDoLists(this.accountId, 1, 4);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(expected.Count));
            Assert.True(result.All(x => !x.Hidden));
        }

        [Test]
        public void ReadAllNotHiddenToDoLists_CorrectData_ThereAreLessToDoListsThanPageSize_ShouldReturnCollectionOfTwoToDoLists()
        {
            // Arrange
            this.context.ToDoLists.AddRange(this.toDoListCollection.Take(2));
            this.context.SaveChanges();

            var expectedCount = 2;

            // Act
            var result = this.toDoListRepository.ReadAllNotHiddenToDoLists(this.accountId, 1, 4);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(expectedCount));
        }

        [Test]
        public void ReadAllNotHiddenToDoLists_CorrectData_NoToDoListsExists_ShouldReturnEmptyCollection()
        {
            // Act
            var result = this.toDoListRepository.ReadAllNotHiddenToDoLists(this.accountId, 1, 4);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ReadToDoList_IncorrectData_GivenIdIsGuidEmpty_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListRepository.ReadToDoList(Guid.Empty));
        }

        [Test]
        public void ReadToDoList_CorrectData_OnlyOneToDoListInDatabase()
        {
            // Arrange
            this.context.ToDoLists.Add(this.toDoListCollection[0]);
            this.context.SaveChanges();

            var ToDoListID = this.context.ToDoLists.First(x => x.Title == this.toDoListCollection[0].Title).Id;

            // Act
            var result = this.toDoListRepository.ReadToDoList(ToDoListID);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Title, Is.EqualTo(this.toDoListCollection[0].Title));
        }

        [Test]
        public void ReadToDoList_CorrectData_ThisToDoListDoesNotExists_ShouldReturnNull()
        {
            // Act
            var result = this.toDoListRepository.ReadToDoList(Guid.NewGuid());

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void ReadToDoList_CorrectData_ThereIsMoreThanOneToDoList_ShouldReturnToDoListWithSpecificID()
        {
            // Arrange
            var tdlCollection = this.toDoListCollection.Take(2).ToList();

            this.context.ToDoLists.AddRange(tdlCollection);
            this.context.SaveChanges();

            var firstToDoListID = this.context.ToDoLists.First(x => x.Title == tdlCollection[0].Title).Id;

            // Act
            var result = this.toDoListRepository.ReadToDoList(firstToDoListID);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Title, Is.EqualTo(tdlCollection[0].Title));
        }

        [Test]
        public void CreateToDoList_IncorrectData_ToDoListIsNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CreateToDoList(null!));
        }

        [Test]
        public void CreateToDoList_IncorrectData_AccountIdIsNullOrEmpty()
        {
            // Arrange
            var toDoListToAdd = this.toDoListCollection[0];
            toDoListToAdd.AccountId = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CreateToDoList(toDoListToAdd));
            toDoListToAdd.AccountId = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CreateToDoList(toDoListToAdd));
        }

        [Test]
        public void CreateToDoList_IncorrectData_TitleIsNullOrEmpty()
        {
            // Arrange
            var toDoListToAdd = this.toDoListCollection[0];
            toDoListToAdd.Title = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CreateToDoList(toDoListToAdd));
            toDoListToAdd.Title = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CreateToDoList(toDoListToAdd));
        }

        [Test]
        public void CreateToDoList_IncorrectData_AllDataAreWrong_ShouldThrowArgumentNullException()
        {
            // Arrange
            var toDoListToAdd = this.toDoListCollection[0];
            toDoListToAdd.AccountId = string.Empty;
            toDoListToAdd.Title = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CreateToDoList(toDoListToAdd));
            toDoListToAdd.AccountId = null!;
            toDoListToAdd.Title = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CreateToDoList(toDoListToAdd));
        }

        [Test]
        public void CreateToDoList_IncorrectData_TitleIsTooLong()
        {
            // Arrange
            var toDoListToAdd = this.toDoListCollection[0];
            toDoListToAdd.Title = new string('!', 101); // one character too much.

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListRepository.CreateToDoList(toDoListToAdd));
        }

        [Test]
        public void CreateToDoList_CorrectData_ShouldAddToDoListToDatabase()
        {
            // Arrange
            var toDoListToAdd = this.toDoListCollection[0];

            // Act
            this.toDoListRepository.CreateToDoList(toDoListToAdd);
            var result = this.context.ToDoLists.FirstOrDefault()!;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.AccountId, Is.EqualTo(toDoListToAdd.AccountId));
            Assert.That(result.Title, Is.EqualTo(toDoListToAdd.Title));
        }

        [Test]
        public void UpdateToDoList_IncorrectData_ToDoListIsNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.UpdateToDoList(null!));
        }

        [Test]
        public void UpdateToDoList_IncorrectData_AccountOfToDoListIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var toDoListToUpdate = this.toDoListCollection[0];
            toDoListToUpdate.AccountId = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.UpdateToDoList(toDoListToUpdate));
            toDoListToUpdate.AccountId = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.UpdateToDoList(toDoListToUpdate));
        }

        [Test]
        public void UpdateToDoList_IncorrectData_TitleOfToDoListIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Arrange
            var toDoListToUpdate = this.toDoListCollection[0];
            toDoListToUpdate.Title = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.UpdateToDoList(toDoListToUpdate));
            toDoListToUpdate.Title = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.UpdateToDoList(toDoListToUpdate));
        }

        [Test]
        public void UpdateToDoList_IncorrectData_AllDataAreWrong_ShouldThrowArgumentNullException()
        {
            // Arrange
            var toDoListToUpdate = this.toDoListCollection[0];
            toDoListToUpdate.AccountId = string.Empty;
            toDoListToUpdate.Title = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.UpdateToDoList(toDoListToUpdate));
            toDoListToUpdate.AccountId = null!;
            toDoListToUpdate.Title = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.UpdateToDoList(toDoListToUpdate));
        }

        [Test]
        public void UpdateToDoList_IncorrectData_TitleIsTooLong()
        {
            // Arrange
            var toDoListToAdd = this.toDoListCollection[0];
            toDoListToAdd.Id = Guid.NewGuid();
            toDoListToAdd.Title = new string('!', 101); // one character too much.

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListRepository.UpdateToDoList(toDoListToAdd));
        }

        [Test]
        public void UpdateToDoList_CorrectData_ShouldModifyToDoList()
        {
            // Arrange
            var toDoList = this.toDoListCollection[0];

            this.context.ToDoLists.Add(toDoList);
            this.context.SaveChanges();

            var toDoListFromDbWithId = this.context.ToDoLists.First(x => x.AccountId == toDoList.AccountId);
            var newTitle = "Updated Title";
            toDoListFromDbWithId.Title = newTitle;

            // Act
            this.toDoListRepository.UpdateToDoList(toDoListFromDbWithId);

            var result = this.context.ToDoLists.FirstOrDefault(x => x.AccountId == this.accountId);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Title, Is.EqualTo(newTitle));
        }

        [Test]
        public void DeleteToDoList_IncorrectData_ToDoListIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            ToDoList toDoListToDelete = null!;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.DeleteToDoList(toDoListToDelete));
        }

        [Test]
        public void DeleteToDoList_IncorrectData_AccountIdsNullOrEmpty_ShouldReturnArgumentNullException()
        {
            // Arrange
            var toDoListToUpdate = this.toDoListCollection[0];

            toDoListToUpdate.AccountId = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.DeleteToDoList(toDoListToUpdate));
            toDoListToUpdate.AccountId = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.DeleteToDoList(toDoListToUpdate));
        }

        [Test]
        public void DeleteToDoList_IncorrectData_TitleisNullOrEmpty_ShouldReturnArgumentNullException()
        {
            // Arrange
            var toDoListToUpdate = this.toDoListCollection[0];

            toDoListToUpdate.Title = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.DeleteToDoList(toDoListToUpdate));
            toDoListToUpdate.ToDoEntries = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.DeleteToDoList(toDoListToUpdate));
        }

        [Test]
        public void DeleteToDoList_IncorrectData_AllParamatersAreWrong()
        {
            // Arrange
            var toDoListToUpdate = this.toDoListCollection[0];
            toDoListToUpdate.AccountId = string.Empty;
            toDoListToUpdate.Title = string.Empty;

            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.DeleteToDoList(toDoListToUpdate));
            toDoListToUpdate.ToDoEntries = null!;
            toDoListToUpdate.AccountId = null!;
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.DeleteToDoList(toDoListToUpdate));
        }

        [Test]
        public void DeleteToDoList_CorrectData_ShouldRemoveToDoListFromDatabase()
        {
            // Arrange
            var todoListsToAdd = this.toDoListCollection.Take(2).ToList();

            this.context.ToDoLists.AddRange(todoListsToAdd);
            this.context.SaveChanges();

            var toDoListFromDbToRemove = this.context.ToDoLists.First(x => x.Title == todoListsToAdd[1].Title);

            // Act
            this.toDoListRepository.DeleteToDoList(toDoListFromDbToRemove);

            var collectionFromDB = this.context.ToDoLists.ToList();
            var getDeletedObjectResult = this.context.ToDoLists.FirstOrDefault(x => x.Id == toDoListFromDbToRemove.Id);

            // Assert
            Assert.That(collectionFromDB.Count, Is.EqualTo(todoListsToAdd.Count - 1));
            Assert.IsNull(getDeletedObjectResult);
        }

        [Test]
        public void CountNumberOfCopiesForToDoList_IncorrectData_toDoListTitleIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CountNumberOfCopiesForToDoList(string.Empty, this.accountId));
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CountNumberOfCopiesForToDoList(null!, this.accountId));
        }


        [Test]
        public void CountNumberOfCopiesForToDoList_IncorrectData_AccountIdIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CountNumberOfCopiesForToDoList("title", string.Empty));
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CountNumberOfCopiesForToDoList("title", null!));
        }

        [Test]
        public void CountNumberOfCopiesForToDoList_IncorrectData_AccountIdIsNotRepresentationOfGuid_ShouldThrowArgumentException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => this.toDoListRepository.CountNumberOfCopiesForToDoList("title", "text"));
        }

        [Test]
        public void CountNumberOfCopiesForToDoList_IncorrectData_AllParametersAreWrong_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CountNumberOfCopiesForToDoList(string.Empty, string.Empty));
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CountNumberOfCopiesForToDoList(null!, null!));
        }

        [Test]
        public void CountNumberOfCopiesForToDoList_CorrectData_ExistsThreeCopies_ShouldReturn3()
        {
            // Arrange
            var toDoList = this.toDoListCollection[0];

            var toDoList2 = this.toDoListCollection[1];
            toDoList2.Title = "Tdl 1 -Copy 1";

            var toDoList3 = this.toDoListCollection[2];
            toDoList3.Title = "Tdl 1 -Copy 2";

            this.context.ToDoLists.AddRange(new ToDoList[] { toDoList, toDoList2, toDoList3 });
            this.context.SaveChanges();

            // Act
            var result = this.toDoListRepository.CountNumberOfCopiesForToDoList("Tdl 1", this.accountId);

            // Assert
            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void CountNumberOfCopiesFotToDoList_CorrectData_NoCopiesExists_ShouldReturnZERO()
        {
            // Act
            var result = this.toDoListRepository.CountNumberOfCopiesForToDoList("Tdl 1", Guid.NewGuid().ToString());

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void ReadAllHiddenToDoLists_IncorrectData_AccountIdIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.ReadAllHiddenToDoLists(string.Empty, 1, 4));
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.ReadAllHiddenToDoLists(null!, 1, 4));
        }

        [Test]
        public void ReadAllHiddenToDoLists_IncorrectData_AccountIdIsNotRepresentationOfGuid_ShouldThrowArgumentException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => this.toDoListRepository.ReadAllHiddenToDoLists("text", 1, 4));        
        }

        [Test]
        public void ReadAllHiddenToDoLists_IncorrectData_ListPageIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListRepository.ReadAllHiddenToDoLists(this.accountId, 0, 4));
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListRepository.ReadAllHiddenToDoLists(this.accountId, -1, 4));
        }

        [Test]
        public void ReadAllHiddenToDoLists_IncorrectData_PageSizeIsLessThanOne_ShouldThrowArgumentOutOfRangeException()
        {
            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListRepository.ReadAllHiddenToDoLists(this.accountId, 1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => this.toDoListRepository.ReadAllHiddenToDoLists(this.accountId, 1, -4));
        }

        [Test]
        public void ReadAllHiddenToDoLists_IncorrectData_AllParametersAreWrong_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.ReadAllHiddenToDoLists(string.Empty, -1, 0));
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.ReadAllHiddenToDoLists(null!, -1, -2));
        }

        [Test]
        public void ReadAllHiddenToDoLists_CorrectData_UserHasNoToDoLists_ShouldThrowEmptyCollection() 
        {
            // Act
            var result = this.toDoListRepository.ReadAllHiddenToDoLists(this.accountId, 1, 4);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ReadAllHiddenToDoLists_CorrectData_UserHas2visibleToDoListsAnd3HiddenToDoList_ShouldReturnThreeElementsCollectionOfHiddenToDoLists()
        {
            // Arrange
            var toDoListCollectionForUser = this.toDoListCollection;
            toDoListCollectionForUser[4].AccountId = this.accountId;
            toDoListCollectionForUser[0].Hidden = true;
            toDoListCollectionForUser[1].Hidden = true;
            toDoListCollectionForUser[2].Hidden = true;

            this.context.ToDoLists.AddRange(toDoListCollectionForUser);
            this.context.SaveChanges();

            // Act
            var result = this.toDoListRepository.ReadAllHiddenToDoLists(this.accountId, 1, 4);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result.All(x => x.Hidden));
            Assert.IsInstanceOf<IEnumerable<ToDoList>>(result);
        }

        [Test]
        public void ReadAllHiddenToDoLists_CorrectData_UserHasOnlyVisibleToDoLists_ShouldReturnEmptyCollectionOfToDoLists()
        {
            // Arrange
            var toDoListCollectionForUser = this.toDoListCollection;
            toDoListCollectionForUser[4].AccountId = this.accountId;

            this.context.ToDoLists.AddRange(toDoListCollectionForUser);
            this.context.SaveChanges();

            // Act
            var result = this.toDoListRepository.ReadAllHiddenToDoLists(this.accountId, 1, 4);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(0));
            Assert.IsInstanceOf<IEnumerable<ToDoList>>(result);
        }

        [Test]
        public void CountNotHiddenUserToDoLists_IncorrectData_AccountIdIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CountNotHiddenUserToDoLists(string.Empty));
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CountNotHiddenUserToDoLists(null!));
        }

        [Test]
        public void CountNotHiddenUserToDoLists_IncorrectData_AccountIdIsNotRepresentationOfGuid_ShouldThrowArgumentException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => this.toDoListRepository.CountNotHiddenUserToDoLists("normal string"));
        }

        [Test]
        public void CountNotHiddenUserToDoLists_CorrectData_TwoToDoListsExistForThatUserAndOneForOtherUser_ShouldReturnTwo()
        {
            // Arrange
            var toDoListCollectionForTest = this.toDoListCollection.Take(3).ToList();
            toDoListCollectionForTest[2].AccountId = Guid.NewGuid().ToString();

            this.context.ToDoLists.AddRange(toDoListCollectionForTest);
            this.context.SaveChanges();

            // Act
            var result = this.toDoListRepository.CountNotHiddenUserToDoLists(this.accountId);

            // Assert
            Assert.IsInstanceOf<int>(result);
            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public void CountNotHiddenUserToDoLists_CorrectData_FourToDoListExistsForThatUser_OnlyThatUserExists_ShouldReturnFour()
        {
            // Arrange
            var toDoListCollectionForTest = this.toDoListCollection.Take(4);

            this.context.ToDoLists.AddRange(toDoListCollectionForTest);
            this.context.SaveChanges();

            // Act
            var result = this.toDoListRepository.CountNotHiddenUserToDoLists(this.accountId);

            // Assert
            Assert.IsInstanceOf<int>(result);
            Assert.That(result, Is.EqualTo(4));
        }

        [Test]
        public void CountNotHiddenUserToDoLists_CorrectData_NoToDoListInDb_ShouldreturnZero()
        {
            // Act
            var result = this.toDoListRepository.CountNotHiddenUserToDoLists(this.accountId);

            // Assert
            Assert.IsInstanceOf<int>(result);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void CountHiddenUserToDoLists_IncorrectData_AccountIdIsNullOrEmpty_ShouldThrowArgumentNullException()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CountHiddenUserToDoLists(string.Empty));
            Assert.Throws<ArgumentNullException>(() => this.toDoListRepository.CountHiddenUserToDoLists(null!));
        }

        [Test]
        public void CountHiddenUserToDoLists_IncorrectData_AccountIdIsNotRepresentationOfGuid_ShouldThrowArgumentException()
        {
            // Assert
            Assert.Throws<ArgumentException>(() => this.toDoListRepository.CountHiddenUserToDoLists("normal string"));
        }

        [Test]
        public void CountHiddenUserToDoLists_ThreeToDoListsExistForThatUserAndOneForOtherUser_ShouldReturnThree()
        {
            // Arrange
            var toDoListCollectionForTest = this.toDoListCollection.Take(4).ToList();
            toDoListCollectionForTest[0].Hidden = true;
            toDoListCollectionForTest[1].Hidden = true;
            toDoListCollectionForTest[2].Hidden = true;

            toDoListCollectionForTest[3].AccountId = Guid.NewGuid().ToString();
            toDoListCollectionForTest[3].Hidden = true;
            this.context.ToDoLists.AddRange(toDoListCollectionForTest);
            this.context.SaveChanges();

            // Act
            var result = this.toDoListRepository.CountHiddenUserToDoLists(this.accountId);

            // Assert
            Assert.IsInstanceOf<int>(result);
            Assert.That(result, Is.EqualTo(3));
        }

        [Test]
        public void CountHiddenUserToDoLists_FourToDoListExistsForThatUser_OnlyThatUserExists_ShouldReturnFour()
        {
            // Arrange
            var toDoListCollectionForTest = this.toDoListCollection.Take(4).ToList();
            toDoListCollectionForTest.ForEach(x => x.Hidden = true);

            this.context.ToDoLists.AddRange(toDoListCollectionForTest);
            this.context.SaveChanges();

            // Act
            var result = this.toDoListRepository.CountHiddenUserToDoLists(this.accountId);

            // Assert
            Assert.IsInstanceOf<int>(result);
            Assert.That(result, Is.EqualTo(4));
        }

        [Test]
        public void CountHiddenUserToDoLists_UserHas3HiddenAnd2VisibleToDoLists_ShouldReturn3()
        {
            // Arrange
            var toDoListCollectionForTest = this.toDoListCollection.ToList();
            toDoListCollectionForTest[4].AccountId = this.accountId;

            toDoListCollectionForTest[0].Hidden = true;
            toDoListCollectionForTest[1].Hidden = true;
            toDoListCollectionForTest[2].Hidden = true;


            this.context.ToDoLists.AddRange(toDoListCollectionForTest);
            this.context.SaveChanges();

            // Act
            var result = this.toDoListRepository.CountHiddenUserToDoLists(this.accountId);

            // Assert
            Assert.IsInstanceOf<int>(result);
            Assert.That(result, Is.EqualTo(3));
        }

        [TearDown]
        public void CleanContext()
        {
            this.context.Database.EnsureDeleted();
        }
    }
}
