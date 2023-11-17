namespace Dashboard.Services;

public interface ITftService
{
    public Task<string?> GetSummoner(string gamertag);
    public Task<string?> GetMatches(string gamertag);
    public Task<string> VerifyGamertagExists(string gamertag);
}