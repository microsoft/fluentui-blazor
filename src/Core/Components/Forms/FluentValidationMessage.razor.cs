// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Displays validation messages for a specified field within a cascaded <see cref="EditContext"/>.
/// </summary>
public partial class FluentValidationMessage<TValue> : FluentComponentBase
{
    private EditContext? _previousEditContext;
    private Expression<Func<TValue>>? _previousFieldAccessor;
    private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
    private FieldIdentifier _fieldIdentifier;
    private bool _hasFieldIdentifier;

    /// <summary>
    /// Constructs an instance of <see cref="FluentValidationMessage{TValue}"/>.
    /// </summary>
    public FluentValidationMessage(LibraryConfiguration configuration) : base(configuration)
    {
        _validationStateChangedHandler = (_, _) => StateHasChanged();
    }

    [CascadingParameter]
    private EditContext? CurrentEditContext { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="FieldIdentifier"/> for which validation messages should be displayed.
    /// If set, this parameter takes precedence over <see cref="For"/>.
    /// </summary>
    [Parameter]
    public FieldIdentifier? Field { get; set; }

    /// <summary>
    /// Gets or sets the field expression for which validation messages should be displayed.
    /// </summary>
    [Parameter]
    public Expression<Func<TValue>>? For { get; set; }

    /// <summary>
    /// Gets or sets the icon displayed next to each validation message.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; } = FluentStatus.ErrorIcon;

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-validation-message")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    private IEnumerable<string> ValidationMessages => _hasFieldIdentifier
        ? CurrentEditContext!.GetValidationMessages(_fieldIdentifier)
        : [];

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (CurrentEditContext is null)
        {
            throw new InvalidOperationException($"{GetType()} requires a cascading parameter of type {nameof(EditContext)}. For example, use {GetType()} inside an {nameof(EditForm)}.");
        }

        if (Field is not null)
        {
            _fieldIdentifier = Field.Value;
            _hasFieldIdentifier = true;
        }
        else
        {
            if (For is null)
            {
                throw new InvalidOperationException($"{GetType()} requires a value for either the {nameof(Field)} or {nameof(For)} parameter.");
            }

            if (For != _previousFieldAccessor)
            {
                _fieldIdentifier = FieldIdentifier.Create(For);
                _previousFieldAccessor = For;
            }

            _hasFieldIdentifier = true;
        }

        if (CurrentEditContext != _previousEditContext)
        {
            DetachValidationStateChangedListener();
            CurrentEditContext.OnValidationStateChanged += _validationStateChangedHandler;
            _previousEditContext = CurrentEditContext;
        }
    }

    /// <inheritdoc />
    public override ValueTask DisposeAsync()
    {
        DetachValidationStateChangedListener();
        GC.SuppressFinalize(this);
        return base.DisposeAsync();
    }

    private void DetachValidationStateChangedListener()
    {
        if (_previousEditContext is not null)
        {
            _previousEditContext.OnValidationStateChanged -= _validationStateChangedHandler;
        }
    }

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
