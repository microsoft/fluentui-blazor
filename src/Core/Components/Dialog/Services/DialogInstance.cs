namespace Microsoft.Fast.Components.FluentUI;

public sealed class DialogInstance
{
    public DialogInstance(Type? type, DialogParameters parameters, object content)
    {
        ContentType = type;
        Parameters = parameters;
        Content = content;
        Id = Parameters.Id ?? Identifier.NewId();
    }

    public string Id { get; }

    public Type? ContentType { get; set; }

    public object Content { get; set; } = default!;

    public DialogParameters Parameters { get; }

    public Dictionary<string, object> GetParameterDictionary()
    {
        return new Dictionary<string, object> { { "Content", Content } };
    }
}
