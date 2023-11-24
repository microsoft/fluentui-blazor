using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;
public partial class FluentCheckbox : FluentInputBase<bool>
{
    private const bool VALUE_FOR_INDETERMINATE = false;
    private bool _intermediate = false;
    private bool? _checkState = false;
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

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the CheckBox will allow three check states rather than two.
    /// </summary>
    [Parameter]
    public bool ThreeState { get; set; } = false;

    /// <summary>
    /// Gets or sets the state of the CheckBox: true, false or null.
    /// </summary>
    [Parameter]
    public bool? CheckState
    {
        get => _checkState;
        set
        {
            if (_checkState != value)
            {
                _checkState = value;
                _ = SetCurrentAndIntermediate(value);
            }
        }
    }

    [Parameter]
    public EventCallback<bool?> CheckStateChanged { get; set; }

    /// <summary>
    /// Gets or sets the Intermediate state of the CheckBox.
    /// </summary>
    private bool Intermediate
    {
        get => _intermediate;
        set
        {
            _ = SetIntermediateAsync(value);
        }
    }

    /// <summary />
    private async Task SetCurrentAndIntermediate(bool? value)
    {
        switch (value)
        {
            // Unchecked
            case true:
                await SetCurrentValue(true);
                await SetIntermediateAsync(false);
                break;

            // Checked
            case false:
                await SetCurrentValue(false);
                await SetIntermediateAsync(false);
                break;

            // Indeterminate
            default:
                await SetCurrentValue(VALUE_FOR_INDETERMINATE);
                await SetIntermediateAsync(true);
                break;
        }
    }

    /// <summary />
    private async Task SetIntermediateAsync(bool intermediate)
    {
        // Force the Indeterminate state to be set.
        // Each time the user clicks the checkbox, the Indeterminate state is reset to false.
        Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        await Module.InvokeVoidAsync("setFluentCheckBoxIndeterminate", Id, intermediate, Value);

        _intermediate = intermediate;
    }

    /// <summary />
    private async Task SetCurrentCheckState(bool newChecked)
    {
        // Value:        False   -> True  -> False         -> False
        // Intermediate: False   -> False -> True          -> False
        //               ---------------------------------------------
        //               Uncheck -> Check -> Indeterminate -> Uncheck

        // Uncheck -> Check
        if (newChecked && !Intermediate)
        {
            await SetCurrentValue(true);
            await SetIntermediateAsync(false);
            await UpdateAndRaiseCheckStateEvent(true);
        }

        // Check -> Indeterminate
        else if (!newChecked && !Intermediate)
        {
            await SetCurrentValue(VALUE_FOR_INDETERMINATE);
            await SetIntermediateAsync(true);
            await UpdateAndRaiseCheckStateEvent(null);
        }

        // Indeterminate -> Unckeck
        else if (newChecked && Intermediate)
        {
            await SetCurrentValue(false);
            await SetIntermediateAsync(false);
            await UpdateAndRaiseCheckStateEvent(false);
        }

        // 
        else
        {
            await SetCurrentValue(false);
            await SetIntermediateAsync(false);
            await UpdateAndRaiseCheckStateEvent(false);
        }
    }

    /// <summary />
    private async Task OnCheckedChangeHandlerAsync(CheckboxChangeEventArgs e)
    {
        Console.WriteLine($"OnCheckedChangeHandler - Intermediate: {e.Indeterminate}, Checked: {e.Checked}");

        if (e.Checked == null && e.Indeterminate == null)
        {
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
            await UpdateAndRaiseCheckStateEvent(e.Checked ?? false);
        }
    }

    /// <summary />
    private async Task UpdateAndRaiseCheckStateEvent(bool? value)
    {
        if (_checkState != value)
        {
            _checkState = value;

            if (CheckStateChanged.HasDelegate)
            {
                await CheckStateChanged.InvokeAsync(value);
            }
        }
    }

    /// <summary />
    protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage) => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");

}