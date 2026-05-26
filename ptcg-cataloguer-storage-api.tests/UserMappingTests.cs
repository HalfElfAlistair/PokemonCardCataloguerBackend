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

public class UserMappingTests
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
    public async Task GetAllUsers_ShouldGetUsersCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new UserService(db, helpers);

        var userAId = Guid.NewGuid();
        var userBId = Guid.NewGuid();

        db.Users.Add(new User { UserId = userAId, Name = "User A" });
        db.Users.Add(new User { UserId = userBId, Name = "User B" });

        await db.SaveChangesAsync();

        // Act
        var result = await service.GetAllUsers();

        // Assert
        var ok = result as Ok<List<UserDto>>;
        ok.Should().NotBeNull();

        var users = ok.Value;

        users.Should().HaveCount(2);

        users[0].Name.Should().Be("User A");
        users[0].UserId.Should().Be(userAId);

        users[1].Name.Should().Be("User B");
        users[1].UserId.Should().Be(userBId);
    }

    [Fact(Skip = "Completed")]
    public async Task GetUser_ShouldGetUserCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new UserService(db, helpers);

        var UserId = Guid.NewGuid();

        db.Users.Add(new User { UserId = UserId, Name = "Test User" });

        await db.SaveChangesAsync();

        // Act
        var result = await service.GetUser(UserId);

        // Assert
        var ok = result as Ok<UserDto>;

        ok.Should().NotBeNull();
        ok.Value.Name.Should().Be("Test User");
        ok.Value.UserId.Should().Be(UserId);
    }

    [Fact(Skip = "Completed")]
    public async Task CreateUser_ShouldCreateUserCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new UserService(db, helpers);

        var dto = new UserCreateDto
        {
            Name = "Test User"
        };

        var validator = new UserCreateDtoValidator();

        // Act
        var result = await service.CreateUser(dto, validator);

        // Assert
        var created = result as Created<UserDto>;
        created.Should().NotBeNull();

        created.Value.Name.Should().Be("Test User");
        created.Value.UserId.Should().NotBe(Guid.Empty);
    }

    [Fact(Skip = "Completed")]
    public async Task UpdateUser_ShouldUpdateUserCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new UserService(db, helpers);

        var UserId = Guid.NewGuid();

        db.Users.Add(new User { UserId = UserId, Name = "Test User" });

        await db.SaveChangesAsync();

        var dto = new UserUpdateDto
        {
            Name = "Updated User"
        };

        var validator = new UserUpdateDtoValidator();

        // Act
        var result = await service.UpdateUser(UserId, dto, validator);

        // Assert
        var ok = result as Ok<UserDto>;
        ok.Should().NotBeNull();

        ok.Value.Name.Should().Be("Updated User");
    }

    [Fact(Skip = "Completed")]
    public async Task DeleteUser_ShouldDeleteUserCorrectly()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new UserService(db, helpers);

        var UserId = Guid.NewGuid();

        db.Users.Add(new User { UserId = UserId, Name = "Test User" });

        await db.SaveChangesAsync();

        // Act
        var result = await service.DeleteUser(UserId);

        // Assert
        var noContent = result as NoContent;
        noContent.Should().NotBeNull();

        db.Users.Any(u => u.UserId == UserId).Should().BeFalse();
    }

    [Fact(Skip = "Completed")]
    public async Task DeleteUser_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var db = CreateInMemoryDb();
        var helpers = new Helpers(db);
        var service = new UserService(db, helpers);

        var missingId = Guid.NewGuid();

        // Act
        var result = await service.DeleteUser(missingId);

        // Assert
        var notFound = result as NotFound;
        notFound.Should().NotBeNull();
    }
}