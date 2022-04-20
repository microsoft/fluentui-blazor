using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillHoverDelta design token
/// </summary>
public sealed class NeutralFillHoverDelta : DesignToken<int?>
{
    public NeutralFillHoverDelta()
    {
        Name = Constants.NeutralFillHoverDelta;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillHoverDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillHoverDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillHoverDelta;
    }
}
