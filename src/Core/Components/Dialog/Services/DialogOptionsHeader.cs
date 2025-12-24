// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Options for configuring a dialog header.
/// </summary>
public class DialogOptionsHeader
{
    /// <summary />
    public DialogOptionsHeader()
    {
    }

    /// <summary>
    /// Gets or sets the title of the dialog.
    /// For security reasons, the content is sanitized using the configured <see cref="LibraryConfiguration.MarkupSanitized"/> before rendering.
    /// </summary>
    public string? Title { get; set; }
}
