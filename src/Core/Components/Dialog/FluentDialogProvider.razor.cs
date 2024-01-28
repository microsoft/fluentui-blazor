using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentDialogProvider : IDisposable
{
    private readonly InternalDialogContext _internalDialogContext;
    private readonly RenderFragment _renderDialogs;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Constructs an instance of <see cref="FluentToastProvider"/>.
    /// </summary>
    public FluentDialogProvider()
    {
        _internalDialogContext = new(this);
        _renderDialogs = RenderDialogs;

        var temp1 = new MessageBox(); // To avoid WASM trimming from removing this class
    }

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += LocationChanged;

        DialogService.OnShow += ShowDialog;
        DialogService.OnShowAsync += ShowDialogAsync;
        DialogService.OnUpdate += UpdateDialog;
        DialogService.OnUpdateAsync += UpdateDialogAsync;
        DialogService.OnDialogCloseRequested += DismissInstance;
    }

    private void ShowDialog(IDialogReference dialogReference, Type? dialogComponent, DialogParameters parameters, object content)
    {
        DialogInstance dialog = new(dialogComponent, parameters, content);
        dialogReference.Instance = dialog;

        _internalDialogContext.References.Add(dialogReference);
        InvokeAsync(StateHasChanged);
    }

    private async Task<IDialogReference> ShowDialogAsync(IDialogReference dialogReference, Type? dialogComponent, DialogParameters parameters, object content)
    {
        return await Task.Run(() =>
        {
            DialogInstance dialog = new(dialogComponent, parameters, content);
            dialogReference.Instance = dialog;

            _internalDialogContext.References.Add(dialogReference);
            InvokeAsync(StateHasChanged);

            return dialogReference;
        });
    }

    private void UpdateDialog(string? dialogId, DialogParameters parameters)
    {
        IDialogReference reference = _internalDialogContext.References.SingleOrDefault(x => x.Id == dialogId)!;
        DialogInstance? dialogInstance = reference.Instance;

        if (dialogInstance is not null)
        {
            dialogInstance.Parameters = parameters;

            InvokeAsync(StateHasChanged);
        };
    }

    private async Task<IDialogReference?> UpdateDialogAsync(string? dialogId, DialogParameters parameters)
    {
        return await Task.Run(() =>
        {
            IDialogReference? reference = _internalDialogContext.References.SingleOrDefault(x => x.Id == dialogId)!;
            DialogInstance? dialogInstance = reference?.Instance;

            if (dialogInstance is not null)
            {
                dialogInstance.Parameters = parameters;

                InvokeAsync(StateHasChanged);
            }
            return reference;
        });
    }

    internal void DismissInstance(string id, DialogResult result)
    {
        IDialogReference? reference = GetDialogReference(id);
        if (reference is not null)
        {
            DismissInstance(reference, result);
        }
    }

    internal IDialogReference? GetDialogReference(string id)
    {
        return _internalDialogContext.References.SingleOrDefault(x => x.Id == id);
    }

    public void DismissAll()
    {
        _internalDialogContext.References.ToList().ForEach(r => DismissInstance(r, DialogResult.Cancel()));
        StateHasChanged();
    }

    private void DismissInstance(IDialogReference dialog, DialogResult result)
    {
        if (!dialog.Dismiss(result))
        {
            return;
        }

        _internalDialogContext.References.Remove(dialog);
        StateHasChanged();
    }

    private void LocationChanged(object? sender, LocationChangedEventArgs args)
    {
        DismissAll();
    }

    private void ClearAll()
    {
        _internalDialogContext.References.Clear();
    }

    private async Task OnDismissAsync(DialogEventArgs args)
    {
        if (args is not null && args.Reason is not null && args.Reason == "dismiss" && !string.IsNullOrWhiteSpace(args.Id))
        {
            IDialogReference? dialog = GetDialogReference(args.Id);
            if (dialog == null)
            {
                return;
            }

            if (dialog.Instance.Parameters.PreventDismissOnOverlayClick == false)
            {
                if (dialog.Instance.Parameters.OnDialogClosing.HasDelegate)
                {
                    await dialog.Instance.Parameters.OnDialogClosing.InvokeAsync(dialog.Instance);
                }
                await dialog!.CloseAsync(DialogResult.Cancel());
            }
        }
    }

    public void Dispose()
    {
        if (NavigationManager != null)
        {
            NavigationManager.LocationChanged -= LocationChanged;
        }
    }
}
