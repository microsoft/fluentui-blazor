using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

public partial class FluentSelect<TValue> : FluentInputBase<TValue>
{
    private readonly string _defaultSelectName = Identifier.NewId();
    private FluentOptionContext? _context;

    /// <summary>
    /// Gets or sets the name of the Select.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets if the element is disabled
    /// </summary>
    [Parameter]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or sets the visual appearance. See <seealso cref="FluentUI.Appearance"/>
    /// </summary>
    [Parameter]
    public Appearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets the position. See <see cref="FluentUI.SelectPosition"/>
    /// </summary>
    [Parameter]
    public SelectPosition? Position { get; set; }

    /// <summary>
    /// Gets or sets the list of items. See <see cref="Option{TValue}"/>
    /// </summary>
    [Parameter]
    public IEnumerable<Option<TValue>>? Items { get; set; }

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