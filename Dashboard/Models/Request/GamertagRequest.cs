using Newtonsoft.Json;

namespace Dashboard.Models.Request;

/*
 * Request data model for an individual gamertag.
 */
public class GamertagRequest
{
    public ulong? GamertagId { get; set; }

    public string Tag { get; set; } = null!;
    
    public GameHandler.Game Game { get; set; }
}