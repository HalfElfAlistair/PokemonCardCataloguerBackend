using Microsoft.EntityFrameworkCore;
using Cataloguer.Models;
using Cataloguer.data;

namespace Cataloguer.HelperFunctions;

public class Helpers
{
    private readonly AppDbContext _db;
    public Helpers(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> matchUserID(Guid UserId)
    {
        return await _db.Users
                        .Include(u => u.Cards)
                        .Include(u => u.Lists)
                        .FirstOrDefaultAsync(u => u.UserId == UserId);
    }

    public async Task<Card?> matchCardID(string CardId)
    {
        return await _db.Cards.FirstOrDefaultAsync(c => c.CardId == CardId);
    }

    public async Task<CardsList?> matchListID(Guid ListId)
    {
        return await _db.Lists.FirstOrDefaultAsync(l => l.ListId == ListId);
    }
}