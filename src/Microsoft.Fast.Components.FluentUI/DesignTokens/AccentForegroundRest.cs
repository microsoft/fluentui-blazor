using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentForegroundRest design token
/// </summary>
public sealed class AccentForegroundRest : DesignToken<int?>
{
    public AccentForegroundRest()
    {
        Name = Constants.AccentForegroundRest;
    }

    /// <summary>
    /// Constructs an instance of the AccentForegroundRest design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentForegroundRest(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentForegroundRest;
    }
}
