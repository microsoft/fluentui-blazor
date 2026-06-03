// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentLabelInfo component extends <see cref="FluentLabel"/> by displaying an
/// informational icon next to the label. When the user clicks the icon, a popover
/// shows additional text, an optional action link, or a custom template.
/// </summary>
public partial class FluentLabelInfo : FluentLabel, ILabelInfo
{
    private static readonly Icon DefaultInfoIcon = new CoreIcons.Regular.Size20.Info();
    private static readonly Icon DefaultInfoIconActive = new CoreIcons.Filled.Size20.Info().WithColor(Color.Primary);

    private readonly string _infoButtonId = $"info-{Identifier.NewId()}";

    private bool _infoOpened;

    /// <summary />
    public FluentLabelInfo(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected override string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-label-info")
        .Build();

    /// <summary>
    /// Gets or sets the FluentField component that this label is associated with.
    /// </summary>
    [CascadingParameter(Name = "FluentField")]
    internal FluentField? FluentField { get; set; }

    /// <summary>
    /// Gets or sets the icon displayed next to the label to indicate that additional
    /// information is available. Defaults to a regular Info icon.
    /// </summary>
    [Parameter]
    public Icon InfoIcon { get; set; } = DefaultInfoIcon;

    /// <summary>
    /// Gets or sets the icon displayed when the info popover is hovered or open.
    /// Defaults to a filled Info icon.
    /// </summary>
    [Parameter]
    public Icon InfoIconActive { get; set; } = DefaultInfoIconActive;

    /// <summary>
    /// Gets or sets the informational text displayed inside the popover when the user
    /// clicks the info icon.
    /// </summary>
    [Parameter]
    public string? InfoText { get; set; }

    /// <summary>
    /// Gets or sets the URL displayed as a "learn more" link inside the popover.
    /// </summary>
    [Parameter]
    public string? InfoActionLink { get; set; }

    /// <summary>
    /// Gets or sets the text displayed as a "learn more" link inside the popover.
    /// </summary>
    [Parameter]
    public string? InfoActionText { get; set; }

    /// <summary>
    /// Gets or sets the target for the "learn more" link inside the popover.
    /// Defaults to <see cref="LinkTarget.Blank"/> to open the link in a new tab.
    /// </summary>
    [Parameter]
    public LinkTarget InfoActionTarget { get; set; } = LinkTarget.Blank;

    /// <summary>
    /// Gets or sets a custom template rendered inside the popover.
    /// When set, <see cref="InfoText"/> and <see cref="InfoActionLink"/> are ignored.
    /// </summary>
    [Parameter]
    public RenderFragment? InfoTemplate { get; set; }

    /// <summary />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (FluentField is null && 
            (Size is not null || Weight is not null || Disabled || Required))
        {
            throw new InvalidOperationException("FluentLabelInfo must be used within a FluentField to set Size, Weight, Disabled, or Required parameters.");
        }
    }

    /// <summary />
    private bool HasInfoContent => InfoTemplate is not null
        || !string.IsNullOrEmpty(InfoText)
        || !string.IsNullOrEmpty(InfoActionLink);

    /// <summary />
    private Task OnInfoToggleAsync()
    {
        _infoOpened = !_infoOpened;
        return Task.CompletedTask;
    }
}
