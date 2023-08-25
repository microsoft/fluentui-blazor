using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentFormDialog<TModel> : ComponentBase, IDisposable where TModel : new()
{
    /// <summary>
    /// Reference to <see cref="FluentDialog"/>
    /// </summary>
    private FluentDialog? Dialog { get; set; }

    private bool _hidden;
    /// <summary>
    /// Gets or sets if the dialog is hidden
    /// </summary>
    [Parameter]
    public bool Hidden
    {
        get => _hidden;
        set
        {
            if (value == _hidden)
                return;
            _hidden = value;
            HiddenChangedInternal(value);
            HiddenChanged.InvokeAsync(value);
        }
    }

    /// <summary>
    /// The event callback invoked when <see cref="Hidden"/> change.
    /// </summary>
    [Parameter]
    public EventCallback<bool> HiddenChanged { get; set; }

    /// <summary>
    /// Title of the form
    /// </summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// When true, shows the dismiss button in the header.
    /// </summary>
    [Parameter]
    public bool ShowDismiss { get; set; }

    /// <summary>
    /// Model for the form
    /// </summary>
    [Parameter]
    public TModel Model { get; set; } = default!;

    /// <summary>
    /// Text to display in submit button
    /// </summary>
    [Parameter]
    public string SubmitButtonText { get; set; } = "Submit";

    /// <summary>
    /// Text to display in cancel button
    /// </summary>
    [Parameter]
    public string CancelButtonText { get; set; } = "Cancel";

    /// <summary>
    /// If true, form is validated as soon as dialog open
    /// </summary>
    [Parameter]
    public bool ValidateOnOpen { get; set; } = true;


    /// <summary>
    /// If true, FormDialog subscribes to EditContext.OnFieldChanged and validates the form on every field change
    /// </summary>
    [Parameter]
    public bool ValidateOnFieldChanged { get; set; } = true;

    /// <summary>
    /// Form content
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    /// <summary>
    /// The event callback invoked when submit button is clicked
    /// </summary>
    [Parameter]
    public EventCallback<TModel> OnFormSubmit { get; set; }

    /// <summary>
    /// The event callback invoked when cancel button is clicked
    /// </summary>
    [Parameter]
    public EventCallback OnCancel { get; set; }

    private EditContext? EditContext { get; set; }
    private bool IsSubmitEnabled { get; set; } = false;

    protected override void OnInitialized()
    {
        Model ??= new TModel();

        EditContext = new EditContext(Model);

        if (ValidateOnFieldChanged)
        {
            EditContext.OnFieldChanged += EditContextOnOnFieldChanged;
        }
    }

    private void EditContextOnOnFieldChanged(object? sender, FieldChangedEventArgs e)
    {
        UpdateSubmitEnabled();
    }

    private void UpdateSubmitEnabled()
    {
        bool isFormValid = EditContext!.Validate();
        if (isFormValid != IsSubmitEnabled)
        {
            IsSubmitEnabled = isFormValid;
            StateHasChanged();
        }
    }

    private async Task OnSubmitButtonClick()
    {
        bool isFormValid = EditContext!.Validate();

        if (isFormValid)
        {
            if (OnFormSubmit.HasDelegate)
            {
                await OnFormSubmit.InvokeAsync(Model);
            }

            await Dialog!.CloseAsync();
        }
    }

    private async Task OnCancelButtonClick()
    {
        if (OnCancel.HasDelegate)
        {
            await OnCancel.InvokeAsync();
        }

        await Dialog!.CancelAsync();
    }

    private void HiddenChangedInternal(bool value)
    {
        if (!value && ValidateOnOpen)
        {
            UpdateSubmitEnabled();
        }
    }

    public void Dispose()
    {
        EditContext!.OnFieldChanged -= EditContextOnOnFieldChanged;
    }
}
