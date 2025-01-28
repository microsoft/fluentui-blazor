// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public static class FluentStatus
{
    /// <summary />
    public static string SuccessColor { get; } = Color.Success.ToAttributeValue()!;

    /// <summary />
    public static string WarningColor { get; } = Color.Warning.ToAttributeValue()!;

    /// <summary />
    public static string ErrorColor { get; } = Color.Error.ToAttributeValue()!;

    /// <summary />
    public static string InfoColor { get; } = Color.Info.ToAttributeValue()!;

    /// <summary />
    public static Icon SuccessIcon { get; } = new CoreIcons.Filled.Size20.CheckmarkCircle().WithColor(SuccessColor);

    /// <summary />
    public static Icon WarningIcon { get; } = new CoreIcons.Filled.Size20.Warning().WithColor(WarningColor);

    /// <summary />
    public static Icon ErrorIcon { get; } = new CoreIcons.Filled.Size20.DismissCircle().WithColor(ErrorColor);

    /// <summary />
    public static Icon InfoIcon { get; } = new CoreIcons.Filled.Size20.Info().WithColor(InfoColor);
}
