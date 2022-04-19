using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampPlus5LineHeight design token
/// </summary>
public sealed class TypeRampPlus5LineHeight : DesignToken<float?>
{
    /// <summary>
    /// Constructs an instance of the TypeRampPlus5LineHeight design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampPlus5LineHeight(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampPlus5LineHeight;
    }
}
