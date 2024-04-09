namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
[Flags]
public enum WizardBorder
{
    /// <summary />
    None = 0,

    /// <summary />
    Inside = 1,

    /// <summary />
    Outside = 2,

    /// <summary />
    All = Inside | Outside,
}
