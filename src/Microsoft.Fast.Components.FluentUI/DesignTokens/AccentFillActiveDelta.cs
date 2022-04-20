using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentFillActiveDelta design token
/// </summary>
public sealed class AccentFillActiveDelta : DesignToken<int?>
{
    public AccentFillActiveDelta()
    {
        Name = Constants.AccentFillActiveDelta;
    }

    /// <summary>
    /// Constructs an instance of the AccentFillActiveDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentFillActiveDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentFillActiveDelta;
    }
}
