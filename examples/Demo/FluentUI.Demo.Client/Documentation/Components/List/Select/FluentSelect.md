---
title: Select
route: /List/Select
---

# Select

The **FluentSelect** component allows one option to be selected from multiple options.

View the [Usage Guidance](https://fluent2.microsoft.design/components/web/react/select/usage).

## Default

{{ SelectDefault }}

## Customize the items

You can customize the items in the **FluentSelect** component by using Lambda expressions.
The following example shows how to customize the items:
- `OptionText`: This function is used to customize the text of the option. <br />
   In the following example, the **FirstName** is used.
- `OptionValue`: This function is used to customize the value of the option. <br />
   In the following example, the **Id** is returned.
- `OptionDisabled`: This function is used to define the disabled options. <br />
   In the following example, the third element is disabled.

The preselected items are defined by the **Value** parameter. <br />
In the following example, the second element is preseleced.

{{ SelectCustomized }}

## Multiple

{{ SelectMultiple }}

## API FluentSelect

{{ API Type=FluentSelect }}

{{ API Type=FluentOption }}

## Migrating to v5

TODO
