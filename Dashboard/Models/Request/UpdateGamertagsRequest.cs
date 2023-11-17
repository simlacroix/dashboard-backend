namespace Dashboard.Models.Request;

/*
 * Request data model for updating gamertags.
 */
public class UpdateGamertagsRequest
{
    public IEnumerable<GamertagRequest> GamertagRequests { get; set; } = null!;
}