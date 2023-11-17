using Dashboard.Models;

namespace Dashboard.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetRefreshTokenExchangeByUserId(ulong userId);
    Task Create(RefreshToken refreshToken);
    Task Update(RefreshToken refreshToken);
    Task<RefreshToken?> GetRefreshTokenExchangeByJwtTokenRefreshKey(string jwtToken, string refreshTokenKey);
    Task Delete(RefreshToken refreshToken);
}