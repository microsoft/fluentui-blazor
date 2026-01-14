// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components;

/// <summary>
/// List of icons used and embedded in the FluentUI components.
/// </summary>
internal static partial class CoreIcons
{
    /*
     * *************************** NOTES ***************************
     *
     * Try to use only the Regular.Size20 and Filled.Size20 icons,
     * to avoid duplicating the same icon in different sizes.
     * *************************************************************
     */

    /// <summary>
    /// Regular icons
    /// </summary>
    internal static partial class Regular
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        internal static partial class Size20
        {
            public class Add : Icon { public Add() : base("Add", IconVariant.Regular, IconSize.Size20, "<path d=\"M10 2.5a.5.5 0 0 0-1 0V9H2.5a.5.5 0 0 0 0 1H9v6.5a.5.5 0 0 0 1 0V10h6.5a.5.5 0 0 0 0-1H10V2.5Z\"/>") { } }

            public class ArrowCircleDown : Icon { public ArrowCircleDown() : base("ArrowCircleDown", IconVariant.Regular, IconSize.Size20, "<path d=\"M13.3536 10.8536L10.3536 13.8536C10.1583 14.0488 9.84171 14.0488 9.64645 13.8536L6.64645 10.8536C6.45118 10.6583 6.45118 10.3417 6.64645 10.1464C6.84171 9.95118 7.15829 9.95118 7.35355 10.1464L9.5 12.2929L9.5 6.5C9.5 6.22386 9.72386 6 10 6C10.2761 6 10.5 6.22386 10.5 6.5V12.2929L12.6464 10.1464C12.8417 9.95118 13.1583 9.95118 13.3536 10.1464C13.5488 10.3417 13.5488 10.6583 13.3536 10.8536ZM10 18C14.4183 18 18 14.4183 18 10C18 5.58172 14.4183 2 10 2C5.58172 2 2 5.58172 2 10C2 14.4183 5.58172 18 10 18ZM17 10C17 13.866 13.866 17 10 17C6.13401 17 3 13.866 3 10C3 6.13401 6.13401 3 10 3C13.866 3 17 6.13401 17 10Z\" />") { } }

            public class ArrowCircleUp : Icon { public ArrowCircleUp(): base("ArrowCircleUp", IconVariant.Regular, IconSize.Size20, "<path d=\"M6.64645 9.14645L9.64645 6.14645C9.84171 5.95118 10.1583 5.95118 10.3536 6.14645L13.3536 9.14645C13.5488 9.34171 13.5488 9.65829 13.3536 9.85355C13.1583 10.0488 12.8417 10.0488 12.6464 9.85355L10.5 7.70711V13.5C10.5 13.7761 10.2761 14 10 14C9.72386 14 9.5 13.7761 9.5 13.5V7.70711L7.35355 9.85355C7.15829 10.0488 6.84171 10.0488 6.64645 9.85355C6.45118 9.65829 6.45118 9.34171 6.64645 9.14645ZM10 2C5.58172 2 2 5.58172 2 10C2 14.4183 5.58172 18 10 18C14.4183 18 18 14.4183 18 10C18 5.58172 14.4183 2 10 2ZM3 10C3 6.13401 6.13401 3 10 3C13.866 3 17 6.13401 17 10C17 13.866 13.866 17 10 17C6.13401 17 3 13.866 3 10Z\" />") { } }

            public class ArrowReset : Icon { public ArrowReset() : base("ArrowReset", IconVariant.Regular, IconSize.Size20, "<path d=\"M5.85 2.65c.2.2.2.5 0 .7L4.21 5H11a6 6 0 1 1-6 6 .5.5 0 0 1 1 0 5 5 0 1 0 5-5H4.2l1.65 1.65a.5.5 0 1 1-.7.7l-2.5-2.5a.5.5 0 0 1 0-.7l2.5-2.5c.2-.2.5-.2.7 0Z\"/>") { } }

            public class ArrowSort : Icon { public ArrowSort() : base("ArrowSort", IconVariant.Regular, IconSize.Size20, "<path d=\"M2.35 7.35 5 4.71V16.5a.5.5 0 0 0 1 0V4.7l2.65 2.65a.5.5 0 0 0 .7-.7l-3.49-3.5A.5.5 0 0 0 5.5 3a.5.5 0 0 0-.39.18L1.65 6.65a.5.5 0 1 0 .7.7Zm15.3 5.3L15 15.29V3.5a.5.5 0 0 0-1 0v11.8l-2.65-2.65a.5.5 0 0 0-.7.7l3.49 3.5a.5.5 0 0 0 .36.15.5.5 0 0 0 .39-.18l3.46-3.47a.5.5 0 1 0-.7-.7Z\"/>") { } }

            public class ArrowSortDown : Icon { public ArrowSortDown() : base("ArrowSortDown", IconVariant.Regular, IconSize.Size20, "<path d=\"m10 15.29 2.65-2.64a.5.5 0 0 1 .7.7L9.9 16.82a.5.5 0 0 1-.74.03h-.01l-3.5-3.5a.5.5 0 1 1 .71-.7L9 15.29V3.5a.5.5 0 0 1 1 0v11.79Z\"/>") { } }

            public class ArrowSortUp : Icon { public ArrowSortUp() : base("ArrowSortUp", IconVariant.Regular, IconSize.Size20, "<path d=\"M9 4.71 6.35 7.35a.5.5 0 1 1-.7-.7L9.1 3.18a.5.5 0 0 1 .74-.03h.01l3.5 3.5a.5.5 0 1 1-.71.7L10 4.71V16.5a.5.5 0 0 1-1 0V4.71Z\"/>") { } }

            public class ArrowSyncCircle : Icon { public ArrowSyncCircle() : base("ArrowSyncCircle", IconVariant.Regular, IconSize.Size20, "<path d=\"M10 3C13.866 3 17 6.13401 17 10C17 13.866 13.866 17 10 17C6.13401 17 3 13.866 3 10C3 6.13401 6.13401 3 10 3ZM18 10C18 5.58172 14.4183 2 10 2C5.58172 2 2 5.58172 2 10C2 14.4183 5.58172 18 10 18C14.4183 18 18 14.4183 18 10ZM10.0001 7.5C11.0245 7.5 11.9062 8.11648 12.2922 9H11.4999C11.2237 9 10.9999 9.22386 10.9999 9.5C10.9999 9.77614 11.2237 10 11.4999 10H13.4999C13.776 10 13.9999 9.77614 13.9999 9.5V7.5C13.9999 7.22386 13.776 7 13.4999 7C13.2237 7 12.9999 7.22386 12.9999 7.5V8.19617C12.3877 7.18015 11.2737 6.5 10.0001 6.5C8.96342 6.5 8.03154 6.9513 7.39138 7.66654C7.20721 7.87231 7.22473 8.1884 7.43049 8.37257C7.63625 8.55673 7.95235 8.53922 8.13651 8.33346C8.59499 7.8212 9.25968 7.5 10.0001 7.5ZM7.00012 11.8037V12.5C7.00012 12.7761 6.77626 13 6.50012 13C6.22398 13 6.00012 12.7761 6.00012 12.5V10.5C6.00012 10.2239 6.22398 10 6.50012 10H8.50012C8.77626 10 9.00012 10.2239 9.00012 10.5C9.00012 10.7761 8.77626 11 8.50012 11H7.70793C8.09394 11.8835 8.97557 12.5 10.0001 12.5C10.7404 12.5 11.4051 12.1788 11.8636 11.6665C12.0478 11.4608 12.3639 11.4433 12.5696 11.6274C12.7754 11.8116 12.7929 12.1277 12.6087 12.3335C11.9686 13.0487 11.0367 13.5 10.0001 13.5C8.7263 13.5 7.61231 12.8198 7.00012 11.8037Z\" />") { } }

            public class Calendar : Icon { public Calendar() : base("Calendar", IconVariant.Regular, IconSize.Size20, "<path d=\"M7 11a1 1 0 1 0 0-2 1 1 0 0 0 0 2Zm1 2a1 1 0 1 1-2 0 1 1 0 0 1 2 0Zm2-2a1 1 0 1 0 0-2 1 1 0 0 0 0 2Zm1 2a1 1 0 1 1-2 0 1 1 0 0 1 2 0Zm2-2a1 1 0 1 0 0-2 1 1 0 0 0 0 2Zm4-5.5A2.5 2.5 0 0 0 14.5 3h-9A2.5 2.5 0 0 0 3 5.5v9A2.5 2.5 0 0 0 5.5 17h9a2.5 2.5 0 0 0 2.5-2.5v-9ZM4 7h12v7.5c0 .83-.67 1.5-1.5 1.5h-9A1.5 1.5 0 0 1 4 14.5V7Zm1.5-3h9c.83 0 1.5.67 1.5 1.5V6H4v-.5C4 4.67 4.67 4 5.5 4Z\"></path>") { } };

            public class CheckboxUnchecked : Icon { public CheckboxUnchecked() : base("CheckboxUnchecked", IconVariant.Regular, IconSize.Size20, "<path d=\"M3 6a3 3 0 0 1 3-3h8a3 3 0 0 1 3 3v8a3 3 0 0 1-3 3H6a3 3 0 0 1-3-3V6Zm3-2a2 2 0 0 0-2 2v8c0 1.1.9 2 2 2h8a2 2 0 0 0 2-2V6a2 2 0 0 0-2-2H6Z\" />") { } }

            public class Checkmark : Icon { public Checkmark() : base("Checkmark", IconVariant.Regular, IconSize.Size20, "<path d=\"M3.37371 10.1678C3.19025 9.96143 2.87421 9.94284 2.66782 10.1263C2.46143 10.3098 2.44284 10.6258 2.6263 10.8322L6.6263 15.3322C6.81743 15.5472 7.15013 15.557 7.35356 15.3536L17.8536 4.85355C18.0488 4.65829 18.0488 4.34171 17.8536 4.14645C17.6583 3.95118 17.3417 3.95118 17.1465 4.14645L7.02141 14.2715L3.37371 10.1678Z\" />") { } }

            public class CheckmarkCircle : Icon { public CheckmarkCircle() : base("CheckMarkCircle", IconVariant.Regular, IconSize.Size20, "<path d=\"M10 2C14.4183 2 18 5.58172 18 10C18 14.4183 14.4183 18 10 18C5.58172 18 2 14.4183 2 10C2 5.58172 5.58172 2 10 2ZM10 3C6.13401 3 3 6.13401 3 10C3 13.866 6.13401 17 10 17C13.866 17 17 13.866 17 10C17 6.13401 13.866 3 10 3ZM13.3584 7.64645C13.532 7.82001 13.5513 8.08944 13.4163 8.28431L13.3584 8.35355L9.35355 12.3584C9.17999 12.532 8.91056 12.5513 8.71569 12.4163L8.64645 12.3584L6.64645 10.3584C6.45118 10.1632 6.45118 9.84658 6.64645 9.65131C6.82001 9.47775 7.08944 9.45846 7.28431 9.59346L7.35355 9.65131L9 11.298L12.6513 7.64645C12.8466 7.45118 13.1632 7.45118 13.3584 7.64645Z\" />") { } }

            public class ChevronDown : Icon { public ChevronDown() : base("ChevronDown", IconVariant.Regular, IconSize.Size20, "<path d=\"M15.85 7.65c.2.2.2.5 0 .7l-5.46 5.49a.55.55 0 0 1-.78 0L4.15 8.35a.5.5 0 1 1 .7-.7L10 12.8l5.15-5.16c.2-.2.5-.2.7 0Z\"/>") { } }

            public class ChevronDoubleLeft : Icon { public ChevronDoubleLeft() : base("ChevronDoubleLeft", IconVariant.Regular, IconSize.Size20, "<path d=\"M11.35 15.85a.5.5 0 0 1-.7 0L5.16 10.4a.55.55 0 0 1 0-.78l5.49-5.46a.5.5 0 1 1 .7.7L6.2 10l5.16 5.15c.2.2.2.5 0 .7Zm4 0a.5.5 0 0 1-.7 0L9.16 10.4a.55.55 0 0 1 0-.78l5.49-5.46a.5.5 0 1 1 .7.7L10.2 10l5.16 5.15c.2.2.2.5 0 .7Z\"/>") { } }

            public class ChevronDoubleRight : Icon { public ChevronDoubleRight() : base("ChevronDoubleRight", IconVariant.Regular, IconSize.Size20, "<path d=\"M8.65 4.15c.2-.2.5-.2.7 0l5.49 5.46c.21.22.21.57 0 .78l-5.49 5.46a.5.5 0 0 1-.7-.7L13.8 10 8.65 4.85a.5.5 0 0 1 0-.7Zm-4 0c.2-.2.5-.2.7 0l5.49 5.46c.21.22.21.57 0 .78l-5.49 5.46a.5.5 0 0 1-.7-.7L9.8 10 4.65 4.85a.5.5 0 0 1 0-.7Z\"/>") { } }

            public class ChevronLeft : Icon { public ChevronLeft() : base("ChevronLeft", IconVariant.Regular, IconSize.Size20, "<path d=\"M12.35 15.85a.5.5 0 0 1-.7 0L6.16 10.4a.55.55 0 0 1 0-.78l5.49-5.46a.5.5 0 1 1 .7.7L7.2 10l5.16 5.15c.2.2.2.5 0 .7Z\"/>") { } }

            public class ChevronRight : Icon { public ChevronRight() : base("ChevronRight", IconVariant.Regular, IconSize.Size20, "<path d=\"M7.65 4.15c.2-.2.5-.2.7 0l5.49 5.46c.21.22.21.57 0 .78l-5.49 5.46a.5.5 0 0 1-.7-.7L12.8 10 7.65 4.85a.5.5 0 0 1 0-.7Z\"/>") { } }

            public class ChevronUp : Icon { public ChevronUp() : base("ChevronUp", IconVariant.Regular, IconSize.Size20, "<path d=\"M4.15 12.35a.5.5 0 0 1 0-.7L9.6 6.16a.55.55 0 0 1 .78 0l5.46 5.49a.5.5 0 0 1-.7.7L10 7.2l-5.15 5.16a.5.5 0 0 1-.7 0Z\"/>") { } }

            public class Clock : Icon { public Clock() : base("Clock", IconVariant.Regular, IconSize.Size20, "<path d=\"M10 2a8 8 0 1 1 0 16 8 8 0 0 1 0-16Zm0 1a7 7 0 1 0 0 14 7 7 0 0 0 0-14Zm-.5 2a.5.5 0 0 1 .5.41V10h2.5a.5.5 0 0 1 .09 1H9.5a.5.5 0 0 1-.5-.41V5.5c0-.28.22-.5.5-.5Z\"></path>") { } };

            public class Dismiss : Icon { public Dismiss() : base("Dismiss", IconVariant.Regular, IconSize.Size20, "<path d=\"m4.09 4.22.06-.07a.5.5 0 0 1 .63-.06l.07.06L10 9.29l5.15-5.14a.5.5 0 0 1 .63-.06l.07.06c.18.17.2.44.06.63l-.06.07L10.71 10l5.14 5.15c.18.17.2.44.06.63l-.06.07a.5.5 0 0 1-.63.06l-.07-.06L10 10.71l-5.15 5.14a.5.5 0 0 1-.63.06l-.07-.06a.5.5 0 0 1-.06-.63l.06-.07L9.29 10 4.15 4.85a.5.5 0 0 1-.06-.63l.06-.07-.06.07Z\"></path>") { } };

            public class Filter : Icon { public Filter() : base("Filter", IconVariant.Regular, IconSize.Size20, "<path d=\"M7.5 13h5a.5.5 0 0 1 .09 1H7.5a.5.5 0 0 1-.09-1h5.09-5Zm-2-4h9a.5.5 0 0 1 .09 1H5.5a.5.5 0 0 1-.09-1h9.09-9Zm-2-4h13a.5.5 0 0 1 .09 1H3.5a.5.5 0 0 1-.09-1H16.5h-13Z\"/>") { } }

            public class FilterDismiss : Icon { public FilterDismiss() : base("FilterDismiss", IconVariant.Regular, IconSize.Size20, "<path d=\"M9.2 7H2.42a.5.5 0 0 0 .09 1h7.1c-.16-.32-.3-.65-.4-1Zm2.8 8.5a.5.5 0 0 0-.5-.5H6.41a.5.5 0 0 0 .09 1h5.09a.5.5 0 0 0 .41-.5Zm1.5-4.5H4.41a.5.5 0 0 0 .09 1h9.09a.5.5 0 0 0-.09-1Zm1-1a4.5 4.5 0 1 0 0-9 4.5 4.5 0 0 0 0 9Zm1.85-6.35c.2.2.2.5 0 .7L15.21 5.5l1.14 1.15a.5.5 0 0 1-.7.7L14.5 6.21l-1.15 1.14a.5.5 0 0 1-.7-.7l1.14-1.15-1.14-1.15a.5.5 0 0 1 .7-.7l1.15 1.14 1.15-1.14c.2-.2.5-.2.7 0Z\"/>") { } }

            public class Folder : Icon { public Folder() : base("Folder", IconVariant.Regular, IconSize.Size20, "<path d=\"M4.5 3A2.5 2.5 0 0 0 2 5.5v9A2.5 2.5 0 0 0 4.5 17h11a2.5 2.5 0 0 0 2.5-2.5v-7A2.5 2.5 0 0 0 15.5 5H9.7L8.23 3.51A1.75 1.75 0 0 0 6.98 3H4.5ZM3 5.5C3 4.67 3.67 4 4.5 4h2.48c.2 0 .4.08.53.22L8.8 5.5 7.44 6.85a.5.5 0 0 1-.35.15H3V5.5ZM3 8h4.09c.4 0 .78-.16 1.06-.44L9.7 6h5.79c.83 0 1.5.67 1.5 1.5v7c0 .83-.67 1.5-1.5 1.5h-11A1.5 1.5 0 0 1 3 14.5V8Z\"/>") { } }

            public class Info : Icon { public Info() : base("Info", IconVariant.Regular, IconSize.Size20, "<path d=\"M10.5 8.91a.5.5 0 0 0-1 .09v4.6a.5.5 0 0 0 1-.1V8.91Zm.3-2.16a.75.75 0 1 0-1.5 0 .75.75 0 0 0 1.5 0ZM18 10a8 8 0 1 0-16 0 8 8 0 0 0 16 0ZM3 10a7 7 0 1 1 14 0 7 7 0 0 1-14 0Z\"></path>") { } };

            public class LineHorizontal3 : Icon { public LineHorizontal3() : base("LineHorizontal3", IconVariant.Regular, IconSize.Size20, "<path d=\"M2 4.5c0-.28.22-.5.5-.5h15a.5.5 0 0 1 0 1h-15a.5.5 0 0 1-.5-.5Zm0 5c0-.28.22-.5.5-.5h15a.5.5 0 0 1 0 1h-15a.5.5 0 0 1-.5-.5Zm.5 4.5a.5.5 0 0 0 0 1h15a.5.5 0 0 0 0-1h-15Z\"></path>") { } };

            public class MoreHorizontal : Icon { public MoreHorizontal() : base("MoreHorizontal", IconVariant.Regular, IconSize.Size20, "<path d=\"M6.25 10a1.25 1.25 0 1 1-2.5 0 1.25 1.25 0 0 1 2.5 0Zm5 0a1.25 1.25 0 1 1-2.5 0 1.25 1.25 0 0 1 2.5 0ZM15 11.25a1.25 1.25 0 1 0 0-2.5 1.25 1.25 0 0 0 0 2.5Z\"/>") { } }

            public class QuestionCircle : Icon { public QuestionCircle() : base("QuestionCircle", IconVariant.Regular, IconSize.Size20, "<path d=\"M10 2a8 8 0 1 1 0 16 8 8 0 0 1 0-16Zm0 1a7 7 0 1 0 0 14 7 7 0 0 0 0-14Zm0 10.5a.75.75 0 1 1 0 1.5.75.75 0 0 1 0-1.5Zm0-8a2.5 2.5 0 0 1 1.65 4.38l-.15.12-.22.17-.09.07-.16.15c-.33.36-.53.85-.53 1.61a.5.5 0 0 1-1 0 3.2 3.2 0 0 1 1.16-2.62l.25-.19.12-.1A1.5 1.5 0 0 0 10 6.5c-.83 0-1.5.67-1.5 1.5a.5.5 0 0 1-1 0A2.5 2.5 0 0 1 10 5.5Z\"></path>") { } };

            public class PresenceAvailable : Icon { public PresenceAvailable() : base("PresenceAvailable", IconVariant.Regular, IconSize.Size20, "<path d=\"M10 0a10 10 0 1 0 0 20 10 10 0 0 0 0-20ZM2 10a8 8 0 1 1 16 0 8 8 0 0 1-16 0Zm12.2-3.2a1 1 0 0 1 0 1.4l-4.5 4.5a1 1 0 0 1-1.4 0l-2-2a1 1 0 0 1 1.4-1.4L9 10.58l3.8-3.8a1 1 0 0 1 1.4 0Z\"/>") { } }

            public class PresenceAway : Icon { public PresenceAway() : base("PresenceAway", IconVariant.Regular, IconSize.Size20, "<path d=\"M10 9.59V6a1 1 0 1 0-2 0V10c0 .27.1.52.3.7l3 3a1 1 0 1 0 1.4-1.4L10 9.58Zm-10 .4a10 10 0 1 1 20 0 10 10 0 0 1-20 0ZM10 2a8 8 0 1 0 0 16 8 8 0 0 0 0-16Z\"/>") { } }

            public class PresenceBlocked : Icon { public PresenceBlocked() : base("PresenceBlocked", IconVariant.Regular, IconSize.Size20, "<path d=\"M20 10a10 10 0 1 0-20 0 10 10 0 0 0 20 0Zm-2 0a8 8 0 0 1-12.9 6.32L16.31 5.09A7.97 7.97 0 0 1 18 10Zm-3.1-6.32L3.69 14.91A8 8 0 0 1 14.91 3.68Z\"/>") { } }

            public class PresenceDnd : Icon { public PresenceDnd() : base("PresenceDnd", IconVariant.Regular, IconSize.Size20, "<path d=\"M10 0a10 10 0 1 0 0 20 10 10 0 0 0 0-20ZM2 10a8 8 0 1 1 16 0 8 8 0 0 1-16 0Zm4 0a1 1 0 0 1 1-1h6a1 1 0 1 1 0 2H7a1 1 0 0 1-1-1Z\"/>") { } }

            public class PresenceOffline : Icon { public PresenceOffline() : base("PresenceOffline", IconVariant.Regular, IconSize.Size20, "<path d=\"M13.7 6.3a1 1 0 0 1 0 1.4L11.42 10l2.3 2.3a1 1 0 0 1-1.42 1.4L10 11.42l-2.3 2.3a1 1 0 0 1-1.4-1.42L8.58 10l-2.3-2.3a1 1 0 0 1 1.42-1.4L10 8.58l2.3-2.3a1 1 0 0 1 1.4 0ZM0 10a10 10 0 1 1 20 0 10 10 0 0 1-20 0Zm10-8a8 8 0 1 0 0 16 8 8 0 0 0 0-16Z\"/>") { } }

            public class PresenceOof : Icon { public PresenceOof() : base("PresenceOof", IconVariant.Regular, IconSize.Size20, "<path d=\"M10.7 7.7A1 1 0 1 0 9.28 6.3l-3 3a1 1 0 0 0 0 1.41l3 3a1 1 0 1 0 1.42-1.41l-1.3-1.3H13a1 1 0 1 0 0-2H9.4l1.3-1.29ZM10 0a10 10 0 1 0 0 20 10 10 0 0 0 0-20ZM2 10a8 8 0 1 1 16 0 8 8 0 0 1-16 0Z\"/>") { } }

            public class PresenceTentative : Icon { public PresenceTentative() : base("PresenceTentative", IconVariant.Regular, IconSize.Size20, "<path d=\"M8.95.05a9.96 9.96 0 0 0-8.9 8.9l8.9-8.9ZM.19 11.95 11.95.2c.8.16 1.6.42 2.35.78L.97 14.31a9.97 9.97 0 0 1-.78-2.36Zm1.99 4.29a10.12 10.12 0 0 0 1.58 1.58L17.81 3.76a10.1 10.1 0 0 0-1.58-1.58L2.18 16.24ZM19.02 5.69 5.7 19.03c.76.36 1.55.62 2.36.78L19.8 8.05c-.16-.8-.42-1.6-.79-2.36Zm.92 5.37-8.89 8.88a9.96 9.96 0 0 0 8.89-8.88Z\"/>") { } }

            public class PresenceUnknown : Icon { public PresenceUnknown() : base("PresenceUnknown", IconVariant.Regular, IconSize.Size20, "<path d=\"M10 2a8 8 0 1 0 0 16 8 8 0 0 0 0-16ZM0 10a10 10 0 1 1 20 0 10 10 0 0 1-20 0Z\"/>") { } }

            public class RadioButton : Icon { public RadioButton() : base("RadioButton", IconVariant.Regular, IconSize.Size20, "<path d=\"M10 3a7 7 0 1 0 0 14 7 7 0 0 0 0-14Zm-8 7a8 8 0 1 1 16 0 8 8 0 0 1-16 0Z\" />") { } };

            public class Search : Icon { public Search() : base("Search", IconVariant.Regular, IconSize.Size20, "<path d=\"M12.73 13.44a6.5 6.5 0 1 1 .7-.7l3.42 3.4a.5.5 0 0 1-.63.77l-.07-.06-3.42-3.41Zm-.71-.71A5.54 5.54 0 0 0 14 8.5a5.5 5.5 0 1 0-1.98 4.23Z\"/>") { } }

            public class Subtract : Icon { public Subtract() : base("Subtract", IconVariant.Regular, IconSize.Size20, "<path d=\"M3 10c0-.28.22-.5.5-.5h13a.5.5 0 0 1 0 1h-13A.5.5 0 0 1 3 10Z\"/>") { } }

            public class TableResizeColumn : Icon { public TableResizeColumn() : base("TableResizeColumn", IconVariant.Regular, IconSize.Size20, "<path d=\"M7.35 8.15c.2.2.2.5 0 .7l-.64.65h6.58l-.64-.65a.5.5 0 0 1 .7-.7l1.5 1.5c.2.2.2.5 0 .7l-1.5 1.5a.5.5 0 0 1-.7-.7l.64-.65H6.71l.64.65a.5.5 0 0 1-.7.7l-1.5-1.5a.5.5 0 0 1 0-.7l1.5-1.5c.2-.2.5-.2.7 0ZM17 6a3 3 0 0 0-3-3H6a3 3 0 0 0-3 3v8a3 3 0 0 0 3 3h8a3 3 0 0 0 3-3V6Zm-4-2v3c.36 0 .72.13 1 .38V4a2 2 0 0 1 2 2v8a2 2 0 0 1-2 2v-3.38a1.5 1.5 0 0 1-1 .38v3H7v-3a1.5 1.5 0 0 1-1-.38V16a2 2 0 0 1-2-2V6c0-1.1.9-2 2-2v3.38A1.5 1.5 0 0 1 7 7V4h6Z\"/>") { } }
        }
    }

    /// <summary>
    /// Filled icons
    /// </summary>
    internal static partial class Filled
    {
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        internal static partial class Size20
        {
            public class CheckmarkCircle : Icon { public CheckmarkCircle() : base("CheckmarkCircle", IconVariant.Filled, IconSize.Size20, "<path d=\"M10 2a8 8 0 1 1 0 16 8 8 0 0 1 0-16Zm3.36 5.65a.5.5 0 0 0-.64-.06l-.07.06L9 11.3 7.35 9.65l-.07-.06a.5.5 0 0 0-.7.7l.07.07 2 2 .07.06c.17.11.4.11.56 0l.07-.06 4-4 .07-.08a.5.5 0 0 0-.06-.63Z\"/>") { } }

            public class DismissCircle : Icon { public DismissCircle() : base("DismissCircle", IconVariant.Filled, IconSize.Size20, "<path d=\"M10 2a8 8 0 1 1 0 16 8 8 0 0 1 0-16ZM7.8 7.11a.5.5 0 0 0-.63.06l-.06.07a.5.5 0 0 0 .06.64L9.3 10l-2.12 2.12-.06.07a.5.5 0 0 0 .06.64l.07.06c.2.13.47.11.64-.06L10 10.7l2.12 2.12.07.06c.2.13.46.11.64-.06l.06-.07a.5.5 0 0 0-.06-.64L10.7 10l2.12-2.12.06-.07a.5.5 0 0 0-.06-.64l-.07-.06a.5.5 0 0 0-.64.06L10 9.3 7.88 7.17l-.07-.06Z\"/>") { } }

            public class Info : Icon { public Info() : base("Info", IconVariant.Filled, IconSize.Size20, "<path d=\"M18 10a8 8 0 1 0-16 0 8 8 0 0 0 16 0ZM9.5 8.91a.5.5 0 0 1 1 0V13.6a.5.5 0 0 1-1 0V8.9Zm-.25-2.16a.75.75 0 1 1 1.5 0 .75.75 0 0 1-1.5 0Z\"/>") { } }

            public class Warning : Icon { public Warning() : base("Warning", IconVariant.Filled, IconSize.Size20, "<path d=\"M8.68 2.79a1.5 1.5 0 0 1 2.64 0l6.5 12A1.5 1.5 0 0 1 16.5 17h-13a1.5 1.5 0 0 1-1.32-2.21l6.5-12ZM10.5 7.5a.5.5 0 0 0-1 0v4a.5.5 0 0 0 1 0v-4Zm.25 6.25a.75.75 0 1 0-1.5 0 .75.75 0 0 0 1.5 0Z\"/>") { } }

            public class CheckboxChecked : Icon { public CheckboxChecked() : base("CheckboxChecked", IconVariant.Filled, IconSize.Size20, "<path d=\"M6 3a3 3 0 0 0-3 3v8a3 3 0 0 0 3 3h8a3 3 0 0 0 3-3V6a3 3 0 0 0-3-3H6Zm7.85 4.85-5 5a.5.5 0 0 1-.7 0l-2-2a.5.5 0 0 1 .7-.7l1.65 1.64 4.65-4.64a.5.5 0 0 1 .7.7Z\" />") { } }

            public class CheckboxIndeterminate : Icon { public CheckboxIndeterminate() : base("CheckboxIndeterminate", IconVariant.Filled, IconSize.Size20, "<path d=\"M6 3a3 3 0 0 0-3 3v8a3 3 0 0 0 3 3h8a3 3 0 0 0 3-3V6a3 3 0 0 0-3-3H6ZM4.5 6c0-.83.67-1.5 1.5-1.5h8c.83 0 1.5.67 1.5 1.5v8c0 .83-.67 1.5-1.5 1.5H6A1.5 1.5 0 0 1 4.5 14V6ZM7 6a1 1 0 0 0-1 1v6a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V7a1 1 0 0 0-1-1H7Z\"></path><path d=\"M6 3a3 3 0 0 0-3 3v8a3 3 0 0 0 3 3h8a3 3 0 0 0 3-3V6a3 3 0 0 0-3-3H6ZM4.5 6c0-.83.67-1.5 1.5-1.5h8c.83 0 1.5.67 1.5 1.5v8c0 .83-.67 1.5-1.5 1.5H6A1.5 1.5 0 0 1 4.5 14V6ZM7 6a1 1 0 0 0-1 1v6a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V7a1 1 0 0 0-1-1H7Z\"></path>") { } }

            public class PresenceAvailable : Icon { public PresenceAvailable() : base("PresenceAvailable", IconVariant.Filled, IconSize.Size20, "<path d=\"M10 20a10 10 0 1 0 0-20 10 10 0 0 0 0 20Zm4.2-11.8-4.5 4.5a1 1 0 0 1-1.4 0l-2-2a1 1 0 1 1 1.4-1.4L9 10.58l3.8-3.8a1 1 0 1 1 1.4 1.42Z\"/>") { } }

            public class PresenceAway : Icon { public PresenceAway() : base("PresenceAway", IconVariant.Filled, IconSize.Size20, "<path d=\"M10 20a10 10 0 1 0 0-20 10 10 0 0 0 0 20Zm0-14V9.6l2.7 2.7a1 1 0 0 1-1.4 1.42l-3-3A1 1 0 0 1 8 10V6a1 1 0 1 1 2 0Z\"/>") { } }

            public class PresenceBusy : Icon { public PresenceBusy() : base("PresenceBusy", IconVariant.Filled, IconSize.Size20, "<path d=\"M20 10a10 10 0 1 1-20 0 10 10 0 0 1 20 0Z\"/>") { } }

            public class PresenceDnd : Icon { public PresenceDnd() : base("PresenceDnd", IconVariant.Filled, IconSize.Size20, "<path d=\"M10 20a10 10 0 1 0 0-20 10 10 0 0 0 0 20ZM7 9h6a1 1 0 1 1 0 2H7a1 1 0 1 1 0-2Z\"/>") { } }

            public class RadioButton : Icon { public RadioButton() : base("RadioButton", IconVariant.Regular, IconSize.Size20, "<path d=\"M10 15a5 5 0 1 0 0-10 5 5 0 0 0 0 10Zm0-13a8 8 0 1 0 0 16 8 8 0 0 0 0-16Zm-7 8a7 7 0 1 1 14 0 7 7 0 0 1-14 0Z\" />") { } };

            public class Star : Icon { public Star() : base("Star", IconVariant.Filled, IconSize.Size20, "<path d=\"M9.1 2.9a1 1 0 0 1 1.8 0l1.93 3.91 4.31.63a1 1 0 0 1 .56 1.7l-3.12 3.05.73 4.3a1 1 0 0 1-1.45 1.05L10 15.51l-3.86 2.03a1 1 0 0 1-1.45-1.05l.74-4.3L2.3 9.14a1 1 0 0 1 .56-1.7l4.31-.63L9.1 2.9Z\"></path>") { } };
        }
    }
}
