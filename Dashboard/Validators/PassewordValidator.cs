using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;

namespace Dashboard.Validators;

/*
 * Validator class for password.
 */
public static class PassewordValidator
{
    /*
     * Validate password format.
     * @return string corresponding to the invalidation that occured, null if fine.
     */
    public static string? Validate(String password)
    {
        if (password.IsNullOrEmpty()) 
            return "password can't be empty or null";
        
        if (password.Length > 50) 
            return "password is too long";
        
        if (password.Length < 8) 
            return "password is too short";
        
        if (Regex.Match(password, "/^(?=.[a-z]).+$/").Success)
            return "Password must have at least one lowercase letter";
        
        if (Regex.Match(password, "/^(?=.[A-Z]).+$/").Success)
            return "Password must have at least one uppercase letter";
        
        if (Regex.Match(password, "/^(?=.\\d).+$/").Success)
            return "Password must have at least one number";
        
        if (Regex.Match(password, "/^(?=.[-+_!@#$%^&*.,?]).+$/").Success)
            return "Password must have at least one special character";
        
        return null;
    }
}