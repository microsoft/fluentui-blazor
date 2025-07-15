// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
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

public class CustomerComparer : IEqualityComparer<Customer>
{
    public static readonly CustomerComparer Instance = new();

    public bool Equals(Customer? x, Customer? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        return x.Id == y.Id &&
               x.Name == y.Name;
    }

    public int GetHashCode(Customer obj) => HashCode.Combine(obj.Id, obj.Name);
}
