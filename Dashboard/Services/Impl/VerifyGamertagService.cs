using Dashboard.Models;

namespace Dashboard.Services.Impl;

public class VerifyGamertagService : IVerifyGamertagService
{
    private readonly ITftService _tftService;
    private readonly ILoLService _loLService;
    private readonly ILoRService _loRService;

    public VerifyGamertagService(ITftService tftService, ILoLService loLService, ILoRService loRService)
    {
        _tftService = tftService;
        _loLService = loLService;
        _loRService = loRService;
    }

    public async Task<string> VerifyGamertagExists(string gamertag, GameHandler.Game game)
    {
        switch (game)
        {
            case GameHandler.Game.LeagueOfLegends:
                return await _loLService.VerifyGamertagExists(gamertag);
            case GameHandler.Game.LegendsOfRuneterra:
                return await _loRService.VerifyGamertagExists(gamertag);
            case GameHandler.Game.TeamfightTactics:
                return await _tftService.VerifyGamertagExists(gamertag);
            default:
                throw new ArgumentOutOfRangeException(nameof(game), game, null);
        }
    } 
}