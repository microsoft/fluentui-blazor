using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillHover design token
/// </summary>
public sealed class NeutralFillHover : DesignToken<int?>
{
    public NeutralFillHover()
    {
        Name = Constants.NeutralFillHover;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillHover design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillHover(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillHover;
    }
}
