---
title: RadioGroup and Radio
route: /Radio
---

# RadioGroup and Radio

Radio groups let people select a single item from a short list. Use them in layouts that offer enough space to list up to five options or if it's important to view all options at once.

If there isn't enough space, try a dropdown instead. If you need to let people select more than one option, use checkboxes. To let them immediately turn a setting on or off, try a switch.

## Behavior

Although it is technically possible to show a single Radio button (as can be seen below), it *must* be placed inside a RadioGroup to have any function.

### Default selection
Present a selected option in radio groups by default. The default selection should be placed first and should be the most logical response. Remaining options should be listed in a logical order. For more information, see the Content section.

## Layout
Radio groups can be aligned vertically or horizontally (default). When horizontally aligned, the label can appear next to or under the radio input.

## Accessibility
Include intuitive labels with radio groups.

When tabbing, focus will fall on the first option if no options are selected. If there is a selection, focus will fall on that option first.

## Content
Keep labels short and clear
Keep individual radio labels as concise and descriptive as possible. Use fragments instead of full sentences. If long labels can’t be avoided, text will wrap onto the next line. Never truncate radio text with an ellipsis. Use sentence case with no end punctuation.

Skip the period in radio labels. For the label that introduces the radio group, don’t end with a colon. For more info, go to Periods in the Microsoft Writing Style Guide.

Use sentence style capitalization—only capitalize the first word. For more info, go to Capitalization in the Microsoft Writing Style Guide.

## Examples

### Radio button appearances

A radio button is either unchecked or checked. Usually, once an item in a group has been checked, the result of the group as a whole cannot be unchecked again.
An item can also be disabled and can show a label to indicate the value. 

{{ RadioDefault }}

## RadioGroup

Radios are placed and used inside a radio group. Only one of the items in a group can have a checked state.
You can bind to the `Value` of the group to the get the value of the checked item.

{{ RadioGroupDefault }}

## Strongly typed items and using Label template
Radio items allow for strongly binding to types. Because of this, string values need to be defined in the following way: 
`Value="@("one")"`

As an alternative to of using the `Label` parameter (string value only),
it is possible to use the `LabelTemplate` parameter to specify a template for the label.
In case both are specified, the `Label` parameter is used.

{{ RadioGroupLabelTemplate }}

## RadioGroup with vertical orientation
When the radio group has a vertical orientation, the items are stacked on top of each other.

{{ RadioGroupVertical }}

## Disabled RadioGroup
A radio group can be disabled as a whole. This means that the user cannot select any of the items in the group.
{{ RadioGroupDisabledGroup }}

## RadioGroup with disabled items
Besides disabling the whole group, it is also possible to disable specific items in a group.
{{ RadioGroupDisabledItems }}

## RadioGroup required
{{ RadioGroupRequired }}

## API FluentRadioGroup
{{ API Type=FluentRadioGroup }}

## API FluentRadio
{{ API Type=FluentRadio }}

## Migrating to v5

{{ INCLUDE File=MigrationFluentRadio }}
