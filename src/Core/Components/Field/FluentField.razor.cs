// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// 
/// </summary>
public partial class FluentField
{
    /// <summary />
    [Parameter]
    public string? Label { get; set; }

    /// <summary />
    [Parameter]
    public RenderFragment? LabelTemplate { get; set; }

    /// <summary />
    [Parameter]
    public FieldLabelPosition LabelPosition { get; set; } = FieldLabelPosition.Above;

    /// <summary />
    [Parameter]
    public string? LabelWidth { get; set; }

    /// <summary>
    /// Gets or sets whether the label show a required marking (red star).
    /// </summary>
    [Parameter]
    public bool Required { get; set; }

    /// <summary>
    /// Gets or sets the disabled state of the label.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary />
    [Parameter]
    public string? ForId { get; set; }

    /// <summary />
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    [Parameter]
    public string? Message { get; set; }

    /// <summary />
    [Parameter]
    public Icon? MessageIcon { get; set; }

    /// <summary />
    [Parameter]
    public RenderFragment? MessageTemplate { get; set; }

    private bool HasLabel => !string.IsNullOrWhiteSpace(Label) || LabelTemplate is not null;

    private bool HasMessage => !string.IsNullOrWhiteSpace(Message) || MessageTemplate is not null || MessageIcon is not null;

    private string GetStyle()
    {
        return $"margin-top: 8px; align-self: flex-start;{(string.IsNullOrEmpty(LabelWidth) ? "" : LabelWidth)}";
    }
}
