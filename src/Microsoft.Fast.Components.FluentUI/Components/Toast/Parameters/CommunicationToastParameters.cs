namespace Microsoft.Fast.Components.FluentUI;

public class CommunicationToastParameters : ToastParameters
{
    private string? _subtitle;
    private string? _details;
    private ToastAction? _primaryAction;
    private ToastAction? _secondaryAction;

    public string? Subtitle
    {
        get => _subtitle;
        set
        {
            _subtitle = value;
            if (!string.IsNullOrEmpty(_subtitle))
            {
                Parameters[nameof(Subtitle)] = _subtitle;
            }
        }
    }

    public string? Details
    {
        get => _details;
        set
        {
            _details = value;
            if (!string.IsNullOrEmpty(_details))
            {
                Parameters[nameof(Details)] = _details;
            }
        }
    }

    public ToastAction? PrimaryAction
    {
        get => _primaryAction;
        set
        {
            _primaryAction = value;
            if (_primaryAction is not null)
            {
                Parameters[nameof(PrimaryAction)] = _primaryAction;
            }
        }
    }

    public ToastAction? SecondaryAction
    {
        get => _secondaryAction;
        set
        {
            _secondaryAction = value;
            if (_secondaryAction is not null)
            {
                Parameters[nameof(SecondaryAction)] = _secondaryAction;
            }
        }
    }
}
