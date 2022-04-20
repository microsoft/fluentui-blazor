using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The FillColor design token
/// </summary>
public sealed class FillColor : DesignToken<string>
{
    /// <summary>
    /// Constructs an instance of the FillColor design token
    /// </summary>
    public FillColor()
    {
        Name = Constants.FillColor;
    }

    /// <summary>
    /// Constructs an instance of the FillColor design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public FillColor(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.FillColor;
    }
}
