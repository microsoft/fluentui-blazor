namespace Microsoft.FluentUI.AspNetCore.Components;

public static class OfficeColorUtilities
{
    public static OfficeColor GetRandom(bool skipDefault = true)
    {

        Array values = Enum.GetValues<OfficeColor>().Skip(skipDefault ? 1 : 0).ToArray();
        
        return (OfficeColor)values.GetValue(new Random().Next(values.Length))!;
    }
}
