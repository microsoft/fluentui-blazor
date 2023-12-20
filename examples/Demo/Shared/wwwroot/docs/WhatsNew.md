## V4.2.2
- Fix [#1168](https://github.com/microsoft/fluentui-blazor/issues/1168): Tabs tablist will forever expand despite the tab containers width
- Fix [#1174](https://github.com/microsoft/fluentui-blazor/issues/1174): Forgot to replace svg with icon in templates
- Fix [#1173](https://github.com/microsoft/fluentui-blazor/issues/1174): Replace type="image/png" with type="image/x-icon"in templates

## 4.2.1
- NavMenu and NevMenuGroups can now expand/collpase in SSR mode
- Added PageScript component (See [Static Server Rendeing on MS Learn](https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/static-server-rendering?view=aspnetcore-8.0))
- Move `initializersLoader.webview.js` back into `wwwroot/js` so it gets published in the package again.
- Updates and fixes for FluentDesignTheme: add `OnLoaded`, check if LocalStorage is available 
- `FluentProgress`: Add `Width`, `Stroke`, `Color` and `BackgroundColor` attributes
- `FluentTab`: Add `Visible` parameter 
- Fix [#1160](https://github.com/microsoft/fluentui-blazor/issues/1160): FluentDesignTheme custom color not honored and assertion failure.
- Fix [#1116](https://github.com/microsoft/fluentui-blazor/issues/1116): Create FieldIdentifier when no ValueExpression set 
- Fix [#1138](https://github.com/microsoft/fluentui-blazor/issues/1138): Do not render percent sign for indeterminate ProgressToast
- Fix [#1144](https://github.com/microsoft/fluentui-blazor/issues/1144): [List components] Each item must be instantiated (cannot be null).
- Fix [#1146](https://github.com/microsoft/fluentui-blazor/issues/1146): Loading button with styles issue
- Fix [#1149](https://github.com/microsoft/fluentui-blazor/issues/1149): [List components] maintain consistency between SelectedOption and Value when Multiple is false
- Demo site: Search for icons in all sizes

## 4.2.0
- New: FluentDesignTheme - An easy-to-use component to set a theme and accent color.
- New: FluentValidationMessage and Required indicator for input components
- JavaScript tools project system integration 
- Enhanced: NavMenu
- Enhanced: InputFile
- Enhanced: List components
- Enhanced: Grid: Hiding elements and `OnBreakpointEnter` event callback
- Enhanced: Checkbox
- Enhanced: Splitter
- Updated Templates: responsive NavMenu and other improvements
- Fix [#1057](https://github.com/microsoft/fluentui-blazor/issues/1057): Can't show Fluent UI Blazor dialog on published Blazor webasm app
- Fix [#1070](https://github.com/microsoft/fluentui-blazor/issues/1070): Style not applied for FluentPaginator + added `PaginationTextTemplate` parameter 
- Update Fluent UI System Icons to 1.1.224:
**What's new (Name / Size(s) / Variant(s))**
- Arrow Clockwise Dashes / 16, 32 / Filled & Regular
- Building Swap / 16, 20, 24, 32, 48 / Filled & Regular
- Certificate / 32 / Filled & Regular
- Clipboard Brush / 16, 20, 24, 28, 32 / Filled & Regular
- Cloud Beaker / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Cloud Cube / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Contract Up Right / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Document Data Lock / 16, 20, 24, 32 / Filled & Regular
- Glance Horizontal Sparkles / 20 / Filled & Regular
- Layout Cell Four Focus Bottom Left / 16, 20, 24 / Filled
- Layout Cell Four Focus Bottom Right / 16, 20, 24 / Filled
- Layout Cell Four Focus Top Left / 16, 20, 24 / Filled
- Layout Cell Four Focus Top Right / 16, 20, 24 / Filled
- Layout Cell Four / 16, 20, 24 / Filled & Regular
- Layout Column Four Focus Center Left / 16, 20, 24 / Filled
- Layout Column Four Focus Center Right / 16, 20, 24 / Filled
- Layout Column Four Focus Left / 16, 20, 24 / Filled
- Layout Column Four Focus Right / 16, 20, 24 / Filled
- Layout Column Four / 16, 20, 24 / Filled & Regular
- Layout Column One Third Left / 16, 20, 24 / Filled & Regular
- Layout Column One Third Right Hint / 16, 20, 24 / Filled & Regular
- Layout Column One Third Right / 16, 20, 24 / Filled & Regular
- Layout Column Three Focus Center / 16, 20, 24 / Filled
- Layout Column Three Focus Left / 16, 20, 24 / Filled
- Layout Column Three Focus Right / 16, 20, 24 / Filled
- Layout Column Three / 16, 20, 24 / Filled & Regular
- Layout Column Two Focus Left / 16, 20, 24 / Filled
- Layout Column Two Focus Right / 16, 20, 24 / Filled
- Layout Column Two Split Left Focus Bottom Left / 16, 20, 24 / Filled
- Layout Column Two Split Left Focus Right / 16, 20, 24 / Filled
- Layout Column Two Split Left Focus Top Left / 16, 20, 24 / Filled
- Layout Column Two Split Left / 16, 20, 24 / Filled & Regular
- Layout Column Two Split Right Focus Bottom Right / 16, 20, 24 / Filled
- Layout Column Two Split Right Focus Left / 16, 20, 24 / Filled
- Layout Column Two Split Right Focus Top Right / 16, 20, 24 / Filled
- Layout Column Two Split Right / 16, 20, 24 / Filled & Regular
- Layout Column Two / 16, 20, 24 / Filled & Regular
- Layout Row Four Focus Bottom / 16, 20, 24 / Filled
- Layout Row Four Focus Center Bottom / 16, 20, 24 / Filled
- Layout Row Four Focus Center Top / 16, 20, 24 / Filled
- Layout Row Four Focus Top / 16, 20, 24 / Filled
- Layout Row Four / 16, 20, 24 / Filled & Regular
- Layout Row Three Focus Bottom / 16, 20, 24 / Filled
- Layout Row Three Focus Center / 16, 20, 24 / Filled
- Layout Row Three Focus Top / 16, 20, 24 / Filled
- Layout Row Three / 16, 20, 24 / Filled & Regular
- Layout Row Two Focus Bottom / 16, 20, 24 / Filled
- Layout Row Two Focus Top / 16, 20, 24 / Filled
- Layout Row Two Split Bottom Focus Bottom Left / 16, 20, 24 / Filled
- Layout Row Two Split Bottom Focus Bottom Right / 16, 20, 24 / Filled
- Layout Row Two Split Bottom Focus Top / 16, 20, 24 / Filled
- Layout Row Two Split Bottom / 16, 20, 24 / Filled & Regular
- Layout Row Two Split Top Focus Bottom / 16, 20, 24 / Filled
- Layout Row Two Split Top Focus Top Left / 16, 20, 24 / Filled
- Layout Row Two Split Top Focus Top Right / 16, 20, 24 / Filled
- Layout Row Two Split Top / 16, 20, 24 / Filled & Regular
- Layout Row Two / 16, 20, 24 / Filled & Regular
- Location Target Square / 16, 20, 24, 32 / Filled & Regular
- Resize / 16, 28, 32, 48 / Filled & Regular
- Select All Off / 16 / Filled & Regular
- Select All On / 16 / Filled & Regular
- Share Android / 16, 32 / Filled & Regular
- Text Arrow Down Right Column / 16, 20, 24, 28, 32, 48 / Filled & Regular
- Text Effects Sparkle / 20, 24 / Filled & Regular
- Whiteboard Off / 16, 20, 24 / Filled & Regular
- Whiteboard / 16 / Filled & Regular
 
**What's updated (Name / Size(s) / Variant(s))**
- Contract Down Left / 28 / Filled & Regular
- Resize / 20, 24 / Filled & Regular
- Select All Off / 20, 24 / Filled & Regular
- Select All On / 20, 24 / Filled & Regular


## 4.1.1
- Fix [#939](https://github.com/microsoft/fluentui-blazor/issues/939): Add `OptionTemplate` for `FluentCombobox`, `FluentListBox` and `FluentSelect` 
- Fix [#1040](https://github.com/microsoft/fluentui-blazor/issues/1040): `FluentNumberField` can be changed when `ReadOnly`
- FluentNavMenu: Add `Tooltip` parameter to `FluentNavMenuGroup` (fallback to `Title`) and `FluentNavLink`
- FluentNavMenu: Expand the menu when collapsed and a navitem is clicked 
- FluentInputFile: Add ProgressTemplate and a bindable ProgressPercent attribute

## V4.1.0
- FluentCalendar: Add new views to select month/year 
- FluentCheckbox: Add tri-state support
- FluentAccordionItem: Add HeaderTemplate parameter
- FluentSplitter: Add Panel1MinSize, Panel2MinSize and BarSize parameter
- FluentSplitter: Make resizing always use proportional values
- FluentSplitter: Add support for collapsing panel 2 (right/bottom), add `OnCollapsed`, `OnExpanded` and  `OnResized` event callbacks
- Fix using checkbox, switch, slider being used in `EditForm` not getting set on first few clicks
- Fix Web App template to correctly handle NavMenu based on rendermode and interactivity choices
- Move CacheStorageAccessor and StaticAssetService to Demo.Shared project as these are specific to the demo site and not the library
- Update Fluent UI System Icons to 1.1.223 (no overview of what's new/changed this time)
- Demo: Move ErrorBoundary into DemoSection component so page keeps working when only one section fails
- Fix miscellaneous demo errors caused by trimming

## V4.0.0
- FluentAccordionItem: Add `HeaderTemplate` parameter

## V4.0.0-rc.3
- Undo change template interactivity default to None. Default is now Server (same as regular Blazor template)
- FluentSplitter: Add support for collapsing panel 2 (right/bottom), add `OnCollapsed`, `OnExpanded` and  `OnResized` event callbacks
- `variables.css` is imported in the automatically loaded library's css now

## V4.0.0-rc.2
- Fix initial Highlight style after refresh was not right
- Fix [#918](https://github.com/microsoft/fluentui-blazor/issues/918): .NET 8 initializers + cleanup
- Fix [#923](https://github.com/microsoft/fluentui-blazor/issues/923): WebAssembly template errors
- Fix [#924](https://github.com/microsoft/fluentui-blazor/issues/924): Call SetLuminance after theme change
- Fix [#925](https://github.com/microsoft/fluentui-blazor/issues/925): Fic W3C links in demo site
- Fix [#926](https://github.com/microsoft/fluentui-blazor/issues/926): Work on Dialog height
- Remove Card from Home page in the WebAssembly template
- Change template interactivity default to None (ie SSR mode)
- Adjust FluentNavMenu to work with interactivity change
- Add favicon to demo sites and templates
- Move CacheStorageAccessor and StaticAssetService to Demo.Shared project as these are specific to the demo site and not the library
- Fix WhatsNew archive

## V4.0.0-rc.1
- This version is for .NET 8 **only**. (use `Microsoft.Fast.Components.FluentUI` when using .NET 6 or 7)
- Update all input component to use `ReadOnly` instead of a mix of possible spellings
- Update demo site styling (nav menu and body now scroll independently)
- Support DisabledDateFunc in FluentDatePicker 
- Removed the FluentCodeEditor component because it lacked features and has no Fluent design aspects. You can use the [BlazorMonaco](https://github.com/serdarciplak/BlazorMonaco) component as a replacement
- Fix [#911](https://github.com/microsoft/fluentui-blazor/issues/911): Column resizing in DataGrid breaks when column to narrow
- Fix [#891](https://github.com/microsoft/fluentui-blazor/issues/891): JSDisconnectedException in FluentOverflow
- Fix [#861](https://github.com/microsoft/fluentui-blazor/issues/861): Needed extra `empty-content-cell` class 
- FluentCard: Add Width and Height + docs/examples
- FluentToast: Timeout is now in milliseconds
- FluentToastContainer renamed to FluentToastProvider
- FluentMessageBarContainer renamed to FluentMessageBarProvider

## V4.0.0-preview.2
- Functionally equivalent to v3.2.2
- Replace `AfterBindValue` with native `@bind-Value:after`
- Fix missed or incorrectly replaced namespace errors 
- Update Fluent UI System icons to 1.1.221

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

## V4.0.0-preview.1
- Change root namespace to `Microsoft.FluentUI.AspNetCore.Components`

## Archives
See the [What's New](/WhatsNew-Archive) page on the documentation online to browse the archive