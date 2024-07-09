using System.ComponentModel.DataAnnotations;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// The official product colors for Microsoft Office apps.
/// </summary>
public enum OfficeColor
{
    /// <summary>
    /// The default Fluent UI accent color
    /// </summary>
    [Display(Name = "default")]
    Default,

    /// <summary/>
    [Display(Name = "#a4373a")]
    Access,

    /// <summary/>
    [Display(Name = "#00a99d")]
    Booking,

    /// <summary/>
    [Display(Name = "#0078d4")]
    Exchange,

    /// <summary/>
    [Display(Name = "#217346")]
    Excel,

    /// <summary/>
    [Display(Name = "#00bcf2")]
    GroupMe,

    /// <summary/>
    [Display(Name = "#d83b01")]
    Office,

    /// <summary/>
    [Display(Name = "#0078d4")]
    OneDrive,

    /// <summary/>
    [Display(Name = "#7719aa")]
    OneNote,

    /// <summary />
    [Display(Name = "#0f6cbd")]
    Outlook,

    /// <summary/>
    [Display(Name = "#31752f")]
    Planner,

    /// <summary/>
    [Display(Name = "#742774")]
    PowerApps,

    /// <summary/>
    [Display(Name = "#f2c811")]
    PowerBI,

    /// <summary/>
    [Display(Name = "#b7472a")]
    PowerPoint,

    /// <summary/>
    [Display(Name = "#31752f")]
    Project,

    /// <summary/>
    [Display(Name = "#077568")]
    Publisher,

    /// <summary/>
    [Display(Name = "#0078d4")]
    SharePoint,

    /// <summary/>
    [Display(Name = "#0078d4")]
    Skype,

    /// <summary/>
    [Display(Name = "#bc1948")]
    Stream,

    /// <summary/>
    [Display(Name = "#008272")]
    Sway,

    /// <summary/>
    [Display(Name = "#6264a7")]
    Teams,

    /// <summary/>
    [Display(Name = "#3955a3")]
    Visio,

    /// <summary/>
    [Display(Name = "#0078d4")]
    Windows,

    /// <summary/>
    [Display(Name = "#2b579a")]
    Word,

    /// <summary/>
    [Display(Name = "#106ebe")]
    Yammer
}
