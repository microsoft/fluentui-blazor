using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampPlus6LineHeight design token
/// </summary>
public sealed class TypeRampPlus6LineHeight : DesignToken<float?>
{
    public TypeRampPlus6LineHeight()
    {
        Name = Constants.TypeRampPlus6LineHeight;
    }

    /// <summary>
    /// Constructs an instance of the TypeRampPlus6LineHeight design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampPlus6LineHeight(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampPlus6LineHeight;
    }
}
