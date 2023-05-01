using System.Diagnostics;

namespace Microsoft.Fast.Components.FluentUI;

/// <summary />
internal class ToastMessageState
{
    public ToastMessageState(ToastOptions options)
    {
        Options = options;
    }

    private string AnimationId { get; } = $"toast-{Identifier.NewId()}";

    public ToastOptions Options { get; }

    public ToastState ToastState { get; set; } = ToastState.Initializing;

    public Stopwatch Stopwatch { get; } = new Stopwatch();

    public bool ShowCloseIcon => Options.Global.ShowCloseIcon;

    public string? Icon
    {
        get
        {
            return Options.Intent switch
            {
                ToastIntent.Neutral => FluentIcons.Info,
                ToastIntent.Danger => FluentIcons.DismissCircle,
                ToastIntent.Success => FluentIcons.CheckmarkCircle,
                _ => null,
            };
        }
    }

    internal string AnimationStyle
    {
        get
        {
            const string Template = "animation: {0}ms linear {1};";

            switch (ToastState)
            {
                case ToastState.Showing:
                    var showingDuration = RemainingTransitionMilliseconds(Options.Global.ShowTransitionDuration);
                    return string.Format(Template, showingDuration, AnimationId);

                case ToastState.Hiding:
                    var hidingDuration = RemainingTransitionMilliseconds(Options.Global.HideTransitionDuration);
                    return string.Format(Template, hidingDuration, AnimationId);

                case ToastState.Visible:
                    return $"opacity: 1;";

                default:
                    return string.Empty;
            }
        }
    }

    internal string TransitionClass
    {
        get
        {
            string template = "@keyframes " + AnimationId + " {{from{{ {0}: {1}; }} to{{ {0}: {2}; }}}}";

            return ToastState switch
            {
                ToastState.Showing => string.Format(template, "opacity", "0%", "100%"),
                ToastState.Hiding => string.Format(template, "opacity", "100%", "0%"),
                ToastState.Visible => string.Format(template, "width", "100%", "0%"),
                _ => string.Empty,
            };
        }
    }

    private int RemainingTransitionMilliseconds(int transitionDuration)
    {
        var duration = transitionDuration - (int)Stopwatch.ElapsedMilliseconds;

        return duration >= 0 ? duration : 0;
    }
}
