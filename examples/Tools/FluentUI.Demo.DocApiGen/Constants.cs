// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace FluentUI.Demo.DocApiGen;

/// <summary>
/// List of constants used in the application.
/// </summary>
public static class Constants
{
    /// <summary>
    /// List of types to exclude from the documentation.
    /// </summary>
    public static readonly string[] EXCLUDE_TYPES =
    [
        "TypeInference",
        "InternalListContext`1",
        "SpacingGenerator",
        "FluentLocalizerInternal",
        "FluentJSModule",
        "FluentServiceProviderException`1",
    ];

    /// <summary>
    /// List of members to exclude from the documentation.
    /// </summary>
    public static readonly string[] MEMBERS_TO_EXCLUDE =
    [
        "AdditionalAttributes",
        "ParentReference",
        "Element",
        "Equals",
        "GetHashCode",
        "GetType",
        "SetParametersAsync",
        "ToString",
        "Dispose",
        "DisposeAsync",
        "ValueExpression",
    ];
}
