using System.Collections;

namespace Microsoft.Fast.Components.FluentUI;

public class DialogParameters : IEnumerable<KeyValuePair<string, object>>
{
    private readonly Dictionary<string, object> _parameters;

    public DialogParameters()
    {
        _parameters = new Dictionary<string, object>();
    }

    public int Count =>
        _parameters.Count;

    public object this[string parameterName]
    {
        get => Get<object>(parameterName);
        set => _parameters[parameterName] = value;
    }

    public void Add(string parameterName, object value)
    {
        _parameters[parameterName] = value;
    }

    public T Get<T>(string parameterName)
    {
        if (_parameters.TryGetValue(parameterName, out var value))
        {
            return (T)value;
        }

        throw new KeyNotFoundException($"{parameterName} does not exist in Dialog parameters");
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
        return _parameters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _parameters.GetEnumerator();
    }

    public T TryGet<T>(string parameterName)
    {
        if (_parameters.TryGetValue(parameterName, out var value))
        {
            return (T)value;
        }

        return default!;
    }
}
