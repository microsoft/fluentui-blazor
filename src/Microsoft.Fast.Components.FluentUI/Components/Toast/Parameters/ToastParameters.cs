namespace Microsoft.Fast.Components.FluentUI;

public class ToastParameters
{
    internal readonly Dictionary<string, object> Parameters;

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
