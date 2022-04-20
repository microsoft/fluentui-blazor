using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The StrokeWidth design token
/// </summary>
public sealed class StrokeWidth : DesignToken<int?>
{
    public StrokeWidth()
    {
        Name = Constants.StrokeWidth;
    }

    /// <summary>
    /// Constructs an instance of the StrokeWidth design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public StrokeWidth(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.StrokeWidth;
    }
}
