using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampPlus3FontSize design token
/// </summary>
public sealed class TypeRampPlus3FontSize : DesignToken<float?>
{
    public TypeRampPlus3FontSize()
    {
        Name = Constants.TypeRampPlus3FontSize;
    }

    /// <summary>
    /// Constructs an instance of the TypeRampPlus3FontSize design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampPlus3FontSize(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampPlus3FontSize;
    }
}
