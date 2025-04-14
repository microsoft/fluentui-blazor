// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A Fluent Radio button component.
/// </summary>
public partial class FluentRadio2<TValue> : FluentComponentBase
{
    /// <summary>
    /// 
    /// </summary>
    public FluentRadio2()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    [CascadingParameter]
    public FluentRadioGroup2<TValue>? Owner { get; set; }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder.Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder.Build();

    /// <inheritdoc cref="IFluentField.Disabled" />
    [Parameter]
    public virtual bool? Disabled { get; set; }

    /// <inheritdoc cref="IFluentField.Label" />
    [Parameter]
    public virtual string? Label { get; set; }

    /// <inheritdoc cref="IFluentField.LabelTemplate" />
    [Parameter]
    public virtual RenderFragment? LabelTemplate { get; set; }

    /// <inheritdoc cref="IFluentField.LabelTemplate" />
    [Parameter]
    public virtual RenderFragment? ChildContent { get; set; }

    /// <inheritdoc cref="IFluentField.LabelWidth" />
    [Parameter]
    public virtual string? LabelWidth { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public TValue? Value { get; set; }
}
