using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentFillHover design token
/// </summary>
public sealed class AccentFillHover : DesignToken<int?>
{
    public AccentFillHover()
    {
        Name = Constants.AccentFillHover;
    }

    /// <summary>
    /// Constructs an instance of the AccentFillHover design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentFillHover(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentFillHover;
    }
}
