using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralStrokeRestDelta design token
/// </summary>
public sealed class NeutralStrokeRestDelta : DesignToken<int?>
{
    public NeutralStrokeRestDelta()
    {
        Name = Constants.NeutralStrokeRestDelta;
    }

    /// <summary>
    /// Constructs an instance of the NeutralStrokeRestDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralStrokeRestDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralStrokeRestDelta;
    }
}
