using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Intercom.Shared;

public partial class LoginDisplay
{
    [Inject]
    private NavigationManager? _navigation_manager { get; init; }

    protected void NavigateLogin()
    {
        ArgumentNullException.ThrowIfNull(_navigation_manager);
        _navigation_manager.NavigateToLogout("authentication/login");
    }

    protected void NavigateLogout()
    {
        ArgumentNullException.ThrowIfNull(_navigation_manager);
        _navigation_manager.NavigateToLogout("authentication/logout");
    }
}
