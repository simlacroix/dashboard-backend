using Dashboard.Exceptions;
using Dashboard.Models;
using Dashboard.Models.Request;
using Dashboard.Models.Response;
using Dashboard.Repositories;
using Dashboard.Utils;
using Dashboard.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Dashboard.Services.Impl;

/*
 * User service.
 */
public class UserService : IUserService
{
    private readonly IGamertagRepository _gamertagRepository;
    private readonly IUserRepository _userRepository;

    private readonly IVerifyGamertagService _verifyGamertagService;

    public UserService(IGamertagRepository gamertagRepository, IUserRepository userRepository,
        IVerifyGamertagService gamertagService)
    {
        _gamertagRepository = gamertagRepository;
        _userRepository = userRepository;
        _verifyGamertagService = gamertagService;
    }

    public async Task<ICollection<GamertagResponse>> UpdateGamertags(UpdateGamertagsRequest gamertagsRequest,
        ulong userId)
    {
        var user = await _userRepository.GetUserById(userId) ?? throw new ArgumentException("User not found");
        List<string> errors = new();

        foreach (var gamertagRequest in gamertagsRequest.GamertagRequests)
        {
            gamertagRequest.Tag = gamertagRequest.Tag.Trim();
            if (string.IsNullOrEmpty(gamertagRequest.Tag))
                throw new ArgumentException(
                    $"New gamertag for {GameHandler.GameName(gamertagRequest.Game)} cannot be empty or null");

            try
            {
                gamertagRequest.Tag = await _verifyGamertagService.VerifyGamertagExists(gamertagRequest.Tag, gamertagRequest.Game);
                await UpsertGamertag(user, gamertagRequest); 
            }
            catch (ArgumentException e)
            {
                errors.Add(
                    $"The gamertag {gamertagRequest.Tag} for {GameHandler.GameName(gamertagRequest.Game)} does not exists");
            }
        }

        if (errors.Any())
            throw new InvalidParameterException(errors);

        return await GetGamertags(userId);
    }

    public async Task<ICollection<GamertagResponse>> GetGamertags(ulong userId)
    {
        var responses = new List<GamertagResponse>();
        var result = await _gamertagRepository.GetGamertagsByUser(userId);
        foreach (var game in (GameHandler.Game[])Enum.GetValues(typeof(GameHandler.Game)))
        {
            var matches = result.Where(g => g.Game == game).ToArray();
            if (matches.IsNullOrEmpty())
                responses.Add(new GamertagResponse
                {
                    Tag = "",
                    Game = game,
                    GameName = GameHandler.GameName(game),
                    GamertagId = null
                });
            else
                foreach (var match in matches)
                    if (match is not null)
                        responses.Add(new GamertagResponse
                        {
                            Tag = match.Tag,
                            Game = game,
                            GameName = GameHandler.GameName(game),
                            GamertagId = match.GamertagId
                        });
        }

        return responses;
    }

    public async Task<string> GetGamertagForGame(ulong userId, GameHandler.Game game)
    {
        var gamertag = await _gamertagRepository.GetGamertagByUserAndGame(userId, game);
        if (gamertag != null) return gamertag.Tag;
        throw new ArgumentException($"No gamertag for user {userId} and game {GameHandler.GameName(game)}");
    }

    public async Task UpdatePassword(UpdatePasswordRequest updatePasswordRequest, ulong userId,
        string username)
    {
        try
        {
            UpdatePasswordValidator.Validate(updatePasswordRequest);
        }
        catch (InvalidParameterException e)
        {
            throw new ArgumentException(e.Message);
        }


        var user = _userRepository.GetUserById(userId);
        var userCreds = new LoginRequest
        {
            Username = username,
            Password = updatePasswordRequest.OldPassword
        };
        if (user is null)
            throw new Exception($"Problem occured, could not find the user {username}");

        if (user.Result != null && !VerifPassword(userCreds, user.Result.Password))
            throw new ArgumentException("Provided current password doesn't match the one currently use");

        var newPasswordHash = PwdHasher.PasswordHasher(new LoginRequest
        {
            Username = username,
            Password = updatePasswordRequest.NewPassword
        });

        await _userRepository.UpdateUserPassword(username, newPasswordHash);
    }

    private async Task UpsertGamertag(User user, GamertagRequest gamertagRequest)
    {
        var gamertag = new Gamertag
        {
            UserKey = user.UserId,
            Tag = gamertagRequest.Tag,
            Game = gamertagRequest.Game,
            UserKeyNavigation = user
        };

        if (gamertagRequest.GamertagId is null)
        {
            await _gamertagRepository.Create(gamertag);
        }
        else
        {
            gamertag.GamertagId = (ulong)gamertagRequest.GamertagId;
            await _gamertagRepository.Update(gamertag);
        }
    }

    /*
     * DUPLICATE FROM AUTH SERVICE.
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