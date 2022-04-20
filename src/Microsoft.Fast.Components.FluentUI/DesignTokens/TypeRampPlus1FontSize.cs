using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampPlus1FontSize design token
/// </summary>
public sealed class TypeRampPlus1FontSize : DesignToken<float?>
{
    public TypeRampPlus1FontSize()
    {
        Name = Constants.TypeRampPlus1FontSize;
    }

    /// <summary>
    /// Constructs an instance of the TypeRampPlus1FontSize design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampPlus1FontSize(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampPlus1FontSize;
    }
}
