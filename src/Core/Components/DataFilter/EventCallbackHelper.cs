using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

public class EventCallbackHelper
{
    public static EventCallback<T> Create<T>(object receiver, Action<T> action) => EventCallback.Factory.Create<T>(receiver, action);

    public static Action<T> Action<T>(Action<object> action) => e => action(e!);

    public static object? Make(Type type, object receiver, Action<object> action)
    {
        var evtCrt = typeof(EventCallbackHelper).GetMethod(nameof(Create))!.MakeGenericMethod(type);
        var evtAct = typeof(EventCallbackHelper).GetMethod(nameof(Action))!.MakeGenericMethod(type);
        return evtCrt.Invoke(receiver, [receiver, evtAct.Invoke(receiver, [action])]);
    }
}
