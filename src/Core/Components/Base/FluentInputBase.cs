// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A base class for Fluent UI form input components. This base class automatically
/// integrates with an <see cref="EditContext"/>, which must be supplied
/// as a cascading parameter.
/// </summary>
/// <typeparam name="TValue">The type of the value to be edited.</typeparam>
public abstract partial class FluentInputBase<TValue> : InputBase<TValue>, IFluentComponentBase, IFluentField
{
    private FluentJSModule? _jsModule;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentInputBase{TValue}"/> class.
    /// </summary>
    protected FluentInputBase()
    {
        ValueExpression = () => CurrentValueOrDefault;
    }

    /// <summary />
    [Inject]
    protected IJSRuntime JSRuntime { get; set; } = default!;

    /// <summary />
    [Inject]
    protected IFluentLocalizer Localizer { get; set; } = FluentLocalizerInternal.Default;

    /// <summary>
    /// Gets the JavaScript module imported with the <see cref="FluentJSModule.ImportJavaScriptModuleAsync"/> method.
    /// You need to call this method (in the `OnAfterRenderAsync` method) before using the module.
    /// </summary>
    internal FluentJSModule JSModule => _jsModule ??= new FluentJSModule(JSRuntime);

    /// <summary>
    /// Internal usage only: to define the default `ValueExpression`.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal TValue CurrentValueOrDefault { get => CurrentValue ?? default!; set => CurrentValue = value; }

    #region IFluentComponentBase

    /// <inheritdoc />
    [Parameter]
    public virtual string? Id { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual string? Class { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual string? Style { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual string? Margin { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual string? Padding { get; set; }

    /// <inheritdoc />
    [Parameter]
    public virtual object? Data { get; set; }

    #endregion

    #region FluentInputBase

    /// <summary>
    /// Gets the class builder, containing the default margin and padding values.
    /// </summary>
    protected virtual CssBuilder DefaultClassBuilder => new CssBuilder(Class)
        .AddClass(Margin.ConvertSpacing().Class)
        .AddClass(Padding.ConvertSpacing().Class);

    /// <summary>
    /// Gets the style builder, containing the default margin and padding values.
    /// </summary>
    protected virtual StyleBuilder DefaultStyleBuilder => new StyleBuilder(Style)
        .AddStyle("margin", Margin.ConvertSpacing().Style)
        .AddStyle("padding", Padding.ConvertSpacing().Style);

    /// <summary>
    /// Gets a CSS class string that combines the `Class` attribute and and a string indicating
    /// the status of the field being edited (a combination of "modified", "valid", and "invalid").
    /// Derived components should typically use this value for the primary HTML element class attribute.
    /// </summary>
    protected virtual string? ClassValue => DefaultClassBuilder.AddClass(base.CssClass).Build();

    /// <summary>
    /// Gets the optional in-line styles. If given, these will be included in the style attribute of the component.
    /// </summary>
    protected virtual string? StyleValue => DefaultStyleBuilder.Build();

    /// <summary>
    /// Determines if the element should receive document focus on page load.
    /// </summary>
    [Parameter]
    public virtual bool Autofocus { get; set; }

    /// <summary>
    /// Gets or sets the text used on `aria-label` attribute.
    /// </summary>
    [Parameter]
    public virtual string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets the name of the element.
    /// Allows access by name from the associated form.
    /// ⚠️ This value needs to be set manually for SSR scenarios to work correctly.
    /// </summary>
    [Parameter]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets whether the control will be immutable by user interaction.
    /// </summary>
    [Parameter]
    public virtual bool ReadOnly { get; set; }

    /// <see cref="IFluentField.Disabled" />
    [Parameter]
    public virtual bool? Disabled { get; set; }

    /// <see cref="IFluentField.Label" />
    [Parameter]
    public virtual string? Label { get; set; }

    /// <see cref="IFluentField.LabelTemplate" />
    [Parameter]
    public virtual RenderFragment? LabelTemplate { get; set; }

    /// <see cref="IFluentField.LabelPosition" />
    [Parameter]
    public virtual FieldLabelPosition? LabelPosition { get; set; }

    /// <see cref="IFluentField.LabelWidth" />
    [Parameter]
    public virtual string? LabelWidth { get; set; }

    /// <see cref="IFluentField.Required" />
    [Parameter]
    public virtual bool? Required { get; set; }

    /// <see cref="IFluentField.Message" />"/>
    [Parameter]
    public virtual string? Message { get; set; }

    /// <see cref="IFluentField.MessageIcon" />"/>
    [Parameter]
    public virtual Icon? MessageIcon { get; set; }

    /// <see cref="IFluentField.MessageTemplate" />"/>
    public virtual RenderFragment? MessageTemplate { get; set; }

    /// <see cref="IFluentField.MessageCondition" />
    [Parameter]
    public virtual Func<bool>? MessageCondition { get; set; }

    /// <summary />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0059:Unnecessary assignment of a value", Justification = "TODO")]
    protected virtual async Task ChangeHandlerAsync(ChangeEventArgs e)
    {
        var isValid = TryParseValueFromString(e.Value?.ToString(), out var result, out var validationErrorMessage);

        if (isValid)
        {
            await InvokeAsync(() => CurrentValue = result);
        }
        else
        {
            // TODO
        }
    }

    /// <summary>
    /// Returns the aria-label attribute value with the label and required indicator.
    /// </summary>
    /// <returns></returns>
    protected virtual string? GetAriaLabelWithRequired()
    {
        return (AriaLabel ?? Label ?? string.Empty) +
               (Required == true ? $", {Localizer["FluentInputBase_Required"]}" : string.Empty);
    }

    #endregion
}
