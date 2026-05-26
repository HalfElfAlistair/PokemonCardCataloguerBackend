using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using Cataloguer.Models;
using Cataloguer.Dtos;
using Cataloguer.Services;
using Cataloguer.data;
using Cataloguer.HelperFunctions;
using Cataloguer.Validators;
using Microsoft.AspNetCore.Http.HttpResults;

public class ListMappingTests
{
    // Sets up a clean database for testing
    private AppDbContext CreateInMemoryDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact(Skip = "Completed")]
    public async Task GetLists_ShouldGetListsCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new ListService(db, helpers);
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();

        var user = new User
        {
            UserId = userId,
            Name = "Test User"
        };

        db.Users.Add(user);

        var listAId = Guid.NewGuid();
        var listBId = Guid.NewGuid();
        var listCId = Guid.NewGuid();

        var ListA = new CardsList
        {
            ListId = listAId,
            ListName = "Favourites",
            CardIDs = ["sv03.5-092", "sv03.5-093"],
            IsFavourites = true,
            UserId = userId,
        };

        var ListB = new CardsList
        {
            ListId = listBId,
            ListName = "Binder",
            CardIDs = ["sv03.5-094"],
            IsFavourites = false,
            UserId = userId,
        };

        var ListC = new CardsList
        {
            ListId = listCId,
            ListName = "Favourites",
            CardIDs = ["sv03.5-094"],
            IsFavourites = true,
            UserId = differentUserId,
        };

        db.Lists.Add(ListA);
        db.Lists.Add(ListB);
        db.Lists.Add(ListC);

        await db.SaveChangesAsync();

        // Act
        var result = await service.GetLists(userId);

        // Assert
        var ok = result as Ok<List<CardsListDto>>;
        ok.Should().NotBeNull();

        var lists = ok.Value;

        lists[0].ListName.Should().Be("Favourites");
        lists[0].ListId.Should().Be(listAId);
        lists[0].CardIDs.Should().HaveCount(2);
        lists[0].IsFavourites.Should().Be(true);

        lists[1].ListName.Should().Be("Binder");
        lists[1].ListId.Should().Be(listBId);
        lists[1].CardIDs.Should().HaveCount(1);
        lists[1].IsFavourites.Should().Be(false);
    }

    [Fact(Skip = "Completed")]
    public async Task GetList_ShouldGetListCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new ListService(db, helpers);
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();

        var user = new User
        {
            UserId = userId,
            Name = "Test User"
        };

        db.Users.Add(user);

        var listAId = Guid.NewGuid();
        var listBId = Guid.NewGuid();

        var ListA = new CardsList
        {
            ListId = listAId,
            ListName = "Favourites",
            CardIDs = ["sv03.5-092", "sv03.5-093"],
            IsFavourites = true,
            UserId = userId,
        };

        var ListB = new CardsList
        {
            ListId = listBId,
            ListName = "Favourites",
            CardIDs = ["sv03.5-094"],
            IsFavourites = true,
            UserId = differentUserId,
        };

        db.Lists.Add(ListA);
        db.Lists.Add(ListB);

        await db.SaveChangesAsync();

        // Act
        var result = await service.GetList(userId, listAId);

        // Assert
        var ok = result as Ok<CardsListDto>;

        ok.Should().NotBeNull();
        ok.Value.ListName.Should().Be("Favourites");
        ok.Value.ListId.Should().Be(listAId);
    }

    [Fact(Skip = "Completed")]
    public async Task CreateList_ShouldCreateListCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new ListService(db, helpers);
        var userId = Guid.NewGuid();

        var user = new User
        {
            UserId = userId,
            Name = "Test User"
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        var listId = Guid.NewGuid();

        var dto = new CardsListCreateDto
        {
            ListId = listId,
            ListName = "Binder",
            CardIDs = new() { }
        };

        var validator = new ListCreateDtoValidator();

        // Act
        var result = await service.CreateList(userId, dto, validator);

        // Assert
        var created = result as Created<CardsListDto>;
        created.Should().NotBeNull();

        created.Value.ListName.Should().Be("Binder");
        created.Value.CardIDs.Should().HaveCount(0);
        created.Value.IsFavourites.Should().Be(false);
    }

    [Fact(Skip = "Completed")]
    public async Task UpdateListName_ShouldUpdateListNameCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new ListService(db, helpers);
        var userId = Guid.NewGuid();

        var user = new User
        {
            UserId = userId,
            Name = "Test User"
        };

        db.Users.Add(user);

        var listId = Guid.NewGuid();

        var ExistingList = new CardsList
        {
            ListId = listId,
            ListName = "Old Name",
            CardIDs = ["sv03.5-092", "sv03.5-093"],
            IsFavourites = false,
            UserId = userId,
        };

        db.Lists.Add(ExistingList);

        await db.SaveChangesAsync();

        var dto = new CardsListNameUpdateDto
        {
            ListName = "New Name"
        };

        var validator = new ListNameUpdateDtoValidator();

        // Act
        var result = await service.UpdateListName(userId, listId, dto, validator);

        // Assert
        var ok = result as Ok<CardsListDto>;

        ok.Should().NotBeNull();
        ok.Value.ListName.Should().Be("New Name");
    }

    [Fact(Skip = "Completed")]
    public async Task UpdateList_ShouldUpdateListCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new ListService(db, helpers);
        var userId = Guid.NewGuid();

        var user = new User
        {
            UserId = userId,
            Name = "Test User"
        };

        db.Users.Add(user);

        var listId = Guid.NewGuid();

        var ExistingList = new CardsList
        {
            ListId = listId,
            ListName = "Binder",
            CardIDs = ["sv03.5-092"],
            IsFavourites = false,
            UserId = userId,
        };

        db.Lists.Add(ExistingList);

        await db.SaveChangesAsync();

        var dto = new CardsListUpdateDto
        {
            CardIDs = ["sv03.5-092", "sv03.5-093"]
        };

        // Act
        var result = await service.UpdateList(userId, listId, dto);

        // Assert
        var ok = result as Ok<CardsListDto>;

        ok.Should().NotBeNull();
        ok.Value.CardIDs.Should().HaveCount(2);
    }

    [Fact]
    public async Task DeleteList_ShouldDeleteListCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new ListService(db, helpers);
        var userId = Guid.NewGuid();

        var user = new User
        {
            UserId = userId,
            Name = "Test User"
        };

        db.Users.Add(user);

        var listAId = Guid.NewGuid();
        var listBId = Guid.NewGuid();

        var ListA = new CardsList
        {
            ListId = listAId,
            ListName = "Favourites",
            CardIDs = ["sv03.5-092", "sv03.5-093"],
            IsFavourites = true,
            UserId = userId,
        };

        var ListB = new CardsList
        {
            ListId = listBId,
            ListName = "Binder",
            CardIDs = ["sv03.5-094"],
            IsFavourites = false,
            UserId = userId,
        };

        db.Lists.Add(ListA);
        db.Lists.Add(ListB);

        await db.SaveChangesAsync();

        // Act
        var resultA = await service.DeleteList(userId, listAId);
        var resultB = await service.DeleteList(userId, listBId);

        // Assert
        var noContentA = resultA as NoContent;
        var noContentB = resultB as NoContent;
        noContentB.Should().NotBeNull();

        db.Lists.Any(u => u.ListId == listAId).Should().BeTrue();
        db.Lists.Any(u => u.ListId == listBId).Should().BeFalse();
    }

}