using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentForegroundActiveDelta design token
/// </summary>
public sealed class AccentForegroundActiveDelta : DesignToken<int?>
{
    public AccentForegroundActiveDelta()
    {
        Name = Constants.AccentForegroundActiveDelta;
    }

    /// <summary>
    /// Constructs an instance of the AccentForegroundActiveDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentForegroundActiveDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentForegroundActiveDelta;
    }
}
