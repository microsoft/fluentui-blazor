// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Explorers.Components.Pages;

public abstract class ExplorerBase : ComponentBase
{
    private const int ShowMoreStep = 64;

    public bool SearchInProgress { get; set; }

    public int MaximumOfIcons { get; set; } = 32;

    protected virtual Task ShowMoreHandlerAsync()
    {
        MaximumOfIcons += ShowMoreStep;
        return Task.CompletedTask;
    }

    protected abstract Task StartSearchAsync();

    protected static IEnumerable<T> GetEnumValues<T>()
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Where(value => value!.ToString() != "Custom" &&
                            typeof(T).GetField(value!.ToString()!)!
                                     .GetCustomAttributes(typeof(ObsoleteAttribute), false)
                                     .Length == 0);
    }
}
