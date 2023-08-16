using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDialogContainer : IDisposable
{
    private readonly InternalDialogContext _internalDialogContext;
    private readonly RenderFragment _renderDialogs;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    /// <summary>
    /// Constructs an instance of <see cref="FluentToastContainer"/>.
    /// </summary>
    public FluentDialogContainer()
    {
        _internalDialogContext = new(this);
        _renderDialogs = RenderDialogs;
    }

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += LocationChanged;

        DialogService.OnShow += ShowDialog;
        DialogService.OnShowAsync += ShowDialogAsync;
        DialogService.OnDialogCloseRequested += DismissInstance;
    }

    private void ShowDialog(IDialogReference dialogReference, Type? dialogComponent, DialogParameters parameters, object content)
    {
        _ = InvokeAsync(() =>
        {
            DialogInstance dialog = new(dialogComponent, parameters, content);
            dialogReference.Instance = dialog;

            _internalDialogContext.References.Add(dialogReference);
            StateHasChanged();
        });
    }

    private async Task<IDialogReference> ShowDialogAsync(IDialogReference dialogReference, Type? dialogComponent, DialogParameters parameters, object content)
    {
        return await Task.Run<IDialogReference>(() =>
        {
            DialogInstance dialog = new(dialogComponent, parameters, content);
            dialogReference.Instance = dialog;

            _internalDialogContext.References.Add(dialogReference);
            InvokeAsync(StateHasChanged);

            return dialogReference;
        });
    }

    internal void DismissInstance(string id, DialogResult result)
    {
        var reference = GetDialogReference(id);
        if (reference != null)
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

    //internal void DismissInstance(string id)
    //{
    //    DialogInstance? instance = GetDialogInstance(id);
    //    if (instance != null)
    //    {
    //        DismissInstance(instance);
    //    }
    //}

    //private void DismissInstance(DialogInstance dialog)
    //{
    //    _dialogs.Remove(dialog);
    //    StateHasChanged();
    //}

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
            await dialog!.CloseAsync(DialogResult.Cancel());
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
