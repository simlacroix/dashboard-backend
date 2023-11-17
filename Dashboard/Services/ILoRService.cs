namespace Dashboard.Services;

public interface ILoRService
{
    public Task<string> getStatsForPlayer(string gamertag);
    Task<string> VerifyGamertagExists(string gamertag);
}