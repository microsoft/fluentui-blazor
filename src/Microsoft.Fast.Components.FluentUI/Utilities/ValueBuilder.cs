namespace Microsoft.Fast.Components.FluentUI.Utilities;

public class ValueBuilder
{
    private string? stringBuffer;

    public bool HasValue => !string.IsNullOrWhiteSpace(stringBuffer);
    /// <summary>
    /// Adds a space separated conditional value to a property.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="when"></param>
    /// <returns></returns>
    public ValueBuilder AddValue(string? value, bool when = true) => when ? AddRaw($"{value} ") : this;
    public ValueBuilder AddValue(Func<string?> value, bool when = true) => when ? AddRaw($"{value()} ") : this;

    private ValueBuilder AddRaw(string? style)
    {
        stringBuffer += style;
        return this;
    }

    public override string? ToString() => stringBuffer?.Trim();
}
