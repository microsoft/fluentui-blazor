using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDialogProvider : IDisposable
{
    private readonly Collection<IDialogReference> _dialogs = new();
    private readonly DialogOptions _globalDialogOptions = new();

    [Inject]
    protected IDialogService DialogService { get; set; } = default!;

    [Inject]
    protected NavigationManager NavigationManager { get; set; } = default!;

    protected override void OnInitialized()
    {
        DialogService.OnDialogInstanceAdded += AddInstance;
        DialogService.OnDialogCloseRequested += DismissInstance;
        NavigationManager.LocationChanged += LocationChanged;
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
        return _dialogs.SingleOrDefault(x => x.Id == id);
    }

    private void AddInstance(IDialogReference dialog)
    {
        _dialogs.Add(dialog);
        StateHasChanged();
    }

    public void DismissAll()
    {
        _dialogs.ToList().ForEach(r => DismissInstance(r, DialogResult.Cancel()));
        StateHasChanged();
    }

    private void DismissInstance(IDialogReference dialog, DialogResult result)
    {
        if (!dialog.Dismiss(result))
        {
            return;
        }

        _dialogs.Remove(dialog);
        StateHasChanged();
    }

    private void LocationChanged(object? sender, LocationChangedEventArgs args)
    {
        DismissAll();
    }

    public void Dispose()
    {
        if (NavigationManager != null)
        {
            NavigationManager.LocationChanged -= LocationChanged;
        }
    }
}
