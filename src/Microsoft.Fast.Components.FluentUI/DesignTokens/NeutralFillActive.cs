using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillActive design token
/// </summary>
public sealed class NeutralFillActive : DesignToken<int?>
{
    public NeutralFillActive()
    {
        Name = Constants.NeutralFillActive;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillActive design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillActive(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillActive;
    }
}
