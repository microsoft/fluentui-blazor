---
title: DateTimeProvider
impact: HIGH
impactDescription: Direct DateTime.Now usage makes unit tests non-deterministic
tags: code, datetime, testing, mock
---

## DateTime

Never use `DateTime.Now` or `DateTime.UtcNow` directly. Instead, always use **DateTimeProvider** to allow unit testing and date mocking.

This approach relies on the **Ambient Context Model**, which avoids the need to inject a simple date/time service through Dependency Injection, and makes it easy to adopt in existing code.

**Incorrect:**

```csharp
int trimester = (DateTime.Today.Month - 1) / 3 + 1;
```

**Correct:**

```csharp
int trimester = (DateTimeProvider.Today.Month - 1) / 3 + 1;
```

Reference: [UnitTest DateTime](https://dvoituron.com/2020/01/22/UnitTest-DateTime)
