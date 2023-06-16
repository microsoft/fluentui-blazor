namespace Microsoft.Fast.Components.FluentUI;

public sealed class DialogInstance
{
    public DialogInstance(Type type, DialogParameters parameters, DialogSettings settings)
    {
        ContentType = type;
        Parameters = parameters;
        Settings = settings;
        Title = parameters.Title;
    }

    public string Id { get; } = Identifier.NewId();

    public string? Title { get; set; }

    public Type? ContentType { get; set; }

    public DialogParameters Parameters { get; set; } = new();

    public DialogSettings Settings { get; }

    public Dictionary<string, object> GetParameterDictionary()
    {
        return Parameters.GetDictionary();
    }
}
