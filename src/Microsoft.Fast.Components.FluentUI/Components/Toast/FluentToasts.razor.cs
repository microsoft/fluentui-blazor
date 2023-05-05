using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class FluentToasts : FluentComponentBase, IDisposable
{
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-toasts")
        .AddClass(ToastService.Configuration.ToasterPosition.ToAttributeValue())
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle(Style)
        .Build();

    [Inject]
    private IToastService ToastService { get; set; } = default!;

    protected IEnumerable<Toast> Toasts
        => ToastService.Configuration.NewestOnTop
            ? ToastService.ShownToasts.Reverse()
            : ToastService.ShownToasts;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        ToastService.OnToastUpdated += OnToastUpdated;
    }

    private void OnToastUpdated()
    {
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        ToastService.OnToastUpdated -= OnToastUpdated;
    }
}
