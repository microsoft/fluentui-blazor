using System.Diagnostics;

namespace Microsoft.FluentUI.AspNetCore.Components.AssetsGenerator.Model;

/// <summary />
[DebuggerDisplay("{Cldr} [{Group}]")]
public class EmojiMetaData
{
    /// <summary />
    public string Cldr { get; set; } = string.Empty;

    /// <summary />
    public string Group { get; set; } = string.Empty;

    /// <summary />
    public string[] Keywords { get; set; } = Array.Empty<string>();

    /// <summary />
    public string Tts { get; set; } = string.Empty;
}
