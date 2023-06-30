namespace Microsoft.Fast.Components.FluentUI;

public class CustomToastParameters : ToastParameters
{

    public CustomToastParameters(string title) : this(ToastIntent.Custom, title, ToastEndContentType.Dismiss)
    {
    }

    public CustomToastParameters(ToastIntent intent, string? title, ToastEndContentType endContentType)
    {
        Intent = intent;
        Title = title;
        EndContentType = endContentType;
    }
}
