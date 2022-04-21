using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentForegroundHover design token
/// </summary>
public sealed class AccentForegroundHover : DesignToken<int?>
{
    public AccentForegroundHover()
    {
        Name = Constants.AccentForegroundHover;
    }

    /// <summary>
    /// Constructs an instance of the AccentForegroundHover design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentForegroundHover(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentForegroundHover;
    }
}
