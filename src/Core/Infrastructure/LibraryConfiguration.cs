// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Defines the global Fluent UI Blazor component library services configuration
/// </summary>
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
public class LibraryConfiguration
{
    /// <summary>
    /// Gets the assembly version formatted as a string.
    /// </summary>
    public static readonly string? AssemblyVersion = typeof(LibraryConfiguration).Assembly.GetName().Version?.ToString();

    /// <summary>
    /// Gets or sets a value indicating whether the library should use the TooltipServiceProvider.
    /// If set to true, add the FluentTooltipProvider component at end of the MainLayout.razor page.
    /// </summary>
    public bool UseTooltipServiceProvider { get; set; } = true;

    /// <summary>
    /// Gets or sets the service lifetime for the library services, when using Fluent UI in WebAssembly, it can make sense to use <see cref="ServiceLifetime.Singleton"/>.
    /// Default is <see cref="ServiceLifetime.Scoped"/>.
    /// <para>Only <see cref="ServiceLifetime.Scoped"/> and <see cref="ServiceLifetime.Singleton"/> are supported.</para>
    /// </summary>
    public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Scoped;

    /// <summary>
    /// Gets or sets the FluentLocalizer instance used to localize the library components.
    /// </summary>
    public IFluentLocalizer? Localizer { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether the library should validate CSS class names.
    /// respecting the following regex: "^-?[_a-zA-Z]+[_a-zA-Z0-9-]*$".
    /// Default is true.
    /// </summary>
    public bool ValidateClassNames
    {
        get => Utilities.CssBuilder.ValidateClassNames;
        set => Utilities.CssBuilder.ValidateClassNames = value;
    }

    /// <summary>
    /// Gets or sets the function that formats the URL of the collocated JavaScript file,
    /// adding the return value as a query string parameter.
    /// By default, the function adds a query string parameter with the version of the assembly: `v=[AssemblyVersion]`.
    /// </summary>
    public Func<string, string>? CollocatedJavaScriptQueryString { get; set; } = (url)
        => string.IsNullOrEmpty(AssemblyVersion) ? string.Empty : $"v={AssemblyVersion}";
}
