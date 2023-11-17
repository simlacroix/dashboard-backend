namespace Dashboard.Models;

/*
 * Data model for user.
 */
public class User
{
    public ulong UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public DateTime? LastLogin { get; set; }

    public virtual ICollection<Gamertag> Gamertags { get; } = new List<Gamertag>();
}