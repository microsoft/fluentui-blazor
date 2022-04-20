using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The BaseHorizontalSpacingMultiplier design token
/// </summary>
public sealed class BaseHorizontalSpacingMultiplier : DesignToken<int?>
{
    public BaseHorizontalSpacingMultiplier()
    {
        Name = Constants.BaseHorizontalSpacingMultiplier;
    }

    /// <summary>
    /// Constructs an instance of the BaseHorizontalSpacingMultiplier design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public BaseHorizontalSpacingMultiplier(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.BaseHorizontalSpacingMultiplier;
    }
}
