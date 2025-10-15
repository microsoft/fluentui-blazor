// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;

namespace FluentUI.Explorers.Components.Pages;

public abstract class ExplorerBase : ComponentBase
{
    public const int EmptyEnumValue = -1;

    public static T GetEmptyEnum<T>() where T : Enum
        => (T)Enum.ToObject(typeof(T), EmptyEnumValue);

    private const int ShowMoreStep = 64;

    public bool SearchInProgress { get; set; }

    public int MaximumOfItems { get; set; } = 32;

    protected virtual Task ShowMoreHandlerAsync()
    {
        MaximumOfItems += ShowMoreStep;
        return Task.CompletedTask;
    }

    protected abstract Task StartSearchAsync();

    protected static IEnumerable<T> GetEnumValues<T>(bool addEmptyItem = false)
        where T : Enum
    {
        var values = Enum.GetValues(typeof(T))
                         .Cast<T>()
                         .Where(value => value!.ToString() != "Custom" &&
                                         typeof(T).GetField(value!.ToString()!)!
                                                  .GetCustomAttributes(typeof(ObsoleteAttribute), false)
                                                  .Length == 0)
                         .Cast<T>()
                         .ToArray();

        if (addEmptyItem)
        {
            return new T[] { GetEmptyEnum<T>() }.Concat(values);
        }

        return values;
    }
}
