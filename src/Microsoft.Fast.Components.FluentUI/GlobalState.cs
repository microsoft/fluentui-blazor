namespace Microsoft.Fast.Components.FluentUI;

public class GlobalState
{
    public LocalizationDirection Dir { get; set; } = LocalizationDirection.ltr;
    public StandardLuminance Luminance { get; set; } = StandardLuminance.LightMode;

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

    private void NotifyStateHasChanged()
    {
        OnChange?.Invoke();
    }
}
