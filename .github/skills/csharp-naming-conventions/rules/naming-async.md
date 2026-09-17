---
title: Async Method Naming and Eliding
impact: HIGH
impactDescription: Inconsistent async naming misleads callers about method behavior
tags: naming, async, await, task
---

## Postfix Asynchronous Methods with `Async`

Methods that return commonly awaitable types (`Task`, `Task<T>`, `ValueTask`, `ValueTask<T>`) should have names ending with **Async**.

Reference: [AV1755](https://csharpcodingguidelines.com//naming-guidelines/#AV1755), [VSTHRD200](https://microsoft.github.io/vs-threading/analyzers/VSTHRD200.html)

**Correct:**

```csharp
async Task DoSomethingAsync()
{
    await MyFirstMethodAsync();
    await MySecondMethodAsync();
}

bool DoSomethingElse()
{
    return false;
}
```

## Omitting `Async` and `Await`

Omitting `async`/`await` is more efficient — the compiler skips generating the async state machine, reducing GC pressure and CPU instructions.

**Guidelines:**
- Do **not** elide by default. Use `async`/`await` for natural, easy-to-read code.
- Do consider eliding when the method is **just** a passthrough or overload.

**Correct (passthrough):**

```csharp
Task DoSomethingAsync()
{
    return MyMethodAsync();
}
```

Reference:
- [Eliding Async and Await](https://blog.stephencleary.com/2016/12/eliding-async-await.html)
- [Async Performance](https://docs.microsoft.com/en-us/archive/msdn-magazine/2011/october/asynchronous-programming-async-performance-understanding-the-costs-of-async-and-await)
