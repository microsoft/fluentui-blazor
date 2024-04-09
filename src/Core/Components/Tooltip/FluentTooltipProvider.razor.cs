using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

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
