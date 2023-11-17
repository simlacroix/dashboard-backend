namespace Dashboard.Models.Request;

/*
 * Base request data model for all authentication requests.
 */
public abstract class AuthBaseRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

}