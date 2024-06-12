// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Extensions;
public record Customer(int Id, string Name)
{
    public override string ToString()
    {
        return $"{Id}-{Name}";
    }
}

public static class Customers
{
    public static IEnumerable<Customer> Get()
    {
        yield return new Customer(1, "Denis Voituron");
        yield return new Customer(2, "Vincent Baaij");
        yield return new Customer(3, "Bill Gates");
    }
}

