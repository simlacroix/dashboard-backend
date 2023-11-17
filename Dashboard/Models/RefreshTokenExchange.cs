using Newtonsoft.Json;

namespace Dashboard.Models;

/*
 * Data model use for token refresh exchange.
 */
public class RefreshTokenExchange
{
    public RefreshTokenExchange(string jwtToken, string refreshTokenKey)
    {
        JwtToken = jwtToken;
        RefreshTokenKey = refreshTokenKey;
    }

    [JsonProperty("jwtToken", Required = Required.Always)]
    public string JwtToken { get; set; }

    [JsonProperty("refreshTokenKey", Required = Required.Always)] public string RefreshTokenKey { get; set; }
}