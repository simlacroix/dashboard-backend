using Dashboard.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Dashboard.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class LoRController : JwtTokenReaderController
{
    private readonly ILogger _logger;
    private readonly ILoRService _lorService;
    private readonly IUserService _userService;
    
    public LoRController(ILoRService lorService, IUserService userService, ILogger<LoRController> logger, IOptions<JwtSettings> options,
        ILogger<JwtTokenReaderController> loggerJwt) : base(options, loggerJwt)
    {
        _lorService = lorService;
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("get-stats-for-player")]
    public async Task<IActionResult> GetStatsForPlayer()
    {
        _logger.LogInformation($"Getting LOR stats user: {GetUsernameFromToken()}");
        try
        {
            var basicStats = await _lorService.getStatsForPlayer(
                await _userService.GetGamertagForGame(GetIdFromToken(), GameHandler.Game.LegendsOfRuneterra));
            return Ok(basicStats);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"exception while getting LOL stats for {GetUsernameFromToken()}");
            return BadRequest(e.Message);
        }
    }
}