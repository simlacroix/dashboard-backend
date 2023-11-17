using Dashboard.Models;
using Dashboard.Models.Request;
using Dashboard.Models.Response;

namespace Dashboard.Services;

/*
 * Authentication service interface.
 */
public interface IAuthService
{
    public Task<AuthenticateResponse> RegisterUser(UserRegisterRequest userRegister);

    /*
    * Authenticate a login request.
    */
    public Task<AuthenticateResponse> AuthenticateUSer(LoginRequest userCreds);
    Task<User> GetUser(ulong userId);
}