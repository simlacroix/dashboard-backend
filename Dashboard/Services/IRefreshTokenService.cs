using Dashboard.Models;

namespace Dashboard.Services;

public interface IRefreshTokenService
{
    Task<RefreshTokenExchange> GenerateRefreshTokenExchange(string username, ulong userId);

    Task<RefreshTokenExchange> RefreshToken(string username, ulong userId,
        RefreshTokenExchange inputRefreshTokenExchange);

    Task<ulong> GetRefreshTokenUserId(string jwtToken, string refreshTokenKey);
    Task Logout(RefreshTokenExchange refreshTokenExchange);
}