using System.Text;

namespace Microsoft.Fast.Components.FluentUI.Utilities;

public class ValueBuilder
{
    private readonly StringBuilder? stringBuffer;

    public bool HasValue => stringBuffer?.Length > 0;
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
        stringBuffer?.Append(style);
        return this;
    }

    public override string? ToString() => stringBuffer?.ToString().Trim();
}
