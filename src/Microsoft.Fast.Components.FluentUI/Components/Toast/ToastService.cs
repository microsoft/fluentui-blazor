using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class ToastService : IToastService
{
    /// <summary>
    /// A event that will be invoked when showing a toast
    /// </summary>
    public event Action<ToastIntent, string, Action<ToastSettings>?>? OnShow;

    /// <summary>
    /// A event that will be invoked when clearing all toasts
    /// </summary>
    public event Action? OnClearAll;

    /// <summary>
    /// A event that will be invoked when showing a toast with a custom component
    /// </summary>
    public event Action<Type, ToastParameters?, Action<ToastSettings>?>? OnShowCustomComponent;

    /// <summary>
    /// A event that will be invoked when showing a toast with a custom component
    /// </summary>
    public event Action<Type, ToastIntent, string, Action<ToastAction>?, Action<ToastSettings>?>? OnShowToastComponent;

    /// <summary>
    /// A event that will be invoked when clearing toasts
    /// </summary>
    public event Action<ToastIntent>? OnClearToasts;

    /// <summary>
    /// A event that will be invoked when clearing custom toast components
    /// </summary>
    public event Action? OnClearCustomToasts;

    /// <summary>
    /// A event that will be invoked to clear all queued toasts
    /// </summary>
    public event Action? OnClearQueue;

    /// <summary>
    /// A event that will be invoked to clear queued toast of specified intent
    /// </summary>
    public event Action<ToastIntent>? OnClearQueueToasts;

    /// <summary>
    /// Shows a simple information confirmation toast.
    /// Only shows icon, title and close button or action.
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Action to use for this toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowInfo(string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
        => ShowToast<ConfirmationToast>(ToastIntent.Info, title, action, settings);

    /// <summary>
    /// Shows a success toast 
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowSuccess(string title, Action<ToastSettings>? settings = null)
        => ShowToast(ToastIntent.Success, title, settings);

    /// <summary>
    /// Shows a warning toast 
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowWarning(string title, Action<ToastSettings>? settings = null)
        => ShowToast(ToastIntent.Warning, title, settings);

    /// <summary>
    /// Shows a error toast 
    /// </summary>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowError(string title, Action<ToastSettings>? settings = null)
        => ShowToast(ToastIntent.Error, title, settings);

    /// <summary>
    /// Shows a toast using the supplied settings
    /// </summary>
    /// <param name="intent">Toast intent to display</param>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowToast(ToastIntent intent, string title, Action<ToastSettings>? settings = null)
        => OnShow?.Invoke(intent, title, settings);

    /// <summary>
    /// Shows the toast with the component type
    /// </summary>
    public void ShowToast<TComponent>() where TComponent : IToastComponent
        => ShowToast(typeof(TComponent), new ToastParameters(), null);

    /// <summary>
    /// Shows a toast using the supplied settings
    /// </summary>
    /// <param name="intent">Toast intent to display</param>
    /// <param name="title">Text to display on the toast</param>
    /// <param name="action">Action to show (instead of close button)</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowToast<TComponent>(ToastIntent intent, string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null) where TComponent : IToastComponent
        => ShowToast(typeof(TComponent), intent, title, action, settings);

    /// <summary>
    /// Shows the toast with the component type />,
    /// passing the specified <paramref name="parameters"/> 
    /// </summary>
    /// <param name="contentComponent">Type of component to display.</param>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    /// <param name="settings">Settings to configure the toast component.</param>
    public void ShowToast(Type contentComponent, ToastParameters? parameters, Action<ToastSettings>? settings)
    {
        if (!typeof(IComponent).IsAssignableFrom(contentComponent))
        {
            throw new ArgumentException($"{contentComponent.FullName} must be a Blazor Component");
        }

        OnShowCustomComponent?.Invoke(contentComponent, parameters, settings);
    }

    /// <summary>
    /// Shows the specified toast component type />,
    /// </summary>
    /// <param name="toastComponent">Type of toast component to display.</param>
    /// <param name="intent">The intent of the notification.</param>
    /// <param name="title">The title of the notification</param>
    /// <param name="action">The action to use for this toast.</param>
    /// <param name="settings">Settings to configure the toast component.</param>
    public void ShowToast(Type toastComponent, ToastIntent intent, string title, Action<ToastAction>? action = null, Action<ToastSettings>? settings = null)
    {
        if (!typeof(IToastComponent).IsAssignableFrom(toastComponent))
        {
            throw new ArgumentException($"{toastComponent.FullName} must be a Toast Component");
        }

        OnShowToastComponent?.Invoke(toastComponent, intent, title, action, settings);
    }

    /// <summary>
    /// Shows the toast with the component type />,
    /// passing the specified <paramref name="parameters"/> 
    /// </summary>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    public void ShowToast<TComponent>(ToastParameters parameters) where TComponent : IToastComponent
    => ShowToast(typeof(TComponent), parameters, null);

    /// <summary>
    /// Shows a toast using the supplied settings
    /// </summary>
    /// <param name="settings">Toast settings to be used</param>
    public void ShowToast<TComponent>(Action<ToastSettings>? settings) where TComponent : IToastComponent
        => ShowToast(typeof(TComponent), null, settings);

    /// <summary>
    /// Shows a toast using the supplied parameter and settings
    /// </summary>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    /// <param name="settings">Toast settings to be used</param>
    public void ShowToast<TComponent>(ToastParameters parameters, Action<ToastSettings>? settings) where TComponent : IToastComponent
        => ShowToast(typeof(TComponent), parameters, settings);

    /// <summary>
    /// Removes all toasts
    /// </summary>
    public void ClearAll()
        => OnClearAll?.Invoke();

    /// <summary>
    /// Removes all toasts with a specified <paramref name="intent"/>.
    /// </summary>
    public void ClearToasts(ToastIntent intent)
        => OnClearToasts?.Invoke(intent);

    /// <summary>
    /// Removes all toasts with toast intent warning
    /// </summary>
    public void ClearWarningToasts()
        => OnClearToasts?.Invoke(ToastIntent.Warning);

    /// <summary>
    /// Removes all toasts with toast intent info
    /// </summary>
    public void ClearInfoToasts()
        => OnClearToasts?.Invoke(ToastIntent.Info);

    /// <summary>
    /// Removes all toasts with toast intent success
    /// </summary>
    public void ClearSuccessToasts()
        => OnClearToasts?.Invoke(ToastIntent.Success);

    /// <summary>
    /// Removes all toasts with toast intent error
    /// </summary>
    public void ClearErrorToasts()
        => OnClearToasts?.Invoke(ToastIntent.Error);

    /// <summary>
    /// Removes all custom component toasts
    /// </summary>
    public void ClearCustomToasts()
        => OnClearCustomToasts?.Invoke();

    /// <summary>
    /// Removes all queued toasts
    /// </summary>
    /// 
    public void ClearQueue()
        => OnClearQueue?.Invoke();

    /// <summary>
    /// Removes all queued toasts with a specified <paramref name="toastLevel"/>.
    /// </summary>
    public void ClearQueueToasts(ToastIntent toastLevel)
        => OnClearQueueToasts?.Invoke(toastLevel);

    /// <summary>
    /// Removes all queued toasts with toast intent warning
    /// </summary>
    public void ClearQueueWarningToasts()
        => OnClearQueueToasts?.Invoke(ToastIntent.Warning);

    /// <summary>
    /// Removes all queued toasts with toast intent info
    /// </summary>
    public void ClearQueueInfoToasts()
        => OnClearQueueToasts?.Invoke(ToastIntent.Info);

    /// <summary>
    /// Removes all queued toasts with toast intent success
    /// </summary>
    public void ClearQueueSuccessToasts()
        => OnClearQueueToasts?.Invoke(ToastIntent.Success);

    /// <summary>
    /// Removes all queued toasts with toast intent error
    /// </summary>
    public void ClearQueueErrorToasts()
        => OnClearQueueToasts?.Invoke(ToastIntent.Error);
}
