using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentForegroundActive design token
/// </summary>
public sealed class AccentForegroundActive : DesignToken<int?>
{
    public AccentForegroundActive()
    {
        Name = Constants.AccentForegroundActive;
    }

    /// <summary>
    /// Constructs an instance of the AccentForegroundActive design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentForegroundActive(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentForegroundActive;
    }
}
