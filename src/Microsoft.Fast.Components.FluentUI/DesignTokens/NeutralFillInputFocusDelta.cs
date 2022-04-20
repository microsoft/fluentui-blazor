using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillInputFocusDelta design token
/// </summary>
public sealed class NeutralFillInputFocusDelta : DesignToken<int?>
{
    public NeutralFillInputFocusDelta()
    {
        Name = Constants.NeutralFillInputFocusDelta;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillInputFocusDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillInputFocusDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillInputFocusDelta;
    }
}
