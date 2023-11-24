using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;
public partial class FluentCheckbox : FluentInputBase<bool>
{
    //private bool _updateCheckStateRunning = false;
    private bool _intermediate = false;
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/CheckBox/FluentCheckbox.razor.js";

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CheckboxChangeEventArgs))]
    public FluentCheckbox()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    [Inject]
    private IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    private IJSObjectReference? Module { get; set; }

    //private bool IsIndeterminate { get; set; } = false;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the state of the CheckBox.
    /// </summary>
    [Parameter]
    public bool ThreeState { get; set; } = false;

    [Parameter]
    public bool? CheckState { get; set; } = false;

    [Parameter]
    public EventCallback<bool?> CheckStateChanged { get; set; }

    [Parameter]
    public bool Intermediate
    {
        get => _intermediate;
        set
        {
            _ = SetIntermediateAsync(value);
        }
    }

    [Parameter]
    public EventCallback<bool> IntermediateChanged { get; set; }

    private async Task SetIntermediateAsync(bool value)
    {
        // Force the Indeterminate state to be set.
        // Each time the user clicks the checkbox, the Indeterminate state is reset to false.
        await UpdateUIAsync(value, Value);

        if (_intermediate != value)
        {
            _intermediate = value;

            if (IntermediateChanged.HasDelegate)
            {
                await IntermediateChanged.InvokeAsync(value);
            }
        }
    }

    private async Task SetCurrentCheckState(bool newChecked)
    {
        Console.WriteLine($"SetCurrentCheckState - Intermediate: {Intermediate}, NewChecked: {newChecked}");

        //                [1]       [2]      [3]               [4]
        // Value:        False   -> True  -> False         -> False
        // Intermediate: False   -> False -> True          -> False
        //               ---------------------------------------------
        //               Uncheck -> Check -> Indeterminate -> Uncheck

        // Uncheck -> Check
        if (newChecked && !Intermediate) 
        {
            await SetCurrentValue(true);
            await SetIntermediateAsync(false);
            await UpdateCheckState(true);
        }

        // Check -> Indeterminate
        else if (!newChecked && !Intermediate)
        {
            await SetCurrentValue(false);
            await SetIntermediateAsync(true);
            await UpdateCheckState(null);
        }

        // Indeterminate -> Unckeck
        else if (newChecked && Intermediate)
        {
            await SetCurrentValue(false);
            await SetIntermediateAsync(false);
            await UpdateCheckState(false);
        }

        // 
        else
        {
            await SetCurrentValue(false); 
            await SetIntermediateAsync(false);
            await UpdateCheckState(false);
        }

        async Task UpdateCheckState(bool? value)
        {
            if (CheckState != value)
            {
                CheckState = value;

                if (CheckStateChanged.HasDelegate)
                {
                    await CheckStateChanged.InvokeAsync(value);
                }
            }
        }
    }

    private async Task OnCheckedChangeHandlerAsync(CheckboxChangeEventArgs e)
    {
        Console.WriteLine($"OnCheckedChangeHandler - Intermediate: {e.Indeterminate}, Checked: {e.Checked}");

        if (e.Checked == null && e.Indeterminate == null) {
            Console.WriteLine($"OnCheckedChangeHandler - CANCELED");
            return; 
        }

        if (ThreeState) 
        { 
            await SetCurrentCheckState(e.Checked ?? false); 
        }
        else
        {
            await SetCurrentValue(e.Checked ?? false);
        }
    }

    /*
    /// <summary>
    /// Gets or sets the state of the CheckBox.
    /// </summary>
    [Parameter]
    public bool ThreeState { get; set; } = false;

    private CheckState _checkState = CheckState.Unchecked;

    /// <summary>
    /// Gets or sets the state of the CheckBox.
    /// </summary>
    [Parameter]
    public CheckState CheckState
    {
        get => _checkState;
        set
        {
            _ = UpdateCheckStateAsync(value);
        }
    }

    [Parameter]
    public EventCallback<CheckState> CheckStateChanged { get; set; }

    private async Task UpdateCheckStateAsync(CheckState value)
    {
        if (_checkState == value)
        {
            return;
        }

        if (_updateCheckStateRunning)
        {
            return;
        }

        Console.WriteLine($"CheckState({value})");

        _updateCheckStateRunning = true;

        switch (value)
        {
            case CheckState.Checked:
                _checkState = CheckState.Checked;
                await SetIndeterminateAsync(false);
                await SetCurrentValue(true);
                break;

            case CheckState.Indeterminate:
                _checkState = CheckState.Indeterminate;
                await SetIndeterminateAsync(true);
                await SetCurrentValue(true);
                break;

            default:
                _checkState = CheckState.Unchecked;
                await SetIndeterminateAsync(false);
                await SetCurrentValue(false);
                break;
        }

        if (CheckStateChanged.HasDelegate)
        {
            await CheckStateChanged.InvokeAsync(_checkState);
        }

        _updateCheckStateRunning = false;
    }

    private async Task SetCheckboxAsync(CheckboxChangeEventArgs args)
    {
        bool value = args.Checked;

        var checkbox = (OldValue: Value, NewValue: value);

        if (ThreeState)
        {
            //bool indeterminate = args.Indeterminate;

            
            //   (Old,   New)     Target state  -> CheckState
            //   ------------------------------------------------
            //   (false, true)    Unchecked     -> Checked
            //   (true,  false)   Checked       -> Indeterminate
            //   (true,  true)    Indeterminate -> Unchecked
            //   (false, false)   Unchecked     -> Unchecked
            

            // Unchecked -> Checked
            if (!checkbox.OldValue && checkbox.NewValue)
            {
                CheckState = CheckState.Checked;
                //value = true;
                //indeterminate = false;
            }

            // Checked -> Indeterminate
            else if (checkbox.OldValue && !checkbox.NewValue)
            {
                CheckState = CheckState.Indeterminate;
                //value = true;
                //indeterminate = true;
            }

            // Indeterminate -> Unchecked
            else if (checkbox.OldValue && checkbox.NewValue)
            {
                CheckState = CheckState.Unchecked;
                //value = false;
                //indeterminate = false;
            }

            // Unchecked -> Unchecked
            else
            {
                CheckState = CheckState.Unchecked;
                //value = false;
                //indeterminate = false;
            }

            //await SetIndeterminateAsync(indeterminate);
        }

        else
        {
            await base.SetCurrentValue(value);

            var checkState = value ? CheckState.Checked : CheckState.Unchecked;
            if (CheckState != checkState)
            {
                _checkState = checkState;

                if (CheckStateChanged.HasDelegate)
                {
                    _ = CheckStateChanged.InvokeAsync(_checkState);
                }
            }
        }
    }

    private async Task SetIndeterminateAsync(bool value)
    {
        IsIndeterminate = value;

        Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        await Module.InvokeVoidAsync("setFluentCheckBoxIndeterminate", Id, value);
    }
    */
     
    private async Task UpdateUIAsync(bool isIndeterminate, bool isChecked)
    { 
        Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        await Module.InvokeVoidAsync("setFluentCheckBoxIndeterminate", Id, isIndeterminate, isChecked);
    }

    protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage) => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");

}