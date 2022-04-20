using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampPlus2FontSize design token
/// </summary>
public sealed class TypeRampPlus2FontSize : DesignToken<float?>
{
    public TypeRampPlus2FontSize()
    {
        Name = Constants.TypeRampPlus2FontSize;
    }

    /// <summary>
    /// Constructs an instance of the TypeRampPlus2FontSize design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampPlus2FontSize(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampPlus2FontSize;
    }
}
