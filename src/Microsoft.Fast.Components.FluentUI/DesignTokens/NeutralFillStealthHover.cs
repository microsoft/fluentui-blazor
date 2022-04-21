using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillStealthHover design token
/// </summary>
public sealed class NeutralFillStealthHover : DesignToken<int?>
{
    public NeutralFillStealthHover()
    {
        Name = Constants.NeutralFillStealthHover;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillStealthHover design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillStealthHover(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillStealthHover;
    }
}
