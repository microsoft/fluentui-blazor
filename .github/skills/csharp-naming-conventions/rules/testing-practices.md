---
title: Unit Testing Best Practices
impact: HIGH
impactDescription: Poor tests provide false confidence and miss regressions
tags: testing, unit-test, xunit, aaa-pattern, naming
---

## Unit Testing Best Practices

### 1. Naming Your Tests

The name of your test should consist of three parts:
- The name of the **method** being tested.
- The **scenario** under which it's being tested.
- The **expected behavior** when the scenario is invoked.

```csharp
[Fact]
public void Add_SingleNumber_ReturnsSameNumber()
```

### 2. Arranging Your Tests (AAA Pattern)

Use _Arrange_, _Act_, _Assert_ pattern consistently:

```csharp
[Fact]
public void Add_EmptyString_ReturnsZero()
{
    // Arrange
    var stringCalculator = new StringCalculator();

    // Act
    var actual = stringCalculator.Add("");

    // Assert
    Assert.Equal(0, actual);
}
```

### 3. Write Minimally Passing Tests

The input to be used in a unit test should be the simplest possible to verify the behavior being tested. Tests become more resilient to future changes and closer to testing behavior over implementation.

### 4. Avoid Logic in Tests

Avoid manual string concatenation and logical conditions (`if`, `while`, `for`, `switch`) in tests. Use parameterized tests instead:

```csharp
[Theory]
[InlineData("0,0,0", 0)]
[InlineData("0,1,2", 3)]
[InlineData("1,2,3", 6)]
public void Add_MultipleNumbers_ReturnsSumOfNumbers(string input, int expected)
{
    var stringCalculator = new StringCalculator();

    var actual = stringCalculator.Add(input);

    Assert.Equal(expected, actual);
}
```

### 5. Prefer Helper Methods to Setup and Teardown

Prefer a helper method over `constructor`, `Setup`, or `Teardown` attributes:

```csharp
[Fact]
public void Add_TwoNumbers_ReturnsSumOfNumbers()
{
    var stringCalculator = CreateDefaultStringCalculator();

    var actual = stringCalculator.Add("0,1");

    Assert.Equal(1, actual);
}

private StringCalculator CreateDefaultStringCalculator()
{
    return new StringCalculator();
}
```

### 6. Avoid Multiple Acts

Try to include only **one Act per test**. Create a separate test for each act or use parameterized tests.

### 7. Mock DateTime with DateTimeProvider

Use `DateTimeProvider` instead of `DateTime.Now` for deterministic tests:

```csharp
public void CurrentDate_Today_ReturnsDay()
{
    var MOCK_TODAY = new DateTime(2018, 5, 26);
    using (var context = new DateTimeProviderContext(MOCK_TODAY))
    {
        var now = DateTimeProvider.Now;
        Assert.Equal(26, now.Day);
    }
}
```

Reference: [Unit Testing Best Practices](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
