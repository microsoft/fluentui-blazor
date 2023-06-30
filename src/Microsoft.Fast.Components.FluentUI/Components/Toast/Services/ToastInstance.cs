namespace Microsoft.Fast.Components.FluentUI;

public sealed class ToastInstance
{
    public ToastInstance(Type? type, object data, ToastSettings settings)
    {
        ContentType = type;
        Data = data;
        Settings = settings;
        Id = Settings.Id ?? Identifier.NewId();
    }

    public string Id { get; }

    public Type? ContentType { get; set; }

    public object Data { get; set; } = default!;

    public ToastSettings Settings { get; set; } = default!;

    public Dictionary<string, object> GetParameterDictionary()
    {
        return new Dictionary<string, object> { { "Data", Data! } };
    }
}
