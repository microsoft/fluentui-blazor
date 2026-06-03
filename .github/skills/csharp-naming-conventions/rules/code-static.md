---
title: Avoid Static Classes
impact: MEDIUM
impactDescription: Static classes are difficult to test and lead to tightly coupled code
tags: code, static, extension-methods, testability
---

## Static

Avoid static classes and static methods. See [AV1008](https://csharpcodingguidelines.com//class-design-guidelines/#AV1008).

**Exception:** Extension method containers are acceptable as static classes.

Static classes very often lead to badly designed code. They are also very difficult, if not impossible, to test in isolation.

> If you really need a static class, mark it as `static` so the compiler can prevent instance members and instantiation.
