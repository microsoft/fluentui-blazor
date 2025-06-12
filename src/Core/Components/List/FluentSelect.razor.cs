// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentSelect allows for selecting one or more options from a list of options.
/// </summary>
/// <typeparam name="TOption"></typeparam>
public partial class FluentSelect<TOption> : FluentListBase<TOption>
{
    /// <summary />
    public FluentSelect(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    protected virtual string DropdownType => "dropdown";

    /// <summary />
    protected virtual string? DropdownStyle => new StyleBuilder()
        .AddStyle("width", Width, when: !string.IsNullOrEmpty(Width))
        .Build();

    /// <summary />
    protected virtual string? ListStyle => new StyleBuilder()
        .AddStyle("min-width", Width, when: !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, when: !string.IsNullOrEmpty(Height))
        .Build();

    /// <summary>
    /// Gets or sets the placeholder text to display when no item is selected.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets the size of the list.
    /// Default is `null`. Internally the component uses <see cref="ListSize.Medium"/> as default.
    /// </summary>
    [Parameter]
    public ListSize? Size { get; set; }
}
