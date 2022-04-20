using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampBaseFontSize design token
/// </summary>
public sealed class TypeRampBaseFontSize : DesignToken<float?>
{
    public TypeRampBaseFontSize()
    {
        Name = Constants.TypeRampBaseFontSize;
    }

    /// <summary>
    /// Constructs an instance of the TypeRampBaseFontSize design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampBaseFontSize(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampBaseFontSize;
    }
}
