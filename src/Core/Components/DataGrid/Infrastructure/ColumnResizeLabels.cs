// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

public record ColumnResizeLabels(string DiscreteLabel = "Column width",
    string ExactLabel = "Column width (in pixels)",
    string GrowAriaLabel = "Grow column width",
    string ShrinkAriaLabel = "Shrink column width",
    string ResetAriaLabel = "Reset column widths",
    string SubmitAriaLabel = "Set column widths");
