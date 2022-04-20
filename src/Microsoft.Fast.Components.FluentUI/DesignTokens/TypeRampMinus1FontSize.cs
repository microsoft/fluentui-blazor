using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampMinus1FontSize design token
/// </summary>
public sealed class TypeRampMinus1FontSize : DesignToken<float?>
{
    public TypeRampMinus1FontSize()
    {
        Name = Constants.TypeRampMinus1FontSize;
    }

    /// <summary>
    /// Constructs an instance of the TypeRampMinus1FontSize design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampMinus1FontSize(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampMinus1FontSize;
    }
}
