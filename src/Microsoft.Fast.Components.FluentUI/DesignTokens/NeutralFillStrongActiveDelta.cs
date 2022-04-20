using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillStrongActiveDelta design token
/// </summary>
public sealed class NeutralFillStrongActiveDelta : DesignToken<int?>
{
    public NeutralFillStrongActiveDelta()
    {
        Name = Constants.NeutralFillStrongActiveDelta;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillStrongActiveDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillStrongActiveDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillStrongActiveDelta;
    }
}
