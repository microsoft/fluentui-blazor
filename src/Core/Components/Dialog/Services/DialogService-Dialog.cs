namespace Microsoft.Fast.Components.FluentUI;

public partial class DialogService
{
    /// <summary>
    /// Shows a panel with the dialog component type as the body
    /// </summary>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
    public void ShowPanel<T, TData>(DialogParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class
        => ShowPanel(typeof(T), parameters);

    /// <summary>
    /// Shows a panel with the dialog component type as the body
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
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

    /// <summary>
    /// Shows a dialog with the component type as the body
    /// </summary>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
    public void ShowDialog<T, TContent>(DialogParameters<TContent> parameters)
        where T : IDialogContentComponent<TContent>
        where TContent : class
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

        ShowDialog(typeof(T), parameters.Content, dialogParameters);
    }

    /// <summary>
    /// Shows a dialog with the component type as the body
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="content">Content to pass to component being displayed.</param>
    /// <param name="parameters">Parameters to configure the dialog component.</param>
    public virtual void ShowDialog<TContent>(Type dialogComponent, TContent content, DialogParameters parameters)
        where TContent : class
    {
        if (!typeof(IDialogContentComponent).IsAssignableFrom(dialogComponent))
        {
            throw new ArgumentException($"{dialogComponent.FullName} must be a Dialog Component");
        }

        IDialogReference? dialogReference = new DialogReference(parameters.Id, this);

        OnShow?.Invoke(dialogReference, dialogComponent, parameters, content);
    }

    /// <summary>
    /// Updates a dialog 
    /// </summary>
    /// <param name="id">Id of the dialog to update.</param>
    /// <param name="parameters">Parameters to configure the dialog component.</param>
    public void UpdateDialog<TContent>(string id, DialogParameters<TContent> parameters)
        where TContent : class
    {
        OnUpdate?.Invoke(id, parameters);
    }

    /// <summary>
    /// Shows a panel with the dialog component type as the body
    /// </summary>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
    public async Task<IDialogReference> ShowPanelAsync<T, TData>(DialogParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class
        => await ShowPanelAsync(typeof(T), parameters);

    /// <summary>
    /// Shows a panel with the dialog component type as the body
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
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

    /// <summary>
    /// Shows a dialog with the component type as the body
    /// </summary>
    /// <param name="parameters">Parameters to pass to component being displayed.</param>
    public async Task<IDialogReference> ShowDialogAsync<T, TContent>(DialogParameters<TContent> parameters)
        where T : IDialogContentComponent<TContent>
        where TContent : class
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

        return await ShowDialogAsync(typeof(T), parameters.Content, dialogParameters);
    }

    /// <summary>
    /// Shows a dialog with the component type as the body,
    /// passing the specified <paramref name="content "/> 
    /// </summary>
    /// <param name="dialogComponent">Type of component to display.</param>
    /// <param name="content">Content to pass to component being displayed.</param>
    /// <param name="parameters">Parameters to configure the dialog component.</param>
    public virtual async Task<IDialogReference> ShowDialogAsync<TContent>(Type dialogComponent, TContent content, DialogParameters parameters)
        where TContent : class
    {
        if (!typeof(IDialogContentComponent).IsAssignableFrom(dialogComponent))
        {
            throw new ArgumentException($"{dialogComponent.FullName} must be a Dialog Component");
        }

        IDialogReference? dialogReference = new DialogReference(parameters.Id, this);

        return await OnShowAsync!.Invoke(dialogReference, dialogComponent, parameters, content);
    }

    /// <summary>
    /// Updates a dialog 
    /// </summary>
    /// <param name="id">Id of the dialog to update.</param>
    /// <param name="parameters">Parameters to configure the dialog component.</param>
    public async Task<IDialogReference> UpdateDialogAsync<TContent>(string id, DialogParameters<TContent> parameters)
        where TContent : class
    {
        return await OnUpdateAsync!.Invoke(id, parameters);
    }

}
