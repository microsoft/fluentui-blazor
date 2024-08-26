// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Components.DataGrid.Infrastructure;

public record ResizeAriaLabels
{
    public ResizeAriaLabels(string growAriaLabel, string shrinkAriaLabel, string resetAriaLabel, string SubmitAriaLabel)
    {
    }

    public string? GrowLabel { get; set; }
    public string? ShrinkLabel { get; set; }
    public string? ResetLabel { get; set; }
    public string? SubmitLabel { get; set; }

    public ResizeAriaLabels Default => new(
            GrowLabel = "Grow column width",
            ShrinkLabel = "Shrink column width",
            ResetLabel = "Reset column widths",
            SubmitLabel = "Set column widths");
    }
}
