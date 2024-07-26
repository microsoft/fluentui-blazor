// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Base class for FluentUI Blazor components.
/// </summary>
public abstract class FluentComponentBase : ComponentBase
{
    /// <summary>
    /// Gets or sets the associated web component. 
    /// May be <see langword="null"/> if accessed before the component is rendered.
    /// </summary>
    public virtual ElementReference Element { get; protected set; }

    /// <summary>
    /// Gets or sets the unique identifier.
    /// The value will be used as the HTML <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/id">global id attribute</see>.
    /// </summary>
    [Parameter]
    public virtual string? Id { get; set; }

    /// <summary>
    /// Gets or sets the CSS class names. If given, these will be included in the class attribute of the component.
    /// </summary>
    [Parameter]
    public virtual string? Class { get; set; }

    /// <summary>
    /// Gets or sets the in-line styles. If given, these will be included in the style attribute of the component.
    /// </summary>
    [Parameter]
    public virtual string? Style { get; set; }

    /// <summary>
    /// Gets or sets custom data, to attach any user data object to the component.
    /// </summary>
    [Parameter]
    public virtual object? Data { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public virtual IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }
}
