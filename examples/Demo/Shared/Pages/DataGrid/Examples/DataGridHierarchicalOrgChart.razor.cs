// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.Globalization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FluentUI.Demo.Shared.Pages.DataGrid.Examples;

public partial class DataGridHierarchicalOrgChart
{
    private static readonly Random Random = new();
    private static readonly string[] FirstNames = { "John", "Jane", "Alex", "Emily", "Michael", "Sarah", "David", "Laura", "Robert", "Jessica", "James", "Olivia", "William", "Emma", "Benjamin", "Ava", "Lucas", "Sophia", "Henry", "Isabella", "Alexander", "Mia", "Ethan", "Charlotte", "Michael", "Amelia", "Daniel", "Harper", "Matthew" };
    private static readonly string[] LastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin" };
    private static readonly string[] JobTitles = { "Software Engineer", "Project Manager", "Data Analyst", "Product Manager", "Graphic Designer", "HR Specialist", "Accountant", "Marketing Coordinator", "Sales Representative", "Customer Support", "IT Consultant", "Business Analyst", "Operations Manager", "UX Designer", "DevOps Engineer", "Quality Assurance Engineer", "Financial Analyst", "Content Writer", "Systems Administrator", "Network Engineer" };
    private static readonly string[] Departments = { "Engineering", "Marketing", "Human Resources", "Sales", "IT", "Finance", "Operations", "Customer Support", "Design", "Product Management", "Quality Assurance", "Legal", "Research and Development", "Administration", "Business Development", "Logistics", "Procurement", "Compliance", "Corporate Strategy", "Public Relations" };
    private static readonly string[] Locations = { "Washington", "London", "Tokyo", "Paris", "Berlin", "Ottawa", "Canberra", "Rome", "Madrid", "Beijing", "Moscow", "Brasília", "New Delhi", "Ottawa", "Buenos Aires", "Cairo", "Bangkok", "Seoul", "Jakarta", "Wellington" };
    private static readonly string[] Companies = { "TechCorp", "GlobalSolutions", "BlueSky Enterprises", "Pinnacle Systems", "SynergySoft", "Apex Industries", "QuantumTech", "FutureWave", "Optima Consulting", "Vertex Innovations", "NextGen Solutions", "BrightPath", "Unified Technologies", "Visionary Labs", "EliteWorks", "PrimeTech", "Starline Group", "Skyline Enterprises", "FusionCorp" };

    private PersonGridItem? ceoItem;
    private readonly List<PersonGridItem> items = [];

    protected override void OnInitialized()
    {
        var allPeople = GeneratePersons(30).ToList();

        // Level 0: CEO/Manager
        var ceo = allPeople[0] with { FirstName = "Mads", LastName = "Torgersen", JobTitle = "CEO", Department = "Executive" };
        ceoItem = new PersonGridItem
        {
            Item = ceo,
            IsCollapsed = false
        };
        items.Add(ceoItem);

        for (var i = 1; i <= 3; i++)
        {
            var manager = allPeople[i] with { JobTitle = "Director", Department = "Engineering" };
            var managerItem = new PersonGridItem { Item = manager, Depth = 1, IsCollapsed = i != 2 };
            ceoItem.Children.Add(managerItem);
            items.Add(managerItem);

            // Level 2: Employees
            for (var j = 0; j < 4; j++)
            {
                var employee = allPeople[4 + ((i - 1) * 4) + j];
                var employeeItem = new PersonGridItem { Item = employee, Depth = 2, IsHidden = i != 2 };
                managerItem.Children.Add(employeeItem);
                items.Add(employeeItem);
            }
        }
    }

    private static IEnumerable<Person> GeneratePersons(int count)
    {
        var people = new List<Person>(count);

        for (var i = 0; i < count; i++)
        {
            var id = RandomNumber(10000, 99999).ToString(CultureInfo.InvariantCulture);
            var firstName = FirstNames[Random.Next(FirstNames.Length)];
            var lastName = LastNames[Random.Next(LastNames.Length)];
            var male = Random.Next(2) == 0;
            var initials = $"{firstName[0]}{lastName[0]}";
            var birthDay = new DateTime(RandomNumber(1960, 2000), RandomNumber(1, 12), RandomNumber(1, 28));
            var jobTitle = JobTitles[Random.Next(JobTitles.Length)];
            var company = Companies[Random.Next(Companies.Length)];
            var email = $"{firstName.ToLower()}.{lastName.ToLower()}@{company.ToLower()}.com";
            var department = Departments[Random.Next(Departments.Length)];
            var phone = GeneratePhoneNumber();
            var location = Locations[Random.Next(Locations.Length)];

            people.Add(new Person(id, firstName, lastName, male, initials, birthDay, email, jobTitle, department, company, phone, location));
        }

        return people;
    }

