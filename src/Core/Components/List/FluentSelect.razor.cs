using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

[CascadingTypeParameter(nameof(TOption))]
public partial class FluentSelect<TOption> : ListComponentBase<TOption> where TOption : notnull
{
    /// <summary />
    protected virtual MarkupString InlineStyleValue => new InlineStyleBuilder()
        .AddStyle($"#{Id}::part(listbox)", "max-height", Height, !string.IsNullOrWhiteSpace(Height))
        .AddStyle($"#{Id}::part(listbox)", "height", Height, !string.IsNullOrWhiteSpace(Height))
        .AddStyle($"#{Id}::part(listbox)", "z-index", ZIndex.SelectPopup.ToString())
        .AddStyle($"#{Id}::part(selected-value)", "white-space", "nowrap")
        .AddStyle($"#{Id}::part(selected-value)", "overflow", "hidden")
        .AddStyle($"#{Id}::part(selected-value)", "text-overflow", "ellipsis")
        .BuildMarkupString();

    protected override string? StyleValue => new StyleBuilder(base.StyleValue)
        .AddStyle("min-width", Width, when: !string.IsNullOrEmpty(Width))
        .Build();

    /// <summary>
    /// The open attribute.
    /// </summary>
    [Parameter]
    public bool? Open { get; set; }

    /// <summary>
    /// Reflects the placement for the listbox when the select is open.
    /// See <see cref="FluentUI.SelectPosition"/>
    /// </summary>
    [Parameter]
    public SelectPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="FluentUI.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }
}