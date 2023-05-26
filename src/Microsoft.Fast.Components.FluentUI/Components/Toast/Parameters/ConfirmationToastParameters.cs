namespace Microsoft.Fast.Components.FluentUI;

public class ConfirmationToastParameters : ToastParameters
{
    private ToastAction? _primaryAction;

    public ToastAction? PrimaryAction
    {
        get => _primaryAction;
        set
        {
            _primaryAction = value;
            if (_primaryAction is not null)
            {
                Parameters[nameof(_primaryAction)] = _primaryAction;
            }
        }
    }
}
