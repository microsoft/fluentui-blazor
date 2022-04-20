using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillLayerRestDelta design token
/// </summary>
public sealed class NeutralFillLayerRestDelta : DesignToken<int?>
{
    public NeutralFillLayerRestDelta()
    {
        Name = Constants.NeutralFillLayerRestDelta;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillLayerRestDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillLayerRestDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillLayerRestDelta;
    }
}
