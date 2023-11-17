using Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Repositories.Impl;

/*
 * Communicator with the Database for gamertag.
 */
public class GamertagRepository : IGamertagRepository
{
    private readonly TheTrackingFellowshipContext _context;

    public GamertagRepository(TheTrackingFellowshipContext context)
    {
        _context = context;
    }

    public async Task Create(Gamertag? gamertag)
    {
        _context.Gamertags.Add(gamertag);
        await _context.SaveChangesAsync();
    }

    public async Task<Gamertag?> GetGamertagById(ulong gamertagId)
    {
        return await _context.Gamertags.SingleOrDefaultAsync(gamertag => gamertag.GamertagId == gamertagId);
    }

    public async Task Update(Gamertag? gamertag)
    {
        _context.Gamertags.Update(gamertag);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Gamertag? gamertag)
    {
        _context.Gamertags.Remove(gamertag);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(ulong gamertagId)
    {
        var gamertag = GetGamertagById(gamertagId).Result;
        if (gamertag != null) await Delete(gamertag);
    }

    public async Task<ICollection<Gamertag?>> GetGamertagsByUser(ulong userId)
    {
        var gamertags = _context.Gamertags.Where(g => g.UserKey == userId);
        return await gamertags.ToListAsync();
    }

    public async Task<Gamertag?> GetGamertagByUserAndGame(ulong userId, GameHandler.Game game)
    {
        return await _context.Gamertags.SingleOrDefaultAsync(g => g.UserKey == userId && g.Game == game);
    }
}