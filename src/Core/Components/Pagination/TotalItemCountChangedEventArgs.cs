// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Provides data for the <see cref="PaginationState.TotalItemCountChanged"/> event.
/// </summary>
public class TotalItemCountChangedEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TotalItemCountChangedEventArgs"/> class.
    /// </summary>
    /// <param name="totalItemCount">The total item count.</param>
    public TotalItemCountChangedEventArgs(int? totalItemCount) => TotalItemCount = totalItemCount;

    /// <summary>
    /// Gets the total item count.
    /// </summary>
    public int? TotalItemCount { get; }
}
