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
    private static async Task SendRequestAsync()
    {
        // Replace with your HTTPS server URL and the path to the public key file
        string serverUrl = "https://intercom.byteloch.com/";
        int duration = 4200; // Set the duration to 1800
        int gpio = 0;       // Set the gpio value to 0

        Uri url = new Uri(serverUrl)
            .AddQuery(nameof(duration), duration.ToString())
            .AddQuery(nameof(gpio), gpio.ToString());

        // Create an HttpClient with the public key
        HttpClientHandler handler = new()
        {
            // ClientCertificateOptions = ClientCertificateOption.Manual
            // ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            // ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
        };

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
            // Process the response here
        }
        else
        {
            Console.WriteLine($"Request failed with status code {response.StatusCode}.");
        }
    }
}