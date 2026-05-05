---
title: Allowed and Denied Operators
impact: MEDIUM
impactDescription: Using obscure operators reduces code readability
tags: code, operators, clean-code
---

## Operators Allowed

To keep clean code, use only these operators.

### Allowed

- **Primary:** `x.y, f(x), a[x], x++, x--, new, typeof, default(T)`
- **Unary:** `+, -, !, (T)x, await`
- **Math:** `x * y, x / y, x % y, x + y, x - y`
- **Testing:** `x < y, x > y, x <= y, x >= y, x == y, x != y, is, as`
- **Conditional:** `x && y, x || y, x | y, x ? y : z, x?.y`
- **Null-coalescing:** `x ?? y`
- **Assignment:** `x = y, x += y, x -= y, x => y`

### Denied

- **Primary:** `checked, unchecked, sizeof, ->`
- **Unary:** `++x, --x, ~x, &x, *x`
- **Shift:** `x << y, x >> y`
- **Logical:** `x & y, x ^ y`
- **Assignment:** `x *= y, x /= y, x %= y, x &= y, x |= y, x ^= y, x <<= y, x >>= y`

Reference: [C# Operators](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators)
