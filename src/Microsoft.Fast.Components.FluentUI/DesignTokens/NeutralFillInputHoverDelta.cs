using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillInputHoverDelta design token
/// </summary>
public sealed class NeutralFillInputHoverDelta : DesignToken<int?>
{
    public NeutralFillInputHoverDelta()
    {
        Name = Constants.NeutralFillInputHoverDelta;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillInputHoverDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillInputHoverDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillInputHoverDelta;
    }
}
