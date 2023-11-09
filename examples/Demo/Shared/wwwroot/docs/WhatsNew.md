## V4.0.0-rc.3
- Undo change template interactivity default to None. Default is now Server (same as regular Blazor template)
- FluentSplitter: Add support for collapsing panel 2 (right/bottom), add `OnCollapsed`, `OnExpanded` and  `OnResized` event callbacks

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