using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentDialogContainer : IDisposable
{
    private readonly InternalDialogContext _internalDialogContext;
    private readonly Collection<DialogInstance> _dialogs;
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
        _dialogs = new();
        _renderDialogs = RenderDialogs;
    }

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += LocationChanged;

        DialogService.OnShow += ShowDialog;
    }

    private void ShowDialog(Type? dialogComponent, DialogParameters parameters, object content)
    {
        _ = InvokeAsync(() =>
        {
            //DialogSettings? dialogSettings = BuildDialogSettings(settings);

            DialogInstance dialog = new(dialogComponent, parameters, content);

            _dialogs.Add(dialog);
            StateHasChanged();
        });
    }

    internal FluentDialog GetDialogReference(string id)
    {
        return _internalDialogContext.References[id];
    }

    internal DialogInstance? GetDialogInstance(string id)
    {
        if (!_internalDialogContext.References.TryGetValue(id, out FluentDialog? value))
        {
            return null;
        }
        FluentDialog dialog = value;
        return dialog.Instance;

    }

    public void DismissAll()
    {
        _dialogs.ToList().ForEach(r => DismissInstance(r));
        StateHasChanged();
    }

    internal void DismissInstance(string id)
    {
        DialogInstance? instance = GetDialogInstance(id);
        if (instance != null)
        {
            DismissInstance(instance);
        }
    }

    private void DismissInstance(DialogInstance dialog)
    {
        _dialogs.Remove(dialog);
        StateHasChanged();
    }

    private void LocationChanged(object? sender, LocationChangedEventArgs args)
    {
        DismissAll();
    }

    private void ClearAll()
    {
        _ = InvokeAsync(() =>
        {
            _dialogs.Clear();
        });
    }

    private async Task OnDismissAsync(DialogEventArgs args)
    {
        if (args is not null && args.Reason is not null && args.Reason == "dismiss" && !string.IsNullOrWhiteSpace(args.Id))
        {
            FluentDialog dialog = GetDialogReference(args.Id);
            await dialog.CancelAsync();
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
