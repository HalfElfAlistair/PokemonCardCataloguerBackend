using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Cataloguer.Models;
using Cataloguer.data;
using Cataloguer.Dtos;
using Cataloguer.HelperFunctions;

namespace Cataloguer.Services;

public class UserService
{
    private readonly AppDbContext _db;
    private readonly Helpers _helpers;
    public UserService(AppDbContext db, Helpers helpers)
    {
        _db = db;
        _helpers = helpers;
    }

    // Mapping Functions
    // User Output
    private static UserDto ToUserDto(User user)
    {
        return new UserDto
        {
            UserId = user.UserId,
            Name = user.Name
        };
    }

    // User Input
    private static User FromCreateDto(UserCreateDto dto)
    {
        return new User
        {
            UserId = Guid.NewGuid(),
            Name = dto.Name,
            Cards = new List<Card>(),
            Lists = new List<CardsList>()
        };
    }

    // IResults
    // User outputs
    public async Task<IResult> GetAllUsers()
    {
        var users = await _db.Users.ToListAsync();
        var dtos = users.Select(ToUserDto).ToList();
        return Results.Ok(dtos);
    }

    public async Task<IResult> GetUser(Guid uid)
    {
        var user = await _helpers.matchUserID(uid);
        return user is not null ? Results.Ok(ToUserDto(user)) : Results.NotFound();
    }

    // User inputs
    // POST
    public async Task<IResult> CreateUser(UserCreateDto dto, IValidator<UserCreateDto> validator)
    {
        var result = validator.Validate(dto);
        if (!result.IsValid)
            return Results.BadRequest(result.Errors);

        var user = FromCreateDto(dto);

        // adds empty favourites list
        var defaultChild = new CardsList
        {
            ListId = Guid.NewGuid(),
            IsFavourites = true,
            ListName = "Favourites",
            CardIDs = new List<string>(),
            UserId = user.UserId
        };

        user.Lists.Add(defaultChild);

        await _db.Users.AddAsync(user);

        // write to db
        await _db.SaveChangesAsync();

        return Results.Created($"/users/{user.UserId}", ToUserDto(user));
    }

    // PUT
    public async Task<IResult> UpdateUser(Guid uid, UserUpdateDto dto, IValidator<UserUpdateDto> validator)
    {
        var result = validator.Validate(dto);
        if (!result.IsValid)
            return Results.BadRequest(result.Errors);

        var user = await _helpers.matchUserID(uid);
        if (user is null) return Results.NotFound();

        user.Name = dto.Name;

        // write to db
        await _db.SaveChangesAsync();

        return Results.Ok(ToUserDto(user));
    }

    // DELETE
    public async Task<IResult> DeleteUser(Guid uid)
    {
        var user = await _helpers.matchUserID(uid);
        if (user is null) return Results.NotFound();

        _db.Users.Remove(user);

        // write to db
        await _db.SaveChangesAsync();

        return Results.NoContent();
    }
}