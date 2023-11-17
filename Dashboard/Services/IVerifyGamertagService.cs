using Dashboard.Models;

namespace Dashboard.Services;

public interface IVerifyGamertagService
{
    Task<string> VerifyGamertagExists(string gamertag, GameHandler.Game game);
}