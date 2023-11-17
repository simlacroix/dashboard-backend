using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;

namespace Dashboard.Validators;

/*
 * Validator class for username.
 */
public static class UsernameValidator
{
    /*
     * Validate username format.
     * @return string corresponding to the invalidation that occured, null if fine.
     */
    public static string? Validate(String username)
    {
        if (username.IsNullOrEmpty()) 
            return "username can't be empty or null";
        
        if (username.Length > 50) 
            return "username is too long";
        
        if (Regex.Match(username, "/^[a-zA-Z0-9.-]*$/").Success)
            return "Username can only contains numbers, letters or , .  - '";
        
        return null;
    }
}