using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class DialogService : IDialogService
{
    /// <summary>
    /// A event that will be invoked when showing a dialog with a custom component
    /// </summary>
    public event Action<IDialogReference, Type?, DialogParameters, object>? OnShow;

    public event Func<IDialogReference, Type?, DialogParameters, object, Task<IDialogReference>>? OnShowAsync;

    public event Action<IDialogReference, DialogResult>? OnDialogCloseRequested;

    /// <summary>
    /// Shows the standard <see cref="FluentSplashScreen"/> with the given parameters."/>
    /// </summary>
    /// <param name="receiver">The componente that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public void ShowSplashScreen(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
        => ShowSplashScreen<FluentSplashScreen>(receiver, callback, parameters);

    /// <summary>
    /// Shows a custom splash screen dialog with the given parameters."/>
    /// </summary>
    /// <param name="receiver">The componente that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public void ShowSplashScreen<T>(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
        where T : IDialogContentComponent<SplashScreenContent>
        => ShowSplashScreen(typeof(T), receiver, callback, parameters);


    /// <summary>
    /// Shows a splash screen of the given type with the given parameters."/>
    /// </summary>
    /// <param name="component">The type of the component to show</param>
    /// <param name="receiver">The componente that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public void ShowSplashScreen(Type component, object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
    {
        DialogParameters dialogParameters = new()
        {
            Alignment = HorizontalAlignment.Center,
            Modal = false,
            ShowDismiss = false,
            ShowTitle = false,
            PrimaryAction = null,
            SecondaryAction = null,
            Width = parameters.Width ?? "600px",
            Height = parameters.Height ?? "370px",
            DialogBodyStyle = "width: 100%; height: 100%; margin: 0px;",
            AriaLabel = $"{parameters.Content.Title} splashscreen",
            OnDialogResult = EventCallback.Factory.Create(receiver, callback),
        };

        ShowDialog(component, parameters.Content, dialogParameters);
    }

    /// <summary>
    /// Shows a success message box. Does not have a callback function.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public void ShowSuccess(string message, string? title = null) => ShowMessageBox(new DialogParameters<MessageBoxContent>()
    {
        Content = new MessageBoxContent()
        {
            Title = string.IsNullOrWhiteSpace(title) ? "Success!" /*DialogResources.TitleError*/ : title,
            Intent = MessageBoxIntent.Success,
            Icon = new CoreIcons.Filled.Size24.CheckmarkCircle(),
            IconColor = Color.Success,
            Message = message,
        },
        PrimaryAction = "Ok", /*DialogResources.ButtonOK,*/
        SecondaryAction = string.Empty,
    });

    /// <summary>
    /// Shows a warning message box. Does not have a callback function.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public void ShowWarning(string message, string? title = null) => ShowMessageBox(new DialogParameters<MessageBoxContent>()
    {
        Content = new MessageBoxContent()
        {
            Title = string.IsNullOrWhiteSpace(title) ? "Warning!" /*DialogResources.TitleError*/ : title,
            Intent = MessageBoxIntent.Warning,
            Icon = new CoreIcons.Filled.Size24.Warning(),
            IconColor = Color.Warning,
            Message = message,
        },
        PrimaryAction = "Ok", /*DialogResources.ButtonOK,*/
        SecondaryAction = string.Empty,
    });

    /// <summary>
    /// Shows an error message box. Does not have a callback function.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public void ShowError(string message, string? title = null) => ShowMessageBox(new DialogParameters<MessageBoxContent>()
    {
        Content = new MessageBoxContent()
        {
            Title = string.IsNullOrWhiteSpace(title) ? "Error!" /*DialogResources.TitleError*/ : title,
            Intent = MessageBoxIntent.Error,
            Icon = new CoreIcons.Filled.Size24.DismissCircle(),
            IconColor = Color.Error,
            Message = message,
        },
        PrimaryAction = "Ok", /*DialogResources.ButtonOK,*/
        SecondaryAction = string.Empty,
    });

    /// <summary>
    /// Shows an information message box. Does not have a callback function.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public void ShowInfo(string message, string? title = null) => ShowMessageBox(new DialogParameters<MessageBoxContent>()
    {
        Content = new MessageBoxContent()
        {
            Title = string.IsNullOrWhiteSpace(title) ? "Information" /*DialogResources.TitleInformation*/ : title,
            Intent = MessageBoxIntent.Info,
            Icon = new CoreIcons.Filled.Size24.Info(),
            IconColor = Color.Info,
            Message = message,
        },
        PrimaryAction = "Ok", /*DialogResources.ButtonOK,*/
        SecondaryAction = string.Empty,
    });

    /// <summary>
    /// Shows a confirmation message box. Has a callback function which returns boolean 
    /// (true=PrimaryAction clicked, false=SecondaryAction clicked).
    /// </summary>
    /// <param name="receiver">The component that receives the callback function.</param>
    /// <param name="callback">The callback function.</param>
    /// <param name="message">The message to display.</param>
    /// <param name="primaryText">The text to display on the primary button.</param>
    /// <param name="secondaryText">The text to display on the secondary button.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public void ShowConfirmation(object receiver, Func<DialogResult, Task> callback, string message, string primaryText = "Yes", string secondaryText = "No", string? title = null)
        => ShowMessageBox(new DialogParameters<MessageBoxContent>()
        {
            Content = new MessageBoxContent()
            {
                Title = string.IsNullOrWhiteSpace(title) ? "Confirm" /*DialogResources.TitleConfirmation*/ : title,
                Intent = MessageBoxIntent.Confirmation,
                Icon = new CoreIcons.Regular.Size24.QuestionCircle(),
                IconColor = Color.Success,
                Message = message,
            },
            PrimaryAction = primaryText, /*DialogResources.ButtonYes,*/
            SecondaryAction = secondaryText, /*DialogResources.ButtonNo,*/
            OnDialogResult = EventCallback.Factory.Create(receiver, callback)
        });

    /// <summary>
    /// Shows a custom message box. Has a callback function which returns boolean
    /// (true=PrimaryAction clicked, false=SecondaryAction clicked).
    /// </summary>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
    public void ShowMessageBox(DialogParameters<MessageBoxContent> parameters)
    {
        DialogParameters dialogParameters = new()
        {
            Alignment = HorizontalAlignment.Center,
            Title = parameters.Content.Title,
            Modal = string.IsNullOrEmpty(parameters.SecondaryAction),
            ShowDismiss = false,
            PrimaryAction = parameters.PrimaryAction,
            SecondaryAction = parameters.SecondaryAction,
            Width = parameters.Width,
            Height = parameters.Height,
            AriaLabel = $"{parameters.Content.Title}",
            OnDialogResult = parameters.OnDialogResult,
        };

        ShowDialog(typeof(MessageBox), parameters.Content, dialogParameters);
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
        DialogParameters dialogParameters = new()
        {
            Alignment = parameters.Alignment,
            Title = parameters.Title,
            Modal = parameters.Modal,
            ShowTitle = parameters.ShowTitle,
            ShowDismiss = parameters.ShowDismiss,
            PrimaryAction = parameters.PrimaryAction,
            SecondaryAction = parameters.SecondaryAction,
            Width = parameters.Width,
            AriaLabel = $"{parameters.Title}",
            OnDialogResult = parameters.OnDialogResult,
        };

        ShowDialog(dialogComponent, parameters.Content, dialogParameters);
    }

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// passing the specified <paramref name="parameters "/>
    /// </summary>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
    public void ShowDialog<T, TContent>(DialogParameters<TContent> parameters)
        where T : IDialogContentComponent<TContent>
        where TContent : class
    {
        DialogParameters dialogParameters = new()
        {
            Alignment = HorizontalAlignment.Center,
            Title = parameters.Title,
            Modal = parameters.Modal,
            ShowDismiss = parameters.ShowDismiss,
            ShowTitle = parameters.ShowTitle,
            PrimaryAction = parameters.PrimaryAction,
            PrimaryActionEnabled = parameters.PrimaryActionEnabled,
            SecondaryAction = parameters.SecondaryAction,
            SecondaryActionEnabled = parameters.SecondaryActionEnabled,
            Width = parameters.Width,
            Height = parameters.Height,
            AriaLabel = $"{parameters.Title}",
            OnDialogResult = parameters.OnDialogResult,
        };

        ShowDialog(typeof(T), parameters.Content, dialogParameters);
    }

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// passing the specified <paramref name="content "/> 
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="content">Content to pass to component being displayed.</param>
    /// <param name="parameters">Parameters to configure the dialog component.</param>
    public virtual void ShowDialog<TContent>(Type dialogComponent, TContent content, DialogParameters parameters)
        where TContent : class
    {
        if (!typeof(IDialogContentComponent).IsAssignableFrom(dialogComponent))
        {
            throw new ArgumentException($"{dialogComponent.FullName} must be a Dialog Component");
        }

        IDialogReference? dialogReference = new DialogReference(parameters.Id, this);

        OnShow?.Invoke(dialogReference, dialogComponent, parameters, content);
    }


    /// <summary>
    /// Shows the standard <see cref="FluentSplashScreen"/> with the given parameters."/>
    /// </summary>
    /// <param name="receiver">The componente that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public async Task<IDialogReference> ShowSplashScreenAsync(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
        => await ShowSplashScreenAsync<FluentSplashScreen>(receiver, callback, parameters);

    /// <summary>
    /// Shows a custom splash screen dialog with the given parameters."/>
    /// </summary>
    /// <param name="receiver">The componente that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public async Task<IDialogReference> ShowSplashScreenAsync<T>(object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
        where T : IDialogContentComponent<SplashScreenContent>
        => await ShowSplashScreenAsync(typeof(T), receiver, callback, parameters);


    /// <summary>
    /// Shows a splash screen of the given type with the given parameters."/>
    /// </summary>
    /// <param name="component">The type of the component to show</param>
    /// <param name="receiver">The componente that receives the callback</param>
    /// <param name="callback">Name of the callback function</param>
    /// <param name="parameters"><see cref="SplashScreenContent"/> that holds the content to display</param>
    public async Task<IDialogReference> ShowSplashScreenAsync(Type component, object receiver, Func<DialogResult, Task> callback, DialogParameters<SplashScreenContent> parameters)
    {
        DialogParameters dialogParameters = new()
        {
            Alignment = HorizontalAlignment.Center,
            Modal = false,
            ShowDismiss = false,
            ShowTitle = false,
            PrimaryAction = null,
            SecondaryAction = null,
            Width = parameters.Width ?? "600px",
            Height = parameters.Height ?? "370px",
            DialogBodyStyle = "width: 100%; height: 100%; margin: 0px;",
            AriaLabel = $"{parameters.Content.Title} splashscreen",
            OnDialogResult = EventCallback.Factory.Create(receiver, callback),
        };

        return await ShowDialogAsync(component, parameters.Content, dialogParameters);
    }

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// passing the specified <paramref name="parameters "/>
    /// </summary>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
    public async Task<IDialogReference> ShowDialogAsync<T, TContent>(DialogParameters<TContent> parameters)
        where T : IDialogContentComponent<TContent>
        where TContent : class
    {
        DialogParameters dialogParameters = new()
        {
            Id = parameters.Id,
            Alignment = HorizontalAlignment.Center,
            Title = parameters.Title,
            Modal = parameters.Modal,
            TrapFocus = parameters.TrapFocus,
            ShowDismiss = parameters.ShowDismiss,
            ShowTitle = parameters.ShowTitle,
            PrimaryAction = parameters.PrimaryAction,
            PrimaryActionEnabled = parameters.PrimaryActionEnabled,
            SecondaryAction = parameters.SecondaryAction,
            SecondaryActionEnabled = parameters.SecondaryActionEnabled,
            Width = parameters.Width,
            Height = parameters.Height,
            AriaLabel = $"{parameters.Title}",
            OnDialogResult = parameters.OnDialogResult,
        };

        return await ShowDialogAsync(typeof(T), parameters.Content, dialogParameters);
    }

    /// <summary>
    /// Shows a success message box. Does not have a callback function.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public async Task ShowSuccessAsync(string message, string? title = null) => await ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>()
    {
        Content = new MessageBoxContent()
        {
            Title = string.IsNullOrWhiteSpace(title) ? "Success!" /*DialogResources.TitleError*/ : title,
            Intent = MessageBoxIntent.Success,
            Icon = new CoreIcons.Filled.Size24.CheckmarkCircle(),
            IconColor = Color.Success,
            Message = message,
        },
        PrimaryAction = "Ok", /*DialogResources.ButtonOK,*/
        SecondaryAction = string.Empty,
    });

    /// <summary>
    /// Shows a warning message box. Does not have a callback function.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public async Task ShowWarningAsync(string message, string? title = null) => await ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>()
    {
        Content = new MessageBoxContent()
        {
            Title = string.IsNullOrWhiteSpace(title) ? "Warning!" /*DialogResources.TitleError*/ : title,
            Intent = MessageBoxIntent.Warning,
            Icon = new CoreIcons.Filled.Size24.Warning(),
            IconColor = Color.Warning,
            Message = message,
        },
        PrimaryAction = "Ok", /*DialogResources.ButtonOK,*/
        SecondaryAction = string.Empty,
    });

    /// <summary>
    /// Shows an error message box. Does not have a callback function.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public async Task ShowErrorAsync(string message, string? title = null) => await ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>()
    {
        Content = new MessageBoxContent()
        {
            Title = string.IsNullOrWhiteSpace(title) ? "Error!" /*DialogResources.TitleError*/ : title,
            Intent = MessageBoxIntent.Error,
            Icon = new CoreIcons.Filled.Size24.DismissCircle(),
            IconColor = Color.Error,
            Message = message,
        },
        PrimaryAction = "Ok", /*DialogResources.ButtonOK,*/
        SecondaryAction = string.Empty,
    });

    /// <summary>
    /// Shows an information message box. Does not have a callback function.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public async Task ShowInfoAsync(string message, string? title = null) => await ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>()
    {
        Content = new MessageBoxContent()
        {
            Title = string.IsNullOrWhiteSpace(title) ? "Information" /*DialogResources.TitleInformation*/ : title,
            Intent = MessageBoxIntent.Info,
            Icon = new CoreIcons.Filled.Size24.Info(),
            IconColor = Color.Info,
            Message = message,
        },
        PrimaryAction = "Ok", /*DialogResources.ButtonOK,*/
        SecondaryAction = string.Empty,
    });

    /// <summary>
    /// Shows a confirmation message box. Has a callback function which returns boolean 
    /// (true=PrimaryAction clicked, false=SecondaryAction clicked).
    /// </summary>
    /// <param name="receiver">The component that receives the callback function.</param>
    /// <param name="callback">The callback function.</param>
    /// <param name="message">The message to display.</param>
    /// <param name="primaryText">The text to display on the primary button.</param>
    /// <param name="secondaryText">The text to display on the secondary button.</param>
    /// <param name="title">The title to display on the dialog.</param>
    public async Task<IDialogReference> ShowConfirmationAsync(object receiver, Func<DialogResult, Task> callback, string message, string primaryText = "Yes", string secondaryText = "No", string? title = null)
        => await ShowMessageBoxAsync(new DialogParameters<MessageBoxContent>()
        {
            Content = new MessageBoxContent()
            {
                Title = string.IsNullOrWhiteSpace(title) ? "Confirm" /*DialogResources.TitleConfirmation*/ : title,
                Intent = MessageBoxIntent.Confirmation,
                Icon = new CoreIcons.Regular.Size24.QuestionCircle(),
                IconColor = Color.Success,
                Message = message,
            },
            PrimaryAction = primaryText, /*DialogResources.ButtonYes,*/
            SecondaryAction = secondaryText, /*DialogResources.ButtonNo,*/
            OnDialogResult = EventCallback.Factory.Create(receiver, callback)
        });

    /// <summary>
    /// Shows a custom message box. Has a callback function which returns boolean
    /// (true=PrimaryAction clicked, false=SecondaryAction clicked).
    /// </summary>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
    public async Task<IDialogReference> ShowMessageBoxAsync(DialogParameters<MessageBoxContent> parameters)
    {
        DialogParameters dialogParameters = new()
        {
            Alignment = HorizontalAlignment.Center,
            Title = parameters.Content.Title,
            Modal = string.IsNullOrEmpty(parameters.SecondaryAction),
            ShowDismiss = false,
            PrimaryAction = parameters.PrimaryAction,
            SecondaryAction = parameters.SecondaryAction,
            Width = parameters.Width,
            Height = parameters.Height,
            AriaLabel = $"{parameters.Content.Title}",
            OnDialogResult = parameters.OnDialogResult,
        };

        return await ShowDialogAsync(typeof(MessageBox), parameters.Content, dialogParameters);
    }

    /// <summary>
    /// Shows a panel with the dialog component type as the body,
    /// passing the specified <paramref name="parameters "/>
    /// </summary>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
    public async Task<IDialogReference> ShowPanelAsync<T, TData>(DialogParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class
        => await ShowPanelAsync(typeof(T), parameters);

    /// <summary>
    /// Shows a panel with the dialog component type as the body,
    /// passing the specified <paramref name="parameters "/>
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
    public async Task<IDialogReference> ShowPanelAsync<TData>(Type dialogComponent, DialogParameters<TData> parameters)
        where TData : class
    {
        DialogParameters dialogParameters = new()
        {
            Alignment = parameters.Alignment,
            Title = parameters.Title,
            Modal = parameters.Modal,
            ShowTitle = parameters.ShowTitle,
            ShowDismiss = parameters.ShowDismiss,
            PrimaryAction = parameters.PrimaryAction,
            SecondaryAction = parameters.SecondaryAction,
            Width = parameters.Width,
            AriaLabel = $"{parameters.Title}",
            OnDialogResult = parameters.OnDialogResult,
        };

        return await ShowDialogAsync(dialogComponent, parameters.Content, dialogParameters);
    }

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// passing the specified <paramref name="content "/> 
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="content">Content to pass to component being displayed.</param>
    /// <param name="parameters">Parameters to configure the dialog component.</param>
    public virtual async Task<IDialogReference> ShowDialogAsync<TContent>(Type dialogComponent, TContent content, DialogParameters parameters)
        where TContent : class
    {
        if (!typeof(IDialogContentComponent).IsAssignableFrom(dialogComponent))
        {
            throw new ArgumentException($"{dialogComponent.FullName} must be a Dialog Component");
        }

        IDialogReference? dialogReference = new DialogReference(parameters.Id, this);

        return await OnShowAsync!.Invoke(dialogReference, dialogComponent, parameters, content);
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

    public async Task CloseAsync(DialogReference dialog, DialogResult result)
    {
        await Task.Run(() => { });  // To avoid warning
        OnDialogCloseRequested?.Invoke(dialog, result);
    }

    internal virtual IDialogReference CreateReference(string id)
    {
        return new DialogReference(id, this);
    }
}
