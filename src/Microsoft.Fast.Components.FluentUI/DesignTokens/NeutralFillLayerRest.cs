using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillLayerRest design token
/// </summary>
public sealed class NeutralFillLayerRest : DesignToken<int?>
{
    public NeutralFillLayerRest()
    {
        Name = Constants.NeutralFillLayerRest;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillLayerRest design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillLayerRest(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillLayerRest;
    }
}
