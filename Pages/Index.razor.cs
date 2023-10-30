using Intercom.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Intercom.Pages;

public sealed class AuthenticationStateDto
{
    /// <inheritdoc cref="ClaimsPrincipal.Claims" />
    public required IEnumerable<Claim> Claims { get; init; }

    /// <inheritdoc cref="ClaimsPrincipal.Identities" />
    public required IEnumerable<ClaimsIdentity> Identities { get; init; }

    /// <inheritdoc cref="ClaimsPrincipal.Identity" />
    public required System.Security.Principal.IIdentity? Identity { get; init; }
}

public partial class Index
{

    [Inject]
    public required ISnackbar Snackbar { get; init; }

    [Inject]
    public required AuthenticationStateProvider _authentication_state_provider { get; set; }

    [Inject]
    public required IAccessTokenProvider _token_provider { get; set; }

    private const int _default_duration_ms = 800;
    private const string _server_url = "https://intercom.byteloch.com/";
    private const string _referrer_url = "https://kibblewhite.github.io/Intercom/";
    private const string _origin_url = "https://kibblewhite.github.io";
    private const string _basic_authentication_credentials = "admin:esp8266";

    protected bool _processing = false;
    protected int _gpio = 0;
    protected int _duration_ms = _default_duration_ms;

    protected override async Task OnInitializedAsync()
    {
        AuthenticationState authentication_state = await _authentication_state_provider.GetAuthenticationStateAsync();
        AccessTokenResult access_token_result = await _token_provider.RequestAccessToken();

        if (authentication_state is null || access_token_result is null)
        {
            Console.WriteLine($"Null Check / authentication_state: {authentication_state is null} | access_token_result: {access_token_result is null}");
            return;
        }

        AuthenticationStateDto authentication_state_dto = new()
        {
            Claims = authentication_state.User.Claims,
            Identities = authentication_state.User.Identities,
            Identity = authentication_state.User.Identity
        };
        

        string authentication_state_json = JsonSerializer.Serialize(authentication_state_dto);
        Console.WriteLine(authentication_state_json);

        string access_token_result_json = JsonSerializer.Serialize(access_token_result);
        Console.WriteLine(access_token_result_json);
    }

    protected async Task DurationChangedRequest(int duration_ms)
        => await Task.FromResult(_duration_ms = duration_ms);

    private async Task ProcessDoorToggleRequest()
    {

        _processing = true;

        try
        {

            Uri url = new Uri(_server_url)
                .AddQuery("duration", _duration_ms.ToString())
                .AddQuery("gpio", _gpio.ToString());

            // Create an HttpClient with the public key
            HttpClientHandler handler = new();
            using HttpClient httpClient = new(handler);

            // autnetication, duh...
            string basic_authentication_string = Convert.ToBase64String(Encoding.UTF8.GetBytes(_basic_authentication_credentials));

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basic_authentication_string);
            httpClient.DefaultRequestHeaders.Referrer = new Uri(_referrer_url);
            httpClient.DefaultRequestHeaders.Add("Origin", _origin_url);

            // Send a request to the server
            HttpResponseMessage response = await httpClient.GetAsync(url);

            await Task.Delay(_duration_ms);
            if (response.IsSuccessStatusCode)
            {
                Snackbar.Add("Request was successful. Push gate or door firmly to enter.", Severity.Success);
            }
            else
            {
                Snackbar.Add($"Request failed with status code: {response.StatusCode}", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Request failed with exception message: {ex.Message}", Severity.Error);
        }
        finally
        {
            _processing = false;
        }
    }
}