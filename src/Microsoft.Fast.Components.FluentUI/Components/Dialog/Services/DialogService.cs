using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class DialogService : IDialogService
{
    /// <summary>
    /// A event that will be invoked when showing a dialog with a custom component
    /// </summary>
    public event Action<Type, object, Action<DialogSettings>?>? OnShow;

    public void ShowSplashScreen(object receiver, Func<DialogResult, Task> callback, ISplashScreenParameters parameters)
        => ShowSplashScreen<FluentSplashScreen>(receiver, callback, parameters);

    public void ShowSplashScreen<T>(object receiver, Func<DialogResult, Task> callback, ISplashScreenParameters parameters)
        where T : IDialogContentComponent<SplashScreenData>
        => ShowSplashScreen(typeof(T), receiver, callback, parameters);

    public void ShowSplashScreen(Type component, object receiver, Func<DialogResult, Task> callback, ISplashScreenParameters parameters)
    {
        SplashScreenData splashScreenData = new()
        {
            Title = parameters.Title,
            SubTitle = parameters.SubTitle,
            LoadingText = parameters.LoadingText,
            Message = parameters.Message,
            Logo = parameters.Logo
        };

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
            x.AriaLabel = $"{parameters.Title} splashscreen";
        });

        ShowDialog(component, splashScreenData, settings);
    }

    // ToDo: Add Success, Warning?
    public void ShowError(string message, string? title = null) => ShowMessageBox(new MessageBoxParameters()
    {
        Title = string.IsNullOrWhiteSpace(title) ? "Error!" /*DialogResources.TitleError*/ : title,
        Intent = MessageBoxIntent.Error,
        PrimaryButton = "Ok", /*DialogResources.ButtonOK,*/
        SecondaryButton = string.Empty,
        Icon = FluentIcons.DismissCircle,
        IconColor = Color.Error,
        Message = message,
    });


    public void ShowInfo(string message, string? title = null) => ShowMessageBox(new MessageBoxParameters()
    {
        Title = string.IsNullOrWhiteSpace(title) ? "Information" /*DialogResources.TitleInformation*/ : title,
        Intent = MessageBoxIntent.Info,
        PrimaryButton = "Ok", /*DialogResources.ButtonOK,*/
        SecondaryButton = string.Empty,
        Icon = FluentIcons.Info,
        IconColor = Color.Warning,
        Message = message,
    });

    public void ShowConfirmation(object receiver, Func<DialogResult, Task> callback, string message, string primaryText = "Yes", string secondaryText = "No", string? title = null)
        => ShowMessageBox(new MessageBoxParameters()
        {
            Title = string.IsNullOrWhiteSpace(title) ? "Confirm" /*DialogResources.TitleConfirmation*/ : title,
            Intent = MessageBoxIntent.Confirmation,
            PrimaryButton = primaryText, /*DialogResources.ButtonYes,*/
            SecondaryButton = secondaryText, /*DialogResources.ButtonNo,*/
            Icon = FluentIcons.QuestionCircle,
            IconColor = Color.Success,
            Message = message,
            OnDialogResult = EventCallback.Factory.Create(receiver, callback)
        });

    public void ShowMessageBox(MessageBoxParameters parameters)
    {
        MessageBoxData messageBoxData = new()
        {
            Title = parameters.Title,
            Intent = parameters.Intent,
            Icon = parameters.Icon,
            IconColor = parameters.IconColor,
            Message = parameters.Message,
            MarkupMessage = parameters.MarkupMessage,
        };

        Action<DialogSettings> dialogSettings = new(x =>
        {
            x.Alignment = HorizontalAlignment.Center;
            x.Title = parameters.Title;
            x.Modal = string.IsNullOrEmpty(parameters.SecondaryButton);
            x.ShowDismiss = false;
            x.PrimaryButton = parameters.PrimaryButton;
            x.SecondaryButton = parameters.SecondaryButton;
            x.Width = parameters.Width;
            x.Height = parameters.Height;
            x.OnDialogResult = parameters.OnDialogResult;
        });

        ShowDialog(typeof(MessageBox), messageBoxData, dialogSettings);
    }

    public void ShowPanel<T, TData>(PanelParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class
        => ShowPanel(typeof(T), parameters);

    public void ShowPanel<TData>(Type dialogComponent, PanelParameters<TData> parameters)
        where TData : class
    {
        Action<DialogSettings> settings = new(x =>
        {
            x.Alignment = parameters.Alignment;
            x.Title = parameters.Title;
            x.Modal = parameters.Modal;
            x.ShowDismiss = parameters.ShowDismiss;
            x.PrimaryButton = parameters.PrimaryButton;
            x.SecondaryButton = parameters.SecondaryButton;
            x.Width = parameters.Width;
            x.OnDialogResult = parameters.OnDialogResult;
        });


        if (string.IsNullOrWhiteSpace(parameters.Title))
        {
            parameters.Title = "...";
        }

        ShowDialog(dialogComponent, parameters.Data, settings);
    }

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
            x.ShowTitle = true;
            x.PrimaryButton = parameters.PrimaryButton;
            x.SecondaryButton = parameters.SecondaryButton;
            x.Width = parameters.Width;
            x.Height = parameters.Height;
            x.AriaLabel = $"{parameters.Title} dialog";
            x.OnDialogResult = parameters.OnDialogResult;
        });

        ShowDialog(typeof(T), parameters.Data, settings);
    }

    public void ShowDialog<T, TData>(DialogParameters<TData> parameters, Action<DialogSettings> settings)
        where T : IDialogContentComponent<TData>
        where TData : class
    {
        ShowDialog(typeof(T), parameters.Data, settings);
    }

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// passing the specified <paramref name="data "/> 
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="data">Data to pass to component being displayed.</param>
    /// <param name="settings">Settings to configure the dialog component.</param>
    public virtual void ShowDialog<TData>(Type dialogComponent, TData data, Action<DialogSettings> settings)
        where TData : class
    {
        if (!typeof(IDialogContentComponent).IsAssignableFrom(dialogComponent))
        {
            throw new ArgumentException($"{dialogComponent.FullName} must be a Dialog Component");
        }

        OnShow?.Invoke(dialogComponent, data, settings);
    }

    public EventCallback<DialogResult> CreateDialogCallback(object receiver, Func<DialogResult, Task> callback) => EventCallback.Factory.Create(receiver, callback);


}
