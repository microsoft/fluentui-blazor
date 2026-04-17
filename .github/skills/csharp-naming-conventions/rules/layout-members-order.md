---
title: Member Ordering
impact: HIGH
impactDescription: Inconsistent member ordering forces readers to browse up and down
tags: layout, ordering, members, region
---

## Place Members in a Well-Defined Order

Maintaining a common order allows other team members to find their way in your code more easily. A source file should be readable from top to bottom, as if reading a book.

Reference: [AV2406](https://csharpcodingguidelines.com//layout-guidelines/#AV2406)

**Required order:**

1. Private fields and constants
2. Public constants
3. Public static read-only fields
4. Factory methods
5. Constructors and the finalizer
6. Events
7. Public and protected properties
8. Other methods and private properties in calling order

**Additional rules:**
- Declare **local functions** at the bottom of their containing method bodies (after all executable code).
- **Do not use `#region`**. Regions require extra work without increasing quality or readability. They make code harder to view and refactor.
