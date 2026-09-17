---
title: If Pattern
impact: HIGH
impactDescription: Inconsistent conditional patterns reduce readability and increase bugs
tags: code, if, braces, guard-pattern, conditionals
---

## If Pattern

Prefer **positive conditionals** and method names: `if (a > b)` is better than `if (a !<= b)`;
and `if (a == b)` is better than `if (a != b)`.

### a. Always Use Braces

Always use braces with `if` statement, including when you have only one line of code.

**Correct:**

```csharp
if (condition)
{
    DoSomething();
}
else
{
    DoSomethingOther();
}
```

```csharp
if (condition)
{
    DoSomething();
}
else if (condition)
{
    DoSomethingOther();
}
else
{
    DoSomethingOtherAgain();
}
```

**Incorrect:**

```csharp
if (condition)
    DoSomething();
else
    DoSomethingOther();
```

### b. No Else Return

Skip the `else` block if the `if` block always executes a `return` statement.

**Correct:**

```csharp
if (condition)
{
    return ReturnSomething();
}

return ReturnSomethingOther();
```

### c. Guard Pattern

The guard clause is a block of code at the beginning of the function that returns early if a condition is met.

**Correct:**

```csharp
if (isSeparated)
{
    return null;
}

return ReturnSomething();
```

Reference:
- [No Else Return](https://www.samanthaming.com/tidbits/23-no-else-return)
- [Guard Clauses](https://refactoring.guru/replace-nested-conditional-with-guard-clauses)
