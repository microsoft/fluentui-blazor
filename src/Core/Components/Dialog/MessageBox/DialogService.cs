// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Dialog.MessageBox;
using Microsoft.FluentUI.AspNetCore.Components.Localization;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public partial class DialogService : IDialogService
{
    /// <inheritdoc cref="IDialogService.ShowSuccessAsync(string, string?, string?)"/>
    public Task<DialogResult> ShowSuccessAsync(string message, string? title = null, string? button = null)
    {
        return ShowMessageBoxAsync(new MessageBoxOptions
        {
            Title = title ?? Localizer[LanguageResource.MessageBox_Success],
            Message = message,
            PrimaryButton = button ?? Localizer[LanguageResource.MessageBox_ButtonOk],
            PrimaryShortCut = "Enter;Escape",
            Icon = new CoreIcons.Filled.Size20.CheckmarkCircle(),
            IconColor = Color.Success,
        });
    }

    /// <inheritdoc cref="IDialogService.ShowWarningAsync(string, string?, string?)"/>
    public Task<DialogResult> ShowWarningAsync(string message, string? title = null, string? button = null)
    {
        return ShowMessageBoxAsync(new MessageBoxOptions
        {
            Title = title ?? Localizer[LanguageResource.MessageBox_Warning],
            Message = message,
            PrimaryButton = button ?? Localizer[LanguageResource.MessageBox_ButtonOk],
            PrimaryShortCut = "Enter;Escape",
            Icon = new CoreIcons.Filled.Size20.Warning(),
            IconColor = Color.Warning,
        });
    }

    /// <inheritdoc cref="IDialogService.ShowErrorAsync(string, string?, string?)"/>
    public Task<DialogResult> ShowErrorAsync(string message, string? title = null, string? button = null)
    {
        return ShowMessageBoxAsync(new MessageBoxOptions
        {
            Title = title ?? Localizer[LanguageResource.MessageBox_Error],
            Message = message,
            PrimaryButton = button ?? Localizer[LanguageResource.MessageBox_ButtonOk],
            PrimaryShortCut = "Enter;Escape",
            Icon = new CoreIcons.Filled.Size20.DismissCircle(),
            IconColor = Color.Error,
        });
    }

    /// <inheritdoc cref="IDialogService.ShowInfoAsync(string, string?, string?)"/>
    public Task<DialogResult> ShowInfoAsync(string message, string? title = null, string? button = null)
    {
        return ShowMessageBoxAsync(new MessageBoxOptions
        {
            Title = title ?? Localizer[LanguageResource.MessageBox_Information],
            Message = message,
            PrimaryButton = button ?? Localizer[LanguageResource.MessageBox_ButtonOk],
            PrimaryShortCut = "Enter;Escape",
            Icon = new CoreIcons.Filled.Size20.Info(),
            IconColor = Color.Info,
        });
    }

    /// <inheritdoc cref="IDialogService.ShowConfirmationAsync(string, string?, string?, string?)"/>
    public Task<DialogResult> ShowConfirmationAsync(string message, string? title = null, string? primaryButton = null, string? secondaryButton = null)
    {
        return ShowMessageBoxAsync(new MessageBoxOptions
        {
            Title = title ?? Localizer[LanguageResource.MessageBox_Confirmation],
            Message = message,
            PrimaryButton = primaryButton ?? Localizer[LanguageResource.MessageBox_ButtonYes],
            PrimaryShortCut = "Enter;Y",
            SecondaryButton = secondaryButton ?? Localizer[LanguageResource.MessageBox_ButtonNo],
            SecondaryShortCut = "Escape;N",
            Icon = new CoreIcons.Regular.Size20.QuestionCircle(),
            IconColor = Color.Default,
        });
    }

    /// <inheritdoc cref="IDialogService.ShowMessageBoxAsync(MessageBoxOptions)"/>/>
    public Task<DialogResult> ShowMessageBoxAsync(MessageBoxOptions options)
    {
        return ShowDialogAsync<FluentMessageBox>(config =>
        {
            config.Header.Title = options.Title;
            config.Footer.PrimaryAction.Label = options.PrimaryButton;
            config.Footer.PrimaryAction.ShortCut = options.PrimaryShortCut;
            config.Footer.SecondaryAction.Label = options.SecondaryButton;
            config.Footer.SecondaryAction.ShortCut = options.SecondaryShortCut;
            config.Parameters.Add(nameof(FluentMessageBox.Message), new MarkupString(options.Message ?? ""));
            config.Parameters.Add(nameof(FluentMessageBox.Icon), options.Icon);
            config.Parameters.Add(nameof(FluentMessageBox.IconColor), options.IconColor);
        });
    }
}
