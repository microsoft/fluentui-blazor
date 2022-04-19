using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralStrokeFocusDelta design token
/// </summary>
public sealed class NeutralStrokeFocusDelta : DesignToken<int?>
{
    /// <summary>
    /// Constructs an instance of the NeutralStrokeFocusDelta design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralStrokeFocusDelta(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralStrokeFocusDelta;
    }
}
