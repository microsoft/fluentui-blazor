using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampPlus5FontSize design token
/// </summary>
public sealed class TypeRampPlus5FontSize : DesignToken<float?>
{
    public TypeRampPlus5FontSize()
    {
        Name = Constants.TypeRampPlus5FontSize;
    }

    /// <summary>
    /// Constructs an instance of the TypeRampPlus5FontSize design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampPlus5FontSize(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampPlus5FontSize;
    }
}
