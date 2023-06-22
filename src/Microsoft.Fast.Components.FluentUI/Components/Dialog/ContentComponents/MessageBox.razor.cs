using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public partial class MessageBox : FluentDialogContent, IMessageBoxParameters
{
    private string? _dialogId;
    protected MessageBoxParameters MessageBoxData { get; private set; } = new();

    [Parameter]
    public MessageBoxIntent Intent { get; set; } = MessageBoxIntent.Info;

    [Parameter]
    public string? Message { get; set; } = string.Empty;

    [Parameter]
    public MarkupString? MarkupMessage { get; set; } = default!;

    [Parameter]
    public string? Icon { get; set; } = string.Empty;

    [Parameter]
    public Color IconColor { get; set; } = Color.Accent;


    protected override void OnParametersSet()
    {
        _dialogId = Dialog.Id;
        Settings = Dialog.Settings;

        MessageBoxData = (MessageBoxParameters)Dialog.Data!;
    }
}
