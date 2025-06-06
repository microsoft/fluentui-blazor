// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class FluentTooltipProvider : FluentComponentBase
{
    /// <summary />
    public FluentTooltipProvider(LibraryConfiguration configuration) : base(configuration)
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
    /// <remarks>
    /// We cannot inject `ITooltipService` directly, as an exception will be thrown if the service is not injected.
    /// https://github.com/dotnet/aspnetcore/issues/24193
    /// </remarks>
    private ITooltipService? TooltipService => GetCachedServiceOrNull<ITooltipService>();

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
