using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// This class is used to store the global design values of the Fluent UI components.
/// The name of this class will be changed to 'GlobalDesign' in the next major version.
/// </summary>
// TODO: #vNext: Rename this class to 'GlobalDesign' in the next major version.
public class GlobalState
{
    public LocalizationDirection Dir { get; set; } = LocalizationDirection.LeftToRight;
    public StandardLuminance Luminance { get; set; } = StandardLuminance.LightMode;

    public ElementReference Container { get; set; } = default!;

    public string? Color { get; set; }

    public event Action? OnChange;

    public void SetDirection(LocalizationDirection dir)
    {
        Dir = dir;
        NotifyStateHasChanged();
    }

    public void SetLuminance(StandardLuminance luminance)
    {
        Luminance = luminance;
        NotifyStateHasChanged();
    }

    public void SetContainer(ElementReference container)
    {
        Container = container;
        NotifyStateHasChanged();
    }

    public void SetColor(string? color)
    {
        Color = color;
        NotifyStateHasChanged();
    }

    private void NotifyStateHasChanged()
    {
        OnChange?.Invoke();
    }
}
