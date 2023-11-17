namespace Dashboard.Models.Request;

/*
 * Request data model for updating user password.
 */
public class UpdatePasswordRequest
{
    public string OldPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}