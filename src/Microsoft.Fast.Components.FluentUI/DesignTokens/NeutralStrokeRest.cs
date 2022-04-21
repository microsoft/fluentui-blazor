using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralStrokeRest design token
/// </summary>
public sealed class NeutralStrokeRest : DesignToken<int?>
{
    public NeutralStrokeRest()
    {
        Name = Constants.NeutralStrokeRest;
    }

    /// <summary>
    /// Constructs an instance of the NeutralStrokeRest design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralStrokeRest(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralStrokeRest;
    }
}
