// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class FluentDialogProvider : IAsyncDisposable
{
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Dialog/FluentDialogProvider.razor.js";

    private readonly InternalDialogContext _internalDialogContext;
    private readonly RenderFragment _renderDialogs;
    // Don't set RunContinuationsAsynchronously to true because we want to stay in the same sync context.
    private readonly TaskCompletionSource _moduleLoadedTcs = new(TaskCreationOptions.None);
    private IJSObjectReference? _module;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

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

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (_module is null)
            {
                try
                {
                    _module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
                    _moduleLoadedTcs.TrySetResult();
                }
                catch (Exception ex)
                {
                    _moduleLoadedTcs.TrySetException(ex);
                    throw;
                }
            }
        }
    }

    /// <summary>
    /// Wait for the module to be loaded and return it.
    /// </summary>
    private async Task<IJSObjectReference> GetLoadedModuleAsync()
    {
        if (_module is null)
        {
            await _moduleLoadedTcs.Task;
        }

        // TCS either succeeds and the module is assigned, or it throws and we never get here.
        Debug.Assert(_module is not null);
        return _module;
    }

    private void ShowDialog(IDialogReference dialogReference, Type? dialogComponent, DialogParameters parameters, object content)
    {
        InvokeAsync(async () =>
        {
            var module = await GetLoadedModuleAsync();

            var previouslyFocusedElement = await module.InvokeAsync<IJSObjectReference>("getActiveElement");
            DialogInstance dialog = new(dialogComponent, parameters, content, previouslyFocusedElement);
            dialogReference.Instance = dialog;

            _internalDialogContext.References.Add(dialogReference);
        });
    }

    private async Task<IDialogReference> ShowDialogAsync(IDialogReference dialogReference, Type? dialogComponent, DialogParameters parameters, object content)
    {
        return await Task.Run(async () =>
        {
            var module = await GetLoadedModuleAsync();

            var previouslyFocusedElement = await module.InvokeAsync<IJSObjectReference>("getActiveElement");

            DialogInstance dialog = new(dialogComponent, parameters, content, previouslyFocusedElement);
            dialogReference.Instance = dialog;

            _internalDialogContext.References.Add(dialogReference);
            await InvokeAsync(StateHasChanged);

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
        }
        ;
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

                if (TryGetContent(parameters, out var content) && content != null)
                {
                    dialogInstance.Content = content;
                }

                InvokeAsync(StateHasChanged);
            }
            return reference;
        });
    }

    // Check if the content object is a IDialogParameters<TContent> and get the Content property.
    private bool TryGetContent(object obj, out object? content)
    {
        content = null;

        // Check if the interface is a generic type and inherits from IDialogParameters<TContent>
        foreach (var i in obj.GetType().GetInterfaces())
        {
            if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IDialogParameters<>))
            {
                var contentProperty = i.GetProperty("Content");
                if (contentProperty != null)
                {
                    content = contentProperty.GetValue(obj);
                    return true;
                }
            }
        }

        return false;
    }

    internal void DismissInstance(string id, DialogResult result)
    {
        IDialogReference? reference = GetDialogReference(id);
        if (reference is not null)
        {
            DismissInstance(reference, result);
        }
    }

    internal async Task ReturnFocusAsync(IJSObjectReference element)
    {
        var module = await GetLoadedModuleAsync();

        await module.InvokeVoidAsync("focusElement", element);
        await element.DisposeAsync();
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

    public async ValueTask DisposeAsync()
    {
        if (NavigationManager != null)
        {
            NavigationManager.LocationChanged -= LocationChanged;
        }

        try
        {
            if (_module is not null)
            {
                await _module.DisposeAsync();
            }
        }
        catch (Exception ex) when (ex is JSDisconnectedException ||
                                   ex is OperationCanceledException)
        {
            // The JSRuntime side may routinely be gone already if the reason we're disposing is that
            // the client disconnected. This is not an error.
        }
    }
}
