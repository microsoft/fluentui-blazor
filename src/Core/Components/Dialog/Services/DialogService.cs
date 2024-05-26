using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class DialogService : IDialogService
{
    /// <summary />
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DialogEventArgs))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MessageBoxContent))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MessageBox))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(SplashScreenContent))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(FluentSplashScreen))] 
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DialogParameters))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DialogParameters<object>))]
    public DialogService()
    {
    }

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

    public Task CloseAsync(DialogReference dialog, DialogResult result)
    {
        OnDialogCloseRequested?.Invoke(dialog, result);
        return Task.CompletedTask;
    }

    internal virtual IDialogReference CreateReference(string id)
    {
        return new DialogReference(id, this);
    }

    /// <summary>
    /// An event that will be invoked when showing a dialog with a custom component
    /// </summary>
    public event Action<IDialogReference, Type?, DialogParameters, object>? OnShow;

    public event Func<IDialogReference, Type?, DialogParameters, object, Task<IDialogReference>>? OnShowAsync;

    public event Action<string, DialogParameters>? OnUpdate;

    public event Func<string, DialogParameters, Task<IDialogReference?>>? OnUpdateAsync;

    public event Action<IDialogReference, DialogResult>? OnDialogCloseRequested;

}
