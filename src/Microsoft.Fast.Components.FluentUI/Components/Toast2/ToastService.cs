using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class ToastService2 : IToastService2
{
    /// <summary>
    /// A event that will be invoked when showing a toast
    /// </summary>
    public event Action<ToastIntent, RenderFragment, Action<ToastSettings>?>? OnShow;

    /// <summary>
    /// A event that will be invoked when clearing all toasts
    /// </summary>
    public event Action? OnClearAll;

    /// <summary>
    /// A event that will be invoked when showing a toast with a custom component
    /// </summary>
    public event Action<Type, ToastParameters?, Action<ToastSettings>?>? OnShowComponent;

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
    /// Shows a information toast 
    /// </summary>
    /// <param name="message">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowInfo(string message, Action<ToastSettings>? settings = null)
        => ShowToast(ToastIntent.Info, message, settings);

    /// <summary>
    /// Shows a information toast 
    /// </summary>
    /// <param name="message">RenderFragment to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowInfo(RenderFragment message, Action<ToastSettings>? settings = null)
        => ShowToast(ToastIntent.Info, message, settings);

    /// <summary>
    /// Shows a success toast 
    /// </summary>
    /// <param name="message">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowSuccess(string message, Action<ToastSettings>? settings = null)
        => ShowToast(ToastIntent.Success, message, settings);

    /// <summary>
    /// Shows a success toast 
    /// </summary>
    /// <param name="message">RenderFragment to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowSuccess(RenderFragment message, Action<ToastSettings>? settings = null)
        => ShowToast(ToastIntent.Success, message, settings);

    /// <summary>
    /// Shows a warning toast 
    /// </summary>
    /// <param name="message">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowWarning(string message, Action<ToastSettings>? settings = null)
        => ShowToast(ToastIntent.Warning, message, settings);

    /// <summary>
    /// Shows a warning toast 
    /// </summary>
    /// <param name="message">RenderFragment to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowWarning(RenderFragment message, Action<ToastSettings>? settings = null)
        => ShowToast(ToastIntent.Warning, message, settings);

    /// <summary>
    /// Shows a error toast 
    /// </summary>
    /// <param name="message">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowError(string message, Action<ToastSettings>? settings = null)
        => ShowToast(ToastIntent.Error, message, settings);

    /// <summary>
    /// Shows a error toast 
    /// </summary>
    /// <param name="message">RenderFragment to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowError(RenderFragment message, Action<ToastSettings>? settings = null)
        => ShowToast(ToastIntent.Error, message, settings);

    /// <summary>
    /// Shows a toast using the supplied settings
    /// </summary>
    /// <param name="intent">Toast intent to display</param>
    /// <param name="message">Text to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowToast(ToastIntent intent, string message, Action<ToastSettings>? settings = null)
        => ShowToast(intent, builder => builder.AddContent(0, message), settings);


    /// <summary>
    /// Shows a toast using the supplied settings
    /// </summary>
    /// <param name="intent">Toast intent to display</param>
    /// <param name="message">RenderFragment to display on the toast</param>
    /// <param name="settings">Settings to configure the toast instance</param>
    public void ShowToast(ToastIntent intent, RenderFragment message, Action<ToastSettings>? settings = null)
        => OnShow?.Invoke(intent, message, settings);

    /// <summary>
    /// Shows the toast with the component type
    /// </summary>
    public void ShowToast<TComponent>() where TComponent : IComponent
        => ShowToast(typeof(TComponent), new ToastParameters(), null);

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

        OnShowComponent?.Invoke(contentComponent, parameters, settings);
    }

    /// <summary>
    /// Shows the toast with the component type />,
    /// passing the specified <paramref name="parameters"/> 
    /// </summary>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    public void ShowToast<TComponent>(ToastParameters parameters) where TComponent : IComponent
        => ShowToast(typeof(TComponent), parameters, null);

    /// <summary>
    /// Shows a toast using the supplied settings
    /// </summary>
    /// <param name="settings">Toast settings to be used</param>
    public void ShowToast<TComponent>(Action<ToastSettings>? settings) where TComponent : IComponent
        => ShowToast(typeof(TComponent), null, settings);

    /// <summary>
    /// Shows a toast using the supplied parameter and settings
    /// </summary>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    /// <param name="settings">Toast settings to be used</param>
    public void ShowToast<TComponent>(ToastParameters parameters, Action<ToastSettings>? settings) where TComponent : IComponent
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
