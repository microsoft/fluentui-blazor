using System.ComponentModel;

namespace FluentUI.Demo.Shared.SampleData;

public enum EmployeeType
{   
    Manager,

    [Description("Sales Representative")]
    SalesRepresentative,

    Engineer,
    Technician,

    [Description("Customer Support")]
    CustomerSupport
}

public record Person(int PersonId, string CountryCode, string FirstName, string LastName, DateOnly BirthDate, string Picture, EmployeeType  EmployeeType)
{
    public override string ToString() => $"{FirstName} {LastName} ({BirthDate}, {CountryCode}, {EmployeeType})";
}

public class SimplePerson
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public int Age { get; set; }
}
