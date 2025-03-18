// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentProviders : FluentComponentBase
{
    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-providers")
        .Build();

    /// <summary />
    internal string? StyleValue => DefaultStyleBuilder
        .AddStyle("display", "contents")
        .Build();
}
