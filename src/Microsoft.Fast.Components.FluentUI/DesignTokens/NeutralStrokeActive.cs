using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI.DesignTokens;

/// <summary>
/// The NeutralStrokeActive design token
/// </summary>
public sealed class NeutralStrokeActive : DesignToken<int?>
{
    public NeutralStrokeActive()
    {
        Name = Constants.NeutralStrokeActive;
    }

    /// <summary>
    /// Constructs an instance of the NeutralStrokeActive design token
    /// </summary>
    /// <param name="jsRuntime">IJSRuntime reference (from DI)</param>
    /// <param name="configuration">IConfiguration reference (from DI)</param>
    public NeutralStrokeActive(IJSRuntime jsRuntime, IConfiguration configuration) : base(jsRuntime, configuration)
    {
        Name = Constants.NeutralStrokeActive;
    }
}
