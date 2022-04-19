using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentFillHoverDelta design token
/// </summary>
public sealed class AccentFillHoverDelta : DesignToken<int?>
{
    /// <summary>
    /// Constructs an instance of the AccentFillHoverDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentFillHoverDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentFillHoverDelta;
    }
}
