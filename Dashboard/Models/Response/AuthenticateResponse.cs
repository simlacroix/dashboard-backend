using Newtonsoft.Json;

namespace Dashboard.Models.Response;

/*
 * Response data model for authentication.
 */
public class AuthenticateResponse
{
    [JsonProperty("email")] public string Email;
    [JsonProperty("gamertags")] public ICollection<GamertagResponse> Gamertags;
    [JsonProperty("refreshTokenExchange")] public RefreshTokenExchange? RefreshTokenExchange;
    [JsonProperty("userId")] public ulong UserId;
    [JsonProperty("username")] public string Username;

    public AuthenticateResponse(string username, string email, ulong userId)
    {
        Username = username;
        Email = email;
        UserId = userId;
    }
}