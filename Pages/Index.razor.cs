using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Intercom.Pages;

public partial class Index
{
    private static async Task SendRequestAsync()
    {
        // Replace with your HTTPS server URL and the path to the public key file
        string serverUrl = "https://intercom.byteloch.com/";
        int duration = 4200; // Set the duration to 1800
        int gpio = 0;       // Set the gpio value to 0

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
        httpClient.DefaultRequestHeaders.Add("Origin", "https://kibblewhite.github.io/Intercom/");

        // Append the duration and gpio parameters to the URL
        string urlWithParameters = $"{serverUrl}?duration={duration}&gpio={gpio}";

        // Send a request to the server
        HttpResponseMessage response = await httpClient.GetAsync(urlWithParameters);

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