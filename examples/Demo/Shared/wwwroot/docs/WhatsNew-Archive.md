## V3.2.2
- Fix [#859](https://github.com/microsoft/fluentui-blazor/issues/859) and [#884](https://github.com/microsoft/fluentui-blazor/issues/859): Fix NavMenu for real by moving `<a>` tag up in the rendering tree

### Update Fluent UI System icons to 1.1.221
**What's new (Name / Size(s) / Variant(s))**
- Arrow Download Off / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Border Inside / 16, 20, 24 / Filled & Regular
- Chat Lock / 16, 20, 24, 28 / Filled & Regular
- Error Circle / 48 / Filled & Regular
- Full Screen Maximize / 28, 32 / Filled & Regular
- Full Screen Minimize / 28, 32 / Filled & Regular
- Link Person / 16, 20, 24, 32, 48 / Filled & Regular
- People Chat / 16, 20, 24 / Filled & Regular
- Person Support / 28 / Filled & Regular
- Shapes / 32 / Filled & Regular
- Slide Text Edit / 16, 20, 24, 28 / Filled & Regular
- Subtract Circle / 48 / Filled & Regular
- Subtract Parentheses / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Warning / 48 / Filled & Regular

**What's updated (Name / Size(s) / Variant(s))**
- Border None / 16 / Filled & Regular
- Flag Off / 48 / Filled & Regular
- Person Support / 16, 20, 24 / Filled & Regular
- Shapes / 28 / Filled & Regular
- Subtract Circle / 16, 32 / Filled & Regular

## V3.2.1
- Design Tokens WithDefault method implemented to allow setting a default value for a token. This is technically a breaking chage, but it is unlikely to affect anyone as the previous implementation did not do anything. See the `SiteSettingsPanel.razor.cs` in the demo site for an implementation example.
- Fix [#872](https://github.com/microsoft/fluentui-blazor/issues/872): A11y issue in FluentDivider
- Fix [#864](https://github.com/microsoft/fluentui-blazor/issues/864): A11y issue in FluentAutoComplete
- Fix [#861](https://github.com/microsoft/fluentui-blazor/issues/861): A11y issue in FluentDataGrid EmptyContent
- Fix [#848](https://github.com/microsoft/fluentui-blazor/issues/848): A11y issue in FluentDialogHeader
- Fix [#847](https://github.com/microsoft/fluentui-blazor/issues/847): A11y issues in FluentNavMenu
- Fix [#861](https://github.com/microsoft/fluentui-blazor/issues/861): A11y issue in FluentDataGrid EmptyContent
- Fix [#859](https://github.com/microsoft/fluentui-blazor/issues/859): Fix FluentNavMenuGroup not clickable ouside of <a> tag
- Fix [#857](https://github.com/microsoft/fluentui-blazor/issues/857): Fix copy Emoji code in search page
- Fix [#841](https://github.com/microsoft/fluentui-blazor/issues/841): Overflow causes loss of background with Panel
- Fix [#833](https://github.com/microsoft/fluentui-blazor/issues/833): Use correct abbreviation for day name in FluentCalendar (Chinese)
- Miscelaneous documentation and typo fixes
- Add more unit tests
- Update to FluentUI System Icons 1.1.220

- **What's new (Name / Size(s) / Variant(s))**
- Arrow Download / 28, 32 / Filled & Regular
- Arrow Expand / 16 / Filled & Regular
- Arrow Export Up / 16 / Filled & Regular
- Arrow Import / 16 / Filled & Regular
- Arrow Up Right Dashes / 16 / Filled & Regular
- Battery 10 / 16 / Filled & Regular
- Beaker Empty / 16 / Filled & Regular
- Book / 16 / Filled & Regular
- Border None / 16 / Filled & Regular
- Branch Request / 16 / Filled & Regular
- Clipboard Task List / 16 / Filled & Regular
- Cut / 16 / Filled & Regular
- Folder Search / 16, 20, 24 / Filled & Regular
- Hexagon / 28, 32, 48 / Filled & Regular
- Plug Connected / 16 / Filled & Regular
- Plug Disconnected / 16 / Filled & Regular
- Projection Screen Text / 20 / Filled & Regular
- RSS / 16 / Filled & Regular
- Shape Organic / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Teardrop Bottom Right / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Text Asterisk / 16 / Filled & Regular
- Text Edit Style / 16 / Filled & Regular
- Text Whole Word / 16 / Filled & Regular
- Triangle / 24, 28 / Filled & Regular
  
**What's updated (Name / Size(s) / Variant(s))**
- Arrow Bidirectional Left Right / 16 / Filled & Regular
- Arrow Download / 24, 48 / Filled & Regular
- Beaker Edit / 20 / Filled & Regular
- Beaker Off / 20 / Filled & Regular
- Beaker Settings / 20 / Filled & Regular
- Clipboard Letter / 24 / Filled & Regular
- Prohibited / 16, 24, 28, 48 / Filled & Regular 

## V3.2.0
- New NavMenu, NavGroup and NavLink components. **Breaking change** - See the [Upgrade guide](https://www.fluentui-blazor.net/UpgradeGuide) for details. See [NavMenu](https://www.fluentui-blazor.net/NavMenu) page for examples.
- New FluentInputLabel component. 
- FluentCard: Add AreaRestricted parameter to allow content to break out of card area.
- Provide error message when FluentDialogProvider missing
- It is now possible to add a tooltip to DataGridColumns
- 
- Fix [#796](https://github.com/microsoft/fluentui-blazor/pull/796): Fix IconColor doc page
- Fix [#797](https://github.com/microsoft/fluentui-blazor/pull/797): Fix MessageBar issues
- Fix [#805](https://github.com/microsoft/fluentui-blazor/pull/805): InlineStyleBuilder on .NET6
- Fix [#810](https://github.com/microsoft/fluentui-blazor/pull/810): FluentDataGrid error if page is quickly exited
- Fix [#815](https://github.com/microsoft/fluentui-blazor/pull/815): Manual upload on iOS
- Fix [#828](https://github.com/microsoft/fluentui-blazor/pull/828): FluentSelect: Fix ValueChanged fired twice
- Fix [#801](https://github.com/microsoft/fluentui-blazor/pull/801): Remove direct call to Items.Count()
- Fix [#834](https://github.com/microsoft/fluentui-blazor/pull/834): Chinese abbreviated day name in FluentCalendar
- Fix [#836](https://github.com/microsoft/fluentui-blazor/pull/836): Setting SelectedOptions does not update FluentSelct

## V3.1.1
- Fix [#776](https://github.com/microsoft/fluentui-blazor/issues/776): Icon throws exception when deployed to Azure
- Fix [#755](https://github.com/microsoft/fluentui-blazor/issues/755): Icon throws exception when deployed to Azure
- Fix [#789](https://github.com/microsoft/fluentui-blazor/issues/789): Navigation to "/" crashes with FluentNavMenu
- Fix [#780](https://github.com/microsoft/fluentui-blazor/issues/780): 'OK' button rendered outside of panel for Site settings
Also addresses some other padding and layout issues with FluentDialog variants introduced in 3.1.0
- Feature [#782](https://github.com/microsoft/fluentui-blazor/issues/782) Add ability to prevent dismissing a modal dialog by clicking on the overlay
- Added `Tooltip` and `Style` parameters to DataGrid's `PropertyColumn` and `TemplateColumn`. See [DataGrid](https://www.fluentui-blazor.net) page for examples.
- Add Icon.WithColor and update the Demo page
- Update Button Icon Color depending of Button Appearance
- Update the way to apply the Icon.Color in a Button

## V3.1.

### New components
* `FluentAutoComplete`
* `FluentPersona`
* `FluentMessageBar`

### What's Changed

#### Components
* [FluentButton] Update the Button.Loading when using with a Icon by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/752
* [FluentCodeEditor] update docs by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/692
* [FluentCombobox, FluentSelect, FluentListbox] Add Width and Height property and associated styles by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/663
* [FluentDataGrid] Add `EmptyContent` parameter by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/731
* [FluentDataGrid] multiline-text word-break by @ssccinng in https://github.com/microsoft/fluentui-blazor/pull/711
* [FluentDataGridCell, FluentDataGridRow] Add item reference (#695) by @hksalessio in https://github.com/microsoft/fluentui-blazor/pull/700
* [FluentDataGridRow] Update for sticky header and example by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/747
* [FluentDatePicker and FluentTimePicker] Keep existing time/date by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/726
* [FluentDatePicker] Update the popup position by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/724
* [FluentDialog] Dialog enhancements (add dialog type, fix panel width setable), 
* [FluentDialog] Enhancements by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/717
* [FluentDialog] Include components to customize the Dialog-box by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/764
* [FluentDialog] Make all DialogService.Show..Async methods return IDialogReference by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/694
* [FluentMenu/FluentPopover/FluentOverlay] Close menu/popover automatically when user clicks outside of it (by using overlay) by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/729
* [FluentNavMenu] Fix route/subroute not selecting correct item by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/755
* [FluentOptionPeople] Add ChildContent by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/728
* [FluentOverflow] Fix the Tooltip used by the FluentOverflow by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/678
* [FluentPresenceBadge  & FluentOptionPeople] - Update the Status badge to be compliant FluentUI 2 by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/732
* [FluentProgressToast] Changes by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/657
* [FluentTreeItem] Fix select area when Text property is used by @ProH4Ck in https://github.com/microsoft/fluentui-blazor/pull/667
* [Multiple] Add `PreventScroll` to Dialog and Overlay by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/758
* [Multiple] Apply the Immediate property to "text" components by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/696
* [Multiple] Set TextField.AutoComplete property to string type by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/682

#### General
* [CssBuilder and StyleBuilder] Fixing the built format and position of the custom styles by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/745
* [Documentation] Add UnitTests markdown page by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/658
* [Documentation] Update the Upgrade Guide and details on Icons by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/673
* Add MaxHeight to DemoSection by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/740
* Fix #621  by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/683
* Fix #674 by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/676
* Fix Icons and Emojis trimmed during the WASM publishing by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/666
* fix typos by @LuohuaRain in https://github.com/microsoft/fluentui-blazor/pull/751
* Re-add scripts to index.html/_Layout.cshtml, Update docs by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/697
* Rewrite the Icons.AllIcons property using relfection by @dvoituron in https://github.com/microsoft/fluentui-blazor/pull/685
* Update CodeSetup.md by @LuohuaRain in https://github.com/microsoft/fluentui-blazor/pull/730
* Update Fluent UI System Icons to 1.1.217  by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/765

#### Templates
* Update templates by @vnbaaij in https://github.com/microsoft/fluentui-blazor/pull/688
* Add missing usings Project template by @agriffard in https://github.com/microsoft/fluentui-blazor/pull/702

#### New Contributors
* @agriffard made their first contribution in https://github.com/microsoft/fluentui-blazor/pull/702
* @hksalessio made their first contribution in https://github.com/microsoft/fluentui-blazor/pull/700
* @ssccinng made their first contribution in https://github.com/microsoft/fluentui-blazor/pull/711

Thanks to all contributors!

## V3.0.1
- Fix [#643](https://github.com/microsoft/fluentui-blazor/issues/643)**: Visual Studio 2022 freezes and then crashes with v3-RC-1
- Fix [#684](https://github.com/microsoft/fluentui-blazor/pull/684): Make FluentToastContainer work better on mobile devices
- Fix [#621](https://github.com/microsoft/fluentui-blazor/issues/621): Add area-hidden to FluentSearch clear button
- Fix [#674](https://github.com/microsoft/fluentui-blazor/issues/674): Collapse FluentNavMenuGroup on click if no custom action/href is assigned
- Fix [#680](https://github.com/microsoft/fluentui-blazor/issues/680): FluentTextField AutoComplete should be a string or enum and not a boolean 
- Fix [#668](https://github.com/microsoft/fluentui-blazor/issues/668): v3.0.0 IIS debugging/deployments 'Failed to load resource'
- Fix [#677](https://github.com/microsoft/fluentui-blazor/issues/677): FluentOverflow not working correctly
- Fix [#667](https://github.com/microsoft/fluentui-blazor/issues/667): FluentTreeItem select area when Text property is used
- Fix [#660](https://github.com/microsoft/fluentui-blazor/issues/660): Allow FluentMessageBox Text to not show an Icon
- Fix [#624](https://github.com/microsoft/fluentui-blazor/issues/624): v3 Unhandled exception rendering in NavMenuContent of FluentMainLayout
- Fix [#655](https://github.com/microsoft/fluentui-blazor/pull/655): ProgressToast rendering error for WASM

**) To fix this issue update Microsoft.Fast.Components.FluentUI.Icons v3.0.1 

### Other changes
- FluentCombobox, FluentSelect and FluentListbox now support Width and Height properties
- Update several documentation pages about how to use Icons and Emoji
- Included the script needed to implement Blazor Hybrid workaround easier (see readme)

## V3.0.0
** 26 New components**
- Header
- Footer
- BodyContent
- Grid
- Layout
- MainLayout
- Spacer
- Splitter
- Stack
- CounterBadge
- PresenceBadge
- CodeEditor
- Date & Time (Calendar, DatePicker, TimePicker)
- Dialog
- DragDrop
- Highlighter
- Label
- MessageBox
- Overflow
- Overlay
- Panel
- Popover
- SplashScreen
- Toast


**Completely redone components (and configuration)**
- Icon
- Emoji

**Other changes**
- Added initializersLoader.webview.js to work around Blazor Hybrid bug (see readme)

## 2.4.3
- Fix [#645](https://github.com/microsoft/fluentui-blazor/issues/645): FluentIcon sometimes  fails to render
- Fix [#644](https://github.com/microsoft/fluentui-blazor/issues/644): FluentDataGrid column resize isseu (thanks @konvolution)
- Update Fluent UI System Icons to 1.1.211 (from 1.1.204)

**What's new (Name / Size(s) / Variant(s))**
- Add Square Multiple / 24 / Filled & Regular
- Arrow Bidirectional Left Right / 16, 20, 24, 28 / Filled & Regular
- Arrow Outline Down Left / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Arrow Step In Diagonal Down Left / 16, 20, 24, 28 / Filled & Regular
- Arrow Swap / 16, 28 / Filled & Regular
- Arrow Up Square Settings / 24 / Filled & Regular
- Braces Variable / 48 / Filled & Regular
- Briefcase Person / 24 / Filled & Regular
- Building Cloud / 24 / Filled & Regular
- Building Mosque / 12, 16, 20, 24, 28, 32, 48 / Filled & Regular
- Calendar Eye / 20 / Filled & Regular
- Checkmark Circle Square / 16, 20, 24 / Filled & Regular
- Cube / 48 / Filled & Regular
- Desk / 16, 28, 32, 48 / Filled & Regular
- Folder Add / 32 / Filled & Regular
- Folder Arrow Left / 48 / Filled & Regular
- Folder Arrow Right / 32 / Filled & Regular
- Folder Arrow Up / 32 / Filled & Regular
- Folder Link / 16, 32 / Filled & Regular
- Folder Open Vertical / 24 / Filled & Regular
- Folder Prohibited / 32 / Filled & Regular
- Globe / 48 / Filled & Regular
- Globe Shield / 48 / Filled & Regular
- Hand Right Off / 16, 24, 28 / Filled & Regular
- Hat Graduation Sparkle / 16 / Filled & Regular
- Hat Graduation Sparkle / 20, 24, 28 / Filled & Regular
- Heart Off / 16, 20, 24 / Filled & Regular
- Hexagon / 16, 20 / Filled & Regular
- Hexagon Three / 16, 20 / Filled & Regular
- Key Multiple / 16, 24 / Filled & Regular
- Kiosk / 24 / Filled & Regular
- Laptop Multiple / 24 / Filled & Regular
- Line Horizontal 1 / 16, 24, 28 / Filled & Regular
- Line Horizontal 1 Dashes / 16, 20, 24, 28 / Filled & Regular
- Line Horizontal 2 Dashes Solid / 16, 20, 24, 28 / Filled & Regular
- Link Add / 24 / Filled & Regular
- Link Multiple / 16, 20, 24 / Filled & Regular
- Link Settings / 24 / Filled & Regular
- Lock Closed / 28, 48 / Filled & Regular
- Lock Open / 12, 32, 48 / Filled & Regular
- Mail Off / 16 / Filled & Regular
- Mic Record / 20, 24, 28 / Filled & Regular
- Open / 12 / Filled & Regular
- Pen Sparkle / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Person Edit / 48 / Filled & Regular
- Person Phone / 24 / Filled & Regular
- Phone Briefcase / 24 / Filled & Regular
- Phone Multiple / 24 / Filled & Regular
- Phone Multiple Settings / 24 / Filled & Regular
- Phone Person / 24 / Filled & Regular
- Phone Subtract / 24 / Filled & Regular
- Plug Disconnected / 48 / Filled & Regular
- Rectangle Landscape Hint Copy / 16, 20, 24 / Filled & Regular
- Remix Add / 16, 20, 24, 32 / Filled & Regular
- Script / 20, 24, 32 / Filled & Regular
- Server Link / 24 / Filled & Regular
- Stream / 48 / Filled & Regular
- Tab Desktop / 28 / Filled & Regular
- Tab Desktop Link / 16, 20, 24, 28 / Filled & Regular
- Table Arrow Up / 20, 24 / Filled & Regular
- Tablet Laptop / 24 / Filled & Regular
- Text Bullet List Square / 48 / Filled & Regular
- Text Bullet List Square Shield / 48 / Filled & Regular
- Video Person Sparkle Off / 20, 24 / Filled & Regular
- Warning / 32 / Filled & Regular
- Window Database / 32 / Filled & Regular
 

**What's updated (Name / Size(s) / Variant(s))**
- Add Square Multiple / 24 / Filled & Regular
- Arrow Outline Up Right / 32 / Filled & Regular
- Braces Variable / 48 / Filled & Regular
- Cast Multiple / 20, 24, 28 / Filled & Regular
- Circle Hint Half Vertical / 16, 20, 24 / Filled & Regular
- Cube / 48 / Filled & Regular
- Desk / 16, 28, 32, 48 / Filled & Regular
- Document Data / 16, 20, 24, 32 / Filled & Regular
- Document Data Link / 16, 20, 24, 32 / Filled & Regular
- Document Link / 20 / Filled & Regular
- Flash Sparkle / 20, 24 / Filled & Regular
- Folder Add / 16, 20, 24, 28, 48 / Filled & Regular
- Folder Arrow Left / 16, 20, 24, 28, 32 / Filled & Regular
- Folder Arrow Right / 16, 20, 24, 28, 48 / Filled & Regular
- Folder Arrow Up / 16, 20, 24, 28, 48 / Filled & Regular
- Folder Briefcase / 20 / Filled & Regular
- Folder Globe / 16, 20 / Filled & Regular
- Folder Lightning / 16, 20, 24 / Filled & Regular
- Folder Link / 20, 24, 28, 48 / Filled & Regular
- Folder List / 16, 20 / Filled & Regular
- Folder Mail / 16, 20, 24, 28 / Filled & Regular
- Folder Multiple / 16 / Filled & Regular
- Folder Open / 16, 20, 24 / Filled & Regular
- Folder Open / 16, 20, 24 / Filled & Regular
- Folder Open Vertical / 16, 20 / Filled & Regular
- Folder Open Vertical / 16, 20 / Filled & Regular
- Folder Open Vertical / 24 / Filled & Regular
- Folder People / 20, 24 / Filled & Regular
- Folder Person / 16, 20 / Filled & Regular
- Folder Prohibited / 16, 20, 24, 28, 48 / Filled & Regular
- Folder Swap / 16, 20, 24 / Filled & Regular
- Folder Sync / 16, 20, 24 / Filled & Regular
- Folder Zip / 16, 20, 24 / Filled & Regular
- Globe / 48 / Filled & Regular
- Globe Shield / 48 / Filled & Regular
- Hand Right Off / 16, 24, 28 / Filled & Regular
- Hat Graduation Sparkle / 16 / Filled & Regular
- Hexagon / 12, 24 / Filled & Regular
- Hexagon Three / 12, 24 / Filled & Regular
- Key Multiple / 16, 24 / Filled & Regular
- Line Style / 24 / Filled & Regular
- Link Multiple / 16, 20, 24 / Filled & Regular
- Lock Closed / 12, 16, 20, 24, 32 / Filled & Regular
- Lock Open / 16, 20, 24, 28 / Filled & Regular
- Mail Off / 16 / Filled & Regular
- Next Frame / 20, 24 / Filled & Regular
- Open / 24 / Filled
- Open Off / 24 / Filled
- Person Edit / 48 / Filled & Regular
- Plug Disconnected / 48 / Filled & Regular
- Previous Frame / 20, 24 / Filled & Regular
- Stream / 48 / Filled & Regular
- Text Bullet List Square / 48 / Filled & Regular
- Text Bullet List Square Shield / 48 / Filled & Regular
- TextBox / 16 / Filled & Regular
- TextBox Align Bottom / 20, 24 / Filled & Regular
- TextBox Align Bottom Center / 16, 20, 24 / Filled & Regular
- TextBox Align Bottom Left / 16, 20, 24 / Filled & Regular
- TextBox Align Bottom Right / 16, 20, 24 / Filled & Regular
- TextBox Align Bottom Rotate 90 / 20, 24 / Filled & Regular
- TextBox Align Center / 16 / Filled & Regular
- TextBox Align Center / 24 / Filled & Regular
- TextBox Align Middle / 20, 24 / Filled
- TextBox Align Middle Left / 16, 20, 24 / Filled & Regular
- TextBox Align Middle Right / 16, 20, 24 / Filled & Regular
- TextBox Align Middle Rotate 90 / 20, 24 / Filled & Regular
- TextBox Align Top / 20, 24 / Filled
- TextBox Align Top Center / 16, 20, 24 / Filled & Regular
- TextBox Align Top Left / 16, 20, 24 / Filled & Regular
- TextBox Align Top Right / 16, 20, 24 / Filled & Regular
- TextBox Align Top Rotate 90 / 20, 24 / Filled & Regular
- Triangle Down / 24 / Filled & Regular
- What’s updated
- Window Arrow Up / 16, 20, 24 / Filled & Regular

🏳️‍🌈 Happy Pride 🏳️‍🌈
Note we've revamped our Flag Pride icon to reflect the Traditional Pride Flag,
and added other umbrella flags:

- Flag Pride  / 32 / Filled
- Flag Pride Philadelphia / 16, 20, 24, 28, 32, 48 / Filled
- Flag Pride Progress / 16, 20, 24, 28, 32, 48 / Filled
- Flag Pride Intersex Inclusive Progress / 16, 20, 24, 28, 32, 48 / Filled

## V2.4.2
- Fix [#72](https://github.com/microsoft/fluentui-blazor/pull/72): rows attribute in TextArea.
- Fix [#466](https://github.com/microsoft/fluentui-blazor/pull/466): Divider vertical orientation.

## V2.4.1
- Fix [#537](https://github.com/microsoft/fluentui-blazor/issues/537): Make SetTotalItemCountAsync in PaginationState public
- Fix [#539](https://github.com/microsoft/fluentui-blazor/issues/539): Slider bugs
- Fix [#528](https://github.com/microsoft/fluentui-blazor/issues/537): FluentDesignSystemProvider fixes

## V2.4.0
- The `FluentSlider` is now generic which means it also supports `double`, `float`, `decimal`, `long' and `short` values besides `int` 
values. An example for this has been added to the demo site
- Add Style to `FluentDesignSystemProvider` (thanks @luohuaRain)
- Fix missing `ColumnOptions` button in `FluentDataGrid` when column style = `Align.Center` (thanks @cupsos)

## V2.3.7

- Small Templates package updates. Version is now 2.0.6 (thanks @c0g1t8) 
- Update Fluent UI System Icons to version 1.1.204

**What's new (Name / Size(s) / Variant(s))**
- Clipboard Paste / 32 / Filled & Regular
- Cloud Bidirectional / 20, 24 / Filled & Regular
- Comment Edit / 16 / Filled & Regular
- Crown Subtract / 24 / Filled & Regular
- Crown / 24 / Filled & Regular
- Pause Circle / 32, 48 / Filled & Regular
- Person Subtract / 24 / Filled & Regular
- Plug Connected Settings / 20, 24 / Filled & Regular
- Signature / 32 / Filled & Regular
- Speaker Mute / 32 / Filled & Regular
- Thumb Like Dislike / 16, 20, 24 / Filled & Regular

**What's updated (Name / Size(s) / Variant(s))**
- Folder / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Paint Brush / 16, 32 / Filled & Regular

## V2.3.6
- Fix [#418](https://github.com/microsoft/fluentui-blazor/issues/418): Partial fix for not being able to use arrow keys in FluentTextArea inside a FluentDataGrid. Does not work for virtualized grid.
- Fix [#419](https://github.com/microsoft/fluentui-blazor/issues/419): Partial fix for feat: Add multi line text to FluentDataGrid PropertyColumn
- Fix [#424](https://github.com/microsoft/fluentui-blazor/issues/422): Demo site not working on iPhone
- Fix [#424](https://github.com/microsoft/fluentui-blazor/issues/424): [Demo apps] Infinite rendering loop in TableOfContents
- Fix [#370](https://github.com/microsoft/fluentui-blazor/pull/439): FluentIcon performance improvements by @andreisaperski 
- Make arrow keys work in FluentDataGrid colum options
- Add required icon assets for all sizes
- Add all Presence icons as required
- Update Fluent UI System Icons to version 1.1.203
**What's new (Name / Size(s) / Variant(s))**
- Book Default / 28 / Filled & Regular
- Folder Lightning / 16, 20, 24 / Filled & Regular
- Hat Graduation / 28 / Filled & Regular
- Image Sparkle / 16, 20, 24 / Filled & Regular
- Mail / 32 / Filled & Regular
- Person Info / 24 / Filled & Regular
- Prohibited Multiple / 28 / Filled & Regular
- Prohibited / 32 / Filled & Regular
- Spinner iOS / 16 / Filled & Regular
- Star Emphasis / 16 / Filled & Regular
- Text Direction Rotate 315 Right / 20, 24 / Filled & Regular
- Text Direction Rotate 45 Right / 20, 24 / Filled & Regular

**What's updated (Name / Size(s) / Variant(s))**
- Add Square / 16, 20, 28, 32, 48 / Filled & Regular
- Book Add / 24, 28 / Filled & Regular
- Subtract / 12, 16, 20 / Filled & Regular

## V2.3.5
- Fix [#386](https://github.com/microsoft/fluentui-blazor/issues/386) by updating web-components.min.js to 2.5.15


## V2.3.4
- Fix not being able to use library in a Razor Class Library (discussion [#391](https://github.com/microsoft/fluentui-blazor/discussions/391))
- Fix [#414](https://github.com/microsoft/fluentui-blazor/issues/414): Table of Contents doesn't work correctly with Markdown Section in demo site/new components 
- Fix [#413](https://github.com/microsoft/fluentui-blazor/issues/413): The default selected of the Navigation Menu is incorrect in Demo Site
- Fix [#411](https://github.com/microsoft/fluentui-blazor/issues/411): Component FluentRadio / FluentRadioGroup not working Readonly or Disabled 

## V2.3.3
- Fix [#405](https://github.com/microsoft/fluentui-blazor/issues/405): **Important if you are using icons** Incomplete GetIconConfiguration() is generated if PublishFluentEmojiAssets is set to false 
- Fix [#406](https://github.com/microsoft/fluentui-blazor/issues/406): Build error if FluentIconSizes doesn't have 20, but has 32
V2.3.2 will be hidden from NuGet packages because of icon issue

## V2.3.2

- [Unit Tests] Refactoring and fixing of unit tests
- Rework of generators after code review
- Update Fluent UI Sysem Icons to version 1.1.202
- Fix [#402](https://github.com/microsoft/fluentui-blazor/issues/402): FluentUI Combobox in Blazor still showing value, not text 

**What's new (Name / Size(s) / Variant(s))**
- Airplane Landing / 16, 20, 24 / Filled & Regular
- Align Space Evenly Horizontal / 24 / Filled & Regular
- Align Space Evenly Vertical / 24 / Filled & Regular
- Arrow Flow Diagonal Up Right / 16, 20, 24, 32 / Filled & Regular
- Arrow Flow Up Right / 16, 32 / Filled & Regular
- Arrow Square Up Right / 20, 24 / Filled & Regular
- Bin Recycle Full / 20, 24 / Filled & Regular
- Bin Recycle / 20, 24 / Filled & Regular
- Briefcase Search / 20, 24 / Filled & Regular
- Circle Line / 16 / Filled & Regular
- Desk / 20, 24 / Filled & Regular
- Filmstrip Off / 48 / Filled & Regular
- Filmstrip / 48 / Filled & Regular
- Flash / 32 / Filled & Regular
- Flow / 24, 32 / Filled & Regular
- Heart Pulse Checkmark / 20 / Filled & Regular
- Heart Pulse Error / 20 / Filled & Regular
- Heart Pulse Warning / 20 / Filled & Regular
- Home Heart / 16, 20, 24, 32 / Filled & Regular
- Image Off / 28, 32, 48 / Filled & Regular
- Money Hand / 16 / Filled & Regular
- Money Settings / 16, 24 / Filled & Regular
- People Edit / 16, 24 / Filled & Regular
- Square Line Hint / 20, 24 / Filled & Regular

**What's updated (Name / Size(s) / Variant(s))**
- Airplane Take Off / 16, 20, 24 / Filled
- Arrow Flow Up Right Rectangle Multiple / 20, 24 / Filled & Regular
- Arrow Flow Up Right / 20, 24 / Filled & Regular
- Column Triple Edit / 20, 24 / Filled & Regular
- Column Triple / 20, 24 / Filled & Regular
- Flow / 16, 20 / Filled & Regular
- Fluent / 20, 24, 32, 48 / Filled & Regular
- Heart Pulse / 24 / Filled
- Image Off / 20, 24 / Filled & Regular
- Real Estate / 20, 24 / Filled & Regular
- Triangle Down / 20 / Filled & Regular
- Triangle Left / 20 / Filled & Regular
- Triangle Right / 20 / Filled & Regular
- Triangle Up / 20 / Filled & Regular

**⚠️ Renamed icons**
- Arrow Routing → Arrow Flow Up Left
- Arrow Routing Rectangle Multiple → Arrow Flow Up Left Rectangle Multiple

## V2.3.1
- Update to .NET 8.0.0-preview.4
- Accessibility compliance issue in DataGrid filter and pagination buttons fixed [#390](https://github.com/microsoft/fluentui-blazor/issues/390)
- Unable to GET FluentIcon in Blazor Server fixed [#399](https://github.com/microsoft/fluentui-blazor/issues/399) 
- Dead link in demo site fixed [#393](https://github.com/microsoft/fluentui-blazor/issues/393)
- Update NumberFieldDefault.razor 
- Create SearchInteractiveWithDebounce.razor 

Thanks @Ogglas and @pk9r327 for your contributions!

## V2.3.0
- **IMPORTANT Web components script is now included in the library**
- New documentation pages added to demo site
- Old demo environment no longer updated
- Icon/Emoji fix for when using Windows authentication 

## Script change
The heart of this library is formed by the Fluent UI Web Components and the accompanying `web-components.min.js` file. From now 
on, the script is included in the library itself and no longer needs to be added to your `index.html` or `_Layout.cshtml`. In fact, doing this might lead to 
unpredictable results. 

> **If you are upgrading from an earlier version please remove the script from your `index.html` or `_Layout.cshtml` file.**

We made this change to make certain you'll always get the version of the script that works best with the Blazor components. It also eliminates the risk of having
multiple versions of the script being used on a page. On top of all this, it also makes installation of the package in your new projects easier.

## Readme and docs updates
The `Readme` file was getting bigger and bigger. Now that we have our own domain name, it was time to split the `Readme` file into multiple smaller parts.
See the 'More information' section at [https://www.fluentui-blazor.net](https://www.fluentui-blazor.net) for the documentation on project setup,
code setup and Design Tokens.

## No cliffhangers...
Also, because of the new domain, we sun-setted the older brave-cliff demo environment. The site is still up, but updates won't be deployed to that 
environment anymore. A remark about that has been added to the home page at that URL.

## Icon/Emoji fix
An issue has been addressed where the FluentIcon/FluentEmoji components would not render when the site was running under IIS (Express) and Windows Authentication was enabled.


## V2.2.1

Updated readme to use new domain name (www.fluentui-blazor.net)

Updated Fluent UI Sysem Icons to version 1.1.201
    
**What's new (Name / Size(s) / Variant(s))**
- App Generic / 48 / Filled & Regular
- Arrow Enter / 16 / Filled & Regular
- Arrow Sprint / 16, 20 / Filled & Regular
- Beaker Settings / 16, 20 / Filled & Regular
- Binder Triangle / 16 / Filled & Regular
- Book Dismiss / 16, 20 / Filled & Regular
- Button / 16, 20 / Filled & Regular
- Card UI / 20, 24 / Filled & Regular
- Chevron Down Up / 16, 20, 24 / Filled & Regular
- Column Single Compare / 16, 20 / Filled & Regular
- Crop Sparkle / 24 / Filled & Regular
- Cursor Prohibited / 16, 20 / Filled & Regular
- Cursor / 16 / Filled & Regular
- Data Histogram / 16 / Filled & Regular
- Document Image / 16, 20 / Filled & Regular
- Document JAVA / 16, 20 / Filled & Regular
- Document One Page Beaker / 16 / Filled & Regular
- Document One Page Multiple / 16, 20, 24 / Filled & Regular
- Document SASS / 16, 20 / Filled & Regular
- Document YML / 16, 20 / Filled & Regular
- Filmstrip Split / 16, 20, 24, 32 / Filled & Regular
- Gavel Prohibited / 16, 20 / Filled & Regular
- Gavel / 16 / Filled & Regular
- Gift Open / 16, 20, 24 / Filled & Regular
- Globe / 12 / Filled & Regular
- Grid Kanban / 16 / Filled & Regular
- Image Stack / 16, 20 / Filled & Regular
- Laptop Shield / 16, 20 / Filled & Regular
- List Bar Tree Offset / 16, 20 / Filled & Regular
- List Bar Tree / 16, 20 / Filled & Regular
- List Bar / 16, 20 / Filled & Regular
- List RTL / 16, 20 / Filled & Regular
- Panel Left Text Add / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Panel Left Text Dismiss / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Panel Left Text / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Person Lightning / 16, 20 / Filled & Regular
- Text Bullet List Square Sparkle / 16, 20, 24 / Filled & Regular
- Text Bullet List Square / 16, 32 / Filled & Regular
- Translate Auto / 16, 20, 24 / Filled & Regular
  
**What's updated (Name / Size(s) / Variant(s))**
- Book Add / 20 / Filled
- Book Default / 20 / Filled
- Briefcase Medical / 16, 20, 24, 32 / Filled & Regular
- Briefcase Off / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Briefcase / 12, 16, 20, 24, 28, 32, 48 / Filled & Regular
- Calendar Add / 16 / Filled & Regular
- Calendar Arrow Right / 16 / Filled & Regular
- Calendar Assistant / 16 / Filled & Regular
- Calendar Cancel / 16 / Filled & Regular
- Calendar Checkmark / 16 / Filled & Regular
- Calendar Clock / 16 / Filled & Regular
- Chevron Up Down / 20 / Filled
- Document One Page Add / 16 / Filled & Regular
- List / 20, 24, 28 / Filled & Regular
- Text Bullet List Square Clock / 20 / Filled & Regular
- Text Bullet List Square Edit / 20, 24 / Filled & Regular
- Text Bullet List Square Person / 20, 32 / Filled & Regular
- Text Bullet List Square Search / 20 / Filled & Regular
- Text Bullet List Square Settings / 20 / Filled & Regular
- Text Bullet List Square Shield / 20 / Filled & Regular
- Text Bullet List Square Toolbox / 20 / Filled & Regular
- Text Bullet List Square Warning / 16, 20, 24 / Filled & Regular
- Text Bullet List Square / 20, 24 / Filled & Regular
- Translate Off / 16, 20, 24 / Filled & Regular
- Translate / 16, 20, 24 / Filled & Regular

## V2.2
For version 2.2 we started working on adding .NET 8 support. One important new feature in Blazor with .NET 8 is the addition of the QuickGrid component. 
QuickGrid is a high performance grid component for displaying data in tabular form. It is built to be a simple and convenient way to display your data, while 
still providing powerful features like sorting, filtering, paging, and virtualization. 

QuickGrid was originally introduced as an experimental package based on .NET 7 and we copied its code over to the Fluent UI library to re-use its 
features (and some more) but render it with the Fluent UI Web Components instead of its orignal rendering based on HTML table, tr and td elements. As part 
of bringing QuickGrid into .NET 8 the ASP.NET Core team made some changes and improvements to the API. We brought these changes over to the `<FluentDataGrid>` as well. To update an app that uses `<FluentDataGrid>`, 
you may need to make the following adjustments:

**------BREAKING CHANGES------**
- Rename the `Value` attribute on the `Paginator` component to `State`
- Rename the `IsDefaultSort` attribute on columns to `InitialSortDirection` and add `IsDefaultSortColumn=true` to indicate the column should still be sorted by default.

**------BREAKING CHANGES------**

*To use the `<FluentDataGrid>` component, you do not need to add a reference to the `Microsoft.AspNetCore.Components.QuickGrid` package to your project. In fact, if you do so it will lead to compilation errors.*

All the examples in the [demo site](https://aka.ms/fluentui-blazor) have been updated to reflect these changes.

### New features
- Updated Fluent UI System Icons to version 1.1.198


## V2.1

A more detailed description of all the changes and everything new can be found in [this blog post](https://baaijte.net/blog/whats-new-in-the-microsoft-fluent-ui-library-for-blazor-version-21/) 

**Important change:**

**If you are currently *not using* icons and are *not planning* on using icons and/or moji in your application moving forward, 
you do *not* have to make any changes to your project. If you *are* currently using icons, please read on.**

With earlier versions of the library, all (then only icon) assets would always get published. Starting with this version, when not specifying settings
in the project file with regards to usage of icons and/or emoji (see below) **NO** assets will be published to the output folder. 
This means that no icons and/or emoji will be available for rendering (with exception of the icons that are used by the library itself). 

For icons and emoji to work properly with 2.1.1 and later, two changes need to be made:
1) Add properties to the `.csproj` file
2) Add/change code in `Program.cs'

### Changes to `.csproj`
The (annotated) `PropertyGroup` below can be used as a starting point in your own project. Copying this as-is will result in all icon and emoji assets being published.
See the blog post for more information.


```xml
<PropertyGroup>
    <!-- 
        The icon component is part of the library. By default, NO icons (static assets) will be included when publishing the project. 
 
        Setting the property 'PublishFluentIconAssets' to false (default), or leaving the property out completely, will disable publishing of the 
        icon static assets (with exception of the icons that are being used by the library itself). 

        Setting the property 'PublishFluentIconAssets' to 'true' will enable publishing of the icon static assets. You can limit what icon assets get 
        published by specifying a set of icon sizes and a set of variants in the '<FluentIconSizes>' and '<FluentIconVariants>' properties respectively.

        To determine what icons will be published, the specified options for sizes and variants are combined. Specifying sizes '10' and '16' and 
        variants 'Filled' and 'Regular' means all '10/Filled', all '10/Regular', all '16/Filled' and all '16/Regular' icons assets will be published. 
        It is not possible to specify multiple individual combinations like '10/Filled' and '16/Regular' in the same set. 

        When no icon size set is specified in the '<FluentIconSizes>' property, ALL sizes will be included*  
        When no icon variant set is specified in the '<FluentIconVariants>' property, ALL variants will be included*  
        * when publishing of icon assets is enabled 
    -->
    <PublishFluentIconAssets>true</PublishFluentIconAssets>

    <!-- 
        Specify (at least) one or more sizes from the following options (separated by ','):
        10,12,16,20,24,28,32,48 
        Leave out the property to have all sizes included.
    -->
    <FluentIconSizes>10,12,16,20,24,28,32,48</FluentIconSizes>

    <!-- 
        Specify (at least) one or more variants from the following options (separated by ','):
        Filled,Regular 
        Leave out the property to have all variants included.
    -->
    <FluentIconVariants>Filled,Regular</FluentIconVariants>

    <!-- 
        The emoji component is part of the library. By default, NO emojis (static assets) will be included when publishing the project. 
 
        Setting the property 'PublishFluentEmoji' to false (default), or leaving the property out completely, will disable publishing of the emoji static assets. 

        Setting the property 'PublishFluentEmojiAssets' to 'true' will enable publishing of the emoji static assets. You can limit what emoji assets get 
        published by specifying a set of emoji groups and a set of emoji styles in the '<FluentEmojiGroups>' and '<FluentEmojiStyles>' properties respectively.

        To determine what emojis will be published, the specified options for sizes and variants are combined. Specifying emoji groups 'Activities' and 'Flags' 
        and emoji styles 'Color' and 'Flat' means all 'Activities/Color', all 'Activities/Flat', all 'Flags/Color' and all 'Flags/Flat' emoji assets will be published.

        It is not possible to specify multiple individual combinations like 'Activities/Color' and 'Flags/Flat' in the same published set

        When no emoji group set is specified in the '<FluentEmojiGroups>' property, ALL groups will be included*  
        When no emoji variant set is specified in the '<FluentEmojiStyles>' property, ALL styles will be included*  
        * when publishing of emoji assets is enabled 
    -->
    <PublishFluentEmojiAssets>true</PublishFluentEmojiAssets>

    <!-- 
        Specify (at least) one or more groups from the following options (separated by ','):
        Activities,Animals_Nature,Flags,Food_Drink,Objects,People_Body,Smileys_Emotion,Symbols,Travel_Places 
        Leave out the property to have all groups included.
    -->
    <FluentEmojiGroups>Activities,Animals_Nature,Flags,Food_Drink,Objects,People_Body,Smileys_Emotion,Symbols,Travel_Places</FluentEmojiGroups>

    <!-- 
        Specify (at least) one or more styles from the following options (separated by ','): 
        Color,Flat,HighContrast
        Leave out the property to have all styles included.
    -->
    <FluentEmojiStyles>Color,Flat,HighContrast</FluentEmojiStyles>
</PropertyGroup>
```
### Changes to `Program.cs`
The AddFluentUIComponents() service collection extension needs to be changed. This enables the system to check if a requested icon or emoji is available
Services and configuration classed have been added to the library for this. You do not need to specify the configuration in code yourself. A source generator has been added that reads the settings from the project file and adds the necessary code at compile time. That way the settings made in the project file and the source code are always kept in sync.

The two lines that need to be added to the Program.cs file are:
```
LibraryConfiguration config = new(ConfigurationGenerator.GetIconConfiguration(), ConfigurationGenerator.GetEmojiConfiguration());
builder.Services.AddFluentUIComponents(config);
```

## Other changes
**New component**: 
- `<FluentEmoji>` 

**Other changes:** 
- All `<FluentInputBase>` derived components now need to use `@bind-Value` or `ValueExpression`. This means an input derived component needs to be bound now.
  This is in-line with how it works with the built-in Blazor `<Input...>` components. All examples in the demo site have been updated to reflect this. The affected components are:
    - `<FluentCheckbox>`
    - `<FluentNumberField>`
    - `<FluentRadioGroup>`
    - `<FluentSearch>`
    - `<FluentSlider>`
    - `<FluentSwitch>`
    - `<FluentTextArea>`
    - `<FluentTextField>`
- Because of the above change, the `<FluentCheckbox>` and `<FluentSwitch>` no longer have the 'Checked' parameter. Initial state can be set by using `@bind-Value`
- `<FluentSwitch>` has two new parameters to get/set the checked and unchecked message text, called `CheckedMessage` and `UncheckedMessage` respectively.
- `<FluentRadioGroup>` component is now generic, so can be bound to other values than just `string`
- Various bug fixes
- Updated Fluent UI System Icons to release 1.1.194

