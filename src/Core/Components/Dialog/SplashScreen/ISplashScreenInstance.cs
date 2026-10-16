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
    /// Updates the splash screen content and re-renders it with the new values.
    /// </summary>
    /// <param name="content">Action used to update the splash screen content.</param>
    /// <returns></returns>
    Task UpdateAsync(Action<SplashScreenContent> content);
}
