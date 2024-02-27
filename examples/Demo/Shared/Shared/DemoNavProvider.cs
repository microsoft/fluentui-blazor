// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Microsoft.AspNetCore.Components.Routing;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared;

public class DemoNavProvider
{
    public IReadOnlyList<NavItem> NavMenuItems { get; init; }

    public IReadOnlyList<NavItem> FlattenedMenuItems { get; init; }

    public DemoNavProvider()
    {
        NavMenuItems =
        [
            new NavLink(
                href: "/",
                match: NavLinkMatch.All,
                icon: new Icons.Regular.Size20.Home(),
                title: "Home"
            ),

            new NavGroup(
                icon: new Icons.Regular.Size20.PersonRunning(),
                title: "Getting Started",
                expanded: true,
                gap: "10px",
                children:
                [
                    new NavLink(
                        href: "/WhatsNew",
                        icon: new Icons.Regular.Size20.Info(),
                        title: "What's new"
                    ),

                    new NavLink(
                        href: "/UpgradeGuide",
                        icon: new Icons.Regular.Size20.ArrowUp(),
                        title: "Upgrade guide"
                    ),

                    new NavLink(
                        href: "/CodeSetup",
                        icon: new Icons.Regular.Size20.DocumentOnePageSparkle(),
                        title: "Code setup"
                    ),

                    new NavLink(
                        href: "/Templates",
                        icon: new Icons.Regular.Size20.Classification(),
                        title: "Project templates"
                    ),

                    new NavLink(
                        href: "/DesignTheme",
                        icon: new Icons.Regular.Size20.DarkTheme(),
                        title: "Themes"
                    ),

                    new NavLink(
                        href: "/DesignTokens",
                        icon: new Icons.Regular.Size20.DesignIdeas(),
                        title: "Design tokens"
                    ),

                    new NavLink(
                        href: "/Reboot",
                        icon: new Icons.Regular.Size20.ArrowReset(),
                        title: "Reboot"
                    ),

                    new NavLink(
                        href: "/IconsAndEmoji",
                        icon: new Icons.Regular.Size20.Symbols(),
                        title: "Icons and Emoji"
                    ),

                    new NavGroup(
                        icon: new Icons.Regular.Size20.SettingsCogMultiple(),
                        title: "Services",
                        expanded: false,
                        gap: "10px",
                        children:
                        [
                            new NavLink(
                                href: "/DialogService",
                                icon: new Icons.Regular.Size20.AppGeneric(),
                                title: "DialogService"
                            ),

                            new NavLink(
                                href: "/MessageService",
                                icon: new Icons.Regular.Size20.WindowHeaderHorizontal(),
                                title: "MessageService"
                            ),

                            new NavLink(
                                href: "/ToastService",
                                icon: new Icons.Regular.Size20.FoodToast(),
                                title: "ToastService"
                            )
                        ]
                    )
                ]
            ),
            new NavGroup(
                icon: new Icons.Regular.Size20.ContentViewGallery(),
                title: "Layout",
                expanded: false,
                gap: "10px",
                children:
                [
                    new NavLink(
                        href: "/Header",
                        icon: new Icons.Regular.Size20.DocumentHeader(),
                        title: "Header"
                    ),
                    new NavLink(
                        href: "/Footer",
                        icon: new Icons.Regular.Size20.DocumentFooter(),
                        title: "Footer"
                    ),
                    new NavLink(
                        href: "/BodyContent",
                        icon: new Icons.Regular.Size20.ContentViewGallery(),
                        title: "BodyContent"
                    ),
                    new NavLink(
                        href: "/Grid",
                        icon: new Icons.Regular.Size20.Grid(),
                        title: "Grid"
                    ),
                    new NavLink(
                        href: "/Layout",
                        icon: new Icons.Regular.Size20.SlideLayout(),
                        title: "Layout"
                    ),
                    new NavLink(
                        href: "/MainLayout",
                        icon: new Icons.Regular.Size20.MatchAppLayout(),
                        title: "MainLayout"
                    ),
                    new NavLink(
                        href: "/Spacer",
                        icon: new Icons.Regular.Size20.Spacebar(),
                        title: "Spacer"
                    ),
                    new NavLink(
                        href: "/Splitter",
                        icon: new Icons.Regular.Size20.SplitVertical(),
                        title: "Splitter"
                    ),
                    new NavLink(
                        href: "/Stack",
                        icon: new Icons.Regular.Size20.Stack(),
                        title: "Stack"
                    )
                ]),
            new NavGroup(
                icon: new Icons.Regular.Size20.Form(),
                title: "Form & Inputs",
                expanded: false,
                gap: "10px",
                children:
                [
                    new NavLink(
                        href: "/Forms",
                        icon: new Icons.Regular.Size20.Form(),
                        title: "Overview"
                    ),
                    new NavLink(
                        href: "/Checkbox",
                        icon: new Icons.Regular.Size20.CheckboxChecked(),
                        title: "Checkbox"
                    ),
                    new NavLink(
                        href: "/InputFile",
                        icon: new Icons.Regular.Size20.ArrowUpload(),
                        title: "InputFile"
                    ),
                    new NavLink(
                        href: "/NumberField",
                        icon: new Icons.Regular.Size20.NumberSymbolSquare(),
                        title: "Number Field"
                    ),
                    new NavLink(
                        href: "/Radio",
                        icon: new Icons.Regular.Size20.RadioButton(),
                        title: "Radio"
                    ),
                    new NavLink(
                        href: "/RadioGroup",
                        icon: new Icons.Regular.Size20.RadioButton(),
                        title: "Radio Group"
                    ),
                    new NavLink(
                        href: "/Search",
                        icon: new Icons.Regular.Size20.SearchSquare(),
                        title: "Search"
                    ),
                    new NavLink(
                        href: "/Slider",
                        icon: new Icons.Regular.Size20.Options(),
                        title: "Slider"
                    ),
                    new NavLink(
                        href: "/Switch",
                        icon: new Icons.Regular.Size20.ToggleLeft(),
                        title: "Switch"
                    ),
                    new NavLink(
                        href: "/TextArea",
                        icon: new Icons.Regular.Size20.TextboxMore(),
                        title: "TextArea"
                    ),
                    new NavLink(
                        href: "/TextField",
                        icon: new Icons.Regular.Size20.Textbox(),
                        title: "Text Field"
                    ),
                    new NavLink(
                        href: "/DateTime#fluenttimepicker-class",
                        icon: new Icons.Regular.Size20.Clock(),
                        title: "Time picker"
                    )
                ]
            ),

            new NavGroup(
                icon: new Icons.Regular.Size20.PuzzleCubePiece(),
                title: "Components",
                expanded: false,
                gap: "10px",
                children:
                [
                    new NavLink(
                        href: "/FluentComponentBase",
                        icon: new Icons.Regular.Size20.PuzzleCubePiece(),
                        title: "Overview"
                    ),
                    new NavLink(
                        href: "/Accordion",
                        icon: new Icons.Regular.Size20.TextCollapse(),
                        title: "Accordion"
                    ),
                    new NavLink(
                        href: "/Anchor",
                        icon: new Icons.Regular.Size20.Link(),
                        title: "Anchor"
                    ),
                    new NavLink(
                        href: "/AnchoredRegion",
                        icon: new Icons.Regular.Size20.LinkSquare(),
                        title: "Anchored Region"
                    ),
                    new NavGroup(
                        title: "Badge",
                        expanded: true,
                        gap: "10px",
                        icon: new Icons.Regular.Size20.Tag(),
                        children:
                        [
                            new NavLink(
                                href: "/Badge",
                                icon: new Icons.Regular.Size20.Badge(),
                                title: "Badge"
                            ),
                            new NavLink(
                                href: "/CounterBadge",
                                icon: new Icons.Regular.Size20.NumberCircle1(),
                                title: "CounterBadge"
                            ),
                            new NavLink(
                                href: "/PresenceBadge",
                                icon: new Icons.Regular.Size20.PresenceAvailable(),
                                title: "PresenceBadge"
                            )
                        ]
                    ),
                    new NavLink(
                        href: "/Breadcrumb",
                        icon: new Icons.Regular.Size20.DocumentChevronDouble(),
                        title: "Breadcrumb"
                    ),
                    new NavLink(
                        href: "/Button",
                        icon: new Icons.Regular.Size20.ControlButton(),
                        title: "Button"
                    ),
                    new NavLink(
                        href: "/Card",
                        icon: new Icons.Regular.Size20.ContactCardGroup(),
                        title: "Card"
                    ),
                    new NavLink(
                        href: "/DataGrid",
                        icon: new Icons.Regular.Size20.Grid(),
                        title: "Data grid"
                    ),
                    new NavLink(
                        href: "/DateTime",
                        icon: new Icons.Regular.Size20.CalendarLtr(),
                        title: "Date & Time"
                    ),
                    new NavLink(
                        href: "/Dialog",
                        icon: new Icons.Regular.Size20.AppGeneric(),
                        title: "Dialog"
                    ),
                    new NavLink(
                        href: "/Divider",
                        icon: new Icons.Regular.Size20.DividerShort(),
                        title: "Divider"
                    ),
                    new NavLink(
                        href: "/Drag",
                        icon: new Icons.Regular.Size20.Drag(),
                        title: "Drag and Drop"
                    ),
                    new NavLink(
                        href: "/Emoji",
                        icon: new Icons.Regular.Size20.EmojiSmileSlight(),
                        title: "Emoji"
                    ),
                    new NavLink(
                        href: "/Flipper",
                        icon: new Icons.Regular.Size20.Navigation(),
                        title: "Flipper"
                    ),
                    new NavLink(
                        href: "/Highlighter",
                        icon: new Icons.Regular.Size20.Highlight(),
                        title: "Highlighter"
                    ),
                    new NavLink(
                        href: "/HorizontalScroll",
                        icon: new Icons.Regular.Size20.ArrowForward(),
                        title: "Horizontal Scroll"
                    ),
                    new NavLink(
                        href: "/Icon",
                        icon: new Icons.Regular.Size20.Symbols(),
                        title: "Icon"
                    ),
                    new NavLink(
                        href: "/Label",
                        icon: new Icons.Regular.Size20.DoorTag(),
                        title: "Label"
                    ),
                    new NavGroup(
                        title: "List",
                        expanded: true,
                        gap: "10px",
                        icon: new Icons.Regular.Size20.List(),
                        children:
                        [
                            new NavLink(
                                href: "/Autocomplete",
                                icon: new Icons.Regular.Size20.ArrowAutofitContent(),
                                title: "Autocomplete"
                            ),
                            new NavLink(
                                href: "/Combobox",
                                icon: new Icons.Regular.Size20.BoxEdit(),
                                title: "Combobox"
                            ),
                            new NavLink(
                                href: "/Listbox",
                                icon: new Icons.Regular.Size20.List(),
                                title: "Listbox"
                            ),
                            new NavLink(
                                href: "/Select",
                                icon: new Icons.Regular.Size20.GroupList(),
                                title: "Select"
                            ),
                            new NavLink(
                                href: "/Option",
                                icon: new Icons.Regular.Size20.MultiselectRtl(),
                                title: "Option"
                            )
                        ]
                    ),
                    new NavLink(
                        href: "/Menu",
                        icon: new Icons.Regular.Size20.Navigation(),
                        title: "Menu"
                    ),
                    new NavLink(
                        href: "/MenuButton",
                        icon: new Icons.Regular.Size20.ChevronCircleDown(),
                        title: "MenuButton"
                    ),
                    new NavLink(
                        href: "/MessageBar",
                        icon: new Icons.Regular.Size20.WindowHeaderHorizontal(),
                        title: "MessageBar"
                    ),
                    new NavLink(
                        href: "/MessageBox",
                        icon: new Icons.Regular.Size20.MegaphoneLoud(),
                        title: "MessageBox"
                    ),
                    new NavLink(
                        href: "/NavMenu",
                        icon: new Icons.Regular.Size20.Navigation(),
                        title: "NavMenu"
                    ),
                    new NavLink(
                        href: "/NavMenuTree",
                        icon: new Icons.Regular.Size20.Navigation(),
                        title: "NavMenuTree"
                    ),
                    new NavLink(
                        href: "/Overflow",
                        icon: new Icons.Regular.Size20.MultiselectRtl(),
                        title: "Overflow"
                    ),
                    new NavLink(
                        href: "/Overlay",
                        icon: new Icons.Regular.Size20.CursorHover(),
                        title: "Overlay"
                    ),
                    new NavLink(
                        href: "/Panel",
                        icon: new Icons.Regular.Size20.PanelRight(),
                        title: "Panel"
                    ),
                    new NavLink(
                        href: "/Persona",
                        icon: new Icons.Regular.Size20.PersonAvailable(),
                        title: "Persona"
                    ),
                    new NavLink(
                        href: "/Popover",
                        icon: new Icons.Regular.Size20.TooltipQuote(),
                        title: "Popover"
                    ),
                    new NavLink(
                        href: "/Progress",
                        icon: new Icons.Regular.Size20.SquareHint(),
                        title: "Progress"
                    ),
                    new NavLink(
                        href: "/ProgressRing",
                        icon: new Icons.Regular.Size20.ArrowClockwiseDashes(),
                        title: "Progress Ring"
                    ),
                    new NavLink(
                        href: "/Skeleton",
                        icon: new Icons.Regular.Size20.Shortpick(),
                        title: "Skeleton"
                    ),
                    new NavLink(
                        href: "/SortableList",
                        icon: new Icons.Regular.Size20.ArrowSort(),
                        title: "Sortable List"
                    ),
                    new NavLink(
                        href: "/SplashScreen",
                        icon: new Icons.Regular.Size20.Drop(),
                        title: "SplashScreen"
                    ),
                    new NavLink(
                        href: "/Tabs",
                        icon: new Icons.Regular.Size20.TabDesktop(),
                        title: "Tabs"
                    ),
                    new NavLink(
                        href: "/Toast",
                        icon: new Icons.Regular.Size20.FoodToast(),
                        title: "Toast"
                    ),
                    new NavLink(
                        href: "/Toolbar",
                        icon: new Icons.Regular.Size20.WrenchScrewdriver(),
                        title: "Toolbar"
                    ),
                    new NavLink(
                        href: "/Tooltip",
                        icon: new Icons.Regular.Size20.TooltipQuote(),
                        title: "Tooltip"
                    ),
                    new NavLink(
                        href: "/TreeView",
                        icon: new Icons.Regular.Size20.TextBulletListTree(),
                        title: "Tree View"
                    ),
                    new NavLink(
                        href: "/new Icons.Regular.Size20.ArrowStepInRight()",
                        icon: new Icons.Regular.Size20.TextBulletListTree(),
                        title: "Wizard"
                    )
                ]),

            new NavGroup(
                icon: new Icons.Regular.Size20.Beaker(),
                title: "Incubation lab",
                expanded: false,
                gap: "10px",
                children:
                [
                    new NavLink(
                        href: "/Lab/Overview",
                        icon: new Icons.Regular.Size20.Beaker(),
                        title: "Overview"
                    ),

                    new NavLink(
                        href: "/Lab/MarkdownSection",
                        icon: new Icons.Regular.Size20.ArrowSortDown(),
                        title: "MarkdownSection"
                    ),

                    new NavLink(
                        href: "/Lab/TableOfContents",
                        icon: new Icons.Regular.Size20.DocumentTextLink(),
                        title: "TableOfContents"
                    ),

                    new NavLink(
                        href: "/MultiSplitter",
                        icon: new Icons.Regular.Size20.SplitHorizontal(),
                        title: "Multi Splitter"
                    ),

                    new NavLink(
                        href: "/KeyCode",
                        icon: new Icons.Regular.Size20.Keyboard(),
                        title: "KeyCode"
                    )
                ]
            )
        ];

        FlattenedMenuItems = GetFlattenedMenuItems(NavMenuItems)
            .ToList()
            .AsReadOnly();
    }

    private static IEnumerable<NavItem> GetFlattenedMenuItems(IEnumerable<NavItem> items)
    {
        foreach (var item in items)
        {
            yield return item;

            if (item is not NavGroup group || !group.Children.Any())
            {
                continue;
            }

            foreach (var flattenedMenuItem in GetFlattenedMenuItems(group.Children))
            {
                yield return flattenedMenuItem;
            }
        }
    }
}
