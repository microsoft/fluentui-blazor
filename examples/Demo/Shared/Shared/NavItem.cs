// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Routing;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared;

public abstract record NavItem
{
    public string Title { get; init; } = string.Empty;
    public string? Href { get; init; }
    public NavLinkMatch Match { get; init; } = NavLinkMatch.Prefix;
    public Icon Icon { get; init; } = new Icons.Regular.Size20.Document();
}

public record NavLink : NavItem
{
    public NavLink(string? href, Icon icon, string title, NavLinkMatch match = NavLinkMatch.Prefix)
    {
        Href = href;
        Icon = icon;
        Title = title;
        Match = match;
    }
}

public record NavGroup : NavItem
{
    public bool Expanded { get; set; }
    public string Gap { get; init; }
    public IReadOnlyList<NavItem> Children { get; }

    public NavGroup(Icon icon, string title, bool expanded, string gap, List<NavItem> children)
    {
        Href = null;
        Icon = icon;
        Title = title;
        Expanded = expanded;
        Gap = gap;
        Children = children.AsReadOnly();
    }
}
