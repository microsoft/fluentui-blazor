using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

// If needed, additional services configuration objects can be added here

/// <summary>
/// Defines the global Fluent UI Blazor component library services configuration
/// </summary>
public class LibraryConfiguration
{
    private static readonly string? AssemblyVersion = typeof(LibraryConfiguration).Assembly.GetName().Version?.ToString();

    /// <summary>
    /// Gets or sets a value indicating whether the library should use the TooltipServiceProvider.
    /// If set to true, add the FluentTooltipProvider component at end of the MainLayout.razor page.
    /// </summary>
    public bool UseTooltipServiceProvider { get; set; } = true;

    /// <summary>
    /// Gets or sets the required label for the form fields.
    /// </summary>
    public MarkupString RequiredLabel { get; set; } = (MarkupString)
        """
        <span aria-label="required" aria-hidden="true" style="padding-inline-start: calc(var(--design-unit) * 1px); color: var(--error); pointer-events: none;">*</span>
        """;

    /// <summary>
    /// Gets or sets the value indicating whether the library should close the tooltip if the cursor leaves the anchor and the tooltip.
    /// By default, the tooltip closes if the cursor leaves the anchor, but not the tooltip itself.
    /// </summary>
    public bool HideTooltipOnCursorLeave { get; set; } = false;

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
    /// Gets or sets the function that returns the URL of the JavaScript file.
    /// </summary>
    public Func<string, string>? FormatCollocatedJavaScriptUrl { get; set; } = (string url) =>
    {
        if (string.IsNullOrEmpty(AssemblyVersion))
        {
            return url;
        }

        return $"{url}?v={AssemblyVersion}";
    };

    public LibraryConfiguration()
    {
    }

    /// <summary>
    internal static LibraryConfiguration ForUnitTests => new()
    {
        FormatCollocatedJavaScriptUrl = null,
    };
}
