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
 * Controller handling authentication requests.
 */
[ApiController]
[Route("[controller]")]
public class AuthController : JwtTokenReaderController
{
    private readonly IAuthService _authService;
    private readonly ILogger _logger;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IUserService _userService;

    public AuthController(IOptions<JwtSettings> options, IAuthService authService,
        IRefreshTokenService refreshTokenService, IUserService userService,
        ILogger<AuthController> logger, ILogger<JwtTokenReaderController> loggerJwt) : base(options, loggerJwt)
    {
        _authService = authService;
        _refreshTokenService = refreshTokenService;
        _userService = userService;
        _logger = logger;
    }

    /*
     * Register new user.
     */
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequest userRegister)
    {
        _logger.LogInformation($"Registering user: {userRegister.Username}");
        AuthenticateResponse response;

        try
        {
            response = await _authService.RegisterUser(userRegister);
            response.RefreshTokenExchange =
                await _refreshTokenService.GenerateRefreshTokenExchange(response.Username, response.UserId);
            response.Gamertags = await _userService.GetGamertags(response.UserId);
        }
        catch (ArgumentException e)
        {
            _logger.LogInformation(e, $"exception while registering {userRegister.Username}");
            return BadRequest(e.Message);
        }

        return Ok(JsonConvert.SerializeObject(response));
    }

    /*
     * Authenticate user.
     */
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(LoginRequest userCreds)
    {
        _logger.LogInformation($"Authenticating {userCreds.Username}");
        AuthenticateResponse response;

        try
        {
            response = await _authService.AuthenticateUSer(userCreds);
            response.RefreshTokenExchange =
                await _refreshTokenService.GenerateRefreshTokenExchange(response.Username, response.UserId);
            response.Gamertags = await _userService.GetGamertags(response.UserId);
        }
        catch (ArgumentException e)
        {
            _logger.LogInformation(e, $"exception while authenticating {userCreds.Username}");
            return BadRequest(e.Message);
        }

        return Ok(JsonConvert.SerializeObject(response));
    }

    /*
     * Logout user.
     */
    [Authorize]
    [HttpPost("logout")]
    public async Task Logout(RefreshTokenExchange refreshTokenExchange)
    {
        _logger.LogInformation($"Logout user {GetUsernameFromToken()}");
        await _refreshTokenService.Logout(refreshTokenExchange);
    }

    /*
     * Refresh user's token.
     */
    [HttpPatch("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenExchange refreshTokenExchange)
    {
        try
        {
            var userId = await _refreshTokenService.GetRefreshTokenUserId(refreshTokenExchange.JwtToken,
                refreshTokenExchange.RefreshTokenKey);

            var user = await _authService.GetUser(userId);

            _logger.LogInformation($"Refreshing token for {user.Username}");
            return Ok(await _refreshTokenService.RefreshToken(user.Username, user.UserId, refreshTokenExchange));
        }
        catch (Exception e)
        {
            _logger.LogInformation(e, $"exception while refreshing token for user {GetUsernameFromToken()}");
            return BadRequest(e.Message);
        }
    }
}