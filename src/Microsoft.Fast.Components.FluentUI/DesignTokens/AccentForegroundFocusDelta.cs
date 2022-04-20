using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentForegroundFocusDelta design token
/// </summary>
public sealed class AccentForegroundFocusDelta : DesignToken<int?>
{
    public AccentForegroundFocusDelta()
    {
        Name = Constants.AccentForegroundFocusDelta;
    }

    /// <summary>
    /// Constructs an instance of the AccentForegroundFocusDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentForegroundFocusDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentForegroundFocusDelta;
    }
}
