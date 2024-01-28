namespace Microsoft.FluentUI.AspNetCore.Components;

public interface IComponentParameters : IEnumerable<KeyValuePair<string, object>>
{
    object this[string parameterName] { get; set; }

    void Add(string parameterName, object value);
    T Get<T>(string parameterName);
    Dictionary<string, object> GetDictionary();
    T TryGet<T>(string parameterName);
}
