---
title: String Data Type
impact: MEDIUM
impactDescription: Inefficient string handling impacts performance in loops
tags: code, string, interpolation, stringbuilder
---

## String Data Type

Use **string interpolation** to concatenate short strings:

```csharp
string displayName = $"{nameList[n].LastName}, {nameList[n].FirstName}";
```

## Culture

Always specify a **culture** when comparing, sorting, or formatting strings.
Use `StringComparison.Ordinal` or `StringComparison.OrdinalIgnoreCase` for culture-independent comparisons,
and `StringComparison.CurrentCulture` or `StringComparison.InvariantCulture` when linguistic rules matter.

```csharp
// Correct: explicit culture for comparison
bool isEqual = string.Equals(name1, name2, StringComparison.OrdinalIgnoreCase);
int order = string.Compare(name1, name2, StringComparison.CurrentCulture);
bool starts = name1.StartsWith("prefix", StringComparison.Ordinal);
```

**Incorrect:**

```csharp
// Wrong: no culture specified, uses default which may vary across environments
bool isEqual = name1 == name2;
bool isEqual = name1.Equals(name2);
int order = string.Compare(name1, name2);
bool starts = name1.StartsWith("prefix");
```

## StringBuilder

To append strings in loops, especially with large amounts of text, use a **StringBuilder**:

```csharp
var phrase = "lalalalalalalalalalalalalalalalalalalalalalalalalalalalalala";
var manyPhrases = new StringBuilder();
for (var i = 0; i < 10000; i++)
{
    manyPhrases.Append(phrase);
}
```

**Incorrect:**

```csharp
var result = "";
for (var i = 0; i < 10000; i++)
{
    result += phrase;  // Creates a new string every iteration
}
```
