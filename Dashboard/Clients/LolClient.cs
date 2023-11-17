namespace Dashboard.Clients;

public class LolClient : HttpClient
{
    public LolClient()
    {
        string url = Environment.GetEnvironmentVariable("LOL_BASE_URL") ??
                     throw new MissingFieldException("Missing Environment Variable for LOL Module address");
        BaseAddress = new Uri(url);
    }
}