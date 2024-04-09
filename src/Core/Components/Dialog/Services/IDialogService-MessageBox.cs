namespace Microsoft.FluentUI.AspNetCore.Components;

public partial interface IDialogService
{
    void ShowSuccess(string message, string? title = null);

    void ShowWarning(string message, string? title = null);

    void ShowError(string message, string? title = null);

    void ShowInfo(string message, string? title = null);

    void ShowConfirmation(object receiver, Func<DialogResult, Task> callback, string message, string primaryText = "Yes", string secondaryText = "No", string? title = null);

    void ShowMessageBox(DialogParameters<MessageBoxContent> parameters);

    Task<IDialogReference> ShowSuccessAsync(string message, string? title = null);

    Task<IDialogReference> ShowWarningAsync(string message, string? title = null);

    Task<IDialogReference> ShowErrorAsync(string message, string? title = null);

    Task<IDialogReference> ShowInfoAsync(string message, string? title = null);

    Task<IDialogReference> ShowConfirmationAsync(object receiver, Func<DialogResult, Task> callback, string message, string primaryText = "Yes", string secondaryText = "No", string? title = null);

    Task<IDialogReference> ShowConfirmationAsync(string message, string primaryText = "Yes", string secondaryText = "No", string? title = null);

    Task<IDialogReference> ShowMessageBoxAsync(DialogParameters<MessageBoxContent> parameters);
}
