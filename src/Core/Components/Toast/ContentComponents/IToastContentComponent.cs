namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A component implementing this interface can be used as toast content.
/// </summary>
public interface IToastContentComponent
{

}

public interface IToastContentComponent<TContent> : IToastContentComponent
{
    TContent Content { get; set; }
}
