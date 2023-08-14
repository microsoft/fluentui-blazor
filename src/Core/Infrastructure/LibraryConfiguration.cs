﻿namespace Microsoft.Fast.Components.FluentUI;

// If needed, additional services configuration objects can be added here

/// <summary>
/// Defines the global Fluent UI Blazor component library services configuration
/// </summary>
public class LibraryConfiguration
{
    public BlazorHostingModel HostingModel { get; set; } = BlazorHostingModel.NotSpecified;

    public StaticAssetServiceConfiguration StaticAssetServiceConfiguration { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether the library should use the TooltipServiceProvider.
    /// If set to true, add the FluentTooltipProvider component at end of the MainLayout.razor page.
    /// </summary>
    public bool UseTooltipServiceProvider { get; set; } = true;

    public LibraryConfiguration()
    {
    }        
}
