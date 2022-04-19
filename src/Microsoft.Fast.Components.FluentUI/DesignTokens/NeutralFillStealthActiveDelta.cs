using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillStealthActiveDelta design token
/// </summary>
public sealed class NeutralFillStealthActiveDelta : DesignToken<int?>
{
    /// <summary>
    /// Constructs an instance of the NeutralFillStealthActiveDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillStealthActiveDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillStealthActiveDelta;
    }
}
