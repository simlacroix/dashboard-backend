namespace Dashboard.Clients;

public class LorClient : HttpClient
{
    public LorClient()
    {
        string url = Environment.GetEnvironmentVariable("LOR_BASE_URL") ??
                     throw new MissingFieldException("Missing Environment Variable for LOR Module address");
        BaseAddress = new Uri(url);
    }
}