namespace Microsoft.Fast.Components.FluentUI;

public partial interface IDialogService
{
    void ShowDialog<T, TData>(DialogParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class;

    void ShowDialog<TData>(Type component, TData data, DialogParameters parameters)
        where TData : class;

    void UpdateDialog<TContent>(string id, DialogParameters<TContent> parameters)
        where TContent : class;

    void ShowPanel<T, TData>(DialogParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class;

    void ShowPanel<TData>(Type component, DialogParameters<TData> parameters)
        where TData : class;

    Task<IDialogReference> ShowDialogAsync<T, TData>(DialogParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class;

    Task<IDialogReference> ShowDialogAsync<TData>(Type component, TData data, DialogParameters parameters)
        where TData : class;

    Task<IDialogReference> ShowDialogAsync<T>(object data, DialogParameters parameters)
         where T : IDialogContentComponent;
    
    Task<IDialogReference> UpdateDialogAsync<TContent>(string id, DialogParameters<TContent> parameters)
        where TContent : class;

    Task<IDialogReference> ShowPanelAsync<T, TData>(DialogParameters<TData> parameters)
        where T : IDialogContentComponent<TData>
        where TData : class;

    Task<IDialogReference> ShowPanelAsync<TData>(Type component, DialogParameters<TData> parameters)
        where TData : class;
}