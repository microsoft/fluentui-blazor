using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

public partial class FluentThemeProvider
{
    [Parameter]
    public string? Id { get; set; }

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public bool PersistTheme { get; set; } = true;

    protected async override Task OnInitializedAsync()
    {
        await ThemeService.InitializeAsync(PersistTheme);
    }
}