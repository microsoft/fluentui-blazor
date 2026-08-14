// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
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
    /// Gets an empty instance of the <see cref="LibraryConfiguration"/> class.
    /// Mainly used for testing purposes or when no configuration is needed.
    /// </summary>
    internal static LibraryConfiguration Empty { get; } = new LibraryConfiguration();

    /// <summary>
    /// Gets the assembly version formatted as a string.
    /// </summary>
    public static readonly string? AssemblyVersion = typeof(LibraryConfiguration).Assembly.GetName().Version?.ToString();

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
    /// Gets the default CSS class and styles for the library components.
    /// </summary>
    public DefaultStyles DefaultStyles { get; } = new DefaultStyles();

    /// <summary>
    /// Gets the default CSS class and styles for the library components.
    /// </summary>
    public DefaultValues DefaultValues { get; } = new DefaultValues();

    /// <summary>
    /// Gets the options for the library tooltip.
    /// </summary>
    public LibraryTooltipOptions Tooltip { get; } = new LibraryTooltipOptions();

    /// <summary>
    /// Gets the options for the library toast.
    /// </summary>
    public LibraryToastOptions Toast { get; } = new LibraryToastOptions();

    /// <summary>
    /// Gets or sets a value indicating whether Fluent input components should use the
    /// browser's native constraint validation UI by default (e.g., showing built-in
    /// validation bubbles). Default is false. Consumers can opt-in to enable the
    /// native validation UI for parity with existing browser behavior.
    /// </summary>
    public bool UseNativeConstraintValidationUI { get; set; }

    /// <summary>
    /// Gets the sanitized markup string for safe rendering in HTML/Styles contexts.
    /// </summary>
    public MarkupSanitizedOptions MarkupSanitized { get; } = new MarkupSanitizedOptions();

    /// <summary>
    /// Gets or sets a value indicating whether to use the global overlay via the <see cref="IDialogService"/>.
    /// The global overlay is a single instance of the <see cref="FluentOverlay"/> component rendered by the <see cref="FluentDialogProvider"/>.
    /// </summary>
    public bool UseGlobalOverlay { get; set; } = true;

    /// <summary>
    /// Static backing store for <see cref="ValidateClassNames"/>: shared process-wide, not per <see cref="LibraryConfiguration"/> instance.
    /// </summary>
    private static bool s_validateClassNames = true;

    /// <summary>
    /// Gets or sets the value indicating whether the library should validate CSS class names.
    /// respecting the following regex: "^-?[_a-zA-Z]+[_a-zA-Z0-9-]*$".
    /// Default is true.
    /// </summary>
    /// <remarks>
    /// This setting is stored statically, so changing it applies to the whole application (all DI scopes/instances),
    /// not just the current <see cref="LibraryConfiguration"/> instance.
    /// </remarks>
    public bool ValidateClassNames
    {
        get => s_validateClassNames;
        set => s_validateClassNames = value;
    }

    /// <summary>
    /// Gets the current, process-wide <see cref="ValidateClassNames"/> value for use by <see cref="Utilities.CssBuilder"/>.
    /// </summary>
    internal static bool ShouldValidateClassNames => s_validateClassNames;

    /* TODO: Implement these properties

    /// <summary>
    /// Gets or sets the function that formats the URL of the collocated JavaScript file,
    /// adding the return value as a query string parameter.
    /// By default, the function adds a query string parameter with the version of the assembly: `v=[AssemblyVersion]`.
    /// </summary>
    public Func<string, string>? CollocatedJavaScriptQueryString { get; set; } = (url)
        => string.IsNullOrEmpty(AssemblyVersion) ? string.Empty : $"v={AssemblyVersion}";
    */
}
