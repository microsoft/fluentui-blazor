using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial interface IDialogService
{
    Task CloseAsync(DialogReference dialog);

    Task CloseAsync(DialogReference dialog, DialogResult result);

    /// <summary>
    /// An event that will be invoked when showing a dialog with a custom component
    /// </summary>
    public event Action<IDialogReference, Type?, DialogParameters, object>? OnShow;

    public event Func<IDialogReference, Type?, DialogParameters, object, Task<IDialogReference>>? OnShowAsync;

    public event Action<string, DialogParameters>? OnUpdate;

    public event Func<string, DialogParameters, Task<IDialogReference?>>? OnUpdateAsync;

    public event Action<IDialogReference, DialogResult>? OnDialogCloseRequested;

    public EventCallback<DialogResult> CreateDialogCallback(object receiver, Func<DialogResult, Task> callback);
}
