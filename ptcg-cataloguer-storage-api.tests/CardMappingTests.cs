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

public class CardMappingTests
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
    public async Task GetCards_ShouldGetCardsCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new CardService(db, helpers);
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();

        var user = new User
        {
            UserId = userId,
            Name = "Test User"
        };

        db.Users.Add(user);

        var CardA = new Card
        {
            CardId = "sv03.5-092",
            CardName = "Gastly",
            Count = 2,
            UserId = userId,
        };

        var CardB = new Card
        {
            CardId = "sv03.5-093",
            CardName = "Haunter",
            Count = 1,
            UserId = userId,
        };

        var CardC = new Card
        {
            CardId = "sv03.5-094",
            CardName = "Gengar",
            Count = 1,
            UserId = differentUserId,
        };

        db.Cards.Add(CardA);
        db.Cards.Add(CardB);
        db.Cards.Add(CardC);

        await db.SaveChangesAsync();

        // Act
        var result = await service.GetCards(userId);

        // Assert
        var ok = result as Ok<List<CardDto>>;
        ok.Should().NotBeNull();

        var cards = ok.Value;

        cards.Should().HaveCount(2);

        cards[0].CardName.Should().Be("Gastly");
        cards[0].CardId.Should().Be("sv03.5-092");

        cards[1].CardName.Should().Be("Haunter");
        cards[1].CardId.Should().Be("sv03.5-093");
    }

    [Fact(Skip = "Completed")]

    public async Task GetCard_ShouldGetCardCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new CardService(db, helpers);
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();

        var user = new User
        {
            UserId = userId,
            Name = "Test User"
        };

        db.Users.Add(user);

        var CardA = new Card
        {
            CardId = "sv03.5-092",
            CardName = "Gastly",
            Count = 2,
            UserId = userId,
        };

        var CardB = new Card
        {
            CardId = "sv03.5-093",
            CardName = "Haunter",
            Count = 1,
            UserId = userId,
        };

        var CardC = new Card
        {
            CardId = "sv03.5-094",
            CardName = "Gengar",
            Count = 1,
            UserId = differentUserId,
        };

        db.Cards.Add(CardA);
        db.Cards.Add(CardB);
        db.Cards.Add(CardC);

        await db.SaveChangesAsync();

        // Act
        var result = await service.GetCard(userId, "sv03.5-092");

        // Assert
        var ok = result as Ok<CardDto>;

        ok.Should().NotBeNull();
        ok.Value.CardName.Should().Be("Gastly");
        ok.Value.CardId.Should().Be("sv03.5-092");
    }

    [Fact(Skip = "Completed")]
    public async Task AddCard_ShouldCreateCardCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new CardService(db, helpers);
        var userId = Guid.NewGuid();

        var user = new User
        {
            UserId = userId,
            Name = "Test User"
        };

        db.Users.Add(user);
        await db.SaveChangesAsync();

        var dto = new CardCreateDto
        {
            CardId = "sv03.5-092",
            CardName = "Gastly",
            Count = 1
        };

        var validator = new CardCreateDtoValidator();

        // Act
        var result = await service.AddCard(userId, dto, validator);

        // Assert
        var created = result as Created<CardDto>;
        created.Should().NotBeNull();

        created!.Value.CardName.Should().Be("Gastly");
        created.Value.Count.Should().Be(1);
    }

    [Fact(Skip = "Completed")]
    public async Task UpdateCard_ShouldUpdateCardCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new CardService(db, helpers);
        var userId = Guid.NewGuid();

        var user = new User
        {
            UserId = userId,
            Name = "Test User"
        };

        db.Users.Add(user);

        var CardA = new Card
        {
            CardId = "sv03.5-092",
            CardName = "Gastly",
            Count = 2,
            UserId = userId,
        };

        db.Cards.Add(CardA);

        await db.SaveChangesAsync();

        var dto = new CardUpdateDto
        {
            Count = 2
        };

        var validator = new CardUpdateDtoValidator();

        // Act
        var result = await service.UpdateCard(userId, "sv03.5-092", dto, validator);

        // Assert
        var ok = result as Ok<CardDto>;
        ok.Should().NotBeNull();

        ok.Value.Count.Should().Be(2);
    }

    [Fact(Skip = "Completed")]
    public async Task DeleteCard_ShouldDeleteCardCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new CardService(db, helpers);
        var userId = Guid.NewGuid();
        var differentUserId = Guid.NewGuid();

        var user = new User
        {
            UserId = userId,
            Name = "Test User"
        };

        db.Users.Add(user);

        var CardA = new Card
        {
            CardId = "sv03.5-092",
            CardName = "Gastly",
            Count = 2,
            UserId = userId,
        };

        db.Cards.Add(CardA);

        await db.SaveChangesAsync();

        // Act
        var result = await service.DeleteCard(userId, "sv03.5-092");

        // Assert
        var noContent = result as NoContent;
        noContent.Should().NotBeNull();

        db.Cards.Any(u => u.CardId == "sv03.5-092").Should().BeFalse();
    }
}