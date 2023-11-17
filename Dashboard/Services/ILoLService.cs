namespace Dashboard.Services;

public interface ILoLService
{

    public Task<string> getStatsForPlayer(string summonerName);

    public Task<double> getChampionWinRate(string summonerName, string champName);

    public Task<double> getLaneWinRate(string summonerName, string lane);
    Task<string> VerifyGamertagExists(string gamertag);
}