using MudBlazor;

namespace Intercom.Shared;

public partial class MainLayout
{
    private MudThemeProvider _mud_theme_provider;
    private bool _is_dark_mode;

    public MainLayout()
        => _mud_theme_provider = new MudThemeProvider();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender is false)
        {
            return;
        }

        ArgumentNullException.ThrowIfNull(_mud_theme_provider);
        _is_dark_mode = await _mud_theme_provider.GetSystemPreference();
        await _mud_theme_provider.WatchSystemPreference(DarkModeToggledRequestAsync);
        StateHasChanged();
    }

    private async Task DarkModeToggledRequestAsync(bool is_dark_mode)
    {
        await Task.FromResult(_is_dark_mode = is_dark_mode);
        StateHasChanged();
    }
}