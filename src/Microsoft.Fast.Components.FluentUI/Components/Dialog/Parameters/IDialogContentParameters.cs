namespace Microsoft.Fast.Components.FluentUI
{
    public interface IDialogContentParameters
    {
        Dictionary<string, object> GetDictionary();
    }

    public interface IDialogContentParameters<T> : IDialogContentParameters
    {
        T Data { get; set; }
    }
}
