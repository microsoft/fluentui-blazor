---
title: Pascal Case and Camel Case
impact: CRITICAL
impactDescription: Inconsistent casing makes code harder to read and maintain
tags: naming, casing, pascal, camel
---

## Pascal Case and Camel Case

Use two casing practices consistently: `PascalCasing` and `camelCasing`.

1. **Pascal Case** — Use when naming a `namespace`, `class`, `record`, `struct`, method, property, event, enum, or constant.
2. **Camel Case** — Use when naming `private` or `internal` fields (prefixed with `_`), local variables, and parameters.

**Correct:**

```csharp
public class DataService
{
    private IWorkerQueue _workerQueue;

    public bool IsValid;

    public event Action EventProcessing;

    public int Add(int first, int second)
    {
        return first + second;
    }
}
```

**Complete reference table:**

| Item                                  | Case   | Example                              |
| ------------------------------------- | ------ | ------------------------------------ |
| Namespace                             | Pascal | `System.File.Directory`              |
| Type parameter                        | Pascal | `TView`                              |
| Interface                             | Pascal | `IDisposable` (prefix `I`)           |
| Class, Struct                         | Pascal | `AppDomain`                          |
| Enum Type                             | Pascal | `ErrorLevel`                         |
| Enum Value                            | Pascal | `FatalError`                         |
| Resource key                          | Pascal | `SaveButtonTooltipText`              |
| Constant                              | Pascap | `Blue`, `MaximumItems`               |
| Private static                        | Pascal | `RedValue`                           |
| Class Exception                       | Pascal | `WebException` (suffix `Exception`)  |
| Event                                 | Pascal | `ValueChange`                        |
| Variable Private                      | Camel  | `_lastName` (prefix `_`)             |
| Variable Local                        | Camel  | `lastName`                           |
| Private field                         | Camel  | `listItem`                           |
| Read Only                             | Pascal | `RedColor`                           |
| Method                                | Pascal | `ToString()`                         |
| Property                              | Pascal | `BackColor`                          |
| Parameter                             | Camel  | `typeName`                           |
| Local function                        | Pascal | `FormatText`                         |
| Tuple element names                   | Pascal | `(string First, string Last) name = ("John", "Doe"); var name = (First: "John", Last: "Doe"); (string First, string Last) GetName() => ("John", "Doe");` |
| Variables declared using tuple syntax | Camel | `(string first, string last) = ("John", "Doe"); var (first, last) = ("John", "Doe");` |

Reference: [CSharp Coding Guidelines](https://csharpcodingguidelines.com/)
