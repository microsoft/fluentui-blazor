using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class FluentToast : FluentComponentBase
{
    protected Color _iconColor = Color.Info;
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-toast")
        .AddClass("toast-info", () => ToastItem?.Intent == ToastIntent.Info)
        .AddClass("toast-success", () => ToastItem?.Intent == ToastIntent.Success)
        .AddClass("toast-warning", () => ToastItem?.Intent == ToastIntent.Warning)
        .AddClass("toast-error", () => ToastItem?.Intent == ToastIntent.Error)
        .Build();

    protected RenderFragment Css { get; private set; } = default!;

    [Parameter]
    public Toast ToastItem { get; set; } = default!;

    protected string AnimationStyle => ToastItem?.State.AnimationStyle + Style;

    protected string Message => ToastItem?.Message ?? string.Empty;

    protected bool ShowIcon => ToastItem?.State.Icon != null;

    protected string? Icon => ToastItem?.State.Icon;

    

    protected bool ShowCloseIcon => ToastItem?.State.ShowCloseIcon == true;

    protected ToastAction? Action => ToastItem?.State.Options.Action;

    protected bool ShowActionButton => !string.IsNullOrEmpty(ToastItem?.State.Options.Action?.Text);

    protected async Task ActionClickedAsync()
    {
        if (Action?.OnClick != null)
        {
            await Action.OnClick.Invoke(ToastItem);
        }
    }

    protected async Task CloseIconClickedAsync()
    {
        if (ToastItem != null)
        {
            await ToastItem.ClickedAsync(true);
        }
    }

    protected async Task ToastClickedAsync()
    {
        if (ToastItem != null)
        {
            await ToastItem.ClickedAsync(false);
        }
    }

    protected override void OnInitialized()
    {
        if (ToastItem != null)
        {
            ToastItem.OnUpdate += ToastUpdated;
            ToastItem.Initialize();

            Css = builder =>
            {
                var transitionClass = ToastItem.State.TransitionClass;

                if (!string.IsNullOrWhiteSpace(transitionClass))
                {
                    builder.OpenElement(1, "style");
                    builder.AddContent(2, transitionClass);
                    builder.CloseElement();
                }
            };

            if (ShowIcon)
            {
                _iconColor =  ToastItem.Intent switch
                {
                    
                    ToastIntent.Info => Color.Neutral,
                    ToastIntent.Error => Color.Error,
                    ToastIntent.Success => Color.Success,
                    ToastIntent.Warning => Color.Warning,
                    _ => Color.Neutral
                };
            }
        }
    }

    private void ToastUpdated()
    {
        InvokeAsync(StateHasChanged);
    }
}
