namespace Microsoft.Fast.Components.FluentUI;

public class ProgressToastParameters : ToastParameters
{
    private string? _details;
    private ToastAction? _primaryAction;
    private ToastAction? _secondaryAction;
    private string? _progress;

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

    public string? Progress
    {
        get => _progress;
        set
        {
            _progress = value;
            if (!string.IsNullOrEmpty(_progress))
            {
                Parameters[nameof(Progress)] = _progress;
            }
        }
    }
}
