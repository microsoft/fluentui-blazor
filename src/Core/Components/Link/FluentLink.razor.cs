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
    /// Gets or sets the callback invoked when the user clicks on the link.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Gets or sets the URL of the link (e.g., <c>Href="/page"</c> or <c>Href="https://example.com"</c>).
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the BCP 47 language code for the language of the linked document (e.g., <c>HrefLang="en"</c>). This is advisory only.
    /// </summary>
    [Parameter]
    public string? HrefLang { get; set; }

    /// <summary>
    /// Gets or sets the referrer policy controlling how much referrer information is sent when following the link.
    /// </summary>
    [Parameter]
    public LinkReferrerPolicy? ReferrerPolicy { get; set; }

    /// <summary>
    /// Gets or sets the relationship of the linked document to the current document.
    /// </summary>
    [Parameter]
    public LinkRel? Rel { get; set; }

    /// <summary>
    /// Gets or sets the MIME type of the linked content (e.g., <c>LinkType="application/pdf"</c>).
    /// </summary>
    [Parameter]
    public string? LinkType { get; set; }

    /// <summary>
    /// Gets or sets the browsing context in which to open <see cref="Href"/>
    /// (e.g., <c>Target="LinkTarget.Blank"</c> to open in a new tab).
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
    /// Gets or sets whether the link is rendered inline with surrounding text.
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
