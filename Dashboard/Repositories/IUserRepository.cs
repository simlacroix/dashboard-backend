using Dashboard.Models;

namespace Dashboard.Repositories;

/*
 * User repository interface.
 */
public interface IUserRepository
{
    /*
     * Create new user.
     */
    Task Create(User user);

    /*
    * Update user's password.
    */
    Task UpdateUserPassword(string username, string newHashPassword);

    /*
     * Retrieve a user by its id.
     */
    Task<User?> GetUserById(ulong userId);

    /*
     * Retrieve a user by its username.
     */
    Task<User?> GetUserByUsername(string username);

    /*
     * Retrieve a user by its email.
     */
    Task<User?> GetUserByEmail(string email);
}