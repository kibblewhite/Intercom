using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Intercom.Pages;

public partial class Authentication
{
    [Parameter]
    public string? Action { get; set; }

    [Inject]
    public required AuthenticationStateProvider _authentication_state_provider { get; set; }

    [Inject]
    public required IAccessTokenProvider _token_provider { get; set; }

    protected override async Task OnInitializedAsync()
    {
        AuthenticationState authentication_state = await _authentication_state_provider.GetAuthenticationStateAsync();
        AccessTokenResult access_token_result = await _token_provider.RequestAccessToken();

        Console.WriteLine(authentication_state.ToString());
        Console.WriteLine(access_token_result.ToString());
    }
}
