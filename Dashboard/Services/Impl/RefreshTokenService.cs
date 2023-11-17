using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dashboard.Models;
using Dashboard.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dashboard.Services.Impl;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenService(IOptions<JwtSettings> options, IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _jwtSettings = options.Value;
    }

    public async Task<RefreshTokenExchange> GenerateRefreshTokenExchange(string username, ulong userId)
    {
        var userRefreshToken = await _refreshTokenRepository.GetRefreshTokenExchangeByUserId(userId);

        if (userRefreshToken is not null)
            return new RefreshTokenExchange(userRefreshToken.Token, userRefreshToken.RefreshKey);

        var refreshTokenExchange =
            new RefreshTokenExchange(GenerateToken(username, userId), new Random().Next().ToString());

        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Token = refreshTokenExchange.JwtToken,
            RefreshKey = refreshTokenExchange.RefreshTokenKey
        };

        await _refreshTokenRepository.Create(refreshToken);

        return refreshTokenExchange;
    }

    public async Task<RefreshTokenExchange> RefreshToken(string username, ulong userId,
        RefreshTokenExchange inputRefreshTokenExchange)
    {
        var userRefreshToken = await _refreshTokenRepository.GetRefreshTokenExchangeByUserId(userId);

        if (userRefreshToken is null) throw new ArgumentException($"No refresh token exchange bind to user {userId}");

        if (userRefreshToken.RefreshKey.Equals(inputRefreshTokenExchange.RefreshTokenKey) &&
            userRefreshToken.Token.Equals(inputRefreshTokenExchange.JwtToken))
        {
            var outputRefreshTokenExchange =
                new RefreshTokenExchange(GenerateToken(username, userId), new Random().Next().ToString());

            userRefreshToken.Token = outputRefreshTokenExchange.JwtToken;
            userRefreshToken.RefreshKey = outputRefreshTokenExchange.RefreshTokenKey;

            await _refreshTokenRepository.Update(userRefreshToken);
            return outputRefreshTokenExchange;
        }

        throw new Exception("Token received does not match");
    }

    public async Task<ulong> GetRefreshTokenUserId(string jwtToken, string refreshTokenKey)
    {
        var refreshToken =
            await _refreshTokenRepository.GetRefreshTokenExchangeByJwtTokenRefreshKey(jwtToken, refreshTokenKey);
        if (refreshToken is not null)
            return refreshToken.UserId;
        throw new ArgumentException("No refresh token matching");
    }

    /*
     * Logout, removes user's refreshToken.
     */
    public async Task Logout(RefreshTokenExchange refreshTokenExchange)
    {
        var refreshTokenToDelete = await _refreshTokenRepository.GetRefreshTokenExchangeByJwtTokenRefreshKey(
            refreshTokenExchange.JwtToken, refreshTokenExchange.RefreshTokenKey);
        if (refreshTokenToDelete is null) throw new ArgumentException("RefreshToken to logout was not found");
        await _refreshTokenRepository.Delete(refreshTokenToDelete);
    }

    /*
     * Generate the authorization token.
     * @return string value of the token.
     */
    private string GenerateToken(string username, ulong userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.Securitykey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
                { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.NameIdentifier, userId.ToString()) }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}