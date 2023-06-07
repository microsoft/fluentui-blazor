using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentPanel : FluentComponentBase
{
    private const string DEFAULT_WIDTH = "500px";

    private bool _open;

    [Parameter]
    public RenderFragment? Body { get; set; }

    [Parameter]
    public RenderFragment? Footer { get; set; }

    [Parameter]
    public RenderFragment? Header { get; set; }

    [Parameter]
    public bool Open { get; set; }

    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    [Parameter]
    public EventCallback<bool> OnOpenChanged { get; set; }

    [Parameter]
    public DialogOptions Options { get; set; } = new();

    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("fluent-panel-main")
        .Build();

    protected string? StyleValue => new StyleBuilder()
        .AddStyle(Style)
        .AddStyle("inset", "0px 0px 0px auto", () => Options.Alignment == HorizontalAlignment.Right || Options.Alignment == HorizontalAlignment.End)
        .AddStyle("inset", "0px auto 0px 0px", () => Options.Alignment == HorizontalAlignment.Left || Options.Alignment == HorizontalAlignment.Start)
        .AddStyle("top", "50%", () => Options.Alignment == HorizontalAlignment.Center)
        .AddStyle("left", "50%", () => Options.Alignment == HorizontalAlignment.Center)
        .AddStyle("transform", "translate(-50%, -50%)", () => Options.Alignment == HorizontalAlignment.Center)
        .AddStyle("max-height", "100%", () => Options.Alignment == HorizontalAlignment.Center)
        .AddStyle("width", Options.Width ?? DEFAULT_WIDTH, () => Options.Alignment == HorizontalAlignment.Center)
        .AddStyle("height", Options.Height, () => Options.Alignment == HorizontalAlignment.Center && !string.IsNullOrWhiteSpace(Options.Height))
        .Build();

    [CascadingParameter]
    private FluentDialogProvider? DialogProvider { get; set; }

    public FluentPanel()
    {
        Id = Identifier.NewId();
    }

    private bool HasFooter => Footer != null || Options.ShowOK || Options.ShowCancel;

    public virtual async Task CancelAsync()
    {
        await CloseAsync(DialogResult.Cancel());
    }

    public virtual async Task CloseAsync()
    {
        await CloseAsync(DialogResult.Ok<object?>(null));
    }

    public virtual async Task CloseAsync<T>(T returnValue)
    {
        await CloseAsync(DialogResult.Ok<T>(returnValue));
    }

    public virtual async Task CloseAsync(DialogResult dialogResult)
    {
        if (DialogProvider?.GetDialogReference(Id!)?.Dialog is IDialogInstance dialog && (await dialog.OnValidateAsync(dialogResult.Cancelled)) == false)
        {
            return;
        }

        Open = false;
        DialogProvider?.DismissInstance(Id!, dialogResult);
        await OpenChanged.InvokeAsync(Open);
    }

    public virtual async Task OpenAsync()
    {
        Open = true;
        await OpenChanged.InvokeAsync(Open);
    }

    protected override void OnInitialized()
    {
        _open = Open;
        base.OnInitialized();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (_open != Open)
        {
            _open = Open;
            await OnOpenChanged.InvokeAsync(Open);
        }
    }
}
