// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

internal interface IInternalListBase<TValue>
{
    string? AddOption(FluentOption<TValue> option);

    string? RemoveOption(FluentOption<TValue> option);

    Func<TValue?, TValue?, bool>? OptionSelectedComparer { get; set; }

    Func<TValue?, string>? OptionValue { get; set; }

    TValue? Value { get; set; }
}
