using Dashboard.Models;

namespace Dashboard.Repositories;

/*
 * Gamertag repository interface.
 */
public interface IGamertagRepository
{
    /*
     * Create new gamertag
     */
    Task Create(Gamertag? gamertag);

    /*
     * Retrieve a single gamertag.
     */
    Task<Gamertag?> GetGamertagById(ulong gamertagId);

    /*
    * Update a single gamertage.
    */
    Task Update(Gamertag? gamertag);

    /*
    * Delete a given gamertag.
    */
    Task Delete(Gamertag? gamertag);

    /*
    * Delete a gamertag by id.
    */
    Task Delete(ulong gamertagId);

    /*
    * Get a collection of all the gamertags associated with a user.
    */
    Task<ICollection<Gamertag?>> GetGamertagsByUser(ulong userId);

    /*
     * Get the gamertag of a specified game for the user
     */
    Task<Gamertag?> GetGamertagByUserAndGame(ulong userId, GameHandler.Game game);
}