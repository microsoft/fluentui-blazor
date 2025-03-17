// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentTooltipProvider : FluentComponentBase
{
    /// <summary />
    public FluentTooltipProvider()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    internal string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-tooltip-provider")
        .Build();

    /// <summary />
    internal string? StyleValue => DefaultStyleBuilder
        .AddStyle("z-index", ZIndex.Tooltip.ToString(CultureInfo.InvariantCulture))
        .Build();

    /// <summary>
    /// Gets or sets the injected service provider.
    /// </summary>
    [Inject]
    private ITooltipService? TooltipService { get; set; }

    /// <summary />
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (TooltipService is not null)
        {
            TooltipService.ProviderId = Id;
            TooltipService.OnUpdatedAsync = async (item) =>
            {
                await InvokeAsync(StateHasChanged);
            };
        }
    }
}
