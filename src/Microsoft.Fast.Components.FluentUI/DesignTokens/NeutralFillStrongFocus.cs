using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillStrongFocus design token
/// </summary>
public sealed class NeutralFillStrongFocus : DesignToken<int?>
{
    public NeutralFillStrongFocus()
    {
        Name = Constants.NeutralFillStrongFocus;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillStrongFocus design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillStrongFocus(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillStrongFocus;
    }
}
