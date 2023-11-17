using Dashboard.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Dashboard.Controllers;

/*
 * Controller handling Teamfight tactics data requests.
 */
[Authorize]
[ApiController]
[Route("[controller]")]
public class TftController : JwtTokenReaderController
{
    private readonly ILogger _logger;
    private readonly ITftService _tftService;
    private readonly IUserService _userService;

    public TftController(ITftService tftService, IOptions<JwtSettings> options, IUserService userService,
        ILogger<TftController> logger, ILogger<JwtTokenReaderController> loggerJwt) : base(options, loggerJwt)
    {
        _tftService = tftService;
        _userService = userService;
        _logger = logger;
    }

    /*
     * Get User's TFT summoner.
     */
    [HttpGet("get-summoner")]
    public async Task<IActionResult> GetSummoner()
    {
        _logger.LogInformation($"Getting TFT summoner info for {GetUsernameFromToken()}");
        try
        {
            return Ok(await _tftService.GetSummoner(
                await _userService.GetGamertagForGame(GetIdFromToken(), GameHandler.Game.TeamfightTactics)));
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"exception while getting TFT summoner info for {GetUsernameFromToken()}");
            return BadRequest(e.Message);
        }
    }

    /*
     * Get User's TFT matches.
     */
    [HttpGet("get-matches")]
    public async Task<IActionResult> GetMatches()
    {
        _logger.LogInformation($"Getting TFT matches for {GetUsernameFromToken()}");
        try
        {
            return Ok(await _tftService.GetMatches(
                await _userService.GetGamertagForGame(GetIdFromToken(), GameHandler.Game.TeamfightTactics)));
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"exception while getting TFT matches for {GetUsernameFromToken()}");
            return BadRequest(e.Message);
        }
    }
}