using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Displays a list of validation messages for a specified field within a cascaded <see cref="EditContext"/>.
/// </summary>
public partial class FluentValidationMessage<TValue> : FluentComponentBase, IDisposable
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

    /// <summary />
    protected string? ClassValue => new CssBuilder(Class)
        .AddClass("validation-message")
        .Build();

    /// <summary />
    protected string? StyleValue => new StyleBuilder(Style)
        .Build();

    /// <summary>`
    /// Constructs an instance of <see cref="ValidationMessage{TValue}"/>.
    /// </summary>
    public FluentValidationMessage()
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
        else if (For == null)
        {
            throw new InvalidOperationException($"{GetType()} requires a value for either " +
                $"the {nameof(Field)} or {nameof(For)} parameter.");
        }
        else if (For != _previousFieldAccessor)
        {
            _fieldIdentifier = FieldIdentifier.Create(For);
            _previousFieldAccessor = For;
        }

        if (CurrentEditContext != _previousEditContext)
        {
            DetachValidationStateChangedListener();
            CurrentEditContext.OnValidationStateChanged += _validationStateChangedHandler;
            _previousEditContext = CurrentEditContext;
        }
    }

    /// <summary>
    /// Called to dispose this instance.
    /// </summary>
    /// <param name="disposing"><see langword="true"/> if called within <see cref="IDisposable.Dispose"/>.</param>
    protected virtual void Dispose(bool disposing)
    {
    }

    void IDisposable.Dispose()
    {
        DetachValidationStateChangedListener();
        Dispose(disposing: true);
    }

    private void DetachValidationStateChangedListener()
    {
        if (_previousEditContext != null)
        {
            _previousEditContext.OnValidationStateChanged -= _validationStateChangedHandler;
        }
    }
}
