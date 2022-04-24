using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentButton
{
    [Parameter]
    public Appearance? Appearance { get; set; }

    [Parameter]
    public bool? Disabled { get; set; }

    [Parameter]
    public bool? Autofocus { get; set; }

    [Parameter]
    public string? IconStart { get; set; }

    [Parameter]
    public string? IconEnd { get; set; }

    [Parameter]
    public IconSize IconSize { get; set; } = FluentUI.IconSize.Size20;

    [Parameter]
    public bool IconAccentColor { get; set; } = false;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? AdditionalAttributes { get; set; }
}