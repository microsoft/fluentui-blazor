---
title: Layout and Formatting Conventions
impact: HIGH
impactDescription: Inconsistent formatting distracts readers from code logic
tags: layout, formatting, indentation, braces, line-length
---

## Layout Conventions

Good layout uses formatting to emphasize the structure of your code and to make it easier to read.

### Indentation and Spacing
- Use **four-character** indents, **tabs saved as spaces**.
- Write only **one statement per line**.
- Write only **one declaration per line**.
- Indent continuation lines by **one tab stop** (four spaces).
- Add at least **one blank line** between method and property definitions.

**Correct:**

```csharp
public class Foo
{
    public int Width { get; set; }

    public void DoSomething(int width, int height)
    {
        int x = width * height;
    }
}
```

### Braces and Parentheses
- Always put **opening and closing curly braces on a new line**.
- Parentheses at the end of the line (without space before `(`).

**Correct:**

```csharp
public class Math
{
    private void Increment()
    {
    }
}
```

### Line Length
- Keep the length of each line under **130 characters**. See [AV2400](https://csharpcodingguidelines.com/layout-guidelines/#AV2400).
- Break long lines **after the period sign**:

```csharp
var dto = service.GetEmployees()
                 .Where(i => i.Id > 1000)
                 .OrderBy(i => i.Name)
                 .ToArray();
```

### Keyword and Operator Spacing
- Keep **one space between keywords** and expressions: `if (condition == null)`.
- Do not add spaces after `(` and before `)`.
- Add a **space around operators**: `var z = x + y;`

### Object and Collection Initializers
- Don't indent initializers. Initialize **each property on a new line**:

```csharp
var dto = new ConsumerDto
{
    Id = 123,
    Name = "Microsoft",
    PartnerShip = PartnerShip.Gold,
    ShoppingCart = new()
    {
        VisualStudio = 1,
    },
};
```

### Lambda Statements
- Don't indent **lambda statement** blocks:

```csharp
methodThatTakesAnAction.Do(source =>
{
    // do something like this
});
```

### Expression-Bodied Members
- Keep on one line. Break long lines **after the arrow sign**:

```csharp
private string GetShortText => "ABC ABC ABC";
private string GetLongText =>
    "ABC ABC ABC ABC ABC ABC ABC ABC ABC ABC ABC ABC ABC ABC ABC ABC";
```

Reference: [Microsoft Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
