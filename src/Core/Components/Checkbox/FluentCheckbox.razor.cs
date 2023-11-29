using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.Fast.Components.FluentUI;
public partial class FluentCheckbox : FluentInputBase<bool>
{
    private const bool VALUE_FOR_INDETERMINATE = false;
    private bool _intermediate = false;
    private bool? _checkState = false;
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.Fast.Components.FluentUI/Components/Checkbox/FluentCheckbox.razor.js";


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
    /// Gets or sets a value indicating whether the user can display the indeterminate state by clicking the CheckBox.
    /// If this is not the case, the checkbox can be started in the indeterminate state, but the user cannot activate it with the mouse.
    /// Default is true.
    /// </summary>
    [Parameter]
    public bool ShowIndeterminate { get; set; } = true;

    /// <summary>
    /// Gets or sets the state of the CheckBox: true, false or null.
    /// </summary>
    [Parameter]
    public bool? CheckState
    {
        get => _checkState;
        set
        {
            if (!ThreeState)
            {
                throw new ArgumentException("Set the `ThreeState` attribute to True to use this `CheckState` property.");
            }

            if (_checkState != value)
            {
                _checkState = value;
                _ = SetCurrentAndIntermediate(value);
            }
        }
    }

    /// <summary>
    /// Gets or sets a callback that updates the <see cref="CheckState"/>.
    /// </summary>
    [Parameter]
    public EventCallback<bool?> CheckStateChanged { get; set; }

    protected override string? ClassValue
    {
        get
        {
            return new CssBuilder(base.ClassValue)
                .AddClass("checked", when: Value)
                .Build();
        }
    }

    /// <summary />
    private async Task SetCurrentAndIntermediate(bool? value)
    {
        switch (value)
        {
            // Checked
            case true:
                await SetCurrentValue(true);
                await SetIntermediateAsync(false);
                break;

            // Unchecked
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
        if (newChecked && !_intermediate)
        {
            await SetCurrentAndIntermediate(true);
            await UpdateAndRaiseCheckStateEvent(true);
        }

        // Check -> Indeterminate (or Uncheck is ShowIndeterminate is false)
        else if (!newChecked && !_intermediate)
        {
            if (ShowIndeterminate)
            {
                await SetCurrentAndIntermediate(null);
                await UpdateAndRaiseCheckStateEvent(null);
            }
            else
            {
                await SetCurrentAndIntermediate(false);
                await UpdateAndRaiseCheckStateEvent(false);
            }
        }

        // Indeterminate -> Uncheck
        else if (newChecked && _intermediate)
        {
            await SetCurrentAndIntermediate(false);
            await UpdateAndRaiseCheckStateEvent(false);
        }

        // 
        else
        {
            await SetCurrentAndIntermediate(false);
            await UpdateAndRaiseCheckStateEvent(false);
        }
    }

    /// <summary />
    private async Task OnCheckedChangeHandlerAsync(CheckboxChangeEventArgs e)
    {
        if (e.Checked == null && e.Indeterminate == null)
        {
            return;
        }

        if (ThreeState)
        {
            await SetCurrentCheckState(e.Checked ?? false);
        }
        else
        {
            await SetCurrentValue(e.Checked ?? false);
            await SetIntermediateAsync(false);
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