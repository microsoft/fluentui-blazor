// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Field adds a label, validation message, and hint text to a control.
/// </summary>
public partial class FluentField : FluentComponentBase, IFluentField
{
    private readonly string _defaultId = Identifier.NewId();
    private EditContext? _previousEditContext;
    private LambdaExpression? _previousFieldAccessor;
    private readonly EventHandler<ValidationStateChangedEventArgs>? _validationStateChangedHandler;
    private FieldIdentifier _fieldIdentifier;

    /// <summary />
    public FluentField(LibraryConfiguration configuration) : base(configuration)
    {
        _validationStateChangedHandler = (sender, eventArgs) => StateHasChanged();
    }

    [Inject]
    private LibraryConfiguration Configuration { get; set; } = default!;

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass(Configuration.DefaultStyles.FluentFieldClass, when: HasLabel)
        .AddClass("invalid", when: ValidationMessages.Any())
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    private string? LabelStyle => new StyleBuilder()
        .AddStyle("width", Parameters.LabelWidth, when: () => !string.IsNullOrEmpty(Parameters.LabelWidth) && Parameters.LabelPosition != Components.LabelPosition.Above)
        .Build();

    /// <summary>
    /// Gets or sets a value indicating whether the Fluent field should be hidden. For internal use only.
    /// </summary>
    [CascadingParameter(Name = "HideFluentField")]
    internal bool HideFluentField { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="EditContext"/> for the form.
    /// </summary>
    [CascadingParameter]
    private EditContext? CurrentEditContext { get; set; }

    /// <summary>
    /// Gets or sets an existing FieldInput component to use in the field.
    /// Setting this parameter will define the parameters
    /// Label, LabelTemplate, LabelPosition, LabelWidth,
    /// Required, Disabled,
    /// Message, MessageIcon, MessageTemplate, and MessageCondition.
    /// </summary>
    [Parameter]
    public IFluentField? InputComponent { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="FieldIdentifier"/> for which validation messages should be displayed.
    /// If set, this parameter takes precedence over <see cref="For"/>.
    /// </summary>
    [Parameter]
    public FieldIdentifier? Field { get; set; }

    /// <summary>
    /// Gets or sets the field for which validation messages should be displayed.
    /// </summary>
    [Parameter]
    public LambdaExpression? For { get; set; }

    LambdaExpression? IFluentField.ValueExpression => For;

    /// <summary>
    /// Gets or sets the ID of the FieldInput component to associate with the field.
    /// </summary>
    [Parameter]
    public string? ForId { get; set; }

    /// <inheritdoc cref="IFluentField.FocusLost"/>
    public bool FocusLost { get; internal set; }

    /// <inheritdoc cref="IFluentField.Label"/>
    [Parameter]
    public string? Label { get; set; }

    /// <inheritdoc cref="IFluentField.LabelTemplate"/>
    [Parameter]
    public RenderFragment? LabelTemplate { get; set; }

    /// <inheritdoc cref="IFluentField.LabelPosition"/>
    [Parameter]
    public LabelPosition? LabelPosition { get; set; }

    /// <inheritdoc cref="IFluentField.LabelWidth"/>
    [Parameter]
    public string? LabelWidth { get; set; }

    /// <inheritdoc cref="IFluentField.Required"/>
    [Parameter]
    public bool? Required { get; set; }

    /// <inheritdoc cref="IFluentField.Disabled"/>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="ChildContent" /> should be rendered in an extra `div slot="input"`.
    /// </summary>
    [Parameter]
    public bool IncludeInputSlot { get; set; } = true;

    /// <summary>
    /// Gets or sets the child content of the field.
    /// ⚠️ If the <see cref="InputComponent"/> is not set, you must set the `id="@Id"` and `slot="@FluentSlot.FieldInput"` parameters in YOUR input component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc cref="IFluentField.Message"/>
    [Parameter]
    public string? Message { get; set; }

    /// <inheritdoc cref="IFluentField.MessageIcon"/>
    [Parameter]
    public Icon? MessageIcon { get; set; }

    /// <inheritdoc cref="IFluentField.MessageTemplate"/>
    [Parameter]
    public RenderFragment? MessageTemplate { get; set; }

    /// <inheritdoc cref="IFluentField.MessageCondition" />
    [Parameter]
    public Func<IFluentField, bool>? MessageCondition { get; set; }

    /// <inheritdoc cref="IFluentField.MessageIcon"/>
    [Parameter]
    public MessageState? MessageState { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="FieldSize"/> of the label in the field.
    /// </summary>
    [Parameter]
    public FieldSize? Size { get; set; }

    private FluentFieldParameterSelector Parameters => new(this, Localizer);

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        if (Field != null)
        {
            _fieldIdentifier = Field.Value;
        }
        else if (For != null)
        {
            if (For != _previousFieldAccessor)
            {
                _fieldIdentifier = CreateFieldIdentifier(For);
                _previousFieldAccessor = For;
            }
        }
        else if (InputComponent?.ValueExpression != null)
        {
            if (InputComponent.ValueExpression != _previousFieldAccessor)
            {
                _fieldIdentifier = CreateFieldIdentifier(InputComponent.ValueExpression);
                _previousFieldAccessor = InputComponent.ValueExpression;
            }
        }

        if (CurrentEditContext != _previousEditContext)
        {
            DetachValidationStateChangedListener();
            if (CurrentEditContext != null)
            {
                CurrentEditContext.OnValidationStateChanged += _validationStateChangedHandler;
            }

            _previousEditContext = CurrentEditContext;
        }
    }

    /// <summary />
    public override ValueTask DisposeAsync()
    {
        DetachValidationStateChangedListener();
        GC.SuppressFinalize(this);
        return base.DisposeAsync();
    }

    private void DetachValidationStateChangedListener()
    {
        if (_previousEditContext != null)
        {
            _previousEditContext.OnValidationStateChanged -= _validationStateChangedHandler;
        }
    }

    private static FieldIdentifier CreateFieldIdentifier(LambdaExpression accessor)
    {
        var method = typeof(FieldIdentifier).GetMethod("Create", BindingFlags.Public | BindingFlags.Static)!;
        return (FieldIdentifier)method.MakeGenericMethod(accessor.ReturnType).Invoke(null, [accessor])!;
    }

    private IEnumerable<string> ValidationMessages => CurrentEditContext?.GetValidationMessages(_fieldIdentifier) ?? Enumerable.Empty<string>();

    internal string? GetId(string slot)
    {
        // Wrapper of an FieldInput component
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
        || Parameters.MessageState is not null
        || ValidationMessages.Any();

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
