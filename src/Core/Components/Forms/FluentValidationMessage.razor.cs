// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Displays a list of validation messages for a specified field within a cascaded <see cref="EditContext"/>.
/// </summary>
public partial class FluentValidationMessage<TValue> : FluentComponentBase
{
    private EditContext? _previousEditContext;
    private Expression<Func<TValue>>? _previousFieldAccessor;
    private readonly EventHandler<ValidationStateChangedEventArgs>? _validationStateChangedHandler;
    private FieldIdentifier _fieldIdentifier;

    [CascadingParameter]
    private EditContext CurrentEditContext { get; set; } = default!;

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
    public Expression<Func<TValue>>? For { get; set; }

    /// <summary>
    /// Gets or sets the icon to be displayed with the validation message.
    /// The default is <see cref="CoreIcons.Filled.Size20.DismissCircle"/>.
    /// </summary>
    [Parameter]
    public Icon? Icon { get; set; } = new CoreIcons.Filled.Size20.DismissCircle();

    /// <summary />
    protected string? ClassValue => DefaultClassBuilder
        .AddClass("fluent-validation-message")
        .Build();

    /// <summary />
    protected string? StyleValue => DefaultStyleBuilder
        .Build();

    /// <summary>`
    /// Constructs an instance of <see cref="ValidationMessage{TValue}"/>.
    /// </summary>
    public FluentValidationMessage(LibraryConfiguration configuration) : base(configuration)
    {
        _validationStateChangedHandler = (sender, eventArgs) => StateHasChanged();
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (CurrentEditContext == null)
        {
            throw new InvalidOperationException($"{GetType()} requires a cascading parameter " +
                $"of type {nameof(EditContext)}. For example, you can use {GetType()} inside " +
                $"an {nameof(EditForm)}.");
        }

        if (Field != null)
        {
            _fieldIdentifier = Field.Value;
        }
        else
        {
            if (For == null)
            {
                throw new InvalidOperationException($"{GetType()} requires a value for either " +
                    $"the {nameof(Field)} or {nameof(For)} parameter.");
            }

            if (For != _previousFieldAccessor)
            {
                _fieldIdentifier = FieldIdentifier.Create(For);
                _previousFieldAccessor = For;
            }
        }

        if (CurrentEditContext != _previousEditContext)
        {
            DetachValidationStateChangedListener();
            CurrentEditContext.OnValidationStateChanged += _validationStateChangedHandler;
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
