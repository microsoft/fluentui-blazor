---
name: csharp-naming-conventions
description: |
  C# and .NET naming conventions, coding best practices, layout rules,
  unit testing, and Blazor component guidelines. Use when
  writing, reviewing, or refactoring C# / .NET / Blazor code or writing 
  unit tests with xUnit and bUnit.
license: MIT
metadata:
  author: dvoituron
  version: "0.0.4"
---

# C# Naming Conventions and Best Practices

Comprehensive coding guidelines for .NET and C# developments, covering naming,
layout, code patterns, unit testing, and Blazor conventions.
Prioritized by impact to guide automated code generation and review.

## When to Apply

Reference these guidelines when:
- Writing new C# / .NET code
- Reviewing or refactoring existing C# code
- Naming classes, methods, variables, properties, or parameters
- Formatting and laying out source files
- Working with async/await patterns
- Writing if/else, string, exception, or conversion logic
- Writing or reviewing unit tests (xUnit, bUnit)
- Building Blazor components and pages
- Organizing project file structures

## Rule Categories by Priority

| Priority | Category | Impact | Prefix |
|----------|----------|--------|--------|
| 1 | Naming & Casing | CRITICAL | `naming-` |
| 2 | Code Layout & Formatting | HIGH | `layout-` |
| 3 | Code Patterns | HIGH | `code-` |
| 4 | Unit Testing | HIGH | `testing-` |
| 5 | Blazor Conventions | MEDIUM-HIGH | `blazor-` |

## Quick Reference

### 1. Naming & Casing (CRITICAL)

- [naming-casing](rules/naming-casing.md) - Pascal case and Camel case conventions for all C# identifiers
- [naming-files](rules/naming-files.md) - File organization: one class per file, namespace matches folder
- [naming-items](rules/naming-items.md) - Explicit, unabbreviated, English-only names without prefixes
- [naming-async](rules/naming-async.md) - Postfix async methods with Async, eliding guidelines

### 2. Code Layout & Formatting (HIGH)

- [layout-formatting](rules/layout-formatting.md) - Indentation, braces, line length, and formatting rules
- [layout-members-order](rules/layout-members-order.md) - Member ordering within classes and no #region usage

### 3. Code Patterns (HIGH)

- [code-variables](rules/code-variables.md) - Variable declarations, var usage, access modifiers
- [code-comments](rules/code-comments.md) - Commenting conventions and XML documentation
- [code-using-namespace](rules/code-using-namespace.md) - Using directives and file-scoped namespaces
- [code-if-pattern](rules/code-if-pattern.md) - If braces, no-else-return, and guard pattern
- [code-strings](rules/code-strings.md) - String interpolation and StringBuilder usage
- [code-exceptions](rules/code-exceptions.md) - Exception handling and avoiding generic catches
- [code-operators](rules/code-operators.md) - Allowed and denied C# operators
- [code-conversion](rules/code-conversion.md) - Data type conversion rules, never use ToString()
- [code-datetime](rules/code-datetime.md) - DateTimeProvider for testable date/time code
- [code-static](rules/code-static.md) - Avoid static classes except for extension methods

### 4. Unit Testing (HIGH)

- [testing-practices](rules/testing-practices.md) - Seven best practices for unit tests
- [testing-coverage](rules/testing-coverage.md) - Code coverage with Coverlet and ReportGenerator
- [testing-blazor](rules/testing-blazor.md) - Blazor unit tests with bUnit and Verifier

### 5. Blazor Conventions (MEDIUM-HIGH)

- [blazor-components](rules/blazor-components.md) - Component naming, routing, and file structure
- [blazor-structure](rules/blazor-structure.md) - Project folder organization with SOLID principles
- [blazor-code](rules/blazor-code.md) - Methods, properties order, inject, and nullable defaults
- [blazor-css](rules/blazor-css.md) - CSS isolation, root class and attribute selectors
- [blazor-javascript](rules/blazor-javascript.md) - JavaScript interop and collocated JS files
- [blazor-css](rules/blazor-css.md) - CSS isolation and ::deep pseudo-element usage
- [blazor-performance](rules/blazor-performance.md) - Rendering, cascading parameters, and optimization

## How to Use

Use the linked rule files above for detailed explanations and code examples.
Each rule file contains:
- Brief explanation of why it matters
- Correct code examples
- Incorrect patterns to avoid (when applicable)
- References to external guidelines
