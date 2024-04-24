namespace Microsoft.FluentUI.AspNetCore.Components;

public static class OfficeColorUtilities
{
    public static readonly OfficeColor[] AllColors = Enum.GetValues<OfficeColor>();

    public static OfficeColor GetRandom(bool skipDefault = true)
    {

        IEnumerable<OfficeColor>? values = AllColors.Skip(skipDefault ? 1 : 0);

        return values.ElementAt(new Random().Next(values.Count()));
    }
}
