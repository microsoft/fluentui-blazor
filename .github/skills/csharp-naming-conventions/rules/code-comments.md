---
title: Commenting Conventions
impact: MEDIUM
impactDescription: Poor comments reduce code comprehension
tags: code, comments, xml-documentation
---

## Commenting Conventions

- Place the comment on a **separate line**, not at the end of a line of code.
- Begin comment text with an **uppercase letter**.
- End comment text with a **period**.
- Insert **one space** between the comment delimiter (`//`) and the comment text.
- Don't create formatted blocks of **asterisks** around comments.
- Ensure **all public members** have the necessary XML comments providing appropriate descriptions about their behavior.

**Correct:**

```csharp
// The following declaration creates a query. It does not run
// the query.
```

**Incorrect:**

```csharp
var x = 5; // set x to 5
/***************************
 * Don't do this
 ***************************/
```
