using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillInputActiveDelta design token
/// </summary>
public sealed class NeutralFillInputActiveDelta : DesignToken<int?>
{
    public NeutralFillInputActiveDelta()
    {
        Name = Constants.NeutralFillInputActiveDelta;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillInputActiveDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillInputActiveDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillInputActiveDelta;
    }
}
