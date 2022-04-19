using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillStealthHoverDelta design token
/// </summary>
public sealed class NeutralFillStealthHoverDelta : DesignToken<int?>
{
    /// <summary>
    /// Constructs an instance of the NeutralFillStealthHoverDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillStealthHoverDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillStealthHoverDelta;
    }
}
