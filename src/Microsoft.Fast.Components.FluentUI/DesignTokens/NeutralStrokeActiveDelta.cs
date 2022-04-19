using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralStrokeActiveDelta design token
/// </summary>
public sealed class NeutralStrokeActiveDelta : DesignToken<int?>
{
    /// <summary>
    /// Constructs an instance of the NeutralStrokeActiveDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralStrokeActiveDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralStrokeActiveDelta;
    }
}
