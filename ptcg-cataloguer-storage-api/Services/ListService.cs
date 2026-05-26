using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Cataloguer.Models;
using Cataloguer.data;
using Cataloguer.Dtos;
using Cataloguer.HelperFunctions;

namespace Cataloguer.Services;

public class ListService
{
    private readonly AppDbContext _db;
    private readonly Helpers _helpers;
    public ListService(AppDbContext db, Helpers helpers)
    {
        _db = db;
        _helpers = helpers;
    }

    // Mapping Functions
    // List output
    private static CardsListDto ToCardsListDto(CardsList list)
    {
        return new CardsListDto
        {
            ListId = list.ListId,
            ListName = list.ListName,
            CardIDs = list.CardIDs,
            IsFavourites = list.IsFavourites
        };
    }

    // List Input
    private static CardsList FromCardsListCreateDto(CardsListCreateDto dto)
    {
        return new CardsList
        {
            // ListId = dto.ListId,
            ListName = dto.ListName,
            CardIDs = dto.CardIDs,
        };
    }

    // IResults
    // List outputs
    public async Task<IResult> GetLists(Guid uid)
    {
        var user = await _helpers.matchUserID(uid);
        if (user is null) return Results.NotFound();

        var userLists = user.Lists
            .Select(ToCardsListDto)
            .ToList();

        return Results.Ok(userLists);
    }

    public async Task<IResult> GetList(Guid UserId, Guid ListId)
    {
        var user = await _helpers.matchUserID(UserId);
        if (user is null) return Results.NotFound();

        var list = await _helpers.matchListID(ListId);
        return list is not null ? Results.Ok(ToCardsListDto(list)) : Results.NotFound();
    }

    // List inputs
    // POST
    public async Task<IResult> CreateList(Guid UserId, CardsListCreateDto dto, IValidator<CardsListCreateDto> validator)
    {
        var result = validator.Validate(dto);
        if (!result.IsValid)
            return Results.BadRequest(result.Errors);

        var user = await _helpers.matchUserID(UserId);
        if (user is null) return Results.NotFound();

        var list = FromCardsListCreateDto(dto);

        // set foreign key to list
        list.UserId = UserId;

        // sets new list to not be default favourites
        list.IsFavourites = false;

        _db.Lists.Add(list);

        // write to db
        await _db.SaveChangesAsync();

        return Results.Created($"/users/{UserId}/cards/{list.ListId}", ToCardsListDto(list));
    }

    // PUT
    public async Task<IResult> UpdateListName(Guid UserId, Guid ListId, CardsListNameUpdateDto dto, IValidator<CardsListNameUpdateDto> validator)
    {
        var result = validator.Validate(dto);
        if (!result.IsValid)
            return Results.BadRequest(result.Errors);

        var user = await _helpers.matchUserID(UserId);
        if (user is null) return Results.NotFound();

        var list = await _helpers.matchListID(ListId);
        if (list is null) return Results.NotFound();

        // favourites list is a default, only the cardIds can be updated
        if (list.IsFavourites == true) return Results.StatusCode(405);

        list.ListName = dto.ListName;

        // write to db
        await _db.SaveChangesAsync();

        return Results.Ok(ToCardsListDto(list));
    }

    public async Task<IResult> UpdateList(Guid UserId, Guid ListId, CardsListUpdateDto dto)
    {
        var user = await _helpers.matchUserID(UserId);
        if (user is null) return Results.NotFound();

        var list = await _helpers.matchListID(ListId);
        if (list is null) return Results.NotFound();

        list.CardIDs = dto.CardIDs;

        // write to db
        await _db.SaveChangesAsync();

        return Results.Ok(ToCardsListDto(list));
    }

    // DELETE
    public async Task<IResult> DeleteList(Guid UserId, Guid ListId)
    {
        var user = await _helpers.matchUserID(UserId);
        if (user is null) return Results.NotFound();

        var list = await _helpers.matchListID(ListId);
        if (list is null) return Results.NotFound();

        // favourites list is a default, only the cardIds can be updated
        if (list.IsFavourites == true) return Results.StatusCode(405);

        user.Lists.Remove(list);

        // write to db
        await _db.SaveChangesAsync();

        return Results.NoContent();
    }
}