namespace Microsoft.Fast.Components.FluentUI;

public sealed class ToastInstance
{
    public ToastInstance(Type? type, object toastContent, ToastParameters parameters)
    {
        ContentType = type;
        ToastContent = toastContent;
        Settings = parameters;
        Id = Settings.Id ?? Identifier.NewId();
    }
    public string Id { get; }

    public Type? ContentType { get; set; }

    public object ToastContent { get; set; } = default!;

    public ToastParameters Settings { get; set; } = default!;

    public Dictionary<string, object> GetParameterDictionary()
    {
        return new Dictionary<string, object> { { "ToastContent", ToastContent! } };
    }
}
