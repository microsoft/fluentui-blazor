using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentForegroundHoverDelta design token
/// </summary>
public sealed class AccentForegroundHoverDelta : DesignToken<int?>
{
    public AccentForegroundHoverDelta()
    {
        Name = Constants.AccentForegroundHoverDelta;
    }

    /// <summary>
    /// Constructs an instance of the AccentForegroundHoverDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentForegroundHoverDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentForegroundHoverDelta;
    }
}
