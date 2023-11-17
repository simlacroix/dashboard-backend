using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Dashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dashboard.Controllers;

/*
 * Abstract controller providing functionality to read the JwtToken.
 */
public class JwtTokenReaderController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger _loggerJwt;

    public JwtTokenReaderController(IOptions<JwtSettings> options, ILogger<JwtTokenReaderController> logger)
    {
        _loggerJwt = logger;
        _jwtSettings = options.Value;
    }

    /*
     * Get user id in the authorization token.
     * @return userId.
     */
    protected ulong GetIdFromToken()
    {
        _loggerJwt.LogInformation("Getting user id from jwt token");
        return ulong.Parse(ReadToken().Claims.First(x => x.Type == "nameid").Value);
    }

    /*
     * Get user id in a provided authorization token.
     * @return userId.
     */
    protected ulong GetIdFromProvidedToken(string providedToken)
    {
        _loggerJwt.LogInformation("Getting user id from a provided jwt token");
        return ulong.Parse(ReadProvidedToken(providedToken).Claims.First(x => x.Type == "nameid").Value);
    }

    /*
     * Get username in the authorization token.
     * @return username.
     */
    protected string GetUsernameFromToken()
    {
        _loggerJwt.LogInformation("Getting user usernamse from jwt token");
        return ReadToken().Claims.First(x => x.Type == "unique_name").Value;
    }

    /*
     * Get username in the authorization token.
     * @return username.
     */
    protected string GetUsernameFromProvidedToken(string providedToken)
    {
        _loggerJwt.LogInformation("Getting user username from provided jwt token");
        return ReadProvidedToken(providedToken).Claims.First(x => x.Type == "unique_name").Value;
    }

    /*
     * Read the authorization token and validate it.
     * @return JwtSecurityToken.
     */
    private JwtSecurityToken ReadToken()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
        return ReadProvidedToken(token);
    }

    /*
     * Read the a provided token and validate it.
     * @return JwtSecurityToken.
     */
    private JwtSecurityToken ReadProvidedToken(string providedToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Securitykey);

        tokenHandler.ValidateToken(providedToken, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        return (JwtSecurityToken)validatedToken;
    }
}