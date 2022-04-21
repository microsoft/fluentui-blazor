using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentFillFocus design token
/// </summary>
public sealed class AccentFillFocus : DesignToken<int?>
{
    public AccentFillFocus()
    {
        Name = Constants.AccentFillFocus;
    }

    /// <summary>
    /// Constructs an instance of the AccentFillFocus design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentFillFocus(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentFillFocus;
    }
}
