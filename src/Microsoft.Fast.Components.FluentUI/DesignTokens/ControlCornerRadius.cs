using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The ControlCornerRadius design token
/// </summary>
public sealed class ControlCornerRadius : DesignToken<int?>
{
    public ControlCornerRadius()
    {
        Name = Constants.ControlCornerRadius;
    }

    /// <summary>
    /// Constructs an instance of the ControlCornerRadius design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public ControlCornerRadius(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.ControlCornerRadius;
    }
}
