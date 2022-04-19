using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampPlus3LineHeight design token
/// </summary>
public sealed class TypeRampPlus3LineHeight : DesignToken<float?>
{
    /// <summary>
    /// Constructs an instance of the TypeRampPlus3LineHeight design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampPlus3LineHeight(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampPlus3LineHeight;
    }
}
