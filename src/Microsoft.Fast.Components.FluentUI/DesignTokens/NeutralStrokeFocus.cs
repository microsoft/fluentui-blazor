using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralStrokeFocus design token
/// </summary>
public sealed class NeutralStrokeFocus : DesignToken<int?>
{
    public NeutralStrokeFocus()
    {
        Name = Constants.NeutralStrokeFocus;
    }

    /// <summary>
    /// Constructs an instance of the NeutralStrokeFocus design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralStrokeFocus(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralStrokeFocus;
    }
}
