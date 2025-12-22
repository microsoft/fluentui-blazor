// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Event arguments for the FluentAccordion expanded changed event.
/// </summary>
public class AccordionItemEventArgs : EventArgs
{
    /// <summary>
    /// The id of the accordion item that was changed.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Gets the item that was changed.
    /// </summary>
    public FluentAccordionItem? Item { get; internal set; }

    /// <summary>
    /// Gets or sets the expanded state of the accordion item.
    /// </summary>
    public bool Expanded { get; set; }

    /// <summary>
    /// Gets or sets the header text of the accordion item.
    /// When the item is expanded/collapsed by user interaction, this property is set to the innerText value of the item that was clicked.
    /// When an item is expanded/collapsed programmatically, this property is set to the value of the header parameter.
    /// </summary>
    public string? HeaderText { get; set; }
}
