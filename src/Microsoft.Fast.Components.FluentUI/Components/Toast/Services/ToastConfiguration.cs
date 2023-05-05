namespace Microsoft.Fast.Components.FluentUI;

public class ToastConfiguration : CommonToastOptions
{
    private bool _newestOnTop;
    private int _maxToasts;
    private ToastPosition _toasterPosition;
    private bool _clearAfterNavigation;

    internal event Action? OnUpdate;

    public bool NewestOnTop
    {
        get => _newestOnTop;
        set
        {
            _newestOnTop = value;
            OnUpdate?.Invoke();
        }

    }

    public int MaxToasts { 
        get => _maxToasts;
        set
        {
            _maxToasts = value;
            OnUpdate?.Invoke();
        }
    }

    public ToastPosition ToasterPosition
    {
        get => _toasterPosition;
        set
        {
            _toasterPosition = value;
            OnUpdate?.Invoke();
        }
    }

    public bool ClearAfterNavigation 
    {

        get => _clearAfterNavigation;
        set
        {
            _clearAfterNavigation = value;
            OnUpdate?.Invoke();
        }
    } 

    public ToastConfiguration()
    {
        NewestOnTop = false;
        MaxToasts = 4;
        ToasterPosition = ToastPosition.TopEnd;
        ClearAfterNavigation = true;
    }


}
