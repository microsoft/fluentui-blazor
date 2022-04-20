using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillActiveDelta design token
/// </summary>
public sealed class NeutralFillActiveDelta : DesignToken<int?>
{
    public NeutralFillActiveDelta()
    {
        Name = Constants.NeutralFillActiveDelta;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillActiveDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillActiveDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillActiveDelta;
    }
}
