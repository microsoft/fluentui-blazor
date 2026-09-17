// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentProviders : FluentComponentBase
{
    /// <summary />
    public FluentProviders(LibraryConfiguration configuration) : base(configuration) { }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-providers")
        .Build();

    /// <summary />
    internal string? StyleValue => DefaultStyleBuilder
        .AddStyle("display", "contents")
        .Build();
}
