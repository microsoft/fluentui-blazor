using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;
public partial class FluentCheckbox : FluentInputBase<bool>
{
    private const bool VALUE_FOR_INDETERMINATE = false;
    private bool _intermediate = false;
    private bool? _checkState = false;
    private const string JAVASCRIPT_FILE = "./_content/Microsoft.FluentUI.AspNetCore.Components/Components/Checkbox/FluentCheckbox.razor.js";

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(CheckboxChangeEventArgs))]
    public FluentCheckbox()
    {
        Id = Identifier.NewId();
    }

    /// <summary />
    [Inject]
    private LibraryConfiguration LibraryConfiguration { get; set; } = default!;

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
    /// Gets or sets a value indicating the order of the three states of the CheckBox.
    /// False (by default), the order is Unchecked -> Checked -> Intermediate.
    /// True: the order is Unchecked -> Intermediate -> Checked.
    /// </summary>
    [Parameter]
    public bool ThreeStateOrderUncheckToIntermediate { get; set; } = false;

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
                _ = SetCurrentAndIntermediateAsync(value);
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
                .AddClass("disabled", when: Disabled)
                .AddClass("checked", when: Value)
                .AddClass("indeterminate", when: ThreeState && CheckState is null)
                .Build();
        }
    }

    /// <summary />
    private async Task SetCurrentAndIntermediateAsync(bool? value)
    {
        switch (value)
        {
            // Checked
            case true:
                await SetCurrentValueAsync(true);
                await SetIntermediateAsync(false);
                break;

            // Unchecked
            case false:
                await SetCurrentValueAsync(false);
                await SetIntermediateAsync(false);
                break;

            // Indeterminate
            default:
                await SetCurrentValueAsync(VALUE_FOR_INDETERMINATE);
                await SetIntermediateAsync(true);
                break;
        }
    }

    /// <summary />
    private async Task SetIntermediateAsync(bool intermediate)
    {
        // Force the Indeterminate state to be set.
        // Each time the user clicks the checkbox, the Indeterminate state is reset to false.
        Module ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE.FormatCollocatedUrl(LibraryConfiguration));
        await Module.InvokeVoidAsync("setFluentCheckBoxIndeterminate", Id, intermediate, Value);

        _intermediate = intermediate;
    }

    /// <summary />
    private async Task SetCurrentCheckStateAsync(bool newChecked)
    {
        bool? newState = null;

        // Uncheck -> Indeterminate -> Check
        if (ThreeStateOrderUncheckToIntermediate)
        {
            // NewChecked  |  Intermediate  |  NewState
            //   True             False          [-]
            //   True             True           [x]
            //   False            False          [ ]

            // Uncheck -> Intermediate (or Check is ShowIndeterminate is false)
            if (newChecked && !_intermediate)
            {
                newState = ShowIndeterminate ? null : true;
            }

            // Indeterminate -> Checked
            else if (newChecked && _intermediate)
            {
                newState = true;
            }

            // Checked -> Uncheck
            else
            {
                newState = false;
            }
        }

        // Uncheck -> Check -> Indeterminate
        else
        {
            // NewChecked  |  Intermediate  |  NewState
            //   True             False          [x]
            //   False            False          [-]
            //   True             true           [ ]

            // Uncheck -> Check
            if (newChecked && !_intermediate)
            {
                newState = true;
            }

            // Check -> Indeterminate (or Uncheck is ShowIndeterminate is false)
            else if (!newChecked && !_intermediate)
            {
                newState = ShowIndeterminate ? null : false;
            }

            // Indeterminate -> Uncheck
            else
            {
                newState = false;
            }
        }

        await SetCurrentAndIntermediateAsync(newState);
        await UpdateAndRaiseCheckStateEventAsync(newState);
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
            await SetCurrentCheckStateAsync(e.Checked ?? false);
        }
        else
        {
            await SetCurrentValueAsync(e.Checked ?? false);
            await SetIntermediateAsync(false);
            await UpdateAndRaiseCheckStateEventAsync(e.Checked ?? false);
        }
    }

    /// <summary />
    private async Task UpdateAndRaiseCheckStateEventAsync(bool? value)
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
