using Dashboard.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Dashboard.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class LoLController : JwtTokenReaderController
{
    private readonly ILogger _logger;
    private readonly ILoLService _lolService;
    private readonly IUserService _userService;

    public LoLController(ILoLService lolService, IUserService userService, IOptions<JwtSettings> options,
        ILogger<LoLController> logger, ILogger<JwtTokenReaderController> loggerJwt) : base(options, loggerJwt)
    {
        _lolService = lolService;
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("get-stats-for-player")]
    public async Task<IActionResult> GetStatsForPlayer()
    {
        _logger.LogInformation($"Getting LOL stats user: {GetUsernameFromToken()}");
        try
        {
            var basicStats = await _lolService.getStatsForPlayer(
                await _userService.GetGamertagForGame(GetIdFromToken(), GameHandler.Game.LeagueOfLegends));
            return Ok(basicStats);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"exception while getting LOL stats for {GetUsernameFromToken()}");
            return BadRequest(e.Message);
        }
    }

    [HttpGet("get-champion-win-rate")]
    public async Task<IActionResult> GetChampionWinRate(string champName)
    {
        _logger.LogInformation($"Getting LOL champion win rate {champName} for {GetUsernameFromToken()}");
        try
        {
            var champWinRate = await _lolService.getChampionWinRate(
                await _userService.GetGamertagForGame(GetIdFromToken(), GameHandler.Game.LeagueOfLegends), champName);
            return Ok(champWinRate);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e,
                $"exception while getting LOL champion win rate {champName} for {GetUsernameFromToken()}");
            return BadRequest(e.Message);
        }
    }

    [HttpGet("get-lane-win-rate")]
    public async Task<IActionResult> GetLaneWinRate(string lane)
    {
        _logger.LogInformation($"Getting LOL lane win rate {lane} for {GetUsernameFromToken()}");
        try
        {
            var laneWinRate = await _lolService.getLaneWinRate(
                await _userService.GetGamertagForGame(GetIdFromToken(), GameHandler.Game.LeagueOfLegends), lane);
            return Ok(laneWinRate);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"exception while getting LOL lane win rate {lane} for {GetUsernameFromToken()}");
            return BadRequest(e.Message);
        }
    }
}