namespace Microsoft.Fast.Components.FluentUI;

public class Option<TValue>
{
    public TValue? Key { get; set; }
    public TValue? Value { get; set; }
    public bool Disabled { get; set; } = false;
    public bool Selected { get; set; } = false;
}

