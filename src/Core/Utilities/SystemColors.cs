// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary />
public class SystemColors : StylesVariables.Colors
{
}

/// <summary />
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0048:File name must match type name", Justification = "To simplify the class")]
public partial class StylesVariables
{
    public partial class Colors
    {
        /// <summary />
        public partial class Alerts
        {
            /// <summary />
            public const string Success = "var(--success)";

            /// <summary />
            public const string Warning = "var(--warning)";

            /// <summary />
            public const string Error = "var(--error)";

            /// <summary />
            public const string Info = "var(--info)";
        }

        /// <summary />
        public partial class Presence
        {
            /// <summary />
            public const string Available = "var(--presence-available)";

            /// <summary />
            public const string Away = "var(--presence-away)";

            /// <summary />
            public const string Busy = "var(--presence-busy)";

            /// <summary />
            public const string Dnd = "var(--presence-dnd)";

            /// <summary />
            public const string Offline = "var(--presence-offline)";

            /// <summary />
            public const string Oof = "var(--presence-oof)";

            /// <summary />
            public const string Blocked = "var(--presence-blocked)";

            /// <summary />
            public const string Unknown = "var(--presence-unknown)";
        }

        /// <summary />
        public partial class Highlight
        {
            /// <summary />
            public const string Background = "var(--highlight-bg)";
        }
    }
}
