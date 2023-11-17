using System.Net;
using System.Web;
using Dashboard.Clients;

namespace Dashboard.Services.Impl;

public class LoRService : ILoRService
{
    private readonly LorClient _client;

    public LoRService()
    {
        _client = new LorClient();
    }
    
    public async Task<string> getStatsForPlayer(string gamertag)
    {
        string url = $"/LoR/getStatsForPlayer?summonerName={HttpUtility.UrlEncode(gamertag)}";
        var response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        
        // Generic exception for now, look for proper exception to throw
        throw new Exception($"A problem occured while trying to get the summoner {gamertag}");
    }

    public async Task<string> VerifyGamertagExists(string gamertag)
    {
        string url = $"/LoR/userExists?summonerName={HttpUtility.UrlEncode(gamertag)}";
        var response = await _client.GetAsync(url);
        
        if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.NotFound) throw new ArgumentException("Gamertag not found");

        throw new Exception($"A problem occured while trying to get the summoner {gamertag}");
    }
}