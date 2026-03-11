// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Client.Documentation.Components.Theme.Designer;

public partial class CreateAndAlterTheme
{
    private ElementReference _exampleBlocks;
    private Microsoft.FluentUI.AspNetCore.Components.Theme? _customTheme = new();
    private readonly ThemeSettings _themeSettings = new("#0FFC0D", 0, 0, ThemeMode.Light, false);

    [Inject]
    private IThemeService ThemeService { get; set; } = default!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _customTheme = await ThemeService.CreateCustomThemeAsync(_themeSettings);
        }
    }

    private async Task ApplyCustomThemeAsync()
    {
        if (_customTheme is not null)
        {
            _customTheme.Borders.Radius.Medium = "40px";
            _customTheme.Fonts.Family.Base = "Comic Sans MS, cursive, sans-serif";
            _customTheme.Fonts.Size.Base300 = "10px";
            _customTheme.Lines.Height.Base300 = "8px";

            await ThemeService.SetThemeToElementAsync(_exampleBlocks, _customTheme);
        }
    }
}
