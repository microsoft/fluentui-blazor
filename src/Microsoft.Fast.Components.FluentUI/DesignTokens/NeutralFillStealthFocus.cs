using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillStealthFocus design token
/// </summary>
public sealed class NeutralFillStealthFocus : DesignToken<int?>
{
    public NeutralFillStealthFocus()
    {
        Name = Constants.NeutralFillStealthFocus;

    }

    /// <summary>
    /// Constructs an instance of the NeutralFillStealthFocus design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillStealthFocus(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillStealthFocus;
    }
}
