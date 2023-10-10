using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Intercom.Shared;

public partial class RedirectToLogin
{
    [Inject]
    private NavigationManager? _navigation_manager { get; init; }

    protected override void OnInitialized()
    {
        ArgumentNullException.ThrowIfNull(_navigation_manager);
        _navigation_manager.NavigateToLogin("authentication/login");
    }
}
