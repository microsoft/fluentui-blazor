namespace Microsoft.Fast.Components.FluentUI;

public class ConfirmationToastParameters : ToastParameters
{
    private ToastIntent _intent;
    private string? _title;
    private ToastEndContentType _endContentType = ToastEndContentType.Dismiss;
    private ToastAction? _primaryAction;

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

    public ToastAction? PrimaryAction
    {
        get => _primaryAction;
        set
        {
            _primaryAction = value;
            if (_primaryAction is not null)
            {
                Parameters.Add(nameof(_primaryAction), _primaryAction);
            }
        }
    }
}
