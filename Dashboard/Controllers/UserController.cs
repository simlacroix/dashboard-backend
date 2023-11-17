using Dashboard.Exceptions;
using Dashboard.Models;
using Dashboard.Models.Request;
using Dashboard.Models.Response;
using Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Dashboard.Controllers;

/*
 * Controller handling user management requests.
 */
[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : JwtTokenReaderController
{
    private readonly ILogger _logger;
    private readonly IUserService _userService;

    public UserController(IUserService userService, IOptions<JwtSettings> options, ILogger<UserController> logger,
        ILogger<JwtTokenReaderController> loggerJwt) : base(options, loggerJwt)
    {
        _userService = userService;
        _logger = logger;
    }

    /*
     * Update gamertags of the user.
     */
    [HttpPost("update-gamertags")]
    public async Task<IActionResult> UpdateGamertags([FromBody] UpdateGamertagsRequest gamertagsRequest)
    {
        _logger.LogInformation($"Updating gamertags for {GetUsernameFromToken()}");
        ICollection<GamertagResponse> response;

        try
        {
            response = await _userService.UpdateGamertags(gamertagsRequest, GetIdFromToken());
        }
        catch (ArgumentException e)
        {
            _logger.LogInformation(e, $"exception while updating gamertags for {GetUsernameFromToken()}");
            return BadRequest(e.Message);
        }
        catch (InvalidParameterException e)
        {
            _logger.LogInformation(e, "One or many gamertags do not exists");
            return BadRequest(e.Message);
        }

        return Ok(JsonConvert.SerializeObject(response));
    }

    /*
     * Get all supported games gamertags of the user.
     */
    [HttpGet("get-gamertags")]
    public async Task<IActionResult> GetGamertags()
    {
        _logger.LogInformation($"Retrieving gamertags for {GetUsernameFromToken()}");
        ICollection<GamertagResponse> response;
        try
        {
            var userId = GetIdFromToken();

            response = await _userService.GetGamertags(userId);
        }
        catch (ArgumentException e)
        {
            _logger.LogInformation(e, $"exception while retrieving gamertags for {GetUsernameFromToken()}");
            return BadRequest(e.Message);
        }

        return Ok(JsonConvert.SerializeObject(response));
    }

    /*
     * Update user password.
     */
    [HttpPost("update-password")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest updatePasswordRequest)
    {
        _logger.LogInformation($"Updating password for {GetUsernameFromToken()}");
        try
        {
            var userId = GetIdFromToken();
            var username = GetUsernameFromToken();

            await _userService.UpdatePassword(updatePasswordRequest, userId, username);
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"exception while updating password for {GetUsernameFromToken()}");
            return BadRequest(e.Message);
        }

        return Ok();
    }
}