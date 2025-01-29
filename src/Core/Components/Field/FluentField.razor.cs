// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Field adds a label, validation message, and hint text to a control.
/// </summary>
public partial class FluentField : FluentComponentBase, IFluentField
{
    private readonly string _defaultId = Identifier.NewId();

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    private string? LabelStyle => new StyleBuilder("margin-top: 8px; align-self: flex-start;")
        .AddStyle("width", LabelWidth, when: () => !string.IsNullOrEmpty(LabelWidth))
        .Build();

    /// <summary>
    /// Gets or sets an existing Input component to use in the field.
    /// </summary>
    [Parameter]
    public IFluentField? InputComponent { get; set; }

    /// <summary>
    /// Gets or sets the ID of the Input component to associate with the field.
    /// </summary>
    [Parameter]
    public string? ForId { get; set; }

    /// <see cref="IFluentField.Label"/>"
    [Parameter]
    public string? Label { get; set; }

    /// <see cref="IFluentField.LabelTemplate"/>"
    [Parameter]
    public RenderFragment? LabelTemplate { get; set; }

    /// <see cref="IFluentField.LabelPosition"/>"
    [Parameter]
    public FieldLabelPosition? LabelPosition { get; set; } = FieldLabelPosition.Above;

    /// <see cref="IFluentField.LabelWidth"/>"
    [Parameter]
    public string? LabelWidth { get; set; }

    /// <see cref="IFluentField.Required"/>"
    [Parameter]
    public bool? Required { get; set; }

    /// <see cref="IFluentField.Disabled"/>"
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets the child content of the field.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <see cref="IFluentField.Message"/>"
    [Parameter]
    public string? Message { get; set; }

    /// <see cref="IFluentField.MessageIcon"/>"
    [Parameter]
    public Icon? MessageIcon { get; set; }

    /// <see cref="IFluentField.MessageTemplate"/>"
    [Parameter]
    public RenderFragment? MessageTemplate { get; set; }

    /// <see cref="IFluentField.MessageCondition" />
    [Parameter]
    public Func<bool>? MessageCondition { get; set; }

    private FluentFieldParameters Parameters => new FluentFieldParameters(this);

    private string GetId(string? slot = null) => $"{Id ?? _defaultId}{(string.IsNullOrEmpty(slot) ? "" : "-" + slot)}";

    private bool HasLabel => !string.IsNullOrWhiteSpace(Parameters.Label) || Parameters.LabelTemplate is not null;

    private bool HasMessage => !string.IsNullOrWhiteSpace(Parameters.Message) || Parameters.MessageTemplate is not null || Parameters.MessageIcon is not null;

    // Conversion class for FluentField parameters
    private class FluentFieldParameters
    {
        private readonly FluentField _component;
        public bool HasInputComponent => _component.InputComponent != null;
        public FluentFieldParameters(FluentField component) => _component = component;
        public string? Label => _component.Label ?? _component.InputComponent?.Label;
        public RenderFragment? LabelTemplate => _component.LabelTemplate ?? _component.InputComponent?.LabelTemplate;
        public string? LabelWidth => _component.LabelWidth ?? _component.InputComponent?.LabelWidth;
        public FieldLabelPosition LabelPosition => _component.LabelPosition ?? _component.InputComponent?.LabelPosition ?? FieldLabelPosition.Above;
        public bool Required => _component.Required ?? _component.InputComponent?.Required ?? false;
        public bool Disabled => _component.Disabled ?? _component.InputComponent?.Disabled ?? false;
        public string? Message => _component.Message ?? _component.InputComponent?.Message;
        public Icon? MessageIcon => _component.MessageIcon ?? _component.InputComponent?.MessageIcon;
        public RenderFragment? MessageTemplate => _component.MessageTemplate ?? _component.InputComponent?.MessageTemplate;
        public Func<bool> MessageCondition => _component.MessageCondition ?? _component.InputComponent?.MessageCondition ?? (() => true);
    }
}
