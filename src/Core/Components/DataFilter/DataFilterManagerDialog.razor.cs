using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.FluentUI.AspNetCore.Components;

public partial class DataFilterManagerDialog<TItem> : IDialogContentComponent<DataFilterManagerDialogContext<TItem>>
{
    public DataFilterManagerDialog() => _idUpload = Identifier.NewId();

    [Parameter]
    public DataFilterManagerDialogContext<TItem> Content { get; set; } = default!;

    [CascadingParameter]
    private FluentDialog Dialog { get; set; } = default!;

    private DataFilterCriteria<TItem> _originalCriteria = default!;
    private EditContext _editContext = default!;
    private readonly string _idUpload;

    protected override void OnInitialized()
    {
        _originalCriteria = Content.FilterManager.Criteria.Clone();
        _editContext = new EditContext(Content);
    }

    private async Task SaveAsync() => await Dialog.CloseAsync(Content);

    private async Task CancelAsync() => await Dialog.CancelAsync();

    private async Task DeleteAsync()
    {
        Content.IsDeleted = true;
        await Dialog.CloseAsync(Content);
    }

    private async Task ResetCriteriaAsync()
    {
        Content.FilterManager.Criteria.Import(_originalCriteria);
        await Content.FilterManager.CriteriaChangedAsync();
    }

    private async Task DownloadCriteriaAsync()
    {
    }

    private async Task OnUploadCompletedAsync(IEnumerable<FluentInputFileEventArgs> files)
    {
        var fileName = files.ToArray()[0].LocalFile!.FullName;
        var json = File.ReadAllText(fileName);
        File.Delete(fileName);

        var criteria = DataFilterCriteria<TItem>.FromJson(json);
        Content.FilterManager.Criteria.Import(criteria);
        await Content.FilterManager.CriteriaChangedAsync();
    }
}
