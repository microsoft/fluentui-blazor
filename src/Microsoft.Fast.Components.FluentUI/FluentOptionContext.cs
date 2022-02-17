using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI;

internal class FluentOptionContext
{
    private readonly FluentOptionContext? _parentContext;

    /// <summary>
    /// Gets the name of the option 'container' (FluentSelect, FluentListBox, FluentCombobox)
    /// </summary>
    public string ContainerComponentName { get; }

    /// <summary>
    /// Gets the current selected value in the input radio group.
    /// </summary>
    public object? CurrentValue { get; }

    /// <summary>
    /// Gets a css class indicating the validation state of input radio elements.
    /// </summary>
    public string FieldClass { get; }

    /// <summary>
    /// Gets the event callback to be invoked when the selected value is changed.
    /// </summary>
    public EventCallback<ChangeEventArgs> ChangeEventCallback { get; }

    /// <summary>
    /// Instantiates a new <see cref="FluentOptionContext" />.
    /// </summary>
    /// <param name="parentContext">The parent <see cref="FluentOptionContext" />.</param>
    /// <param name="containerName">The name of the option.</param>
    /// <param name="currentValue">The current selected value in the input radio group.</param>
    /// <param name="fieldClass">The css class indicating the validation state of input radio elements.</param>
    /// <param name="changeEventCallback">The event callback to be invoked when the selected value is changed.</param>
    public FluentOptionContext(
        FluentOptionContext? parentContext,
        string containerName,
        object? currentValue,
        string fieldClass,
        EventCallback<ChangeEventArgs> changeEventCallback)
    {
        _parentContext = parentContext;

        ContainerComponentName = containerName;
        CurrentValue = currentValue;
        FieldClass = fieldClass;
        ChangeEventCallback = changeEventCallback;
    }

    /// <summary>
    /// Finds an <see cref="FluentOptionContext"/> in the context's ancestors with the matching <paramref name="containerName"/>.
    /// </summary>
    /// <param name="containerName">The group name of the ancestor <see cref="FluentOptionContext"/>.</param>
    /// <returns>The <see cref="FluentOptionContext"/>, or <c>null</c> if none was found.</returns>
    public FluentOptionContext? FindContextInAncestors(string containerName)
        => string.Equals(ContainerComponentName, containerName) ? this : _parentContext?.FindContextInAncestors(containerName);

}