namespace Dashboard.Models;

/*
 * Data model for Refresh Token
 */
public class RefreshToken
{
    public ulong RefreshId { get; set; }
    public ulong UserId { get; set; }
    public string RefreshKey { get; set; } = null!;
    public string Token { get; set; } = null!;
}