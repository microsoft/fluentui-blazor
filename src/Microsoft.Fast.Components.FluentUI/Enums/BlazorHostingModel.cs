namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// The ASP.NET Core Blazor hosing models
/// </summary>
public enum BlazorHostingModel
{
    /// <summary>
    /// No hosting model is specified
    /// </summary>
    NotSpecified,
    
    /// <summary>
    /// Blazor Server 
    /// </summary>
    Server,

    /// <summary>
    /// Blazor WebAssembly
    /// </summary>
    WebAssembly,

    /// <summary>
    /// Blazor Hybrid
    /// </summary>
    Hybrid
}
