namespace Microsoft.Fast.Components.FluentUI;

public class ToastParameters
{
    private ToastIntent _intent;
    private string? _title;
    private ToastEndContentType _endContentType;

    internal readonly Dictionary<string, object> Parameters;

    public ToastIntent Intent
    {
        get => _intent;
        set
        {
            _intent = value;
            Parameters[nameof(Intent)] = _intent;
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
                Parameters[nameof(Title)] = _title;
            }
        }
    }

    public ToastEndContentType EndContentType
    {
        get => _endContentType;
        set
        {
            _endContentType = value;

            Parameters[nameof(EndContentType)] = _endContentType;
        }
    }

    public ToastParameters()
    {
        Parameters = new Dictionary<string, object>();
    }
    public ToastParameters Add(string parameterName, object value)
    {
        Parameters[parameterName] = value;
        return this;
    }

    public T Get<T>(string parameterName)
    {
        if (Parameters.TryGetValue(parameterName, out var value))
        {
            return (T)value;
        }

        throw new KeyNotFoundException($"{parameterName} does not exist in toast parameters");
    }

    public T? TryGet<T>(string parameterName)
    {
        if (Parameters.TryGetValue(parameterName, out var value))
        {
            return (T)value;
        }

        return default;
    }
}
