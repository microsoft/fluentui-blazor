// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TOption"></typeparam>
public partial class FluentSelect<TOption> : FluentListBase<TOption>
{
    /// <summary />
    protected override string? StyleValue => new StyleBuilder(base.StyleValue)
        .AddStyle("min-width", Width, when: !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, when: !string.IsNullOrEmpty(Height))
        .Build();
}
