namespace Microsoft.Fast.Components.FluentUI;

public class CustomToastParameters : ToastParameters
{
    private ToastIntent _intent = ToastIntent.Avatar;
    private string? _title;
    private ToastEndContentType _endContentType = ToastEndContentType.Dismiss;


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

    public CustomToastParameters(string title) : this(ToastIntent.Custom, title, ToastEndContentType.Dismiss)
    {
    }

    public CustomToastParameters(ToastIntent intent, string? title, ToastEndContentType endContentType)
    {
        Intent = intent;
        Title = title;
        EndContentType = endContentType;
    }
}
