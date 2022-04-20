using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The FocusStrokeWidth design token
/// </summary>
public sealed class FocusStrokeWidth : DesignToken<int?>
{
    public FocusStrokeWidth()
    {
        Name = Constants.FocusStrokeWidth;
    }

    /// <summary>
    /// Constructs an instance of the FocusStrokeWidth design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public FocusStrokeWidth(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.FocusStrokeWidth;
    }
}
