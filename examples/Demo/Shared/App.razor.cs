namespace FluentUI.Demo.Shared;
public partial class App
{
    public static string PageTitle(string page)
    {
        return $"{page} - FluentUI Blazor Components";
    }

    public const string MESSAGES_NOTIFICATION_CENTER = "NOTIFICATION_CENTER";
    public const string MESSAGES_TOP = "TOP";
    public const string MESSAGES_DIALOG = "DIALOG";
    public const string MESSAGES_CARD = "CARD";

}
