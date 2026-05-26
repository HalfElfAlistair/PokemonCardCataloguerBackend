using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Cataloguer.Models;
using Cataloguer.data;
using Cataloguer.Dtos;
using Cataloguer.HelperFunctions;

namespace Cataloguer.Services;

public class CardService
{
    private readonly AppDbContext _db;
    private readonly Helpers _helpers;
    public CardService(AppDbContext db, Helpers helpers)
    {
        _db = db;
        _helpers = helpers;
    }

    // Mapping Functions
    // Card output
    private static CardDto ToCardDto(Card card)
    {
        return new CardDto
        {
            CardId = card.CardId,
            CardName = card.CardName,
            Count = card.Count
        };
    }

    // Card Input
    private static Card FromCardCreateDto(CardCreateDto dto)
    {
        return new Card
        {
            CardId = dto.CardId,
            CardName = dto.CardName,
            Count = dto.Count,
            Illustrator = dto.Illustrator
        };
    }

    // IResults
    // Card outputs
    public async Task<IResult> GetCards(Guid uid)
    {
        var user = await _helpers.matchUserID(uid);
        if (user is null) return Results.NotFound();

        var userCards = user.Cards
            .Select(ToCardDto)
            .ToList();

        return Results.Ok(userCards);
    }

    public async Task<IResult> GetCard(Guid UserId, string CardId)
    {
        var user = await _helpers.matchUserID(UserId);
        if (user is null) return Results.NotFound();

        var card = await _helpers.matchCardID(CardId);
        return card is not null ? Results.Ok(ToCardDto(card)) : Results.NotFound();
    }

    // Card inputs
    // POST
    public async Task<IResult> AddCard(Guid UserId, CardCreateDto dto, IValidator<CardCreateDto> validator)
    {
        var result = validator.Validate(dto);
        if (!result.IsValid)
            return Results.BadRequest(result.Errors);

        var user = await _helpers.matchUserID(UserId);
        if (user is null) return Results.NotFound();

        // creates card entity from DTO
        var card = FromCardCreateDto(dto);

        // set foreign key to card
        card.UserId = UserId;

        var existingCard = user.Cards
            .FirstOrDefault(c => c.CardId == card.CardId);

        if (existingCard != null)
            return Results.StatusCode(405);

        // Checks to see if card is already present, prevents adding if so
        var existingCardMatch = await _helpers.matchCardID(card.CardId);
        if (existingCardMatch is not null) return Results.StatusCode(405);

        _db.Cards.Add(card);

        // write to db
        await _db.SaveChangesAsync();

        return Results.Created($"/users/{UserId}/cards/{card.CardId}", ToCardDto(card));
    }

    // PUT
    public async Task<IResult> UpdateCard(Guid UserId, string CardId, CardUpdateDto dto, IValidator<CardUpdateDto> validator)
    {
        var result = validator.Validate(dto);
        if (!result.IsValid)
            return Results.BadRequest(result.Errors);

        var user = await _helpers.matchUserID(UserId);
        if (user is null) return Results.NotFound();

        var card = await _helpers.matchCardID(CardId);
        if (card is null) return Results.NotFound();

        card.Count = dto.Count;

        // write to db
        await _db.SaveChangesAsync();

        return Results.Ok(ToCardDto(card));
    }

    // DELETE
    public async Task<IResult> DeleteCard(Guid UserId, string CardId)
    {
        var user = await _helpers.matchUserID(UserId);
        if (user is null) return Results.NotFound();

        var card = await _helpers.matchCardID(CardId);
        if (card is null) return Results.NotFound();

        user.Cards.Remove(card);

        // write to db
        await _db.SaveChangesAsync();

        return Results.NoContent();
    }
}