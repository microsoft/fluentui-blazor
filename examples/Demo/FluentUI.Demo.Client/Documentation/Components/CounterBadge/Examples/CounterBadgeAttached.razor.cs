// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Client.Documentation.Components.CounterBadge.Examples;
public partial class CounterBadgeAttached
{
    private string? count = "3";
    private string? offsetX;
    private string? offsetY;

    private Positioning _positioning;

    private void HandlePositioning(Positioning positioning)
    {
        Console.WriteLine($"Positioning: {positioning}");

        _positioning = positioning;
    }
}
