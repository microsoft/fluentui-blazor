# DateTimeProvider

## Introduction

Today, a developer came to see me to ask how to test his code that contains a reference to `DateTime.Now`.
This is because your application sometimes processes its data differently, depending on the current date.

For example, how do you check the following code, which depends on the current quarter?

```csharp
int quarter = (DateTime.Today.Month - 1) / 3 + 1;
if (quarter <= 2)
    ...
else 
    ...
```

The main problem is that the date obviously changes every day. And the quarterly calculation will run
unit tests today, but maybe not tomorrow.

## Dependency injection

A clean way to do this, if you're using **dependency injection** (IoC) in your project, 
is to create an interface to inject wherever you want to get the system's current date, 
and define it, as required, in the unit tests.

```csharp
public interface IDateTimeHelper
{
    DateTime GetDateTimeNow();
}

public class DateTimeHelper : IDateTimeHelper
{
    public DateTime GetDateTimeNow()
    {
        return DateTime.Now;
    }
}
```

This works fine, as long as you use dependency injection. 
But some people don't like injecting such a simple class. 
Plus, what if you have existing code and just want to rewrite it to replace the date and time?

## Ambient Context Model

To avoid injecting such a simple class and to simplify updating existing code, 
We propose a solution that uses the **Ambient Context Model**.

To do this, use a `DateTimeProvider` class that determines the current context of use: 
`DateTime.Now` is replaced by `DateTimeProvider.Now` in your code.

```csharp
int trimester = (DateTimeProvider.Today.Month - 1) / 3 + 1;
```

This **provider** returns the system's current date. 
However, by using it in a unit test, we can adapt the context to specify a predefined date.

## Unit Test - Simulate a date

```csharp
var result = DateTimeProvider.Now;      // Returns DateTime.Now

var fakeDate = new DateTime(2018, 5, 26);
using (var context = new DateTimeProviderContext(fakeDate))
{
    var result = DateTimeProvider.Now;  // Returns 2018-05-26
}
```

As you can see from the code above, the only thing we need to do to simulate the system's current date is to wrap 
our method call in a using block. This creates a new instance of **DateTimeProviderContext** and specifies 
the **desired date** as an argument to the constructor. That's it!

## Unit Test - Sequential dates

Some uses require dates that evolve with each call. For example, to simulate a kind of `StopWatch`.
You can define a function (Lambda) that returns a different date for each call.

For example, using the call index as the starting element :

```csharp
using var contextSequence = new DateTimeProviderContext(i => i switch
{
    0 => new DateTime(2018, 5, 26),
    1 => new DateTime(2019, 5, 27),
    _ => DateTime.MinValue,
});
```

However, if you already know the list of dates to be returned for each call, you can define them in a list :

```csharp
using var contextSequence = new DateTimeProviderContext(
[
    new DateTime(2018, 5, 26),
    new DateTime(2019, 5, 27)
]);
```

If you make more than 2 calls, an `InvalidOperationException` exception will be thrown.
This indicates that you have exhausted the defined return sequence and that there is no return value for the next call.

## Unit Test - Requirement to use context

Some uses (like Unit Tests) require that the date be defined in a context.
You can define this requirement setting the `DateTimeProvider.RequiredActiveContext` property to `true`.

## Bechmarks

These benchmarks results check the performance of `DateTimeProvider` against `DateTime.Now`.
- **SystemDateTime**: `DateTime.Now`
- **DateTimeProvider**: `DateTimeProvider.Now`

These results show that `DateTimeProvider` performs just as well as `DateTime.Now`.

```
BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3321)
11th Gen Intel Core i7-11850H 2.50GHz, 1 CPU, 16 logical and 8 physical cores```
.NET SDK 9.0.200-preview.0.25057.12
  [Host]     : .NET 9.0.2 (9.0.225.6610), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 9.0.2 (9.0.225.6610), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


| Method               | Mean     | Error    | StdDev   | Median   |
|--------------------- |---------:|---------:|---------:|---------:|
| SystemDateTime       | 69.06 ns | 1.774 ns | 5.174 ns | 68.09 ns |
| DateTimeProvider     | 68.78 ns | 1.896 ns | 5.561 ns | 67.79 ns |
```
