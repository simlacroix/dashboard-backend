using Dashboard.Exceptions;
using Dashboard.Models.Request;
using Microsoft.IdentityModel.Tokens;

namespace Dashboard.Validators;

/*
 * Validator class for password update.
 */
public static class UpdatePasswordValidator
{
    /*
     * Validate new password for update.
     * @return string corresponding to the invalidation that occured, null if fine.
     */
    public static void Validate(UpdatePasswordRequest updatePasswordRequest)
    {
        var validationErrorMsg = new List<string>();

        var newPassword = PassewordValidator.Validate(updatePasswordRequest.NewPassword);

        if (newPassword != null) validationErrorMsg.Add(newPassword);
        if (updatePasswordRequest.OldPassword.Equals(updatePasswordRequest.NewPassword))
            validationErrorMsg.Add("New password cannot be the same as the one currently use");

        if (!validationErrorMsg.IsNullOrEmpty()) throw new InvalidParameterException(validationErrorMsg);
    }
}