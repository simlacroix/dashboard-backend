using Dashboard.Models.Request;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.Utils;

/*
 * Utility class for password hashing
 */
internal static class PwdHasher
{
    /*
     * Hash a password based of login credentials.
     */
    internal static string PasswordHasher(LoginRequest userCreds)
    {
        return new PasswordHasher<LoginRequest>().HashPassword(userCreds, userCreds.Password);
    }
}