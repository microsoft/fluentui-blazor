// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentOverflowItem : IDisposable
{
    //private bool _disposed;

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-overflow-item")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    /// <summary>
    /// Gets or sets the reference to the associated container.
    /// </summary>
    /// <value>The splitter.</value>
    [CascadingParameter]
    public FluentOverflow? Owner { get; set; }

    /// <summary>
    /// Gets or sets the content to display. All first HTML elements are included in the items flow.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets whether this element does not participate in overflow logic, and will always be visible.
    /// Defaults to false
    /// </summary>
    [Parameter]
    public OverflowItemFixed Fixed { get; set; } = OverflowItemFixed.None;

    /// <summary>
    /// Gets True if this component is out of panel.
    /// </summary>
    public bool? Overflow { get; private set; }

    /// <summary>
    /// Gets the InnerText of <see cref="ChildContent"/>.
    /// </summary>
    public string Text { get; private set; } = string.Empty;

    public FluentOverflowItem()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected override void OnInitialized()
    {
        Owner?.Register(this);
    }

    /// <summary />
    internal void SetProperties(bool? isOverflow, string? text)
    {
        Overflow = isOverflow == true ? isOverflow : null;
        Text = text ?? string.Empty;
    }

    /// <summary />
    public void Dispose() => Owner?.Unregister(this);
}
