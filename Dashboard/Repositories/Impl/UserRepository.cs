using Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Repositories.Impl;

/*
 * Communicator with the Database for user.
 */
public class UserRepository : IUserRepository
{
    private readonly TheTrackingFellowshipContext _context;

    public UserRepository(TheTrackingFellowshipContext context)
    {
        _context = context;
    }


    public async Task Create(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }


    public async Task<User?> GetUserById(ulong userId)
    {
        return await _context.Users.SingleOrDefaultAsync(user => user.UserId == userId);
    }


    public async Task<User?> GetUserByUsername(string username)
    {
        return await _context.Users.SingleOrDefaultAsync(user => user.Username == username);
    }


    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.SingleOrDefaultAsync(user => user.Email == email);
    }

    public async Task UpdateUserPassword(string username, string newPassword)
    {
        var user = await _context.Users.SingleAsync(user => user.Username == username);

        user.Password = newPassword;

        await _context.SaveChangesAsync();
    }
}