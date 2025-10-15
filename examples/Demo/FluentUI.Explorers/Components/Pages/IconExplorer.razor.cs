// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Explorers.Components.Pages;

public partial class IconExplorer
{
    private IconInfo[] IconsFound = Array.Empty<IconInfo>();
    private readonly IconSearchCriteria Criteria = new();
    private bool SearchInProgress;
    private const int ShowMoreStep = 64;

    private int MaximumOfIcons { get; set; } = 32;

    private Task ShowMoreHandlerAsync()
    {
        MaximumOfIcons += ShowMoreStep;
        return Task.CompletedTask;
    }

    private async Task StartSearchAsync()
    {
        SearchInProgress = true;
        StateHasChanged();

        await Task.Delay(10);   // To display the Spinner

        IconsFound =
        [
            .. IconsExtensions
                    .AllIcons
                    .Where(i => i.Variant == Criteria.Variant
                             && i.Size == Criteria.Size
                             && (string.IsNullOrWhiteSpace(Criteria.SearchTerm) || i.Name.Contains(Criteria.SearchTerm, StringComparison.InvariantCultureIgnoreCase)))
                    .OrderBy(i => i.Name)
        ];

        SearchInProgress = false;
        StateHasChanged();
    }

    private static IEnumerable<T> GetEnumValues<T>()
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Where(value => value!.ToString() != "Custom" &&
                           !typeof(T).GetField(value!.ToString()!)!
                                      .GetCustomAttributes(typeof(ObsoleteAttribute), false)
                                      .Any());
    }

    private class IconSearchCriteria
    {
        public string SearchTerm { get; set; } = string.Empty;
        public IconVariant Variant { get; set; } = IconVariant.Regular;
        public IconSize Size { get; set; } = IconSize.Size20;
        public Color Color { get; set; } = Color.Default;
    }
}
