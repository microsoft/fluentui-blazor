namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
public interface IDialogInstance
{
    /// <summary>
    /// Returns True is the validation is correct and the process can continue.
    /// Returns False to stop the dialog from closing.
    /// </summary>
    /// <param name="cancelled"></param>
    /// <returns></returns>
    Task<bool> OnValidateAsync(bool cancelled)
    {
        return Task.FromResult(true);
    }

    /// <summary>
    /// Dialog closing.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    Task OnCloseAsync(DialogResult result);
}
