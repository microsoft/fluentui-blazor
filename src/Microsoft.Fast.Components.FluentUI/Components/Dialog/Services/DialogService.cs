﻿using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public class DialogService : IDialogService
{
    /// <summary>
    /// A event that will be invoked when showing a dialog with a custom component
    /// </summary>
    public event Action<Type, DialogParameters, Action<DialogSettings>?>? OnShow;

    //public event Action<DialogInstance>? OnDialogInstanceAdded;
    //public event Action<string>? OnDialogCloseRequested;

    public void ShowSplashScreen<T>(object receiver, Func<DialogResult, Task> callback, SplashScreenParameters parameters)
         where T : IDialogContentComponent
    {
        ShowSplashScreen(typeof(T), receiver, callback, parameters);
    }

    public void ShowSplashScreen(Type component, object receiver, Func<DialogResult, Task> callback, SplashScreenParameters parameters)
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
            x.AriaLabel = $"{parameters.Title} splashscreen";
        });

        ShowDialog(component, parameters.Title ?? "...", parameters, settings);
    }

    // ToDo: Add Success, Warning?
    public void ShowError(string message, string? title = null)
    {
        ShowMessageBox(new MessageBoxParameters()
        {
            Intent = MessageBoxIntent.Error,
            PrimaryButtonText = "Ok", /*DialogResources.ButtonOK,*/
            SecondaryButtonText = string.Empty,
            Icon = FluentIcons.DismissCircle,
            IconColor = Color.Error,
            Title = string.IsNullOrWhiteSpace(title) ? "Error!" /*DialogResources.TitleError*/ : title,
            Message = message,
        }); ;
    }

    public void ShowInfo(string message, string? title = null)
    {
        ShowMessageBox(new MessageBoxParameters()
        {
            Intent = MessageBoxIntent.Info,
            PrimaryButtonText = "Ok", /*DialogResources.ButtonOK,*/
            SecondaryButtonText = string.Empty,
            Icon = FluentIcons.Info,
            IconColor = Color.Warning,
            Title = string.IsNullOrWhiteSpace(title) ? "Information" /*DialogResources.TitleInformation*/ : title,
            Message = message,
        });
    }

    public void ShowConfirmation(object receiver, Func<DialogResult, Task> callback, string message, string primaryText = "Yes", string secondaryText = "No", string? title = null)
    {
        ShowMessageBox(new MessageBoxParameters()
        {
            Intent = MessageBoxIntent.Confirmation,
            PrimaryButtonText = primaryText, /*DialogResources.ButtonYes,*/
            SecondaryButtonText = secondaryText, /*DialogResources.ButtonNo,*/
            Icon = FluentIcons.QuestionCircle,
            IconColor = Color.Success,
            Title = string.IsNullOrWhiteSpace(title) ? "Confirm" /*DialogResources.TitleConfirmation*/ : title,
            Message = message,
            OnDialogResult = EventCallback.Factory.Create(receiver, callback)
        });
    }

    public void ShowMessageBox(MessageBoxParameters parameters)
    {
        Action<DialogSettings> dialogSettings = new(x =>
        {
            x.Alignment = HorizontalAlignment.Center;
            x.Modal = string.IsNullOrEmpty(parameters.SecondaryButtonText);
            x.ShowDismiss = false;
            x.PrimaryButton = parameters.PrimaryButtonText;
            x.SecondaryButton = parameters.SecondaryButtonText;
            x.Width = parameters.Width;
            x.Height = parameters.Height;
        });


        if (string.IsNullOrWhiteSpace(parameters.Title))
        {
            parameters.Title = "...";
        }

        ShowDialog<MessageBox>(parameters.Title, parameters, dialogSettings);
    }

    public void ShowPanel<T>(PanelParameters parameters)
        where T : IDialogContentComponent
    {
        ShowPanel(typeof(T), parameters);
    }

    public void ShowPanel(Type dialogComponent, PanelParameters parameters)
    {
        Action<DialogSettings> settings = new(x =>
        {
            x.Alignment = parameters.Alignment;
            x.Modal = parameters.Modal;
            x.ShowDismiss = parameters.ShowDismiss;
            x.PrimaryButton = parameters.PrimaryButton;
            x.SecondaryButton = parameters.SecondaryButton;
            x.Width = parameters.Width;
        });


        if (string.IsNullOrWhiteSpace(parameters.Title))
        {
            parameters.Title = "...";
        }

        ShowDialog(dialogComponent, parameters.Title, parameters, settings);
    }

    public void ShowDialog<T>(string title, DialogParameters parameters, Action<DialogSettings>? settings = null)
        where T : IDialogContentComponent
    {
        ShowDialog(typeof(T), title, parameters, settings);
    }

    /// <summary>
    /// Shows the dialog with the component type />,
    /// passing the specified <paramref name="parameters"/> 
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="title">TTitle of the dialog</param>
    /// <param name="parameters">Key/Value collection of parameters to pass to component being displayed.</param>
    /// <param name="settings">Settings to configure the dialog component.</param>
    public virtual void ShowDialog(Type dialogComponent, string title, DialogParameters parameters, Action<DialogSettings>? settings = null)
    {
        if (!typeof(IDialogContentComponent).IsAssignableFrom(dialogComponent))
        {
            throw new ArgumentException($"{dialogComponent.FullName} must be a Dialog Component");
        }

        if (!string.IsNullOrEmpty(title))
        {
            parameters.Add("Title", title);
        }

        OnShow?.Invoke(dialogComponent, parameters, settings);
    }

    public EventCallback<DialogResult> CreateDialogCallback(object receiver, Func<DialogResult, Task> callback)
    {
        return EventCallback.Factory.Create(receiver, callback);
    }
}
