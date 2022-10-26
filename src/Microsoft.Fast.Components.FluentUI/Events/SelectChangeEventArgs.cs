namespace Microsoft.Fast.Components.FluentUI;

public class SelectChangeEventArgs : EventArgs
{
    public string? Value { get; set; }

    public int SelectedIndex { get; set; }

    public int ActiveIndex { get; set; }

    public string[]? SelectedOptions { get; set; }
}
