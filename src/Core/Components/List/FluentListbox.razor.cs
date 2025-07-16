// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

[CascadingTypeParameter(nameof(TOption))]
public partial class FluentListbox<TOption> : ListComponentBase<TOption> where TOption : notnull
{
    /// <summary>
    /// Gets or sets the maximum number of options that should be visible in the listbox scroll area.
    /// </summary>
    [Parameter]
    public int Size { get; set; }

    /// <summary>
    /// Called whenever the selection changed.
    /// ⚠️ Only available when Multiple = true.
    /// ⚠️ When using manual options, the internal data structure cannot be updated reliably, because of this, the SelectedOptionsChanged event will not be triggered.
    /// </summary>
    [Parameter]
    public override EventCallback<IEnumerable<TOption>?> SelectedOptionsChanged { get; set; }

    /// <summary />
    protected virtual StyleBuilder BorderStyle => new StyleBuilder()
        .AddStyle("width", Width, when: !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, when: !string.IsNullOrEmpty(Height))
        .AddStyle("overflow-y", "auto")
        .AddStyle("border", "calc(var(--stroke-width) * 1px) solid var(--neutral-stroke-rest)")
        .AddStyle("border-radius", "calc(var(--control-corner-radius) * 1px)");
}
