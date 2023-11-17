using Microsoft.IdentityModel.Tokens;

namespace Dashboard.Validators;

/*
 * Validator class for email.
 */
public static class EmailValidator
{
    /*
     * Validate email format.
     * @return string corresponding to the invalidation that occured, null if fine.
     */
    public static string? Validate(String email)
    {
        try {
            var addr = new System.Net.Mail.MailAddress(email);
        }
        catch (Exception e){
            return "Invalide email format :" + e.Message;
        }
        
        return null;
    }
}