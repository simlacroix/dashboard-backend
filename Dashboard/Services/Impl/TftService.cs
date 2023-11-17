using System.Net;
using System.Web;
using Dashboard.Clients;

namespace Dashboard.Services.Impl;

/*
 * Teamfight tactics service.
 */
public class TftService : ITftService
{
    private readonly TftClient _client;

    public TftService()
    {
        _client = new TftClient();
    }

    /*
     * Verify a given gamertag exists in Teamfight Tactics API.
     */
    public async Task<string> VerifyGamertagExists(string gamertag)
    {
        string url = $"Tft/check-if-summoner-exists?summonerName={HttpUtility.UrlEncode(gamertag)}";
        var response = await _client.GetAsync(url);
        
        if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.NotFound) throw new ArgumentException("Gamertag not found");

        // Generic exception for now, look for proper exception to throw
        throw new Exception($"A problem occured while trying to get the summoner {gamertag}");
    }

    /*
     * Get the user summoner from its gamertag.
     */
    public async Task<string?> GetSummoner(string gamertag)
    {
        string url = $"Tft/get-summoner-by-name?summonerName={HttpUtility.UrlEncode(gamertag)}";
        var response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

        // Generic exception for now, look for proper exception to throw
        throw new Exception($"A problem occured while trying to get the summoner {gamertag}");
    }

    /*
     * Get the user's latest matches.
     */
    public async Task<string?> GetMatches(string gamertag)
    {
        string url = $"Tft/get-matches-for-summoner?summonerName={HttpUtility.UrlEncode(gamertag)}";
        var response = await _client.GetAsync(url);
        if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

        // Generic exception for now, look for proper exception to throw
        throw new Exception($"A problem occured while trying to get the matches for summoner {gamertag}");
    }
}