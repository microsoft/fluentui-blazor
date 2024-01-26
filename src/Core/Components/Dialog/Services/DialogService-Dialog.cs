namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class DialogService
{

    /// <inheritdoc cref="IDialogService.ShowDialogAsync{TData}(Type, TData, DialogParameters)"/>
    public virtual async Task<IDialogReference> ShowDialogAsync<TData>(Type dialogComponent, TData data, DialogParameters parameters)
        where TData : class
    {
        if (!typeof(IDialogContentComponent).IsAssignableFrom(dialogComponent))
        {
            throw new ArgumentException($"{dialogComponent.FullName} must be a Dialog Component");
        }

        if (OnShowAsync is null)
        {
            throw new ArgumentNullException(nameof(OnShowAsync), "<FluentDialogProvider /> needs to be added to the main layout of your application/site.");
        }

        IDialogReference? dialogReference = new DialogReference(parameters.Id, this);
        return await OnShowAsync.Invoke(dialogReference, dialogComponent, parameters, data);
    }

    /// <inheritdoc cref="IDialogService.ShowDialogAsync{TDialog}(object, DialogParameters)"/>
    public async Task<IDialogReference> ShowDialogAsync<TDialog>(object data, DialogParameters parameters)
         where TDialog : IDialogContentComponent
    {
        return await ShowDialogAsync(typeof(TDialog), data, parameters);
    }

    /// <inheritdoc cref="IDialogService.ShowDialogAsync{TDialog}(DialogParameters)"/>
    public async Task<IDialogReference> ShowDialogAsync<TDialog>(DialogParameters parameters)
         where TDialog : IDialogContentComponent
    {
        return await ShowDialogAsync<object>(typeof(TDialog), default!, parameters);
    }

    /// <inheritdoc cref="IDialogService.UpdateDialogAsync{TData}(string, DialogParameters{TData})"/>/>
    public async Task<IDialogReference?> UpdateDialogAsync<TData>(string id, DialogParameters<TData> parameters)
        where TData : class
    {
        return await OnUpdateAsync!.Invoke(id, parameters);
    }

    /// <inheritdoc cref="IDialogService.ShowPanelAsync{TData}(Type, TData, DialogParameters)"/>
    public async Task<IDialogReference> ShowPanelAsync<TData>(Type dialogComponent, TData data, DialogParameters parameters)
        where TData : class
    {
        return await ShowDialogAsync(dialogComponent, data, FixPanelParameters(parameters));
    }

    /// <inheritdoc cref="IDialogService.ShowPanelAsync{TDialog}(object, DialogParameters)"/>
    public async Task<IDialogReference> ShowPanelAsync<TDialog>(object data, DialogParameters parameters)
        where TDialog : IDialogContentComponent
    {
        return await ShowDialogAsync(typeof(TDialog), data, FixPanelParameters(parameters));
    }

    /// <inheritdoc cref="IDialogService.ShowPanelAsync{TDialog}(DialogParameters)"/>
    public async Task<IDialogReference> ShowPanelAsync<TDialog>(DialogParameters parameters)
        where TDialog : IDialogContentComponent
    {
        return await ShowDialogAsync<object>(typeof(TDialog), default!, FixPanelParameters(parameters));
    }

    private DialogParameters FixPanelParameters(DialogParameters value)
    {
        value.DialogType = DialogType.Panel;

        if (value.Alignment == HorizontalAlignment.Center)
        {
            value.Alignment = HorizontalAlignment.Right;
        }

        return value;
    }

    #region obsolete

    /// <inheritdoc cref="IDialogService.ShowDialog{TDialog, TData}(DialogParameters{TData})"/>
    [Obsolete("Use ShowDialogAsync(object, DialogParameters) instead.")]
    public void ShowDialog<TDialog, TData>(DialogParameters<TData> parameters)
        where TDialog : IDialogContentComponent<TData>
        where TData : class
    {
        DialogParameters dialogParameters = new()
        {
            DialogType = parameters.DialogType,
            Alignment = parameters.Alignment,
            Title = parameters.Title,
            Modal = parameters.Modal,
            ShowDismiss = parameters.ShowDismiss,
            ShowTitle = parameters.ShowTitle,
            PrimaryAction = parameters.PrimaryAction,
            PrimaryActionEnabled = parameters.PrimaryActionEnabled,
            SecondaryAction = parameters.SecondaryAction,
            SecondaryActionEnabled = parameters.SecondaryActionEnabled,
            Width = parameters.Width,
            Height = parameters.Height,
            AriaLabel = $"{parameters.Title}",
            OnDialogResult = parameters.OnDialogResult,
        };

        ShowDialog(typeof(TDialog), parameters.Content, dialogParameters);
    }

    /// <inheritdoc cref="IDialogService.ShowDialog{TData}(Type, TData, DialogParameters)"/>
    [Obsolete("Use ShowDialogAsync(object, DialogParameters) instead.")]
    public virtual void ShowDialog<TData>(Type dialogComponent, TData content, DialogParameters parameters)
        where TData : class
    {
        if (!typeof(IDialogContentComponent).IsAssignableFrom(dialogComponent))
        {
            throw new ArgumentException($"{dialogComponent.FullName} must be a Dialog Component");
        }

        IDialogReference? dialogReference = new DialogReference(parameters.Id, this);

        OnShow?.Invoke(dialogReference, dialogComponent, parameters, content);
    }

    /// <inheritdoc cref="IDialogService.ShowDialogAsync{TDialog, TData}(DialogParameters{TData})"/>
    [Obsolete("Use ShowDialogAsync(object, DialogParameters) instead.")]
    public async Task<IDialogReference> ShowDialogAsync<TDialog, TData>(DialogParameters<TData> parameters)
        where TDialog : IDialogContentComponent<TData>
        where TData : class
    {
        DialogParameters dialogParameters = new()
        {
            DialogType = parameters.DialogType,
            Id = parameters.Id,
            Alignment = parameters.Alignment,
            Title = parameters.Title,
            Modal = parameters.Modal,
            TrapFocus = parameters.TrapFocus,
            ShowDismiss = parameters.ShowDismiss,
            ShowTitle = parameters.ShowTitle,
            PrimaryAction = parameters.PrimaryAction,
            PrimaryActionEnabled = parameters.PrimaryActionEnabled,
            SecondaryAction = parameters.SecondaryAction,
            SecondaryActionEnabled = parameters.SecondaryActionEnabled,
            Width = parameters.Width,
            Height = parameters.Height,
            AriaLabel = $"{parameters.Title}",
            OnDialogResult = parameters.OnDialogResult,
        };

        return await ShowDialogAsync(typeof(TDialog), parameters.Content, dialogParameters);
    }

    /// <inheritdoc cref="IDialogService.UpdateDialog{TData}(string, DialogParameters{TData})"/>
    [Obsolete("Use UpdateDialogAsync instead.")]
    public void UpdateDialog<TData>(string id, DialogParameters<TData> parameters)
        where TData : class
    {
        OnUpdate?.Invoke(id, parameters);
    }

    /// <inheritdoc cref="IDialogService.ShowPanelAsync{TData}(Type, DialogParameters{TData})"/>
    [Obsolete("Use ShowPanelAsync(object, DialogParameters) instead.")]
    public void ShowPanel<TData>(Type dialogComponent, DialogParameters<TData> parameters)
        where TData : class
    {
        DialogParameters dialogParameters = new()
        {
            DialogType = DialogType.Panel,
            Alignment = parameters.Alignment,
            Title = parameters.Title,
            Modal = parameters.Modal,
            ShowTitle = parameters.ShowTitle,
            ShowDismiss = parameters.ShowDismiss,
            PrimaryAction = parameters.PrimaryAction,
            SecondaryAction = parameters.SecondaryAction,
            Width = parameters.Width,
            AriaLabel = $"{parameters.Title}",
            OnDialogResult = parameters.OnDialogResult,
        };

        ShowDialog(dialogComponent, parameters.Content, dialogParameters);
    }

    /// <inheritdoc cref="IDialogService.ShowPanel{TDialog, TData}(DialogParameters{TData})"/>
    [Obsolete("Use ShowPanelAsync(object, DialogParameters) instead.")]
    public void ShowPanel<TDialog, TData>(DialogParameters<TData> parameters)
        where TDialog : IDialogContentComponent<TData>
        where TData : class
        => ShowPanel(typeof(TDialog), parameters);

    /// <inheritdoc cref="IDialogService.ShowPanelAsync{TDialog, TData}(DialogParameters{TData})"/>
    [Obsolete("Use ShowPanelAsync(object, DialogParameters) instead.")]
    public async Task<IDialogReference> ShowPanelAsync<TDialog, TData>(DialogParameters<TData> parameters)
        where TDialog : IDialogContentComponent<TData>
        where TData : class
        => await ShowPanelAsync(typeof(TDialog), parameters);

    /// <inheritdoc cref="IDialogService.ShowPanelAsync{TData}(Type, DialogParameters{TData})"/>
    [Obsolete("Use ShowPanelAsync(object, DialogParameters) instead.")]
    public async Task<IDialogReference> ShowPanelAsync<TData>(Type dialogComponent, DialogParameters<TData> parameters)
        where TData : class
    {
        DialogParameters dialogParameters = new()
        {
            DialogType = DialogType.Panel,
            Alignment = parameters.Alignment,
            Title = parameters.Title,
            Modal = parameters.Modal,
            ShowTitle = parameters.ShowTitle,
            ShowDismiss = parameters.ShowDismiss,
            PrimaryAction = parameters.PrimaryAction,
            SecondaryAction = parameters.SecondaryAction,
            Width = parameters.Width,
            AriaLabel = $"{parameters.Title}",
            OnDialogResult = parameters.OnDialogResult,
        };

        return await ShowDialogAsync(dialogComponent, parameters.Content, dialogParameters);
    }

    #endregion
}
