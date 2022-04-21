using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillInputActive design token
/// </summary>
public sealed class NeutralFillInputActive : DesignToken<int?>
{
    public NeutralFillInputActive()
    {
        Name = Constants.NeutralFillInputActive;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillInputActive design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillInputActive(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillInputActive;
    }
}
