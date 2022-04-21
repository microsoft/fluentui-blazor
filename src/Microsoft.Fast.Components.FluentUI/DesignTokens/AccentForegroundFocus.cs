using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentForegroundFocus design token
/// </summary>
public sealed class AccentForegroundFocus : DesignToken<int?>
{
    public AccentForegroundFocus()
    {
        Name = Constants.AccentForegroundFocus;
    }

    /// <summary>
    /// Constructs an instance of the AccentForegroundFocus design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentForegroundFocus(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentForegroundFocus;
    }
}
