using System.Collections;
using System.Reflection;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class ComponentParameters : IComponentParameters
{
    private readonly Dictionary<string, object> _parameters;

    public ComponentParameters()
    {
        _parameters = [];
    }

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

    public Dictionary<string, object> GetDictionary()
    {
        foreach (PropertyInfo property in GetType().GetProperties().Where(x => x.Name != "Item"))
        {
            _parameters[property.Name] = property.GetValue(this)!;
        }
        return _parameters;
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
