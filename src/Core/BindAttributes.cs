using Microsoft.AspNetCore.Components;

namespace Microsoft.FluentUI.AspNetCore.Components;

// Checkbox like items
[BindElement("fluent-checkbox", null, "checked", "onchange")]
[BindElement("fluent-checkbox", "value", "checked", "onchange")]

[BindElement("fluent-switch", null, "checked", "onchange")]
[BindElement("fluent-switch", "value", "checked", "onchange")]

[BindElement("fluent-menu-item", null, "checked", "onchange")]
[BindElement("fluent-menu-item", "value", "checked", "onchange")]

// Value like items
// fluent-slider (value)
[BindElement("fluent-slider", null, "value", "onchange")]
[BindElement("fluent-slider", "value", "value", "onchange")]

// fluent-select (value)
[BindElement("fluent-select", null, "value", "onchange")]
[BindElement("fluent-select", "value", "value", "onchange")]

// fluent-radio-group (value)
[BindElement("fluent-radio-group", null, "value", "onchange")]
[BindElement("fluent-radio-group", "value", "value", "onchange")]

// Do we need these two?
// fluent-progress (value)
[BindElement("fluent-progress", null, "value", "onchange")]
[BindElement("fluent-progress", "value", "value", "onchange")]

// fluent-progress-ring (value)
[BindElement("fluent-progress-ring", null, "value", "onchange")]
[BindElement("fluent-progress-ring", "value", "value", "onchange")]

// Selection like items (is this the right way?)
[BindElement("fluent-listbox", null, "selectedindex", "onchange")]
[BindElement("fluent-listbox", "value", "selectedindex", "onchange")]

// Text like inputs
[BindElement("fluent-combobox", null, "value", "onchange")]
[BindElement("fluent-combobox", "value", "value", "onchange")]

[BindElement("fluent-text-field", null, "value", "onchange")]
[BindElement("fluent-text-field", "value", "value", "onchange")]

[BindElement("fluent-text-area", null, "value", "onchange")]
[BindElement("fluent-text-area", "value", "value", "onchange")]

[BindElement("fluent-number-field", null, "value", "onchange")]
[BindElement("fluent-number-field", "value", "value", "onchange")]

[BindElement("fluent-search", null, "value", "onchange")]
[BindElement("fluent-search", "value", "value", "onchange")]
public static class BindAttributes
{
}

