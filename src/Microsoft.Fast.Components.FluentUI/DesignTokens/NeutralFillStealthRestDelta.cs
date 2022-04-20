using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillStealthRestDelta design token
/// </summary>
public sealed class NeutralFillStealthRestDelta : DesignToken<int?>
{
    public NeutralFillStealthRestDelta()
    {
        Name = Constants.NeutralFillStealthRestDelta;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillStealthRestDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillStealthRestDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillStealthRestDelta;
    }
}
