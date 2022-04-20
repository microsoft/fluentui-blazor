using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillInputRestDelta design token
/// </summary>
public sealed class NeutralFillInputRestDelta : DesignToken<int?>
{
    public NeutralFillInputRestDelta()
    {
        Name = Constants.NeutralFillInputRestDelta;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillInputRestDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillInputRestDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillInputRestDelta;
    }
}
