using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampPlus4LineHeight design token
/// </summary>
public sealed class TypeRampPlus4LineHeight : DesignToken<float?>
{
    /// <summary>
    /// Constructs an instance of the TypeRampPlus4LineHeight design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampPlus4LineHeight(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampPlus4LineHeight;
    }
}
