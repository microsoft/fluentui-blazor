using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentFillRestDelta design token
/// </summary>
public sealed class AccentFillRestDelta : DesignToken<int?>
{
    /// <summary>
    /// Constructs an instance of the AccentFillRestDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentFillRestDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentFillRestDelta;
    }
}
