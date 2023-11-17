using Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Repositories.Impl;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly TheTrackingFellowshipContext _context;

    public RefreshTokenRepository(TheTrackingFellowshipContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken?> GetRefreshTokenExchangeByUserId(ulong userId)
    {
        return await _context.RefreshTokens.SingleOrDefaultAsync(item => item.UserId == userId);
    }

    public async Task Create(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task Update(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenExchangeByJwtTokenRefreshKey(string jwtToken,
        string refreshTokenKey)
    {
        return await _context.RefreshTokens.SingleOrDefaultAsync(item =>
            item.Token == jwtToken && item.RefreshKey == refreshTokenKey);
    }

    public async Task Delete(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync();
    }
}