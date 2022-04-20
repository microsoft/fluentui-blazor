using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillFocusDelta design token
/// </summary>
public sealed class NeutralFillFocusDelta : DesignToken<int?>
{
    public NeutralFillFocusDelta()
    {
        Name = Constants.NeutralFillFocusDelta;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillFocusDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillFocusDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillFocusDelta;
    }
}
