// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// Represents a splash screen instance used with the <see cref="IDialogService"/>.
/// </summary>
public class SplashScreenInstance : DialogInstance, ISplashScreenInstance
{
    /// <summary />
    internal SplashScreenInstance(IDialogService dialogService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType, SplashScreenOptions options)
        : base(dialogService, componentType, options)
    {
    }

    /// <inheritdoc cref="ISplashScreenInstance.UpdateAsync(Action{SplashScreenContent})"/>
    public Task UpdateAsync(Action<SplashScreenContent> content)
    {
        return DialogService.UpdateSplashScreenAsync(this, content);
    }
}
