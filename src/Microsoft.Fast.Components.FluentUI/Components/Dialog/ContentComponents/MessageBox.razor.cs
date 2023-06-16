using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class MessageBox : IDialogContentComponent, IMessageBoxParameters
{
    private string? _dialogId;

    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    [Parameter]
    public MessageBoxIntent Intent { get; set; } = MessageBoxIntent.Info;

    [Parameter]
    public string? Title { get; set; } = string.Empty;

    [Parameter]
    public string? Message { get; set; } = string.Empty;

    [Parameter]
    public MarkupString? MarkupMessage { get; set; } = default!;

    [Parameter]
    public string? Icon { get; set; } = string.Empty;

    [Parameter]
    public Color IconColor { get; set; } = Color.Accent;

    [Parameter]
    public string PrimaryButtonText { get; set; } = "Ok"; //FluentPanelResources.ButtonOK;

    [Parameter]
    public string SecondaryButtonText { get; set; } = "Cancel"; //FluentPanelResources.ButtonCancel;

    [Parameter]
    public string? Width { get; set; }

    [Parameter]
    public string? Height { get; set; }

    [Parameter]
    public DialogSettings Settings { get; set; } = default!;

    protected override void OnInitialized()
    {
        _dialogId = Dialog.Id;
        Settings = Dialog.Settings;
    }

    protected virtual Task ConfirmAsync()
    {
        return OnCloseAsync(DialogResult.Ok(true));
    }

    protected virtual Task CancelAsync()
    {
        return OnCloseAsync(DialogResult.Cancel());
    }

    public virtual Task OnCloseAsync(DialogResult result)
    {
        return Dialog.CloseAsync(result);
    }
}
