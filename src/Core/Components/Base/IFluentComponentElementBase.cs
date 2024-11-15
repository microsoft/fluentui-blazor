// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Base class for FluentUI Blazor components, containing an <see cref="Element"/> property.
/// </summary>
public interface IFluentComponentElementBase
{
    /// <summary>
    /// Gets or sets a reference to the rendered element.
    /// May be <see langword="null"/> if accessed before the component is rendered.
    /// </summary>
    ElementReference Element { get; protected set; }
}
