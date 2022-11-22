namespace Microsoft.Fast.Components.FluentUI;

public class GlobalState
{
    public LocalizationDirection Dir { get; set; } = LocalizationDirection.ltr;
    public Luminance Luminance { get; set; } = Luminance.Light;

    public event Action? OnChange;


    public void SetDirection(LocalizationDirection dir)
    {
        Dir = dir;
        NotifyStateHasChanged();
    }

    public void SetLuminance(Luminance luminance)
    {
        Luminance = luminance;
        NotifyStateHasChanged();
    }

    private void NotifyStateHasChanged()
    {
        OnChange?.Invoke();
    }
}
