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
    private static readonly Icon BadgeCloseIcon = new CoreIcons.Regular.Size20.Dismiss();

    private string? _textInput;
    private bool _isOpen;
    private bool _inProgress;

    /// <summary />
    public FluentAutocomplete(LibraryConfiguration configuration) : base(configuration)
    {
        // Default values
        Id = Identifier.NewId();
        Multiple = true;
        Items = [];
    }

    /// <summary>
    /// Gets or sets the delay, in milliseconds, before to raise the event.
    /// Default is 400 milliseconds.
    /// </summary>
    [Parameter]
    public int ImmediateDelay { get; set; } = 400;

    /// <summary>
    /// Filter the list of options (items) using the text written by the user.
    /// </summary>
    [Parameter]
    public EventCallback<OptionsSearchEventArgs<TOption>> OnOptionsSearch { get; set; }

    /// <summary>
    /// Gets or sets the number of maximum options (items) returned by <see cref="OnOptionsSearch"/>.
    /// Default value is 9.
    /// </summary>
    [Parameter]
    public int MaximumOptionsSearch { get; set; } = 9;

    /// <summary>
    /// Gets or sets whether the component will display a progress indicator while fetching data.
    /// A progress ring will be shown at the end of the component, when the <see cref="OnOptionsSearch"/> is invoked.
    /// </summary>
    [Parameter]
    public bool ShowProgressIndicator { get; set; }

    /// <summary />
    private async Task OnTextInputChangedAsync()
    {
        _inProgress = true;

        var args = new OptionsSearchEventArgs<TOption>()
        {
            Items = Items ?? [],
            Text = _textInput ?? string.Empty,
        };

        await OnOptionsSearch.InvokeAsync(args);

        Items = args.Items?.Take(MaximumOptionsSearch) ?? [];

        _isOpen = true;
        _inProgress = false;
    }

    /// <summary />
    private Task RemoveSelectedItemAsync(TOption? item)
    {
        if (item is null)
        {
            return Task.CompletedTask;
        }

        SelectedItems = SelectedItems?.Where(i => !EqualityComparer<TOption>.Default.Equals(i, item)) ?? [];
        return Task.CompletedTask;
    }
}
