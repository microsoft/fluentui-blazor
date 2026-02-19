---
title: Migration FluentAccordion
route: /Migration/Accordion
hidden: true
---

### General
The `HeadingLevel` parameter type has been changed from `string?` to `int?`

### Renamed parameters FluentAccordionItem
- `Heading` → `Header`
- `HeadingTemplate` → `HeaderTemplate`

### Migrating to v5
The `OnAccordionItemChange` callback now uses the new `AccordionItemEventArgs` type as the callback data
instead of the `FluentAccordionItem`. The item affected can be found int the event arguments in the `Item` property

The AccordionItem does not expose `start` and `end` slots anymore. Technically they can still be used but they are not styled correctly anymore. As part of the migration work, new styles need to be added manually.
