// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

public record ColumnResizeLabels
{
    /// <summary>
    /// Gets or sets the text shown in the column menu
    /// </summary>
    public string ResizeMenu { get; set; } = "Resize";

    /// <summary>
    /// Gets or sets the label in the discrete mode resize UI
    /// </summary>
    public string DiscreteLabel { get; set; } = "Column width";

    /// <summary>
    /// Gets or sets the label in the exact mode resize UI
    /// </summary>
    public string ExactLabel { get; set; } = "Column width (in pixels)";

    /// <summary>
    /// Gets or sets the aria label for the grow button in the discrete resize UI
    /// </summary>
    public string? GrowAriaLabel { get; set; } = "Grow column width";

    /// <summary>
    /// Gets or sets the aria label for the shrink button in the discrete resize UI
    /// </summary>
    public string? ShrinkAriaLabel { get; set; } = "Shrink column width";

    /// <summary>
    /// Gets or sets the aria label for the reset button in the resize UI
    /// </summary>
    public string? ResetAriaLabel { get; set; } = "Reset column widths";

    /// <summary>
    /// Gets or sets the aria label for the submit button in the resize UI
    /// </summary>
    public string? SubmitAriaLabel { get; set; } = "Set column widths";

    /// <summary>
    /// Gets or sets the icon to show in the column menu
    /// </summary>
    public Icon? Icon { get; set; } = new CoreIcons.Regular.Size16.TableResizeColumn();

    /// <summary>
    /// Gets or sets whether the icon is positioned at the start (true) or
    /// at the end (false) of the menu item
    /// </summary>
    public bool IconPositionStart { get; set; } = true;

    /// <summary>
    /// Gets the default labels for the resize UI.
    /// </summary>
    public static ColumnResizeLabels Default => new();
}
