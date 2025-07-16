## V4.12.1

### General
- \[General\] Add file header to all .cs files ([#3946](https://github.com/microsoft/fluentui-blazor/pull/3946))
- \[General\] Update the issue template ([#3995](https://github.com/microsoft/fluentui-blazor/pull/3995))
- \[General\] Update to latest SDKs and NuGet packages ([#4006](https://github.com/microsoft/fluentui-blazor/pull/4006))

### Components
- \[Autocomplete\] Fix keyboard usage in Single mode ([#3930](https://github.com/microsoft/fluentui-blazor/pull/3930))
- \[AutoComplete\] Only render placeholder attribute when the parameter is provided ([#3919](https://github.com/microsoft/fluentui-blazor/pull/3919))
- \[DataGrid\] Add IsFixed parameter ([#3927](https://github.com/microsoft/fluentui-blazor/pull/3927))
- \[DataGrid\] Add parameter to configure full column resize ([#3903](https://github.com/microsoft/fluentui-blazor/pull/3903))
- \[DataGrid\] Fix header not being sticky in certain situations ([#3890](https://github.com/microsoft/fluentui-blazor/pull/3890))
- \[DataGrid\] Make SelectColumn respond to programmatic changes in grid data ([#3949](https://github.com/microsoft/fluentui-blazor/pull/3949))
- \[DataGrid\] Set correct initial height for resize indicator ([#3965](https://github.com/microsoft/fluentui-blazor/pull/3965))
- \[MAUI\] Add DynamicDependency to DesignToken constructor ([#4005](https://github.com/microsoft/fluentui-blazor/pull/4005))
- \[NavLink\] Track active state for menu items with an OnClick handler ([#3999](https://github.com/microsoft/fluentui-blazor/pull/3999))
- \[NavLink\] Fix keyboard navigation not working for FluentNavLink inside a FluentNavGroup ([#3998](https://github.com/microsoft/fluentui-blazor/pull/3998))
- \[Select\] Use correct CSS variable for placeholder color ([#3941](https://github.com/microsoft/fluentui-blazor/pull/3941))
- \[SortableList\] Extend sortable list parameters ([#3891](https://github.com/microsoft/fluentui-blazor/pull/3891))
- \[Tabs\] Implement proper tab sizing ([#4002](https://github.com/microsoft/fluentui-blazor/pull/4002))
- \[Templates\] Updates to Aspire and WebAssembly templates ([#3945](https://github.com/microsoft/fluentui-blazor/pull/3945))
- \[TimePicker\] Fix the Time format when another culture is used, add TimeDisplay enum ([#3961](https://github.com/microsoft/fluentui-blazor/pull/3961))
- \[Toast\] Add DynamicDependency to prevent trimming errors ([#3994](https://github.com/microsoft/fluentui-blazor/pull/3994))

### Demo site and documentation
- \[Docs\] Add documentation for possible placeholder autocomplete conflicts ([#3923](https://github.com/microsoft/fluentui-blazor/pull/3923))
- \[Docs\] ComponentBase page add MultiLine to DataGrid ([#3983](https://github.com/microsoft/fluentui-blazor/pull/3983))
- \[Docs\] Fix mismatch in accordion docs between the code and the description. ([#3932](https://github.com/microsoft/fluentui-blazor/pull/3932))
- \[Docs\] Fix typo in AppBar demo ([#3924](https://github.com/microsoft/fluentui-blazor/pull/3924))
- \[Docs\] Remove Autofocus parameter to prevent scrolling ([#3922](https://github.com/microsoft/fluentui-blazor/pull/3922))
- \[Docs\] Update Card example ([#3909](https://github.com/microsoft/fluentui-blazor/pull/3909))
 
### Icons and Emoji
- Update to Fluent UI System Icons 1.1.306.
    See the commit history in the Fluent UI System Icons repository [commit history](https://github.com/microsoft/fluentui-system-icons/commits/main/) for the full list of changes.

## V4.12.0

### General
- \[General\] Use latest .NET SDKs (including .NET 10 preview 4)([#3789](https://github.com/microsoft/fluentui-blazor/pull/3789))

### Components
- \[AutoComplete\] Do not show previous/next when no option is selected ([#3780](https://github.com/microsoft/fluentui-blazor/pull/3780))
- \[Autocomplete\] Fix active highlight size for single-select mode ([#3819](https://github.com/microsoft/fluentui-blazor/pull/3819))
- \[AutoComplete\] Fix using different height for MaxAutoHeight (#3793) ([#3796](https://github.com/microsoft/fluentui-blazor/pull/3796))
- \[DataGrid\] Add UseMenuService parameter to override menu behavior ([#3862](https://github.com/microsoft/fluentui-blazor/pull/3862))
- \[DataGrid\] Make Select/Deselect all more robust ([#3866](https://github.com/microsoft/fluentui-blazor/pull/3866))
- \[DataGrid\] Refine logic that determines if the DataGrid data needs to be reloaded ([#3864](https://github.com/microsoft/fluentui-blazor/pull/3864))
- \[DataGrid\] Resize enhancements ([#3767](https://github.com/microsoft/fluentui-blazor/pull/3767))
- \[DataGrid\] Tweak new resize behavior ([#3787](https://github.com/microsoft/fluentui-blazor/pull/3787))
- \[DataGrid\] Use ColumnOptionsLabels.Icon in more places ([#3865](https://github.com/microsoft/fluentui-blazor/pull/3865))
- \[DataGrid\] Add `HeaderCellTitleTemplate` to `ColumnBase` ([#3860](https://github.com/microsoft/fluentui-blazor/pull/3860))
- \[DesignTheme\] Store selected color in GlobalState ([#3833](https://github.com/microsoft/fluentui-blazor/pull/3833))
- \[Lists\] Do not set internal value when using multiple ([#3835](https://github.com/microsoft/fluentui-blazor/pull/3835))
- \[MainLayout\] Remove double header reduction ([#3841](https://github.com/microsoft/fluentui-blazor/pull/3841))
- \[MenuButton\] Pass through AdditionalAttributes ([#3798](https://github.com/microsoft/fluentui-blazor/pull/3798))
- \[MenuProvider\] Pass through class and style from menu's. ([#3809](https://github.com/microsoft/fluentui-blazor/pull/3809))
- \[MultiSplitter\] Remove position from panel CSS ([#3870](https://github.com/microsoft/fluentui-blazor/pull/3870))
- \[SortableList\] Extend the CSS class with more predefined variables([#3877](https://github.com/microsoft/fluentui-blazor/pull/3877))
- \[Tabs\] Make initialization logic more robust ([#3878](https://github.com/microsoft/fluentui-blazor/pull/3878))

### Demo site and documentation
- \[Docs\] Add docs about `FluentEditForm` ([#3832](https://github.com/microsoft/fluentui-blazor/pull/3832))
- \[Docs\] Fix typo in InputFile documentation ([#3769](https://github.com/microsoft/fluentui-blazor/pull/3769))
- \[Docs\] Fix typo in Card documentation ([#3839](https://github.com/microsoft/fluentui-blazor/pull/3839))
- \[Docs\] Remove Autofocus to prevent scrolling in the docs (#3826) ([#3828](https://github.com/microsoft/fluentui-blazor/pull/3828))
- \[Docs\] Update reboot section for IIS hosting workaround ([#3788](https://github.com/microsoft/fluentui-blazor/pull/3788))

### Icons and Emoji
  - Update to Fluent UI System Icons 1.1.302.
    See the commit history in the Fluent UI System Icons repository [commit history](https://github.com/microsoft/fluentui-system-icons/commits/main/) for the full list of changes.


## Before v4.12.0
For versions before 4.12, see the [What's New? (v4.0 - v4.11.9)](/WhatsNew-Before412) page.

## Archives
For versions before 4.0, see [What's new? (archives)](/WhatsNew-Archive) page.
