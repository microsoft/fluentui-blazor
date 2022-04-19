using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The DisabledOpacity design token
/// </summary>
public sealed class DisabledOpacity : DesignToken<float?>
{
    /// <summary>
    /// Constructs an instance of a DisabledOpacity design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public DisabledOpacity(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.DisabledOpacity;
    }
}
