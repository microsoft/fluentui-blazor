using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentFillActive design token
/// </summary>
public sealed class AccentFillActive : DesignToken<int?>
{
    public AccentFillActive()
    {
        Name = Constants.AccentFillActive;
    }

    /// <summary>
    /// Constructs an instance of the AccentFillActive design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentFillActive(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentFillActive;
    }
}
