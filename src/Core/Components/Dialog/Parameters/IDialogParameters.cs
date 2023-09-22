﻿using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public interface IDialogParameters
    {
        string Id { get; set; }
        HorizontalAlignment Alignment { get; set; }
        string? Title { get; set; }
        bool? Modal { get; set; }
        bool? TrapFocus { get; set; }
        bool ShowTitle { get; set; }
        bool ShowDismiss { get; set; }
        string? DismissTitle { get; set; }
        string? PrimaryAction { get; set; }
        bool PrimaryActionEnabled { get; set; }
        string? SecondaryAction { get; set; }
        bool SecondaryActionEnabled { get; set; }
        string? Width { get; set; }
        string? Height { get; set; }
        string DialogBodyStyle { get; set; }
        string? AriaDescribedby { get; set; }
        string? AriaLabel { get; set; }
        string? AriaLabelledby { get; set; }
        DialogType DialogType { get; set; }
        EventCallback<DialogResult> OnDialogResult { get; set; }
    }

    public interface IDialogParameters<TContent> : IDialogParameters
         where TContent : class
    {
        TContent Content { get; set; }
    }
}