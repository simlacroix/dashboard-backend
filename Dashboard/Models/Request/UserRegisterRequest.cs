namespace Dashboard.Models.Request;

/*
 * Request data model for user registration request.
 */
public class UserRegisterRequest : AuthBaseRequest
{
    public string Email { get; set; } = null!;
}