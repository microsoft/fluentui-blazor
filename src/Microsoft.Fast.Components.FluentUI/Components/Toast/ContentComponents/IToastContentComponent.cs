namespace Microsoft.Fast.Components.FluentUI;

/// <summary>
/// A component implementing this interface can be used as toast content.
/// </summary>
public interface IToastContentComponent
{

}


public interface IToastContentComponent<TToastContent> : IToastContentComponent
{
    TToastContent ToastContent { get; set; }
}
