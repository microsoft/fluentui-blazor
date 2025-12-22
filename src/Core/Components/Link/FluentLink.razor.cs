// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentLink component specifies relationships between the current document and an external resource.
/// </summary>
public partial class FluentLink : FluentComponentBase, ITooltipComponent
{
    /// <summary />
    public FluentLink(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("display", "flex", when:() => IconStart is not null || IconEnd is not null)
        .Build();

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Command executed when the user clicks on the button.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Get or set the href of the anchor.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Get or set the ISO Code language code that specifies the language of the linked document. It is purely advisory.
    /// </summary>
    [Parameter]
    public string? HrefLang { get; set; }

    /// <summary>
    /// Get or set a string indicating which referrer to use when fetching the resource.
    /// </summary>
    [Parameter]
    public LinkReferrerPolicy? ReferrerPolicy { get; set; }

    /// <summary>
    /// Get or set relationship of the linked document to the current document.
    /// </summary>
    [Parameter]
    public LinkRel? Rel { get; set; }

    /// <summary>
    /// Get or set the type of the content linked to.
    /// </summary>
    [Parameter]
    public string? LinkType { get; set; }

    /// <summary>
    /// Get or set the frame or window name that has the defined linking relationship.
    /// </summary>
    [Parameter]
    public LinkTarget? Target { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance.
    /// Default is <see cref="LinkAppearance.Default"/>.
    /// </summary>
    [Parameter]
    public LinkAppearance Appearance { get; set; } = LinkAppearance.Default;

    /// <summary>
    /// Get or set if the link is inline with text.
    /// </summary>
    [Parameter]
    public bool Inline { get; set; } = false;

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the start of link content.
    /// </summary>
    [Parameter]
    public Icon? IconStart { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Icon"/> displayed at the end of link content.
    /// </summary>
    [Parameter]
    public Icon? IconEnd { get; set; }

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
    }

    /// <summary />
    protected async Task OnClickHandlerAsync(MouseEventArgs e)
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(e);
        }
    }
}
