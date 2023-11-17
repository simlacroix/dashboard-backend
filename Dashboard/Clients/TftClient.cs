namespace Dashboard.Clients;

/*
 * Http client for TFT module communication.
 */
public class TftClient : HttpClient
{
    public TftClient()
    {
        var url = Environment.GetEnvironmentVariable("TFT_BASE_URL") ??
                  throw new MissingFieldException("Missing Environment Variable for TFT Module address");
        BaseAddress = new Uri(url);
    }
}