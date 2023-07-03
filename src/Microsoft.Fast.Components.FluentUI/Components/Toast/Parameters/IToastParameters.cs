using Microsoft.AspNetCore.Components;

namespace Microsoft.Fast.Components.FluentUI
{
    public interface IToastParameters
    {
        string? Id { get; set; }
        ToastIntent Intent { get; set; }
        (string Name, Color Color, IconVariant Variant)? Icon { get; set; }
        string? Title { get; set; }

        ToastTopCTAType TopCTAType { get; set; }
        string? TopAction { get; set; }
        EventCallback<ToastResult>? OnTopAction { get; set; }

        string? PrimaryAction { get; set; }
        EventCallback<ToastResult>? OnPrimaryAction { get; set; }

        string? SecondaryAction { get; set; }
        EventCallback<ToastResult>? OnSecondaryAction { get; set; }
        DateTime Timestamp { get; set; }
        int? Timeout { get; set; }
    }

    public interface IToastParameters<TContent> : IToastParameters
         where TContent : class
    {
        TContent Content { get; set; }
    }
}