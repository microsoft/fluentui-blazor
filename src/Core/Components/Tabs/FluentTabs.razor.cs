// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Each tab typically contains a text header and often includes an icon.
/// </summary>
public partial class FluentTabs: FluentComponentBase
{
    private ConcurrentDictionary<string, FluentTab> Tabs { get; } = new(StringComparer.Ordinal);

    /// <summary />
    [DynamicDependency(nameof(TabChangeHandlerAsync))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(TabChangeEventArgs))]
    public FluentTabs()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-tabs")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width, when: !string.IsNullOrEmpty(Width))
        .AddStyle("height", Height, when: !string.IsNullOrEmpty(Height))
        .Build();

    /// <summary>
    /// Gets or sets the appearance affects each of the contained tabs.
    /// </summary>
    [Parameter]
    public TabsAppearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets whether the tabs are disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets the size of the tabs. The default is medium.
    /// </summary>
    [Parameter]
    public TabsSize? Size { get; set; }

    /// <summary>
    /// Gets or sets the orientation of the tabs. The default is horizontal.
    /// </summary>
    [Parameter]
    public Orientation? Orientation { get; set; }

    /// <summary>
    /// Gets or sets the height of the tabs.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the wudth of the tabs.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets the the active selected tab id.
    /// </summary>
    [Parameter]
    public string? ActiveTabId { get; set; }

    /// <summary>
    /// Represents a callback for when the active tab id changes. It can handle a nullable FluentTab parameter.
    /// </summary>
    [Parameter]
    public EventCallback<string?> ActiveTabIdChanged { get; set; }

    /// <summary>
    /// Gets the the active selected tab.
    /// </summary>
    [Parameter]
    public FluentTab? ActiveTab { get; set; }

    /// <summary>
    /// Represents a callback for when the active tab changes. It can handle a nullable FluentTab parameter.
    /// </summary>
    [Parameter]
    public EventCallback<FluentTab?> ActiveTabChanged { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary />
    internal async Task<int> AddTabAsync(FluentTab tab)
    {
        if (tab is not null && !string.IsNullOrEmpty(tab.Id))
        {
            Tabs.TryAdd(tab.Id, tab);

            // Set the default ActiveTab
            if (!string.IsNullOrEmpty(ActiveTabId) && string.Equals(ActiveTabId, tab.Id, StringComparison.Ordinal))
            {
                ActiveTab = tab;

                if (ActiveTabChanged.HasDelegate)
                {
                    await ActiveTabChanged.InvokeAsync(ActiveTab);
                }
            }

            // Set the default ActiveTabId
            else if (ActiveTabId is null  && string.Equals(ActiveTab?.Id, tab.Id, StringComparison.Ordinal))
            {
                ActiveTabId = tab.Id;

                if (ActiveTabIdChanged.HasDelegate)
                {
                    await ActiveTabIdChanged.InvokeAsync(ActiveTabId);
                }
            }

            return Tabs.Count;
        }

        return 0;
    }

    /// <summary />
    internal async Task TabChangeHandlerAsync(TabChangeEventArgs args)
    {
        // Only for the current FluentTabs
        if (!string.Equals(args.Id, TabListId, StringComparison.Ordinal))
        {
            return;
        }

        // Search for the tab
        if (Tabs.TryGetValue(args.ActiveId ?? "", out var tab))
        {
            ActiveTabId = args.ActiveId;
            ActiveTab = tab;

            if (ActiveTabIdChanged.HasDelegate)
            {
                await ActiveTabIdChanged.InvokeAsync(ActiveTabId);
            }

            if (ActiveTabChanged.HasDelegate)
            {
                await ActiveTabChanged.InvokeAsync(ActiveTab);
            }

            // Refresh the active tab content
            await ActiveTab.RefreshAsync();
        }
    }

    /// <summary />
    internal string TabListId => $"{Id}-tablist";
}
