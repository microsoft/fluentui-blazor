using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The BaseHeightMultiplier design token
/// </summary>
public sealed class BaseHeightMultiplier : DesignToken<int?>
{
    /// <summary>
    /// Constructs an instance of a BaseHeightMultiplier design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public BaseHeightMultiplier(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.BaseHeightMultiplier;
    }
}
