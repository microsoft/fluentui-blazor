using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralStrokeDividerRestDelta design token
/// </summary>
public sealed class NeutralStrokeDividerRestDelta : DesignToken<int?>
{
    /// <summary>
    /// Constructs an instance of the NeutralStrokeDividerRestDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralStrokeDividerRestDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralStrokeDividerRestDelta;
    }
}
