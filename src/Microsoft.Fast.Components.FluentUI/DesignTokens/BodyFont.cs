using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The BodyFont design token
/// </summary>
public class BodyFont : DesignToken<string>
{
    public BodyFont()
    {
        Name = Constants.BodyFont;
    }

    /// <summary>
    /// Constructs an instance of the BodyFont design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public BodyFont(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.BodyFont;
    }
}
