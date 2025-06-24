---
title: Radio
route: /Radio
---

# RadioGroup and Radio

**FluentRadioGroup** lets people select a single option from two or more **FluentRadio** items.
Use **FluentRadioGroup** to present all available choices if there's enough space.
For more than 5 choices, consider using a different component such as Dropdown.

## Best practices
**Do**

  - **Give people a choice**. Make sure to give people the option to not make a choice.
    For example, include a "None" option if no answer is required.
  - **Choose a default**. Select the safest, most secure, and private option as the default.
    If safety and security aren't factors, select the most likely or convenient option.
  - **Keep labels short and action-oriented**. Use a phrase for the label, rather than a full sentence.
  - **Use sentence case**. Capitalize only the first word as you would in a sentence.
  - **Place most likely choices first**. List the options in a logical order, such as most likely to be selected to least,
    simplest operation to most complex, or least risk to most. Listing options in alphabetical
    order isn't recommended because the order will change when the text is localized.

**Don't**

  - **Include more than 5 options**. Use FluentRadioGroup when there are 2-5 options,
    and you have enough screen space and the options are important enough to be a good use of that screen space.
    Otherwise, use Dropdown.

## Appearance

Radios are placed and used inside a radio group.
Only one of the items in a group can have a checked state.
You can bind to the `Value` of the group to the get the value of the checked item.

A radio is either unchecked or checked. Usually, once an item in a group has been checked,
the result of the group as a whole cannot be unchecked again.
An item can also be disabled and can show a label to indicate the value. 

{{ RadioDefault }}

## Label template

Instead of using the `Label` parameter (string value only), you can use the `LabelTemplate` parameter to specify a template for the label.

Radio items allow for **strongly binding to types**.
Because of this, string values need to be defined in the following way: `Value="@("one")"`.

[!NOTE] We recommend using strong "non-null" types.
If you use a nullable type, you must ensure that the values of all FluentRadio are converted to nullables,
for example using an explit casting `Value="@((int?)1)"`.
Si ce n'est pas le cas, vous obtiendrez une erreur d'exécution
`InvalidOperationException: The FluentRadio must be included in a FluentRadioGroup component and must be of the same type.`.

{{ RadioGroupLabelTemplate }}

## Vertical layout

When the radio group has a vertical orientation, the items are stacked on top of each other.

{{ RadioGroupVertical }}
