using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The BaseLayerLuminance design token
/// </summary>
public sealed class BaseLayerLuminance : DesignToken<float?>
{
    /// <summary>
    /// Constructs an instance of the BaseLayerLuminance design token
    /// </summary>
    public BaseLayerLuminance()
    {
        Name = Constants.BaseLayerLuminance;
    }

    /// <summary>
    /// Constructs an instance of the BaseLayerLuminance design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public BaseLayerLuminance(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.BaseLayerLuminance;
    }
}
