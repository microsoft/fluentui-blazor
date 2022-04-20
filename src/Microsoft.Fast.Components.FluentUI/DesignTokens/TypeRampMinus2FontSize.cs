using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampMinus2FontSize design token
/// </summary>
public sealed class TypeRampMinus2FontSize : DesignToken<float?>
{
    public TypeRampMinus2FontSize()
    {
        Name = Constants.TypeRampMinus2FontSize;
    }

    /// <summary>
    /// Constructs an instance of the TypeRampMinus2FontSize design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampMinus2FontSize(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampMinus2FontSize;
    }
}
