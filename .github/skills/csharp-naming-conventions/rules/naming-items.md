---
title: Item Naming
impact: CRITICAL
impactDescription: Ambiguous names cause confusion and slow down code reviews
tags: naming, identifiers, boolean, english
---

## Item Naming

All items created must be defined:

- **Explicitly**: with understandable names adapted to the field of use. Don't use `MyVariable` or `Item`.
- **Without abbreviation**: to avoid confusion with other developers. Don't use `Msg` or `PR`.
- **Without prefix**: Don't use `The` or `A`.
- **In English only**.

> For **Boolean** variables and properties, add the prefix `Is` or `Has` before the name.

**Incorrect:**

```csharp
var Msg = "Hello";
var PR = GetPullRequest();
bool valid = CheckUser();
string aName = "John";
```

**Correct:**

```csharp
var message = "Hello";
var pullRequest = GetPullRequest();
bool isValid = CheckUser();
string name = "John";
```
