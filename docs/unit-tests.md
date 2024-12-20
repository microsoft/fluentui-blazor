# How to create Unit Tests for the FluentUI Blazor project?

- [Overview](#overview)
- [Unit Tests](#unit-tests)
- [Why unit test?](#why-unit-test)
- [Six Best practices](#six-best-practices)
- [Code Coverage](#code-coverage)
- [FluentUI Blazor Unit Tests](#fluentui-blazor-unit-tests)
   - [Test File Template](#test-file-template)
   - [Example with a simple property](#example-with-a-simple-property)
   - [Example with an event handler](#example-with-an-event-handler)
   - [Example with parameters](#example-with-parameters)

## Overview

In the dynamic field of Blazor web development, creating applications that are both innovative and reliable is a top priority.
As the complexity of our projects increases, so does the risk of bugs and malfunctions.
This is where the importance of unit testing comes into its own.

**Unit testing** offers a systematic approach to the verification of small individual units of code.
By subjecting these units to a variety of scenarios and inputs, developers can ensure that their code behaves as expected, 
identify weaknesses and detect problems early in the development cycle. In the context of Blazor, 
where backend and frontend logic converge, unit testing plays a central role in maintaining stability and performance.

In this article, we explore the world of unit testing used in the FluentUI Blazor projects.
We'll dive into the basic concepts, understand its importance in maintaining code quality,
and discover best practices for creating tests quickly and efficiently.

## Unit Tests

Automated testing is a great way to ensure that the application code does what its developers want it to do.

There are different types of tests that can be used to validate the code. 3 of them are:

1. **Unit tests**

   A unit test is a test that exercises individual software components or methods, also known as "unit of work". Unit tests should only test code within the developer's control. 
   They do not test infrastructure concerns. Infrastructure concerns include interacting with databases, file systems, and network resources.

2. **Integration tests**

   An integration test differs from a unit test in that it exercises two or more software components' ability to function together, also known as their "integration." These tests 
   operate on a broader spectrum of the system under test, whereas unit tests focus on individual components. Often, integration tests do include infrastructure concerns.

3. **Load tests**

   A load test aims to determine whether or not a system can handle a specified load, for example, the number of concurrent users using an application and the app's ability to 
   handle interactions responsively.

<br /><br /><br />

## Why unit test?

- **Less time performing functional tests**

   Functional tests are expensive. They typically involve opening up the application and performing a series of steps that you (or someone else), must follow in order to validate the expected behavior. These steps may not always be known to the tester, which means they will have to reach out to someone more knowledgeable in the area in order to carry out the test. Testing itself could take seconds for trivial changes, or minutes for larger changes. Lastly, this process must be repeated for every change that you make in the system.

  Unit tests, on the other hand, take milliseconds, can be run at the press of a button, and don't necessarily require any knowledge of the system at large. Whether or not the test passes or fails is up to the test runner, not the individual.

- **Protection against regression**

  Regression defects are defects that are introduced when a change is made to the application. It is common for testers to not only test their new feature but also features that existed beforehand in order to verify that previously implemented features still function as expected.

  With unit testing, it's possible to rerun your entire suite of tests after every build or even after you change a line of code. Giving you confidence that your new code does not break existing functionality.

- **Executable documentation**

  It may not always be obvious what a particular method does or how it behaves given a certain input. You may ask yourself: How does this method behave if I pass it a blank string? Null?

  When you have a suite of well-named unit tests, each test should be able to clearly explain the expected output for a given input. In addition, it should be able to verify that it actually works.

- **Less coupled code**

  When code is tightly coupled, it can be difficult to unit test. Without creating unit tests for the code that you're writing, coupling may be less apparent.

  Writing tests for your code will naturally decouple your code, because it would be more difficult to test otherwise.


<br />

> Copied from [Unit testing Best Practice](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices).


<br /><br /><br />

## Six Best practices

> See [detailed best practices](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices).


1. **Naming your tests**

   The name of your test should consist of three parts:
   - The name of the **method** being tested.
   - The **scenario** under which it's being tested.
   - The **expected behavior** when the scenario is invoked.

   **Why?** 

   - Naming standards are important because they explicitly express the intent of the test.

   **Example:**
   ```csharp
   [Fact]
   public void Add_SingleNumber_ReturnsSameNumber()
   ```
   <br />

2. **Arranging your tests**

   _Arrange_, _Act_, _Assert_ is a common pattern when unit testing. As the name implies, it consists of three main actions:

   - Arrange your objects, creating and setting them up as necessary.
   - Act on an object.
   - Assert that something is as expected.

   **Why?**

   - Clearly separates what is being tested from the arrange and assert steps.
   - Less chance to intermix assertions with "Act" code.

   **Example:**
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
   <br />

3. **Write minimally passing tests**

   The input to be used in a unit test should be the simplest possible in order to verify the behavior that you are currently testing.

   **Why?**

   - Tests become more resilient to future changes in the codebase.
   - Closer to testing behavior over implementation.

   <br />

4. **Avoid logic in tests**

   When writing your unit tests avoid manual string concatenation and logical conditions such as if, while, for, switch, etc.

   **Why?**

   - Less chance to introduce a bug inside of your tests.
   - Focus on the end result, rather than implementation details.

   **Example:**
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
   <br />

5. **Prefer helper methods to setup and teardown**

   If you require a similar object or state for your tests, prefer a helper method than leveraging `constructor` or `Setup` and `Teardown` attributes if they exist.

   **Why?**

   - Less confusion when reading the tests since all of the code is visible from within each test.
   - Less chance of setting up too much or too little for the given test.
   - Less chance of sharing state between tests, which creates unwanted dependencies between them.

   **Example:**
   ```csharp
   // Bad
   // public StringCalculatorTests()
   // {
   //    stringCalculator = new StringCalculator();
   // }

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
   <br />

6. **Avoid multiple acts**

   When writing your tests, try to only include **one Act per test**. 
   Common approaches to using only one act include:

   Create a separate test for each act or use parameterized tests.

   **Why?**
   - When the test fails it is not clear which Act is failing.
   - Ensures the test is focussed on just a single case.
   - Gives you the entire picture as to why your tests are failing.

   <br /><br />
   
## Code Coverage

Unit tests help to ensure functionality and provide a means of verification for refactoring efforts. Code coverage is a measurement of the amount of code that is run by unit tests - either lines, branches, or methods. 

This chapter discusses the usage of code coverage for unit testing with **Coverlet** and report generation using **ReportGenerator**.

1. **Requirements** (already included in the project)

   Include the NuGet Packages `coverlet.msbuild` and `coverlet.collector` in your Unit Tests Project (csproj).

   ```xml
   <PackageReference Include="coverlet.msbuild" />
   <PackageReference Include="coverlet.collector" />
   ```

2. **Tools**

   To generate a code coverage report **locally**, install these tools: [Coverlet](https://dotnetfoundation.org/projects/coverlet) and [ReportGenerator](https://reportgenerator.io).

   ```
   dotnet tool install --global coverlet.console --version 6.0.2
   dotnet tool install --global dotnet-reportgenerator-globaltool --version 5.3.7
   ```
   
   Use this command to list and verify existing installed tools:
   ```
   dotnet tool list --global
   ```

3. **Start a code coverage**

   You can start the unit test and code coverage tool using this command (in the solution folder).
   Each unit test project folders will contain a file `coverage.cobertura.xml`.

   ```
   dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
   ```

4. **Generate a report**

   Merge and convert all `Cobertura.xml` files to an HTML report (change the sample folder Temp/FluentUI/Coverage).

   ```
   reportgenerator "-reports:coverage.cobertura.xml" "-targetdir:C:\Temp\FluentUI\Coverage" -reporttypes:HtmlInline_AzurePipelines -classfilters:"-Microsoft.FluentUI.AspNetCore.Components.DesignTokens.*"
   ```

> **⚠️ Note:** The `_StartCodeCoverage.cmd` file contains these two command lines.
> Using this [Mads Kristensen's VS extension](https://github.com/madskristensen/OpenCommandLine), you can easily execute this .cmd file
> from Visual Studio 2022.

<br /><br />

## FluentUI Blazor Unit Tests

In the [FluentUI.Blazor](https://github.com/microsoft/fluentui-blazor) project, 
all Blazor unit tests use the [bUnit](https://bunit.dev/) library. 
Its objective is to facilitate the writing of _complete_ and _stable_ unit tests.

With bUnit, you can:
- Setup and define components under tests using C# or Razor syntax
- Verify outcomes using semantic HTML comparer
- Interact with and inspect components as well as trigger event handlers
- Pass parameters, cascading values and inject services into components under test
- Mock `IJSRuntime`, Blazor authentication and authorization, and others

bUnit builds on top of existing unit testing frameworks such as xUnit, NUnit, and MSTest

### Test File Template

To create a unit test in **FluentUI Blazor**, you need to create a file
`<ComponentName>Tests.razor` containing this.

```csharp
@using Xunit;
@inherits TestContext
@code
{
    public <ComponentName>Tests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton<LibraryConfiguration>();
    }

    [Fact]
    public void <ComponentName>_Default()
    {
        ...
    }
}
```

### Example with a simple property

In the [FluentUI.Blazor](https://github.com/microsoft/fluentui-blazor) project, we added a `Verify` method to 
generate a **.received.html** or **.received.razor.html** file which will be compared
to a predefined **.verified.html** or **.verified.razor.html** file.

```csharp
[Fact]
public void FluentButton_Color()
{
    // Arrange && Act
    var cut = Render(@<FluentButton Id="MyButton" Color="#00ff00">
                          My button
                      </FluentButton>);

    // Assert
    cut.Verify();
}
```

```html
<!-- FluentButton_Color.verified.razor.html -->
<fluent-button style="color: #00ff00;"
               id="xxx">
   My button
</fluent-button>
```

> **⚠️ Note:** When modifying components, it's easy to
> check where the modifications are located: using a file comparison tool.

### Example with an event handler

In this example, a FluentButton component is tested to verify that the `OnClick` event is triggered.

```csharp
[Fact]
public void FluentButton_OnClick()
{
    bool clicked = false;

    // Arrange
    var cut = Render(@<FluentButton OnClick="@(e => { clicked = true; } )">
                          My button
                      </FluentButton>);

    // Act
    cut.Find("fluent-button").Click();

    // Assert
    Assert.True(clicked);
}
```

### Example with parameters

You can use the xUnit `[InlineData]` attribute to test several scenarios.
La méthode `Verify` dispose alors d'un attribut `suffix` pour générer plusieurs fichiers

  - `FluentButtonTests.FluentButton_FormIdAttribute-form-id-attribute.verified.razor.html`
  - `FluentButtonTests.FluentButton_FormIdAttribute-[null].received.razor.html`
  - `FluentButtonTests.FluentButton_FormIdAttribute-[empty].verified.razor.html`
  - `FluentButtonTests.FluentButton_FormIdAttribute-[space].received.razor.html`

```csharp
[Theory]
[InlineData("form-id-attribute")]
[InlineData(null)]
[InlineData("")]
[InlineData(" ")]
public void FluentButton_FormIdAttribute(string? formId)
{
    // Arrange && Act
    var cut = Render(@<FluentButton FormId="@formId">
                         fluent-button
                      </FluentButton>);

    // Assert
    cut.Verify(suffix: formId);
}
```

