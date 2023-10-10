using System.Text;
using System.Web;

namespace Intercom.Pages;

public static class HttpExtensions
{
    public static Uri AddQuery(this Uri uri, string name, string value)
    {
        System.Collections.Specialized.NameValueCollection httpValueCollection = HttpUtility.ParseQueryString(uri.Query);

        httpValueCollection.Remove(name);
        httpValueCollection.Add(name, value);

        UriBuilder ub = new(uri)
        {
            Query = httpValueCollection.ToString()
        };

        return ub.Uri;
    }
}

public partial class Index
{

    private const int _default_duration_ms = 800;
    private const string _server_url = "https://intercom.byteloch.com/";

    protected bool _processing = false;
    protected int _gpio = 0;
    protected int _duration_ms = _default_duration_ms;

    protected async Task DurationChangedRequest(int duration_ms)
        => await Task.FromResult(_duration_ms = duration_ms);

    private async Task ProcessDoorToggleRequest()
    {

        _processing = true;

        Uri url = new Uri(_server_url)
            .AddQuery("duration", _duration_ms.ToString())
            .AddQuery("gpio", _gpio.ToString());

        // Create an HttpClient with the public key
        HttpClientHandler handler = new();
        using HttpClient httpClient = new(handler);

        // autnetication, duh...
        string basic_authentication_string = Convert.ToBase64String(Encoding.UTF8.GetBytes("admin:esp8266"));

        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basic_authentication_string);
        httpClient.DefaultRequestHeaders.Referrer = new Uri("https://kibblewhite.github.io/Intercom/");
        httpClient.DefaultRequestHeaders.Add("Origin", "https://kibblewhite.github.io");

        // Send a request to the server
        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Request was successful.");
        }
        else
        {
            Console.WriteLine($"Request failed with status code {response.StatusCode}.");
        }

        _processing = false;

    }
}