namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;

public static class SampleIcons
{
    public static readonly Icon Info = new Samples.Info();

    public static readonly Icon Warning = new Samples.Warning();

    public static readonly Icon PresenceAvailable = new Samples.PresenceAvailable();

    internal class Samples
    {
        internal class Info : Icon { public Info() : base("Info", IconVariant.Filled, IconSize.Size24, "<path d=\"M12 2a10 10 0 1 1 0 20 10 10 0 0 1 0-20Zm0 8.25a1 1 0 0 0-1 .88v5.74a1 1 0 0 0 2 0v-5.62l-.01-.12a1 1 0 0 0-1-.88Zm0-3.75A1.25 1.25 0 1 0 12 9a1.25 1.25 0 0 0 0-2.5Z\"/>") { } }

        internal class Warning : Icon { public Warning() : base("Warning", IconVariant.Filled, IconSize.Size24, "<path d=\"M10.03 3.66a2.25 2.25 0 0 1 3.94 0l7.74 14A2.25 2.25 0 0 1 19.74 21H4.25a2.25 2.25 0 0 1-1.97-3.34l7.75-14ZM13 17a1 1 0 1 0-2 0 1 1 0 0 0 2 0Zm-.26-7.85a.75.75 0 0 0-1.5.1v4.5l.02.1a.75.75 0 0 0 1.49-.1v-4.5l-.01-.1Z\"/>") { } }

        internal class PresenceAvailable : Icon { public PresenceAvailable() : base("PresenceAvailable", IconVariant.Filled, IconSize.Size24, "<path d=\"M12 24a12 12 0 1 0 0-24 12 12 0 0 0 0 24Zm5.06-13.44-5.5 5.5a1.5 1.5 0 0 1-2.12 0l-2-2a1.5 1.5 0 0 1 2.12-2.12l.94.94 4.44-4.44a1.5 1.5 0 0 1 2.12 2.12Z\"/>") { } }

        internal class MyCircle : Icon
        {
            public MyCircle() : base("MyCircle", IconVariant.Regular, IconSize.Custom, "<svg version='1.1' xmlns='http://www.w3.org/2000/svg' viewBox='0 0 320 320'><circle cx='160' cy='160' r='140'/></svg>")
            {
                WithColor("#F97316");
            }
        }
    }
}
