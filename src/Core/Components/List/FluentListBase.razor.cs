// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
[CascadingTypeParameter(nameof(TValue))]
public abstract partial class FluentListBase<TOption, TValue> : FluentInputBase<TValue>, ITooltipComponent, IInternalListBase<TValue>
{
    // List of items rendered with an ID to retrieve the element by ID.
    private Dictionary<string, TOption> InternalOptions { get; } = new(StringComparer.Ordinal);
    private Dictionary<string, TValue> InternalValues { get; } = new(StringComparer.Ordinal);

    /// <summary />
    [DynamicDependency(nameof(OnDropdownChangeHandlerAsync))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DropdownEventArgs))]
    protected FluentListBase(LibraryConfiguration configuration) : base(configuration)
    {
        SelectedItemsExpression = () => SelectedItems;

        // If TOption implements IEqualityComparer<TOption> and exposes a public parameterless
        // constructor, use a new instance of TOption as the default OptionSelectedComparer.
        if (OptionSelectedComparer is null && _defaultOptionSelectedComparer.Value is { } defaultComparer)
        {
            OptionSelectedComparer = defaultComparer;
        }
    }

    /// <inheritdoc />
    protected override string? StyleValue => DefaultStyleBuilder
        .AddStyle("width", Width)
        .Build();

    /// <summary>
    /// Gets or sets the width of the component.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the height of the component.
    /// </summary>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Gets or sets the appearance of the list.
    /// Default is `null`. Internally the component uses <see cref="ListAppearance.Outline"/> as default.
    /// </summary>
    [Parameter]
    public virtual ListAppearance? Appearance { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public virtual RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the content source of all items to display in this list.
    /// Each item must be instantiated (cannot be null).
    /// </summary>
    [Parameter]
    public virtual IEnumerable<TOption>? Items { get; set; }

    /// <summary>
    /// Gets or sets whether the list allows multiple selections.
    /// </summary>
    [Parameter]
    public virtual bool Multiple { get; set; }

    /// <summary>
    /// Gets or sets the items that are selected in the list.
    /// </summary>
    [Parameter]
    public virtual IEnumerable<TOption> SelectedItems { get; set; } = [];

    /// <summary>
    /// Event callback that is invoked when the selected items change.
    /// </summary>
    [Parameter]
    public virtual EventCallback<IEnumerable<TOption>> SelectedItemsChanged { get; set; }

    /// <summary>
    /// Gets or sets an expression that identifies the bound <see cref="SelectedItems"/> value.
    /// This is required to enable the <c>@bind-SelectedItems</c> syntax (Razor automatically
    /// supplies it). When using manual one-way binding through <see cref="SelectedItems"/>
    /// and <see cref="SelectedItemsChanged"/>, providing this expression is optional: a
    /// default expression pointing to <see cref="SelectedItems"/> is set in the constructor.
    /// </summary>
    [Parameter]
    public virtual Expression<Func<IEnumerable<TOption>>>? SelectedItemsExpression { get; set; }

    /// <summary>
    /// Gets or sets the template for the <see cref="FluentListBase{TOption, TValue}.Items"/> items.
    /// </summary>
    [Parameter]
    public virtual RenderFragment<TOption>? OptionTemplate { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine which value to apply to the binded value.
    /// </summary>
    [Parameter]
    public virtual Func<TOption?, TValue?>? OptionValue { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine which value to apply to the "HTML option value" attribute.
    /// </summary>
    [Parameter]
    public virtual Func<TValue?, string>? OptionValueToString { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine which text to display for each option.
    /// </summary>
    [Parameter]
    public virtual Func<TOption?, string>? OptionText { get; set; }

    /// <summary>
    /// Gets or sets the function used to determine if an option is disabled.
    /// </summary>
    [Parameter]
    public virtual Func<TOption?, bool>? OptionDisabled { get; set; }

    /// <summary>
    /// Gets or sets the equality comparer used to determine whether two options are considered equal for selection purposes.
    /// </summary>
    [Parameter]
    public virtual IEqualityComparer<TOption>? OptionSelectedComparer { get; set; }

    /// <inheritdoc cref="ITooltipComponent.Tooltip" />
    [Parameter]
    public string? Tooltip { get; set; }

    /// <summary />
    protected override async Task OnInitializedAsync()
    {
        await base.RenderTooltipAsync(Tooltip);
    }

    /// <summary />
    string? IInternalListBase<TValue>.AddOption(FluentOption<TValue> option)
    {
        var id = option.Id ?? "";

        // Bound list
        if (Items != null)
        {
            if (option.Data is null)
            {
                InternalOptions.TryAdd(id, default!);
                return option.Id;
            }

            if (option.Data is TOption item)
            {
                InternalOptions.TryAdd(id, item);
                return option.Id;
            }
        }

        // Manual list
        if (option.Value is TValue value)
        {
            InternalValues.TryAdd(id, value);
            return option.Id;
        }

        return null;
    }

    /// <summary />
    string? IInternalListBase<TValue>.RemoveOption(FluentOption<TValue> option)
    {
        var id = option.Id ?? "";

        return InternalOptions.Remove(id) || InternalValues.Remove(id)
            ? option.Id
            : null;
    }

    /// <summary />
    bool IInternalListBase<TValue>.AreValuesEqual(TValue? value1, TValue? value2)
    {
        if (OptionSelectedComparer != null && value1 is TOption option1 && value2 is TOption option2)
        {
            return OptionSelectedComparer.Equals(option1, option2);
        }

        return EqualityComparer<TValue>.Default.Equals(value1, value2);
    }

    /// <summary />
    protected virtual bool GetOptionSelected(TOption? item)
    {
        if (item == null)
        {
            return false;
        }

        // Multiple items
        if (Multiple)
        {
            if (OptionSelectedComparer != null)
            {
                return SelectedItems?.Any(selectedItem => OptionSelectedComparer.Equals(item, selectedItem)) ?? false;
            }

            return SelectedItems?.Contains(item) ?? false;
        }

        // Single item
        if (OptionSelectedComparer != null && CurrentValue is TOption currentAsOption)
        {
            return OptionSelectedComparer.Equals(item, currentAsOption);
        }

        if (OptionValue is not null || IsOptionTypeCompatibleWithValue())
        {
            return Equals(GetOptionValue(item), CurrentValue);
        }

        return item is null && CurrentValue is null;
    }

    /// <summary />
    protected virtual TValue? GetOptionValue(TOption? item)
    {
        if (OptionValue is not null)
        {
            return OptionValue.Invoke(item);
        }

        if (IsOptionTypeCompatibleWithValue())
        {
            return (TValue?)(object?)item;
        }

        return default;
    }

    /// <summary />
    protected virtual string? GetOptionText(TOption? item)
    {
        return OptionText?.Invoke(item) ?? item?.ToString() ?? null;
    }

    /// <summary />
    protected virtual bool GetOptionDisabled(TOption? item)
    {
        return OptionDisabled?.Invoke(item) ?? false;
    }

    /// <summary>
    /// Renders the FreeOption option.
    /// </summary>
    /// <returns></returns>
    protected virtual RenderFragment? RenderFreeFormOption() => null;

    /// <summary>
    /// Renders the list options.
    /// </summary>
    /// <returns></returns>
    protected virtual RenderFragment? RenderOptions() => InternalRenderOptions;

    /// <summary>
    /// Provides an optional additional fragment of UI content to render after the main component output.
    /// </summary>
    protected virtual RenderFragment? RenderExtraFragment() => null;

    /// <summary>
    /// Handler for the OnFocus event.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected virtual Task FocusOutHandlerAsync(FocusEventArgs e)
    {
        FocusLost = true;
        return Task.CompletedTask;
    }

    internal virtual async Task OnDropdownChangeHandlerAsync(DropdownEventArgs e)
    {
        // List of IDs received from the web component.
        var selectedIds = e.SelectedOptions?.Split(';', StringSplitOptions.TrimEntries) ?? [];

        // Bind Items
        if (InternalOptions.Count > 0)
        {
            SelectedItems = selectedIds.Length > 0
                          ? InternalOptions.Where(kvp => selectedIds.Contains(kvp.Key, StringComparer.Ordinal)).Select(kvp => kvp.Value).ToList()
                          : Array.Empty<TOption>();

            if (SelectedItemsChanged.HasDelegate)
            {
                await SelectedItemsChanged.InvokeAsync(SelectedItems);
            }

            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(GetOptionValue(SelectedItems.FirstOrDefault()));
            }
        }

        // Manual FluentOptions
        if (InternalValues.Count > 0)
        {
            var SelectedValue = selectedIds.Length > 0
                              ? InternalValues.Where(kvp => selectedIds.Contains(kvp.Key, StringComparer.Ordinal)).Select(kvp => kvp.Value).FirstOrDefault()
                              : default;

            if (ValueChanged.HasDelegate)
            {
                await ValueChanged.InvokeAsync(SelectedValue);
            }
        }
    }

    /// <summary />
    protected override bool TryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        return this.TryParseSelectableValueFromString(value, out result, out validationErrorMessage);
    }

    /// <summary>
    /// For unit testing purposes only.
    /// </summary>
    internal bool InternalTryParseValueFromString(string? value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string? validationErrorMessage)
    {
        return TryParseValueFromString(value, out result, out validationErrorMessage);
    }

    /// <summary />
    internal InternalListContext<TValue> GetCurrentContext()
    {
        return new InternalListContext<TValue>(this);
    }

    /// <summary>
    /// Checks whether <typeparamref name="TOption"/> is the same type as <typeparamref name="TValue"/>,
    /// or <typeparamref name="TValue"/> is <see cref="Nullable{T}"/> of <typeparamref name="TOption"/>.
    /// </summary>
    private static bool IsOptionTypeCompatibleWithValue()
    {
        return typeof(TOption) == typeof(TValue)
            || Nullable.GetUnderlyingType(typeof(TValue)) == typeof(TOption);
    }

    // Cached default comparer (computed once per closed generic type).
    private static readonly Lazy<IEqualityComparer<TOption>?> _defaultOptionSelectedComparer = new(CreateDefaultOptionSelectedComparer);

    [UnconditionalSuppressMessage("Trimming", "IL2090:'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method.", Justification = "Best-effort default comparer detection; safely returns null when the constructor is trimmed.")]
    [UnconditionalSuppressMessage("Trimming", "IL2087:'type' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method.", Justification = "Best-effort default comparer detection; falls back to null when the constructor is trimmed.")]
    private static IEqualityComparer<TOption>? CreateDefaultOptionSelectedComparer()
    {
        var optionType = typeof(TOption);

        if (!typeof(IEqualityComparer<TOption>).IsAssignableFrom(optionType))
        {
            return null;
        }

        if (optionType.GetConstructor(Type.EmptyTypes) is null)
        {
            return null;
        }

        try
        {
            return Activator.CreateInstance(optionType) as IEqualityComparer<TOption>;
        }
        catch (MissingMethodException)
        {
            return null;
        }
    }
}
