// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Interface for a splash screen instance, displayed using <see cref="IDialogService.ShowSplashScreenAsync(Type, SplashScreenOptions)"/>.
/// </summary>
public interface ISplashScreenInstance : IDialogInstance
{
    /// <summary>
    /// Updates the options (including the <see cref="DialogOptions.Parameters"/>) of the splash screen,
    /// and re-renders the splash screen content with these new values.
    /// </summary>
    /// <param name="options">Action used to update the splash screen options.</param>
    /// <returns></returns>
    Task UpdateAsync(Action<SplashScreenOptions> options);
}
