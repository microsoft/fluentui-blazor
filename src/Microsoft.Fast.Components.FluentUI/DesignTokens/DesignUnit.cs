using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The DesignUnit design token
/// </summary>
public sealed class DesignUnit : DesignToken<int?>
{
    /// <summary>
    /// Constructs an instance of the DesignUnit design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public DesignUnit(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.DesignUnit;
    }
}
