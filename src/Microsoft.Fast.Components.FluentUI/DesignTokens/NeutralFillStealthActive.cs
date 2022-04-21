using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillStealthActive design token
/// </summary>
public sealed class NeutralFillStealthActive : DesignToken<int?>
{
    public NeutralFillStealthActive()
    {
        Name = Constants.NeutralFillStealthActive;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillStealthActive design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillStealthActive(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillStealthActive;
    }
}
