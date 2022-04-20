using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillRestDelta design token
/// </summary>
public sealed class NeutralFillRestDelta : DesignToken<int?>
{
    public NeutralFillRestDelta()
    {
        Name = Constants.NeutralFillRestDelta;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillRestDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillRestDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillRestDelta;
    }
}
