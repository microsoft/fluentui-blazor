using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public enum MessageIntent
{
    /// <summary />
    [Display(Name = "info")]
    Info,

    /// <summary />
    [Display(Name = "warning")]
    Warning,

    /// <summary />
    [Display(Name = "error")]
    Error,

    /// <summary />
    [Display(Name = "success")]
    Success,

    /// <summary />
    [Display(Name = "custom")]
    Custom,
}
