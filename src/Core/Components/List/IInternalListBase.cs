// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

internal interface IInternalListBase<TValue>
{
    string? AddOption(FluentOption option);

    string? RemoveOption(FluentOption option);

    Func<TValue?, TValue?, bool>? OptionSelectedComparer { get; set; }
}
