using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillFocus design token
/// </summary>
public sealed class NeutralFillFocus : DesignToken<int?>
{
    public NeutralFillFocus()
    {
        Name = Constants.NeutralFillFocus;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillFocus design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillFocus(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillFocus;
    }
}
