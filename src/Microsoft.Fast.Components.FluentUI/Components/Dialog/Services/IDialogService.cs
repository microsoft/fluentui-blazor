﻿using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public interface IDialogService
{
    /// <summary>
    /// A event that will be invoked when showing a dialog with a custom component
    /// </summary>
    public event Action<Type, DialogParameters, Action<DialogSettings>?>? OnShow;

    //public event Action<DialogInstance> OnDialogInstanceAdded;
    //public event Action<string> OnDialogCloseRequested;


    void ShowSplashScreen<T>(object receiver, Func<DialogResult, Task> callback, SplashScreenParameters parameters)
        where T : IDialogContentComponent;

    void ShowSplashScreen(Type component, object receiver, Func<DialogResult, Task> callback, SplashScreenParameters parameters);


    void ShowError(string message, string? title = null);

    void ShowInfo(string message, string? title = null);

    void ShowConfirmation(object receiver, Func<DialogResult, Task> callback, string message, string primaryText = "Yes", string secondaryText = "No", string? title = null);

    void ShowMessageBox(MessageBoxParameters parameters);

    void ShowPanel<T>(PanelParameters parameters)
        where T : IDialogContentComponent;

    void ShowPanel(Type component, PanelParameters parameters);

    void ShowDialog<T>(string title, DialogParameters parameters, Action<DialogSettings>? settings)
        where T : IDialogContentComponent;

    void ShowDialog(Type component, string title, DialogParameters parameters, Action<DialogSettings>? settings);

    public EventCallback<DialogResult> CreateDialogCallback(object receiver, Func<DialogResult, Task> callback);
}
