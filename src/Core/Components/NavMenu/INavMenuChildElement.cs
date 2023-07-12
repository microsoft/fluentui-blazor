﻿namespace Microsoft.Fast.Components.FluentUI;

internal interface INavMenuChildElement
{
    string? Id { get; }
    bool HasIcon { get; }
    string? Href { get; }
    FluentTreeItem TreeItem { get; }
}
