// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Utilities;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A FluentAutocomplete allows for selecting one or more options from a list of options with autocomplete functionality.
/// </summary>
/// <typeparam name="TOption"></typeparam>
/// <typeparam name="TValue"></typeparam>
[CascadingTypeParameter(nameof(TValue))]
public partial class FluentAutocomplete<TOption, TValue> : FluentListBase<TOption, TValue>
{
    private static readonly string[] Samples = { "Apple", "Banana", "Cherry", "Date", "Elderberry" };
    private string? _textInput;
    private bool _isOpen;

    /// <summary />
    public FluentAutocomplete(LibraryConfiguration configuration) : base(configuration)
    {
        Id = Identifier.NewId();
    }

    /// <summary>
    /// Gets or sets the delay, in milliseconds, before to raise the event.
    /// Default is 400 milliseconds.
    /// </summary>
    [Parameter]
    public int ImmediateDelay { get; set; } = 100;

    /// <summary>
    /// Renders the selected items.
    /// </summary>
    /// <returns></returns>
    protected virtual RenderFragment? RenderSelectedItems() => InternalRenderSelectedItems;

    private async Task OnTextInputChangedAsync()
    {
        await Task.CompletedTask;
        Items = Samples.Where(item => item.Contains(_textInput ?? string.Empty, StringComparison.OrdinalIgnoreCase)).Cast<TOption>().ToList();
        _isOpen = true;
    }
}
