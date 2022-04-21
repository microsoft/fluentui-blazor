using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillInputFocus design token
/// </summary>
public sealed class NeutralFillInputFocus : DesignToken<int?>
{
    public NeutralFillInputFocus()
    {
        Name = Constants.NeutralFillInputFocus;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillInputFocus design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillInputFocus(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillInputFocus;
    }
}
