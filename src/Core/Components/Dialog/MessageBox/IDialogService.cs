// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial interface IDialogService
{

    /// <summary>
    /// Shows a dialog with a success (green) icon, a message and an OK button.
    /// </summary>
    /// <param name="message">Message to display in the dialog.</param>
    /// <param name="title">Title to display in the dialog header.</param>
    /// <param name="button">Text to display in the primary action button.</param>
    /// <returns>Result of the dialog. Always `Cancelled = false`.</returns>
    Task<DialogResult> ShowSuccessAsync(string message, string? title = null, string? button = null);

    /// <summary>
    /// Shows a dialog with a warning (orange) icon, a message and an OK button.
    /// </summary>
    /// <param name="message">Message to display in the dialog.</param>
    /// <param name="title">Title to display in the dialog header. Default is "Success".</param>
    /// <param name="button">Text to display in the primary action button. Default is "OK".</param>
    /// <returns>Result of the dialog. Always `Cancelled = false`.</returns>
    Task<DialogResult> ShowWarningAsync(string message, string? title = null, string? button = null);

    /// <summary>
    /// Shows a dialog with an error (red) icon, a message and an OK button.
    /// </summary>
    /// <param name="message">Message to display in the dialog.</param>
    /// <param name="title">Title to display in the dialog header. Default is "Error".</param>
    /// <param name="button">Text to display in the primary action button. Default is "OK".</param>
    /// <returns>Result of the dialog. Always `Cancelled = false`.</returns>
    Task<DialogResult> ShowErrorAsync(string message, string? title = null, string? button = null);

    /// <summary>
    /// Shows a dialog with an information (gray) icon, a message and an OK button.
    /// </summary>
    /// <param name="message">Message to display in the dialog.</param>
    /// <param name="title">Title to display in the dialog header. Default is "Information".</param>
    /// <param name="button">Text to display in the primary action button. Default is "OK".</param>
    /// <returns>Result of the dialog. Always `Cancelled = false`.</returns>
    Task<DialogResult> ShowInfoAsync(string message, string? title = null, string? button = null);

    /// <summary>
    /// Shows a dialog with a confirmation icon, a message and a Yes/No buttons.
    /// </summary>
    /// <param name="message">Message to display in the dialog.</param>
    /// <param name="title">Title to display in the dialog header. Default is "Confirmation".</param>
    /// <param name="primaryButton">Text to display in the primary action button.  Default is "Yes".</param>
    /// <param name="secondaryButton">Text to display in the secondary action button.  Default is "No".</param>
    /// <returns>Result of the dialog: Yes returns `Cancelled = false`, No returns `Cancelled = true`</returns>
    Task<DialogResult> ShowConfirmationAsync(string message, string? title = null, string? primaryButton = null, string? secondaryButton = null);

    /// <summary>
    /// Shows a dialog with the specified options.
    /// </summary>
    /// <param name="options">Options to configure the dialog.</param>
    /// <returns></returns>
    Task<DialogResult> ShowMessageBoxAsync(MessageBoxOptions options);
}
