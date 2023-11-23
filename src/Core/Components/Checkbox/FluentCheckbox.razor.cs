using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;
public partial class FluentCheckbox : FluentInputBase<bool>
{
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

    private bool IsIndeterminate { get; set; } = false;

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
            Console.WriteLine($"CheckState({value})");

            if (_checkState == value)
            {
                return;
            }

            switch (value)
            {
                case CheckState.Checked:
                    _checkState = CheckState.Checked;
                    _ = SetIndeterminateAsync(false);
                    _ = SetCurrentValue(true);
                    break;
                case CheckState.Indeterminate:
                    _checkState = CheckState.Indeterminate;
                    _ = SetIndeterminateAsync(true);
                    _ = SetCurrentValue(true);
                    break;
                default:
                    _checkState = CheckState.Unchecked;
                    _ = SetIndeterminateAsync(false);
                    _ = SetCurrentValue(false);
                    break;
            }

            if (CheckStateChanged.HasDelegate)
            {
                _ = CheckStateChanged.InvokeAsync(_checkState);
            }
        }
    }

    [Parameter]
    public EventCallback<CheckState> CheckStateChanged { get; set; }

    private async Task SetCheckboxAsync(CheckboxChangeEventArgs args)
    {
        bool value = args.Checked;

        var checkbox = (OldValue: Value, NewValue: value);

        if (ThreeState)
        {
            //bool indeterminate = args.Indeterminate;

            /*
               (Old,   New)     Target state  -> CheckState
               ------------------------------------------------
               (false, true)    Unchecked     -> Checked
               (true,  false)   Checked       -> Indeterminate
               (true,  true)    Indeterminate -> Unchecked
               (false, false)   Unchecked     -> Unchecked
            */

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

    protected override bool TryParseValueFromString(string? value, out bool result, [NotNullWhen(false)] out string? validationErrorMessage) => throw new NotSupportedException($"This component does not parse string inputs. Bind to the '{nameof(CurrentValue)}' property, not '{nameof(CurrentValueAsString)}'.");

    private async Task SetIndeterminateAsync(bool value)
    {
        IsIndeterminate = value;

        Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
        await Module.InvokeVoidAsync("setFluentCheckBoxIndeterminate", Id, value);
    }
}