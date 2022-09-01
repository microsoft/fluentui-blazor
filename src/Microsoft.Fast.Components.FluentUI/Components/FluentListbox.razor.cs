using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentListbox<TValue> : FluentInputBase<TValue>
{
    private readonly string _defaultSelectName = Identifier.NewId();
    private FluentOptionContext? _context;

    /// <summary>
    /// The maximum number of options that should be visible in the listbox scroll area.
    /// </summary>
    [Parameter]
    public int Size { get; set; }

    /// <summary>
    /// Indicates if the listbox is in multi-selection mode.
    /// </summary>
    [Parameter]
    public bool Multiple { get; set; }

    /// <summary>
    /// Gets or sets the list of options in the listbox. See <see cref="Option{TValue}"/>
    /// </summary>
    [Parameter]
    public IEnumerable<Option<TValue>>? Options { get; set; }

    /// <summary>
    /// Gets or set the cascaded context 
    /// </summary>
    [CascadingParameter] private FluentOptionContext? CascadedContext { get; set; }

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        //var selectName = !string.IsNullOrEmpty(Name) ? Name : _defaultSelectName;
        var fieldClass = string.Empty;
        var changeEventCallback = EventCallback.Factory.CreateBinder<string?>(this, __value => CurrentValueAsString = __value, CurrentValueAsString);
        _context = new FluentOptionContext(CascadedContext, _defaultSelectName, CurrentValue, fieldClass, changeEventCallback);

    }
    protected override bool TryParseValueFromString(string? value, out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
        => this.TryParseSelectableValueFromString(value, out result!, out validationErrorMessage);
}