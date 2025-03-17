// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for the Fluent UI Blazor component library.
/// </summary>
public class LibraryTooltipOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LibraryTooltipOptions"/> class.
    /// </summary>
    internal LibraryTooltipOptions()
    {
    }

    /// <summary>
    /// Gets or sets a value indicating whether the library should use the TooltipServiceProvider.
    /// If set to true, add the FluentTooltipProvider component at end of the MainLayout.razor page.
    /// </summary>
    public bool UseServiceProvider { get; set; } = true;
}
