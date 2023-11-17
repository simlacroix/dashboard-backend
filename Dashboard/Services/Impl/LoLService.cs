using System.Net;
using System.Web;
using Dashboard.Clients;
using Newtonsoft.Json;

namespace Dashboard.Services.Impl;

public class LoLService : ILoLService
{
    private readonly LolClient _client;

    public LoLService()
    {
        _client = new LolClient();
    }
    
    public async Task<string> getStatsForPlayer(string gamertag)
    {
        string url = $"/LoL/getStatsForPlayer?summonerName={HttpUtility.UrlEncode(gamertag)}";
        var response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        
        // Generic exception for now, look for proper exception to throw
        throw new Exception($"A problem occured while trying to get the summoner {gamertag}");
    }

    public async Task<double> getChampionWinRate(string gamertag, string championName)
    {
        string url = $"/LoL/getChampionWinRate?summonerName={HttpUtility.UrlEncode(gamertag)}&champName={HttpUtility.UrlEncode(championName)}";
        var response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            return double.Parse(content);            
        }
        
        // Generic exception for now, look for proper exception to throw
        throw new Exception($"A problem occured while trying to get the win rate for the champion {championName}");
    }

    public async Task<double> getLaneWinRate(string gamertag, string lane)
    {
        string url = $"/LoL/getLaneWinRate?summonerName={HttpUtility.UrlEncode(gamertag)}&lane={HttpUtility.UrlEncode(lane)}";
        var response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            return double.Parse(content);            
        }
        
        // Generic exception for now, look for proper exception to throw
        throw new Exception($"A problem occured while trying to get the win rate for the lane {lane}");
    }

    public async Task<string> VerifyGamertagExists(string gamertag)
    {
        string url = $"/LoL/userExists?summonerName={HttpUtility.UrlEncode(gamertag)}";
        var response = await _client.GetAsync(url);
        
        if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.NotFound) throw new ArgumentException("Gamertag not found");

        throw new Exception($"A problem occured while trying to get the summoner {gamertag}");
    }
}