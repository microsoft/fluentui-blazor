using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillRest design token
/// </summary>
public sealed class NeutralFillRest : DesignToken<int?>
{
    public NeutralFillRest()
    {
        Name = Constants.NeutralFillRest;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillRest design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillRest(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillRest;
    }
}
