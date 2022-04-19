using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampMinus1LineHeight design token
/// </summary>
public sealed class TypeRampMinus1LineHeight : DesignToken<float?>
{
    /// <summary>
    /// Constructs an instance of the TypeRampMinus1LineHeight design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampMinus1LineHeight(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampMinus1LineHeight;
    }
}
