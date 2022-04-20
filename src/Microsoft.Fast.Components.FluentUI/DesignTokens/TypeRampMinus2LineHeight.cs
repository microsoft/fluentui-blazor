using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampMinus2LineHeight design token
/// </summary>
public sealed class TypeRampMinus2LineHeight : DesignToken<float?>
{
    public TypeRampMinus2LineHeight()
    {
        Name = Constants.TypeRampMinus2LineHeight;
    }

    /// <summary>
    /// Constructs an instance of the TypeRampMinus2LineHeight design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampMinus2LineHeight(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampMinus2LineHeight;
    }
}
