// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The component cascades this so that descendant options can talk back to it.
/// It's an internal type so it doesn't show up in unrelated components by mistake
/// </summary>
internal class InternalListContext<TOption>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InternalListContext{TOption}"/> class.
    /// </summary>
    /// <param name="component"></param>
    public InternalListContext(FluentListBase<TOption> component)
    {
        ListComponent = component;
    }

    /// <summary>
    /// Gets the list component.
    /// </summary>
    public FluentListBase<TOption> ListComponent { get; }

    /// <summary>
    /// Gets the event callback to be invoked when the selected value is changed.
    /// </summary>
    public EventCallback<string?> ValueChanged { get; set; }
}
