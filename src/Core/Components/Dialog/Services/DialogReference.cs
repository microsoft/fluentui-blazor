namespace Microsoft.FluentUI.AspNetCore.Components;

public class DialogReference : IDialogReference
{
    private readonly IDialogService _dialogService;

    private readonly TaskCompletionSource<DialogResult> _resultCompletion = new();

    public DialogReference(string dialogInstanceId, IDialogService dialogService)
    {
        Id = dialogInstanceId;
        _dialogService = dialogService;
    }

    public DialogInstance Instance { get; set; } = default!;

    public string Id { get; }

    public Task<DialogResult> Result => _resultCompletion.Task;

    public async Task CloseAsync()
    {
        await _dialogService.CloseAsync(this);
    }

    public async Task CloseAsync(DialogResult result)
    {
        await _dialogService.CloseAsync(this, result);
    }

    public virtual bool Dismiss(DialogResult result)
    {
        _resultCompletion.TrySetResult(result);

        return true;
    }

    public async Task<T?> GetReturnValueAsync<T>()
    {
        DialogResult? result = await Result;
        try
        {
            if (result.Data == null)
            {
                return default;
            }
            else
            {
                return (T)result.Data ?? default;
            }
        }
        catch (InvalidCastException)
        {
            return default;
        }
    }
}
