using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The LayerCornerRadius  design token
/// </summary>
public sealed class LayerCornerRadius : DesignToken<int?>
{
    public LayerCornerRadius()
    {
        Name = Constants.LayerCornerRadius;
    }

    /// <summary>
    /// Constructs an instance of the LayerCornerRadius  design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public LayerCornerRadius(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.LayerCornerRadius;
    }
}
