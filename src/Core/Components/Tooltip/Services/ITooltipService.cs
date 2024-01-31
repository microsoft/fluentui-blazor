namespace Microsoft.FluentUI.AspNetCore.Components.Components.Tooltip;

/// <summary>
/// Service for managing tooltips.
/// </summary>
public interface ITooltipService : IDisposable
{
    /// <summary>
    /// Action that is invoked when the tooltip list is updated.
    /// </summary>
    event Action? OnTooltipUpdated;

    /// <summary>
    /// Gets the list of tooltips currently registered with the service.
    /// </summary>
    IEnumerable<TooltipOptions> Tooltips { get; }

    /// <summary>
    /// Gets the global options for tooltips.
    /// </summary>
    TooltipGlobalOptions GlobalOptions { get; }

    /// <summary>
    /// Adds a tooltip to the service.
    /// </summary>
    /// <param name="options"></param>
    void Add(TooltipOptions options);

    /// <summary>
    /// Clears all tooltips from the service.
    /// </summary>
    void Clear();

    /// <summary>
    /// Updates all service tooltips.
    /// </summary>
    void Refresh();

    /// <summary>
    /// removes a tooltip from the service.
    /// </summary>
    /// <param name="value">Identifier of the tooltip</param>
    void Remove(Guid value);
}
