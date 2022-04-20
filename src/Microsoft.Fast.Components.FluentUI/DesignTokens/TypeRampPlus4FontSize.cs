using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampPlus4FontSize design token
/// </summary>
public sealed class TypeRampPlus4FontSize : DesignToken<float?>
{
    public TypeRampPlus4FontSize()
    {
        Name = Constants.TypeRampPlus4FontSize;
    }

    /// <summary>
    /// Constructs an instance of the TypeRampPlus4FontSize design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampPlus4FontSize(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampPlus4FontSize;
    }
}
