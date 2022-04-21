using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralStrokeHover design token
/// </summary>
public sealed class NeutralStrokeHover : DesignToken<int?>
{
    public NeutralStrokeHover()
    {
        Name = Constants.NeutralStrokeHover;
    }

    /// <summary>
    /// Constructs an instance of the NeutralStrokeHover design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralStrokeHover(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralStrokeHover;
    }
}
