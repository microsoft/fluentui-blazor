// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Demo.Client.Layout;

public partial class DemoMainLayout
{
    [Parameter]
    public RenderFragment? Body { get; set; }
}
