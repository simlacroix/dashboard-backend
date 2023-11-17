using Newtonsoft.Json;

namespace Dashboard.Models.Response;

/*
 * Response data model for gamertags.
 */
public class GamertagResponse
{

    [JsonProperty("gamertagId")]
    public ulong? GamertagId { get; set; }

    [JsonProperty("tag")]
    public string Tag { get; set; } = null!;
    
    [JsonProperty("game")]
    public GameHandler.Game Game { get; set; }
    
    [JsonProperty("gameName")]
    public string? GameName { get; set; }

}