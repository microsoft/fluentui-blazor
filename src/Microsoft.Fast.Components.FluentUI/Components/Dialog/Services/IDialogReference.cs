using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public interface IDialogReference
{
    string Id { get; }

    RenderFragment? RenderFragment { get; set; }

    bool AreParametersRendered { get; set; }

    Task<DialogResult> Result { get; }

    object? Dialog { get; }

    Task CloseAsync();

    Task CloseAsync(DialogResult result);

    bool Dismiss(DialogResult result);

    void InjectRenderFragment(RenderFragment rf);

    void InjectDialog(object inst);

    Task<T?> GetReturnValueAsync<T>();
}
