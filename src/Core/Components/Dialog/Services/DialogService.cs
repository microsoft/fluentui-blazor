using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class DialogService : IDialogService
{
    /// <summary>
    /// Convenience method to create a <see cref="EventCallback"/> for a dialog result.
    /// You can also call <code>EventCallback.Factory.Create</code> directly.
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public EventCallback<DialogResult> CreateDialogCallback(object receiver, Func<DialogResult, Task> callback) => EventCallback.Factory.Create(receiver, callback);

    public Task CloseAsync(DialogReference dialog)
    {
        return CloseAsync(dialog, DialogResult.Ok<object?>(null));
    }

    public async Task CloseAsync(DialogReference dialog, DialogResult result)
    {
        await Task.Run(() => { });  // To avoid warning
        OnDialogCloseRequested?.Invoke(dialog, result);
    }

    internal virtual IDialogReference CreateReference(string id)
    {
        return new DialogReference(id, this);
    }

    /// <summary>
    /// A event that will be invoked when showing a dialog with a custom component
    /// </summary>
    public event Action<IDialogReference, Type?, DialogParameters, object>? OnShow;

    public event Func<IDialogReference, Type?, DialogParameters, object, Task<IDialogReference>>? OnShowAsync;

    public event Action<string, DialogParameters>? OnUpdate;

    public event Func<string, DialogParameters, Task<IDialogReference>>? OnUpdateAsync;

    public event Action<IDialogReference, DialogResult>? OnDialogCloseRequested;

}
