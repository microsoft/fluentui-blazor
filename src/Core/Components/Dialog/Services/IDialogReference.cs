namespace Microsoft.FluentUI.AspNetCore.Components;

public interface IDialogReference
{
    string Id { get; }

    Task<DialogResult> Result { get; }

    DialogInstance Instance { get; set; }

    Task CloseAsync();

    Task CloseAsync(DialogResult result);

    bool Dismiss(DialogResult result);

    Task<T?> GetReturnValueAsync<T>();
}
