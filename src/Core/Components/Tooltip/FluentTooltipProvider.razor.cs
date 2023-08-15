using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Components.Tooltip;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentTooltipProvider : FluentComponentBase, IDisposable
{
    /// <summary />
    protected string? ClassValue
        => new CssBuilder(Class).AddClass("fluent-tooltip-provider")
                                 .Build();

    [Inject]
    private ITooltipService TooltipService { get; set; } = default!;

    protected IEnumerable<TooltipOptions> Tooltips => TooltipService.Tooltips;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        TooltipService.OnTooltipUpdated += OnTooltipUpdated;
    }

    private void OnTooltipUpdated()
    {
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        TooltipService.OnTooltipUpdated -= OnTooltipUpdated;
    }
}
