using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class DialogService : IDialogService
{
    /// <summary>
    /// A event that will be invoked when showing a dialog with a custom component
    /// </summary>
    public event Action<Type, object, Action<DialogSettings>?>? OnShow;

    /// <summary>
    /// Shows the standard <see cref="FluentSplashScreen"/> with the given parameters."/>
    /// </summary>
    /// <param name="receiver">The componente that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenData"/> that holds the data to display</param>
    public void ShowSplashScreen(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenData> parameters)
        => ShowSplashScreen<FluentSplashScreen>(receiver, callback, parameters);

    /// <summary>
    /// Shows a custom splash screen dialog with the given parameters."/>
    /// </summary>
    /// <param name="receiver">The componente that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenData"/> that holds the data to display</param>
    public void ShowSplashScreen<T>(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenData> parameters)
        where T : IDialogContentComponent<SplashScreenData>
        => ShowSplashScreen(typeof(T), receiver, callback, parameters);


    /// <summary>
    /// Shows a splash screen of the given type with the given parameters."/>
    /// </summary>
    /// <param name="component">The type of the component to show</param>
    /// <param name="receiver">The componente that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenData"/> that holds the data to display</param>
    public void ShowSplashScreen(Type component, object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenData> parameters)
    {
        Action<DialogSettings> settings = new(x =>
        {
            x.Alignment = HorizontalAlignment.Center;
            x.Modal = false;
            x.ShowDismiss = false;
            x.ShowTitle = false;
            x.PrimaryButton = null;
            x.SecondaryButton = null;
            x.Width = parameters.Width ?? "600px";
            x.Height = parameters.Height ?? "370px";
            x.DialogBodyStyle = "width: 100%; height: 100%; margin: 0px;";
            x.AriaLabel = $"{parameters.Content.Title} splashscreen";
            x.OnDialogResult = EventCallback.Factory.Create(receiver, callback);
        });

        ShowDialog(component, parameters.Content, settings);
    }

    /// <summary>
    /// Shows a success message box. Does not have a callback function.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public void ShowSuccess(string message, string? title = null) => ShowMessageBox(new DialogParameters<MessageBoxData>()
    {
        Content = new MessageBoxData()
        {
            Title = string.IsNullOrWhiteSpace(title) ? "Success!" /*DialogResources.TitleError*/ : title,
            Intent = MessageBoxIntent.Success,
            Icon = new CoreIcons.Filled.Size24.CheckmarkCircle(),
            IconColor = Color.Success,
            Message = message,
        },
        PrimaryButton = "Ok", /*DialogResources.ButtonOK,*/
        SecondaryButton = string.Empty,
    });

    /// <summary>
    /// Shows a warning message box. Does not have a callback function.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public void ShowWarning(string message, string? title = null) => ShowMessageBox(new DialogParameters<MessageBoxData>()
    {
        Content = new MessageBoxData()
        {
            Title = string.IsNullOrWhiteSpace(title) ? "Warning!" /*DialogResources.TitleError*/ : title,
            Intent = MessageBoxIntent.Warning,
            Icon = new CoreIcons.Filled.Size24.Warning(),
            IconColor = Color.Warning,
            Message = message,
        },
        PrimaryButton = "Ok", /*DialogResources.ButtonOK,*/
        SecondaryButton = string.Empty,
    });

    /// <summary>
    /// Shows an error message box. Does not have a callback function.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public void ShowError(string message, string? title = null) => ShowMessageBox(new DialogParameters<MessageBoxData>()
    {
        Content = new MessageBoxData()
        {
            Title = string.IsNullOrWhiteSpace(title) ? "Error!" /*DialogResources.TitleError*/ : title,
            Intent = MessageBoxIntent.Error,
            Icon = new CoreIcons.Filled.Size24.DismissCircle(),
            IconColor = Color.Error,
            Message = message,
        },
        PrimaryButton = "Ok", /*DialogResources.ButtonOK,*/
        SecondaryButton = string.Empty,
    });

    /// <summary>
    /// Shows an information message box. Does not have a callback function.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public void ShowInfo(string message, string? title = null) => ShowMessageBox(new DialogParameters<MessageBoxData>()
    {
        Content = new MessageBoxData()
        {
            Title = string.IsNullOrWhiteSpace(title) ? "Information" /*DialogResources.TitleInformation*/ : title,
            Intent = MessageBoxIntent.Info,
            Icon = new CoreIcons.Filled.Size24.Info(),
            IconColor = Color.Info,
            Message = message,
        },
        PrimaryButton = "Ok", /*DialogResources.ButtonOK,*/
        SecondaryButton = string.Empty,
    });

    /// <summary>
    /// Shows a confirmation message box. Has a callback function which returns boolean 
    /// (true=PrimaryButton clicked, false=SecondaryButton clicked).
    /// </summary>
    /// <param name="receiver">The component that receives the callback function.</param>
    /// <param name="callback">The callback function.</param>
    /// <param name="message">The message to display.</param>
    /// <param name="primaryText">The text to display on the primary button.</param>
    /// <param name="secondaryText">The text to display on the secondary button.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public void ShowConfirmation(object receiver, Func<DialogResult, Task> callback, string message, string primaryText = "Yes", string secondaryText = "No", string? title = null)
        => ShowMessageBox(new DialogParameters<MessageBoxData>()
        {
            Content = new MessageBoxData()
            {
                Title = string.IsNullOrWhiteSpace(title) ? "Confirm" /*DialogResources.TitleConfirmation*/ : title,
                Intent = MessageBoxIntent.Confirmation,
                Icon = new CoreIcons.Regular.Size24.QuestionCircle(),
                IconColor = Color.Success,
                Message = message,
            },
            PrimaryButton = primaryText, /*DialogResources.ButtonYes,*/
            SecondaryButton = secondaryText, /*DialogResources.ButtonNo,*/
            OnDialogResult = EventCallback.Factory.Create(receiver, callback)
        });

    /// <summary>
    /// Shows a custom message box. Has a callback function which returns boolean
    /// (true=PrimaryButton clicked, false=SecondaryButton clicked).
    /// </summary>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
    public void ShowMessageBox(DialogParameters<MessageBoxData> parameters)
    {
        Action<DialogSettings> dialogSettings = new(x =>
       {
           x.Alignment = HorizontalAlignment.Center;
           x.Title = parameters.Content.Title;
           x.Modal = string.IsNullOrEmpty(parameters.SecondaryButton);
           x.ShowDismiss = false;
           x.PrimaryButton = parameters.PrimaryButton;
           x.SecondaryButton = parameters.SecondaryButton;
           x.Width = parameters.Width;
           x.Height = parameters.Height;
           x.AriaLabel = $"{parameters.Content.Title}";
           x.OnDialogResult = parameters.OnDialogResult;
       });

        ShowDialog(typeof(MessageBox), parameters.Content, dialogSettings);
    }

    /// <summary>
    /// Shows a panel with the dialog component type as the body,
    /// passing the specified <paramref name="parameters "/>
    /// </summary>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
    public void ShowPanel<T, TData>(DialogParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class
        => ShowPanel(typeof(T), parameters);

    /// <summary>
    /// Shows a panel with the dialog component type as the body,
    /// passing the specified <paramref name="parameters "/>
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
    public void ShowPanel<TData>(Type dialogComponent, DialogParameters<TData> parameters)
        where TData : class
    {
        Action<DialogSettings> settings = new(x =>
        {
            x.Alignment = parameters.Alignment;
            x.Title = parameters.Title;
            x.Modal = parameters.Modal;
            x.ShowTitle = parameters.ShowTitle;
            x.ShowDismiss = parameters.ShowDismiss;
            x.PrimaryButton = parameters.PrimaryButton;
            x.SecondaryButton = parameters.SecondaryButton;
            x.Width = parameters.Width;
            x.AriaLabel = $"{parameters.Title}";
            x.OnDialogResult = parameters.OnDialogResult;
        });

        ShowDialog(dialogComponent, parameters.Content, settings);
    }

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// passing the specified <paramref name="parameters "/>
    /// </summary>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
    public void ShowDialog<T, TData>(DialogParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class
    {
        Action<DialogSettings> settings = new(x =>
        {
            x.Alignment = HorizontalAlignment.Center;
            x.Title = parameters.Title;
            x.Modal = parameters.Modal;
            x.ShowDismiss = parameters.ShowDismiss;
            x.ShowTitle = parameters.ShowTitle;
            x.PrimaryButton = parameters.PrimaryButton;
            x.SecondaryButton = parameters.SecondaryButton;
            x.Width = parameters.Width;
            x.Height = parameters.Height;
            x.AriaLabel = $"{parameters.Title}";
            x.OnDialogResult = parameters.OnDialogResult;
        });

        ShowDialog(typeof(T), parameters.Content, settings);
    }

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// passing the specified <paramref name="parameters "/> and <paramref name="settings "/>
    /// </summary>
    /// <param name="parameters">Content to pass to component being displayed.</param>
    /// <param name="settings">Parameters to configure the dialog component.</param>
    public void ShowDialog<T, TData>(DialogParameters<TData> parameters, Action<DialogSettings>? settings = null)
        where T : IDialogContentComponent<TData>
        where TData : class
    {
        ShowDialog(typeof(T), parameters.Content, settings);
    }

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// passing the specified <paramref name="data "/> 
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="data">Content to pass to component being displayed.</param>
    /// <param name="settings">Parameters to configure the dialog component.</param>
    public virtual void ShowDialog<TData>(Type dialogComponent, TData data, Action<DialogSettings>? settings = null)
        where TData : class
    {
        if (!typeof(IDialogContentComponent).IsAssignableFrom(dialogComponent))
        {
            throw new ArgumentException($"{dialogComponent.FullName} must be a Dialog Component");
        }

        OnShow?.Invoke(dialogComponent, data, settings);
    }

    /// <summary>
    /// Convenience method to create a <see cref="EventCallback"/> for a dialog result.
    /// You can also call <code>EventCallback.Factory.Create</code> directly.
    /// </summary>
    /// <param name="receiver"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public EventCallback<DialogResult> CreateDialogCallback(object receiver, Func<DialogResult, Task> callback) => EventCallback.Factory.Create(receiver, callback);


}
