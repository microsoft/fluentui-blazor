using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The AccentFillFocusDelta design token
/// </summary>
public sealed class AccentFillFocusDelta : DesignToken<int?>
{
    public AccentFillFocusDelta()
    {
        Name = Constants.AccentFillFocusDelta;
    }

    /// <summary>
    /// Constructs an instance of the AccentFillFocusDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public AccentFillFocusDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.AccentFillFocusDelta;
    }
}
