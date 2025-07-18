// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

[CascadingTypeParameter(nameof(TOption))]
public partial class FluentSelect<TOption> : ListComponentBase<TOption> where TOption : notnull
{
    /// <summary>
    /// Gets the `Required` aria label.
    /// </summary>
    public static string RequiredAriaLabel = "Required";

    /// <summary />
    protected virtual MarkupString InlineStyleValue => new InlineStyleBuilder()
        .AddStyle($"#{Id}::part(listbox)", "position", "relative", Multiple)
        .AddStyle($"#{Id}::part(listbox)", "max-height", Height, !string.IsNullOrWhiteSpace(Height))
        .AddStyle($"#{Id}::part(listbox)", "height", "fit-content", !string.IsNullOrWhiteSpace(Height))
        .AddStyle($"#{Id}::part(listbox)", "z-index", ZIndex.SelectPopup.ToString(), !Multiple)
        .AddStyle($"#{Id}::part(selected-value)", "white-space", "nowrap")
        .AddStyle($"#{Id}::part(selected-value)", "overflow", "hidden")
        .AddStyle($"#{Id}::part(selected-value)", "text-overflow", "ellipsis")
        .AddStyle($"#{Id}::part(selected-value)", "color", "var(--input-placeholder-rest)", when: !string.IsNullOrEmpty(Placeholder) && SelectedOption is null)
        .BuildMarkupString();

    protected override string? StyleValue => new StyleBuilder(base.StyleValue)
        .AddStyle("min-width", Width, when: !string.IsNullOrEmpty(Width))
        .Build();

    /// <summary>
    /// Gets or sets the open attribute.
    /// </summary>
    [Parameter]
    public bool? Open { get; set; }

    /// <summary>
    /// Reflects the placement for the listbox when the select is open.
    /// See <see cref="AspNetCore.Components.SelectPosition"/>
    /// </summary>
    [Parameter]
    public SelectPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="AspNetCore.Components.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }

    /// <summary>
    /// Called whenever the selection changed.
    /// ⚠️ Only available when Multiple = true.
    /// ⚠️ When using manual options, the internal data structure cannot be updated reliably, because of this, the SelectedOptionsChanged event will not be triggered.
    /// </summary>
    [Parameter]
    public override EventCallback<IEnumerable<TOption>?> SelectedOptionsChanged { get; set; }

    private string? GetAriaLabelWithRequired()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        var label = AriaLabel ?? Label ?? Title ?? string.Empty;
#pragma warning restore CS0618 // Type or member is obsolete

        return label + (Required ? $", {RequiredAriaLabel}" : string.Empty);
    }
}
