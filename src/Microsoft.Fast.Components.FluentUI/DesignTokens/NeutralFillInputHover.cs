using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillInputHover design token
/// </summary>
public sealed class NeutralFillInputHover : DesignToken<int?>
{
    public NeutralFillInputHover()
    {
        Name = Constants.NeutralFillInputHover;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillInputHover design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillInputHover(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillInputHover;
    }
}
