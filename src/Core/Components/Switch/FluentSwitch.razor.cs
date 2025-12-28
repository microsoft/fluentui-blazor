// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentSwitch component represents a physical switch that allows a choice between two mutually exclusive options.
/// </summary>
public partial class FluentSwitch : FluentInputBase<bool>, ITooltipComponent, IFluentComponentElementBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentSwitch"/> class.
    /// </summary>
    public FluentSwitch(LibraryConfiguration configuration) : base(configuration)
    {
        LabelPosition = Components.LabelPosition.After;
    }

    /// <inheritdoc cref="IFluentComponentElementBase.Element" />
    [Parameter]
    public ElementReference Element { get; set; }

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
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

    /// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("Microsoft.FluentUI.Blazor.Utilities.Attributes.observeAttributeChange", Element, "checked", "boolean");
        }
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
