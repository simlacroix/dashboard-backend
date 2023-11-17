using Dashboard.Exceptions;
using Dashboard.Models;
using Dashboard.Models.Request;
using Dashboard.Models.Response;
using Dashboard.Repositories;
using Dashboard.Utils;
using Dashboard.Validators;
using Microsoft.AspNetCore.Identity;

namespace Dashboard.Services.Impl;

/*
 * Authentication service.
 */
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    /*
     * Register a new user.
     */
    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<AuthenticateResponse> RegisterUser(UserRegisterRequest userRegister)
    {
        if (_userRepository.GetUserByUsername(userRegister.Username).Result is not null)
            throw new ArgumentException("username already used");

        if (_userRepository.GetUserByEmail(userRegister.Email).Result is not null)
            throw new ArgumentException("email already used");

        try
        {
            UserRegisterValidator.ValidateUserRegistrationParams(userRegister);
        }
        catch (InvalidParameterException e)
        {
            throw new ArgumentException(e.Message);
        }

        var userCreds = new LoginRequest
        {
            Username = userRegister.Username,
            Password = userRegister.Password
        };

        var user = new User
        {
            Username = userRegister.Username,
            Email = userRegister.Email,
            Password = PwdHasher.PasswordHasher(userCreds),
            CreatedOn = DateTime.Now
        };

        await _userRepository.Create(user);
        return new AuthenticateResponse(user.Username, user.Email, user.UserId);
    }

    public async Task<AuthenticateResponse> AuthenticateUSer(LoginRequest userCreds)
    {
        var user = await _userRepository.GetUserByUsername(userCreds.Username);
        if (user is null || !VerifPassword(userCreds, user.Password))
            throw new ArgumentException("Password or username invalid");

        return new AuthenticateResponse(user.Username, user.Email, user.UserId);
    }

    public async Task<User> GetUser(ulong userId)
    {
        var user = await _userRepository.GetUserById(userId);
        if (user is not null)
        {
            return user;
        }

        throw new ArgumentException($"No user using with id {userId}");
    }

    /*
     * DUPLICATE FROM USER SERVICE.
     * Verify if password match the one in the database.
     */
    private bool VerifPassword(LoginRequest userCreds, string hashedPassword)
    {
        bool success;
        var result =
            new PasswordHasher<LoginRequest>().VerifyHashedPassword(userCreds, hashedPassword, userCreds.Password);

        switch (result)
        {
            case PasswordVerificationResult.Success:
                success = true;
                break;
            case PasswordVerificationResult.SuccessRehashNeeded:
                var newHash = new PasswordHasher<LoginRequest>().HashPassword(userCreds, userCreds.Password);
                _userRepository.UpdateUserPassword(userCreds.Username, newHash);
                success = true;
                break;
            case PasswordVerificationResult.Failed:
                success = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return success;
    }
}