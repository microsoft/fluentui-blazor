// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A base class for Fluent UI form input components, including an immediate validation mode (using the `OnInput` event).
/// This base class automatically integrates with an <see cref="Microsoft.AspNetCore.Components.Forms.EditContext"/>,
/// which must be supplied as a cascading parameter.
/// </summary>
/// <typeparam name="TValue">The type of the value to be edited.</typeparam>
public abstract class FluentInputImmediateBase<TValue> : FluentInputBase<TValue>, IFluentInputImmediate
{
    private readonly FluentInputImmediateManager _immediateManager;

    /// <summary />
    protected FluentInputImmediateBase(LibraryConfiguration configuration) : base(configuration)
    {
        _immediateManager = new FluentInputImmediateManager(this);
    }

    /// <summary>
    /// Change the content of this input field when the user write text (based on 'OnInput' HTML event).
    /// </summary>
    [Parameter]
    public bool Immediate { get; set; } = false;

    /// <summary>
    /// Gets or sets the delay, in milliseconds, before to raise the event.
    /// Default is 200 milliseconds.
    /// </summary>
    [Parameter]
    public int ImmediateDelay { get; set; } = 200;

    /// <summary>
    /// Raised when the field gains focus. Since the native `onfocusin` DOM event is owned internally
    /// by <see cref="FocusInHandlerAsync"/> (an <c>@onfocusin</c> passed via unmatched attributes would
    /// silently replace it instead of merging), use this callback to observe focus-in from a consumer.
    /// </summary>
    [Parameter]
    public EventCallback<FocusEventArgs> OnFocusIn { get; set; }

    /// <summary>
    /// Raised when the field loses focus. Since the native `onfocusout` DOM event is owned internally
    /// by <see cref="FocusOutHandlerAsync"/> (an <c>@onfocusout</c> passed via unmatched attributes would
    /// silently replace it instead of merging), use this callback to observe focus-out from a consumer.
    /// </summary>
    [Parameter]
    public EventCallback<FocusEventArgs> OnFocusOut { get; set; }

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        _immediateManager.CheckAndSetExternalValue(parameters, Value, FormatValueAsString);
        return base.SetParametersAsync(parameters);
    }

    /// <see cref="FluentInputImmediateManager.GetImmediateValueAsString(string?)" />
    protected string? ImmediateValueAsString => _immediateManager.GetImmediateValueAsString(CurrentValueAsString);

    /// <see cref="FluentInputImmediateManager.InputHandlerAsync(ChangeEventArgs, Func{ChangeEventArgs, Task})" />
    protected virtual Task InputHandlerAsync(ChangeEventArgs e) => _immediateManager.InputHandlerAsync(e, ChangeHandlerAsync);

    /// <see cref="FluentInputImmediateManager.ShouldRender" />
    protected override bool ShouldRender() => _immediateManager.ShouldRender();

    /// <see cref="FluentInputImmediateManager.FocusInHandlerAsync(FocusEventArgs)" />
    protected virtual Task FocusInHandlerAsync(FocusEventArgs e) => _immediateManager.FocusInHandlerAsync(e);

    /// <see cref="FluentInputImmediateManager.FocusOutHandlerAsync(FocusEventArgs, Func{Task}?)" />
    protected virtual Task FocusOutHandlerAsync(FocusEventArgs e) => _immediateManager.FocusOutHandlerAsync(e, async () => FocusLost = true);

    /// <see cref="FluentInputImmediateManager.InitializeImmediateAsync(JSInterop.IJSRuntime, string?)" />
    protected virtual Task InitializeImmediateAsync() => _immediateManager.InitializeImmediateAsync(JSRuntime, Id);
}
