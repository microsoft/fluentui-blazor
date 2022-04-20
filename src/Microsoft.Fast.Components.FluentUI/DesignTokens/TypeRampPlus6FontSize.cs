using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampPlus6FontSize design token
/// </summary>
public sealed class TypeRampPlus6FontSize : DesignToken<float?>
{
    public TypeRampPlus6FontSize()
    {
        Name = Constants.TypeRampPlus6FontSize;
    }

    /// <summary>
    /// Constructs an instance of the TypeRampPlus6FontSize design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampPlus6FontSize(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampPlus6FontSize;
    }
}
