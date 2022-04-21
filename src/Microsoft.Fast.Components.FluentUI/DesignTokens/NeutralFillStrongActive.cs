using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillStrongActive design token
/// </summary>
public sealed class NeutralFillStrongActive : DesignToken<int?>
{
    public NeutralFillStrongActive()
    {
        Name = Constants.NeutralFillStrongActive;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillStrongActive design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillStrongActive(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillStrongActive;
    }
}
