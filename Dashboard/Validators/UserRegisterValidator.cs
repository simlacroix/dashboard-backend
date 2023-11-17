using Dashboard.Exceptions;
using Dashboard.Models.Request;
using Microsoft.IdentityModel.Tokens;

namespace Dashboard.Validators;

/*
 * Validator class for user registration.
 */
public static class UserRegisterValidator
{    
    /*
     * Validate user registration fields.
     * @throw InvalidParameterException in case of validation fail.
     */
    public static void ValidateUserRegistrationParams(UserRegisterRequest userRegisterRequest)
    {
        var validationErrorMsg = new List<string>();

        var username = UsernameValidator.Validate(userRegisterRequest.Username);
        var email = EmailValidator.Validate(userRegisterRequest.Email);
        var password = PassewordValidator.Validate(userRegisterRequest.Password);

        if (username != null) validationErrorMsg.Add(username);
        if (email != null) validationErrorMsg.Add(email);
        if (password != null) validationErrorMsg.Add(password);

        if (!validationErrorMsg.IsNullOrEmpty()) throw new InvalidParameterException(validationErrorMsg);
    }
}