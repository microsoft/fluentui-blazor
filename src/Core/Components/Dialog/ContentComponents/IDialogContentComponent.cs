namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// A component implementing this interface can be used as dialog content.
/// </summary>
public interface IDialogContentComponent
{
}

public interface IDialogContentComponent<TContent> : IDialogContentComponent
{
    TContent Content { get; set; }
}
