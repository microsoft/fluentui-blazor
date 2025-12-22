### General
The `HeadingLevel` parameter type has been changed from `string?` to `int?`

### New parameters FluentAccordion
- `Size`
- `ExpandMode`
- `Block`
- `MarkerPosition`

### New EventCallbacks and methods for FluentAccordion
- `ExpandModeChanged`
- `ExpandItemAsync` and `CollapsItemAsync`. Supply these with an id parameter to programmatically change the status of an item.


### New parameters FluentAccordionItem
- `Size`
- `ExpandMode`
- `Block`
- `MarkerPosition`
- `Header`; this replaces `Heading` parameter
- `HeaderTemplate`; this replaces `HeadingTemplate` parameter.
It does not really make sense to set these parameters on a item level. We therefore copy teh values for the parent Accordion component.

### New EventCallback for FluentAccordionItem
- `ExpandedChanged`


### Migrating to v5
The `OnAccordionItemChange` callback now uses the new `AccordionItemEventArgs` type as the callback data
instead of the `FluentAccordionItem`. The item affected can be found int the event arguments in the `Item` property

The AccordionItem does not expose `start` and `end` slots anymore. Technically they can still be used but they are not styled correctly anymore. As part of the migration work, new styles need to be added manually.
