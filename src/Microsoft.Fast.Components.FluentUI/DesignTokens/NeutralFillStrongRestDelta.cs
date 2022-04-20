using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillStrongRestDelta design token
/// </summary>
public sealed class NeutralFillStrongRestDelta : DesignToken<int?>
{
    public NeutralFillStrongRestDelta()
    {
        Name = Constants.NeutralFillStrongRestDelta;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillStrongRestDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillStrongRestDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillStrongRestDelta;
    }
}
