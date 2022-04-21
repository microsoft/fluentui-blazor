using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralFillStealthRest design token
/// </summary>
public sealed class NeutralFillStealthRest : DesignToken<int?>
{
    public NeutralFillStealthRest()
    {
        Name = Constants.NeutralFillStealthRest;
    }

    /// <summary>
    /// Constructs an instance of the NeutralFillStealthRest design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralFillStealthRest(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralFillStealthRest;
    }
}
