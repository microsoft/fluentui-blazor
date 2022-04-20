using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampPlus2LineHeight design token
/// </summary>
public sealed class TypeRampPlus2LineHeight : DesignToken<float?>
{
    public TypeRampPlus2LineHeight()
    {
        Name = Constants.TypeRampPlus2LineHeight;
    }

    /// <summary>
    /// Constructs an instance of the TypeRampPlus2LineHeight design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampPlus2LineHeight(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampPlus2LineHeight;
    }
}
