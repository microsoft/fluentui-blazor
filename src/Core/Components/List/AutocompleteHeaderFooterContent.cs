// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The AutocompleteHeaderFooterContent class represents the content to be displayed in the header or footer of the FluentAutocomplete component.
/// </summary>
/// <typeparam name="TOption"></typeparam>
public class AutocompleteHeaderFooterContent<TOption>
{
    /// <summary />
    internal AutocompleteHeaderFooterContent(IEnumerable<TOption>? items, bool inProgress)
    {
        Items = items ?? Array.Empty<TOption>();
        InProgress = inProgress;
    }
    
    /// <summary>
    /// Gets a value indicating whether the operation is currently in progress.
    /// </summary>
    public bool InProgress { get; init; }

    /// <summary>
    /// Gets the items to display in the header or footer.
    /// </summary>
    public IEnumerable<TOption> Items { get; init; }
}
