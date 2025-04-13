// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;
//using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A Fluent Radio button component.
/// </summary>
/// <typeparam name="TValue">The type for the value of the radio button</typeparam>
public partial class FluentRadio<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TValue> : FluentInputBase<TValue>, IFluentField
{
    bool _trueValueToggle;

    internal FluentRadioContext? Context { get; private set; }

    [CascadingParameter]
    private FluentRadioContext? CascadedContext { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element is checked.
    /// </summary>
    [Parameter]
    public bool? Checked { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        Context = string.IsNullOrEmpty(Name) ? CascadedContext : CascadedContext?.FindContextInAncestors(Name);

        if (Context == null)
        {
            throw new InvalidOperationException($"{GetType()} must have an ancestor {typeof(FluentRadioGroup<TValue>)} " +
                $"with a matching 'Name' property, if specified.");
        }
    }

    /// <inheritdoc />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    => this.TryParseSelectableValueFromString(value, out result, out validationErrorMessage);

    internal bool InternalTryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        return TryParseValueFromString(value, out result, out validationErrorMessage);
    }

    // This is an unfortunate hack, but is needed for the scenario described by test InputRadioGroupWorksWithMutatingSetter.
    // Radio groups are special in that modifying one <input type=radio> instantly and implicitly also modifies the previously
    // selected one in the same group. As such, our SetUpdatesAttributeName mechanism isn't sufficient to stay in sync with the
    // DOM, because the 'change' event will fire on the new <input type=radio> you just selected, not the previously-selected
    // one, and so the previously-selected one doesn't get notified to update its state in the old rendertree. So, if the setter
    // reverts the incoming value, the previously-selected one would produce an empty diff (because its .NET value hasn't changed)
    // and hence it would be left unselected in the DOM. If you don't understand why this is a problem, try commenting out the
    // line that toggles _trueValueToggle and see the E2E test fail.
    //
    // This hack works around that by causing InputRadio *always* to force its own 'checked' state to be true in the DOM if it's
    // true in .NET, whether or not it was true before, by continually changing the value that represents 'true'. This doesn't
    // really cause any significant increase in traffic because if we're rendering this InputRadio at all, sending one more small
    // attribute value is inconsequential.
    //
    // Ultimately, a better solution would be to make SetUpdatesAttributeName smarter still so that it knows about the special
    // semantics of radio buttons so that, when one <input type="radio"> changes, it treats any previously-selected sibling
    // as needing DOM sync as well. That's a more sophisticated change and might not even be useful if the radio buttons
    // aren't truly siblings and are in different DOM subtrees (and especially if they were rendered by different components!)
    private string GetToggledTrueValue()
    {
        _trueValueToggle = !_trueValueToggle;
        return _trueValueToggle ? "a" : "b";
    }
}
