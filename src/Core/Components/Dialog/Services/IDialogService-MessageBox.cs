namespace Microsoft.FluentUI.AspNetCore.Components;

public partial interface IDialogService
{

    void ShowSuccess(string message, string? title = null, string? primaryText = null, HorizontalAlignment? actionsHorizontalAlignment = null);

    void ShowWarning(string message, string? title = null, string? primaryText = null, HorizontalAlignment? actionsHorizontalAlignment = null);

    void ShowError(string message, string? title = null, string? primaryText = null, HorizontalAlignment? actionsHorizontalAlignment = null);

    void ShowInfo(string message, string? title = null, string? primaryText = null, HorizontalAlignment? actionsHorizontalAlignment = null);

    void ShowConfirmation(object receiver, Func<DialogResult, Task> callback, string message, string primaryText = "Yes", string secondaryText = "No", string? title = null, HorizontalAlignment? actionsHorizontalAlignment = null, bool actionsReversed = false);

    void ShowMessageBox(DialogParameters<MessageBoxContent> parameters);

    Task<IDialogReference> ShowSuccessAsync(string message, string? title = null, string? primaryText = null, HorizontalAlignment? actionsHorizontalAlignment = null);

    Task<IDialogReference> ShowWarningAsync(string message, string? title = null, string? primaryText = null, HorizontalAlignment? actionsHorizontalAlignment = null);

    Task<IDialogReference> ShowErrorAsync(string message, string? title = null, string? primaryText = null, HorizontalAlignment? actionsHorizontalAlignment = null);

    Task<IDialogReference> ShowInfoAsync(string message, string? title = null, string? primaryText = null, HorizontalAlignment? actionsHorizontalAlignment = null);

    Task<IDialogReference> ShowConfirmationAsync(object receiver, Func<DialogResult, Task> callback, string message, string primaryText = "Yes", string secondaryText = "No", string? title = null, HorizontalAlignment? actionsHorizontalAlignment = null, bool actionsReversed = false);

    Task<IDialogReference> ShowConfirmationAsync(string message, string primaryText = "Yes", string secondaryText = "No", string? title = null, HorizontalAlignment? actionsHorizontalAlignment = null, bool actionsReversed = false);

    Task<IDialogReference> ShowMessageBoxAsync(DialogParameters<MessageBoxContent> parameters);
}
