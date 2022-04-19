using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillStrongFocusDelta design token
/// </summary>
public sealed class NeutralFillStrongFocusDelta : DesignToken<int?>
{
    /// <summary>
    /// Constructs an instance of the NeutralFillStrongFocusDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillStrongFocusDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillStrongFocusDelta;
    }
}
