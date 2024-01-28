namespace FluentUI.Demo.AssetExplorer.Models;

public static class EnumExtensions
{
    public static IEnumerable<TEnum> ContainsString<TEnum>(this IEnumerable<TEnum>? source, string value)
        where TEnum : Enum
    {
        return Enum.GetNames(typeof(TEnum))
                   .Where(i => i.Contains(value, StringComparison.InvariantCultureIgnoreCase))
                   .Select(i => (TEnum)Enum.Parse(typeof(TEnum), i));
    }
}
