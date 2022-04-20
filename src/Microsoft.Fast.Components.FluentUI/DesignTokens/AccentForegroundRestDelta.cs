using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentForegroundRestDelta design token
/// </summary>
public sealed class AccentForegroundRestDelta : DesignToken<int?>
{
    public AccentForegroundRestDelta()
    {
        Name = Constants.AccentForegroundRestDelta;
    }

    /// <summary>
    /// Constructs an instance of the AccentForegroundRestDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentForegroundRestDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentForegroundRestDelta;
    }
}
