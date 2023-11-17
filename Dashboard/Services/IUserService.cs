using Dashboard.Models;
using Dashboard.Models.Request;
using Dashboard.Models.Response;

namespace Dashboard.Services;

/*
 * User service interface.
 */
public interface IUserService
{
    /*
     * Update user's gamertags.
     */
    public Task<ICollection<GamertagResponse>> UpdateGamertags(UpdateGamertagsRequest gamertagsRequest, ulong userId);

    /*
     * Get all user's gamertags for all supported games (even if user doesn't have a registerd gamertag).
     */
    public Task<ICollection<GamertagResponse>> GetGamertags(ulong userId);

    /*
     * Update user's password.
     */
    public Task UpdatePassword(UpdatePasswordRequest updatePasswordRequest, ulong userId,
        string username);

    public Task<string> GetGamertagForGame(ulong userId, GameHandler.Game game);
}