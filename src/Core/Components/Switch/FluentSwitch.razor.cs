// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The FluentSwitch component is used to render a Switch
/// </summary>
public partial class FluentSwitch : FluentInputBase<bool>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentSwitch"/> class.
    /// </summary>
    public FluentSwitch()
    {
        LabelPosition = Components.LabelPosition.After;
    }

    /// <summary>
    /// Handler for the OnFocus event.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected virtual Task FocusOutHandlerAsync(FocusEventArgs e)
    {
        FocusLost = true;
        return Task.CompletedTask;
    }

    private void OnSwitchChangedHandler(ChangeEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(e);
        
        CurrentValue = !CurrentValue;
    }

    /// <summary>
    /// Parses a string to create the <see cref="Microsoft.AspNetCore.Components.Forms.InputBase{TValue}.Value"/>.
    /// </summary>
    /// <param name="value">The string value to be parsed.</param>
    /// <param name="result">The result to inject into the Value.</param>
    /// <param name="validationErrorMessage">If the value could not be parsed, provides a validation error message.</param>
    /// <returns>True if the value could be parsed; otherwise false.</returns>
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out bool result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        // Overriding mandatory because the parent method is abstract and called via the OnChanged.
        throw new NotSupportedException();
    }

    internal bool InternalTryParseValueFromString(string? value, [MaybeNullWhen(false)] out bool result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        return TryParseValueFromString(value, out result, out validationErrorMessage);
    }

#pragma warning disable CS0618
    private string? GetLabel =>
        (!string.IsNullOrEmpty(CheckedMessage) && CurrentValue) ? CheckedMessage : (!string.IsNullOrEmpty(UncheckedMessage) && !CurrentValue ? UncheckedMessage : Label);
#pragma warning restore
}
