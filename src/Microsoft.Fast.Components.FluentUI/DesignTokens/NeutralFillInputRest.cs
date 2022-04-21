using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillInputRest design token
/// </summary>
public sealed class NeutralFillInputRest : DesignToken<int?>
{
    public NeutralFillInputRest()
    {
        Name = Constants.NeutralFillInputRest;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillInputRest design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillInputRest(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillInputRest;
    }
}
