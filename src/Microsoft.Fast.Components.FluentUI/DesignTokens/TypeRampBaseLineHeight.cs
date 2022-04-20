using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The TypeRampBaseLineHeight design token
/// </summary>
public sealed class TypeRampBaseLineHeight : DesignToken<float?>
{
    public TypeRampBaseLineHeight()
    {
        Name = Constants.TypeRampBaseLineHeight;
    }

    /// <summary>
    /// Constructs an instance of the TypeRampBaseLineHeight design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public TypeRampBaseLineHeight(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.TypeRampBaseLineHeight;
    }
}
