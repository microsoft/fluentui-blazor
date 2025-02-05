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

    [Inject]
    private LibraryConfiguration Configuration { get; set; } = default!;

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass(Configuration.DefaultStyles.FluentFieldClass, when: HasLabel || HasMessage)
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    private string? LabelStyle => new StyleBuilder()
        .AddStyle("width", Parameters.LabelWidth, when: () => !string.IsNullOrEmpty(Parameters.LabelWidth) && Parameters.LabelPosition != Components.LabelPosition.Above)
        .Build();

    /// <summary>
    /// Gets or sets an existing Input component to use in the field.
    /// Setting this parameter will define the parameters
    /// Label, LabelTemplate, LabelPosition, LabelWidth,
    /// Required, Disabled,
    /// Message, MessageIcon, MessageTemplate, and MessageCondition.
    /// </summary>
    [Parameter]
    public IFluentField? InputComponent { get; set; }

    /// <summary>
    /// Gets or sets the ID of the Input component to associate with the field.
    /// </summary>
    [Parameter]
    public string? ForId { get; set; }

    /// <see cref="IFluentField.FocusLost"/>"
    public bool FocusLost { get; internal set; }

    /// <see cref="IFluentField.Label"/>"
    [Parameter]
    public string? Label { get; set; }

    /// <see cref="IFluentField.LabelTemplate"/>"
    [Parameter]
    public RenderFragment? LabelTemplate { get; set; }

    /// <see cref="IFluentField.LabelPosition"/>"
    [Parameter]
    public LabelPosition? LabelPosition { get; set; }

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
    public Func<IFluentField, bool>? MessageCondition { get; set; }

    /// <see cref="IFluentField.MessageIcon"/>"
    [Parameter]
    public MessageState? MessageState { get; set; }

    private FluentFieldParameterSelector Parameters => new(this, Localizer);

    internal string? GetId(string slot)
    {
        // Wrapper of an Input component
        if (Parameters.HasInputComponent)
        {
            var id = (string.IsNullOrEmpty(ForId) ? Id : ForId) ?? _defaultId;
            return slot switch
            {
                "field" => $"{id}-field",
                "input" => $"{id}",
                "label" => $"{id}-label",
                _ => throw new ArgumentException($"Invalid slot: {slot}"),
            };
        }

        // Standalone FluentField
        return slot switch
        {
            "field" => Id,
            "input" => string.IsNullOrEmpty(ForId) ? $"{Id ?? _defaultId}-input" : ForId,
            "label" => string.IsNullOrEmpty(Id) ? null : $"{Id}-label",
            _ => throw new ArgumentException($"Invalid slot: {slot}"),
        };
    }

    private bool HasLabel
        => !string.IsNullOrWhiteSpace(Parameters.Label)
        || Parameters.LabelTemplate is not null;

    private bool HasMessage
        => !string.IsNullOrWhiteSpace(Parameters.Message)
        || Parameters.MessageTemplate is not null
        || Parameters.MessageIcon is not null
        || Parameters.MessageState is not null;

    private bool HasMessageOrCondition
       => HasMessage || Parameters.MessageCondition is not null;

    internal static RenderFragment? CreateIcon(Icon? icon)
    {
        if (icon is null)
        {
            return null;
        }

        return builder =>
        {
            builder.OpenComponent(0, typeof(FluentIcon<Icon>));
            builder.AddAttribute(1, "Value", icon);
            builder.AddAttribute(2, "Width", "12px");
            builder.AddAttribute(3, "Margin", "0px 4px 0 0");
            builder.CloseComponent();
        };
    }
}
