using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

[CascadingTypeParameter(nameof(TValue))]
public partial class FluentCombobox<TValue> : FluentInputBase<TValue>
{
    private readonly string _defaultSelectName = Identifier.NewId();
    private FluentOptionContext? _context;

    /// <summary>
    /// Gets or sets if the element is auto completes. See <seealso cref="FluentUI.ComboboxAutocomplete"/>
    /// </summary>
    [Parameter]
    public ComboboxAutocomplete? Autocomplete { get; set; }

    /// <summary>
    /// The open attribute.
    /// </summary>
    [Parameter]
    public bool? Open { get; set; }

    /// <summary>
    /// Sets the placeholder value of the element, generally used to provide a hint to the user.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// The placement for the listbox when the combobox is open.
    /// See <seealso cref="FluentUI.SelectPosition"/>
    /// </summary>
    [Parameter]
    public SelectPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="FluentUI.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }

    /// <summary>
    /// Gets or set the list of <see cref="Option{TValue}"/> items 
    /// </summary>
    [Parameter]
    public IEnumerable<Option<TValue>>? Options { get; set; }

    /// <summary>
    /// Gets or set the cascaded context 
    /// </summary>
    [CascadingParameter]
    private FluentOptionContext? CascadedContext { get; set; }

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        var selectName = !string.IsNullOrEmpty(Name) ? Name : _defaultSelectName;
        var fieldClass = string.Empty;
        var changeEventCallback = EventCallback.Factory.CreateBinder<string?>(this, __value => CurrentValueAsString = __value, CurrentValueAsString);
        _context = new FluentOptionContext(CascadedContext, selectName, CurrentValue, fieldClass, changeEventCallback);
    }

    protected override bool TryParseValueFromString(string? value, out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
        => this.TryParseSelectableValueFromString(value, out result!, out validationErrorMessage);
}