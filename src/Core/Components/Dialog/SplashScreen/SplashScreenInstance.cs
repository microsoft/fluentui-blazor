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
    internal SplashScreenInstance(IDialogService dialogService, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType, DialogOptions options)
        : base(dialogService, componentType, options)
    {
    }

    /// <inheritdoc cref="ISplashScreenInstance.UpdateAsync(Action{DialogOptions})"/>
    public Task UpdateAsync(Action<DialogOptions> options)
    {
        return DialogService.UpdateSplashScreenAsync(this, options);
    }
}
