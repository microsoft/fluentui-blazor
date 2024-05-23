// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.TreeView;

using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public partial class FluentTreeViewItemsTests
{
    private readonly TreeViewItem[] Items = 
    {
        new TreeViewItem("id1", "Item 1",
        [
            new TreeViewItem("id11", "Item 1.1"),
            new TreeViewItem("id12", "Item 1.2"),
            new TreeViewItem("id13", "Item 1.3"),
        ]),
        new TreeViewItem("id2", "Item 2",
        [
            new TreeViewItem("id21", "Item 2.1"),
            new TreeViewItem("id22", "Item 2.2"),
            new TreeViewItem("id23", "Item 2.3"),
        ]),
    };

    private readonly Icon IconCollapsed = SampleIcons.Info;
    private readonly Icon IconExpanded = SampleIcons.Warning;
}
