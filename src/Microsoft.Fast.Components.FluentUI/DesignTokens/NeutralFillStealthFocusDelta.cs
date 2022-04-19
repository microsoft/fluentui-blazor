using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillStealthFocusDelta design token
/// </summary>
public sealed class NeutralFillStealthFocusDelta : DesignToken<int?>
{
    /// <summary>
    /// Constructs an instance of the NeutralFillStealthFocusDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillStealthFocusDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillStealthFocusDelta;
    }
}
