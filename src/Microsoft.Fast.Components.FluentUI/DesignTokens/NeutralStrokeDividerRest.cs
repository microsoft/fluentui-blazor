using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralStrokeDividerRest design token
/// </summary>
public sealed class NeutralStrokeDividerRest : DesignToken<int?>
{
    public NeutralStrokeDividerRest()
    {
        Name = Constants.NeutralStrokeDividerRest;
    }

    /// <summary>
    /// Constructs an instance of the NeutralStrokeDividerRest design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralStrokeDividerRest(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralStrokeDividerRest;
    }
}
