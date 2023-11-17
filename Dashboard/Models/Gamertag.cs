namespace Dashboard.Models;

/*
 * Data model for gamertags.
 */
public class Gamertag
{
    public ulong GamertagId { get; set; }

    public string Tag { get; set; } = null!;

    public ulong? UserKey { get; set; }

    public GameHandler.Game Game { get; set; }

    public virtual User? UserKeyNavigation { get; set; }
}