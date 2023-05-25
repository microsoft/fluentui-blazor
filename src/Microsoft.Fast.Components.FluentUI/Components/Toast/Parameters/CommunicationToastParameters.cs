namespace Microsoft.Fast.Components.FluentUI;

public class CommunicationToastParameters : ToastParameters
{
    private ToastIntent _intent = ToastIntent.Success;
    private string? _title;
    private ToastEndContentType _endContentType = ToastEndContentType.Dismiss;
    private string? _subtitle;
    private string? _details;
    private ToastAction? _primaryAction;
    private ToastAction? _secondaryAction;

    public ToastIntent Intent
    {
        get => _intent;
        set
        {
            _intent = value;
            Parameters.Add(nameof(Intent), _intent);
        }
    }

    public string? Title
    {
        get => _title;
        set
        {
            _title = value;
            if (!string.IsNullOrEmpty(_title))
            {
                Parameters.Add(nameof(Title), _title);
            }
        }
    }

    public ToastEndContentType EndContentType
    {
        get => _endContentType;
        set
        {
            _endContentType = value;
            Parameters.Add(nameof(EndContentType), _endContentType);
        }
    }

    public string? Subtitle
    {
        get => _subtitle;
        set
        {
            _subtitle = value;
            if (!string.IsNullOrEmpty(_subtitle))
            {
                Parameters.Add(nameof(Subtitle), _subtitle);
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
                Parameters.Add(nameof(Details), _details);
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
                Parameters.Add(nameof(PrimaryAction), _primaryAction);
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
                Parameters.Add(nameof(SecondaryAction), _secondaryAction);
            }
        }
    }
}
