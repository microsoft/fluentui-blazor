namespace Microsoft.Fast.Components.FluentUI;

public sealed class DialogInstance
{
    public DialogInstance(Type type, object data, DialogSettings settings)
    {
        ContentType = type;
        Data = data;
        Settings = settings;
    }

    public string Id { get; } = Identifier.NewId();

    public Type? ContentType { get; set; }

    public object Data { get; set; } = default!;

    public DialogSettings Settings { get; }

    public Dictionary<string, object> GetParameterDictionary()
    {
        return new Dictionary<string, object> { { "Data", Data! } };
    }
}
