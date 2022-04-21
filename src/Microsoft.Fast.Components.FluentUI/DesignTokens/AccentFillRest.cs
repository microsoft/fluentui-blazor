using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentFillRest design token
/// </summary>
public sealed class AccentFillRest : DesignToken<int?>
{
    public AccentFillRest()
    {
        Name = Constants.AccentFillRest;
    }

    /// <summary>
    /// Constructs an instance of the AccentFillRest design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentFillRest(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentFillRest;
    }
}