    private void ToggleCEO()
    {
        ceoItem?.IsCollapsed = !ceoItem.IsCollapsed;
    }

    public class PersonGridItem : HierarchicalGridItem<Person, PersonGridItem>
    {
    }

    /// <summary />
    private static string GeneratePhoneNumber()
    {
        return $"({RandomNumber(100, 999)}) {RandomNumber(100, 999)}-{RandomNumber(1000, 9999)}";
    }

    /// <summary />
    private static int RandomNumber(int min, int max)
    {
        return Random.Next(min, max);
    }

    /// <summary>
    /// Definition of a Person
    /// </summary>
    /// <param name="Id">Id</param>
    /// <param name="FirstName">First name</param>
    /// <param name="LastName">Last name</param>
    /// <param name="Male">Male</param>
    /// <param name="Initials">Initials</param>
    /// <param name="BirthDay">Birth day</param>
    /// <param name="Email">Email</param>
    /// <param name="JobTitle">Job title</param>
    /// <param name="Department">Department</param>
    /// <param name="Company">Company</param>
    /// <param name="Phone">Phone</param>
    /// <param name="Location">Location</param>
    public record Person(string Id, string FirstName, string LastName, bool Male, string Initials, DateTime BirthDay, string Email, string JobTitle, string Department, string Company, string Phone, string Location)
    {
        /// <summary>
        /// Gets the age of the person
        /// </summary>
        public int Age => DateTime.Today.Year - BirthDay.Year;

        /// <summary />
        public override string ToString()
        {
            return $"{Id} - {FirstName} {LastName}";
        }
    }

    /// <summary>
    /// Definition of an Employee
    /// </summary>
    /// <param name="Id">Id</param>
    /// <param name="FirstName">First name</param>
    /// <param name="LastName">Last name</param>
    /// <param name="JobTitle">Job title</param>
    public record Employee(string Id, string FirstName, string LastName, string JobTitle) { }

    /// <summary>
    /// Definition of a Department
    /// </summary>
    /// <param name="Id">Department Id</param>
    /// <param name="Name">Department name</param>
    /// <param name="Employees">List of employees</param>
    public record Department(string Id, string Name, IEnumerable<Employee> Employees) { }

    /// <summary>
    /// Definition of a Company
    /// </summary>
    /// <param name="Id">Id</param>
    /// <param name="Name">Company name</param>
    /// <param name="Departments">List of departments</param>
    public record Company(string Id, string Name, IEnumerable<Department> Departments) { };

    /// <summary>
    /// Generates a list of companies with random data.
    /// </summary>
    /// <param name="companyCount">Number of companies</param>
    /// <param name="departmentCount">Number of departments</param>
    /// <param name="employeeCount">Number of employees</param>
    /// <returns></returns>
    public static IEnumerable<Company> GetOrganization(int companyCount, int departmentCount, int employeeCount)
    {
        var companies = new List<Company>(companyCount);

        for (var i = 0; i < companyCount; i++)
        {
            var companyId = "C" + RandomNumber(1000, 9999).ToString(CultureInfo.InvariantCulture);
            var companyName = Companies[Random.Next(Companies.Length)];

            var departments = new List<Department>(departmentCount);
            for (var j = 0; j < departmentCount; j++)
            {
                var departmentId = "D" + RandomNumber(100, 999).ToString(CultureInfo.InvariantCulture);
                var departmentName = Departments[Random.Next(Departments.Length)];

                var employees = new List<Employee>(employeeCount);
                for (var k = 0; k < employeeCount; k++)
                {
                    var employeeId = "E" + RandomNumber(10000, 99999).ToString(CultureInfo.InvariantCulture);
                    var firstName = FirstNames[Random.Next(FirstNames.Length)];
                    var lastName = LastNames[Random.Next(LastNames.Length)];
                    var jobTitle = JobTitles[Random.Next(JobTitles.Length)];

                    employees.Add(new Employee(employeeId, firstName, lastName, jobTitle));
                }

                departments.Add(new Department(departmentId, departmentName, employees));
            }

            companies.Add(new Company(companyId, companyName, departments));
        }

        return companies;
    }
}
