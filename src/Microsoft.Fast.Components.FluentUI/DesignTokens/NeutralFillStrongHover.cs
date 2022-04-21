using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillStrongHover design token
/// </summary>
public sealed class NeutralFillStrongHover : DesignToken<int?>
{
    public NeutralFillStrongHover()
    {
        Name = Constants.NeutralFillStrongHover;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillStrongHover design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillStrongHover(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillStrongHover;
    }
}
