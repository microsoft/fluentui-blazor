// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Globalization;
using FluentUI.Demo.SampleData.Resources;

namespace FluentUI.Demo.SampleData;

/// <summary>
/// Returns a list of people with random data.
/// </summary>
public partial class People
{
    private const string IMG_PREFIX_JPG = "data:image/jpeg;base64,";

    private static readonly Random Random = new();
    private static readonly string[] FirstNames = { "John", "Jane", "Alex", "Emily", "Michael", "Sarah", "David", "Laura", "Robert", "Jessica", "James", "Olivia", "William", "Emma", "Benjamin", "Ava", "Lucas", "Sophia", "Henry", "Isabella", "Alexander", "Mia", "Ethan", "Charlotte", "Michael", "Amelia", "Daniel", "Harper", "Matthew" };
    private static readonly string[] LastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin" };
    private static readonly string[] JobTitles = { "Software Engineer", "Project Manager", "Data Analyst", "Product Manager", "Graphic Designer", "HR Specialist", "Accountant", "Marketing Coordinator", "Sales Representative", "Customer Support", "IT Consultant", "Business Analyst", "Operations Manager", "UX Designer", "DevOps Engineer", "Quality Assurance Engineer", "Financial Analyst", "Content Writer", "Systems Administrator", "Network Engineer" };
    private static readonly string[] Departments = { "Engineering", "Marketing", "Human Resources", "Sales", "IT", "Finance", "Operations", "Customer Support", "Design", "Product Management", "Quality Assurance", "Legal", "Research and Development", "Administration", "Business Development", "Logistics", "Procurement", "Compliance", "Corporate Strategy", "Public Relations" };
    private static readonly string[] Locations = { "Washington", "London", "Tokyo", "Paris", "Berlin", "Ottawa", "Canberra", "Rome", "Madrid", "Beijing", "Moscow", "Brasília", "New Delhi", "Ottawa", "Buenos Aires", "Cairo", "Bangkok", "Seoul", "Jakarta", "Wellington" };
    private static readonly string[] Companies = { "TechCorp", "GlobalSolutions", "BlueSky Enterprises", "Pinnacle Systems", "SynergySoft", "Apex Industries", "QuantumTech", "FutureWave", "Optima Consulting", "Vertex Innovations", "NextGen Solutions", "BrightPath", "Unified Technologies", "Visionary Labs", "EliteWorks", "PrimeTech", "Starline Group", "Skyline Enterprises", "FusionCorp" };

    /// <summary>
    /// Gets a list of some countries.
    /// </summary>
    public static IEnumerable<string> Countries => Olympics2024.Countries.Select(c => c.Name).Order();

    /// <summary>
    /// Gets a list of first and last name.
    /// </summary>
    public static IEnumerable<string> Names => GeneratePersons(10).Select(p => $"{p.FirstName} {p.LastName}").Order();

    /// <summary>
    /// Gets a list of 8 Face pictures, embedded in the library.
    /// </summary>
    public static IEnumerable<string> FacesForImg
    {
        get
        {
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face1.jpg").ToBase64();
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face2.jpg").ToBase64();
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face3.jpg").ToBase64();
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face4.jpg").ToBase64();
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face5.jpg").ToBase64();
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face6.jpg").ToBase64();
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face7.jpg").ToBase64();
            yield return IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture("Face8.jpg").ToBase64();
        }
    }

    /// <summary>
    /// Gets a list of 50 persons with random data.
    /// </summary>
    public static IEnumerable<Person> Persons => GeneratePersons(50);

    /// <summary>
    /// Gets a list of 50 persons with random data and a very long name to validate components.
    /// </summary>
    public static IEnumerable<Person> PersonsWithVeryLongName => GeneratePersons(50).Union([VeryLongNamePerson]);

    /// <summary>
    /// Get a person with a very long name to validate components.
    /// </summary>
    public static readonly Person VeryLongNamePerson = new Person(
        Id: "91",
        FirstName: "Jean",
        LastName: "With a very long name to validate components",
        Male: true,
        Initials: "JW",
        BirthDay: new DateTime(1984, 4, 27),
        ImageUrl: "https://thispersondoesnotexist.com/?id=91",
        Email: "jean@microsoft.com",
        JobTitle: "Senior Software Engineer",
        Department: "Engineering",
        Company: "Microsoft",
        Phone: "(425) 555-1234",
        Location: "Redmond");

    /// <summary>
    /// Generates a list of persons with random data.
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public static IEnumerable<Person> GeneratePersons(int count)
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
            var imageUrl = $"https://thispersondoesnotexist.com/?id={id}";
            var jobTitle = JobTitles[Random.Next(JobTitles.Length)];
            var company = Companies[Random.Next(Companies.Length)];
            var email = $"{firstName.ToLower()}.{lastName.ToLower()}@{company.ToLower()}.com";
            var department = Departments[Random.Next(Departments.Length)];
            var phone = GeneratePhoneNumber();
            var location = Locations[Random.Next(Locations.Length)];

            people.Add(new Person(id, firstName, lastName, male, initials, birthDay, imageUrl, email, jobTitle, department, company, phone, location));
        }

        return people;
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
    /// <param name="ImageUrl">Image URL: https://thispersondoesnotexist.com/ to get a 1024x1024 picture. These photos may be identical.</param>
    /// <param name="Email">Email</param>
    /// <param name="JobTitle">Job title</param>
    /// <param name="Department">Department</param>
    /// <param name="Company">Company</param>
    /// <param name="Phone">Phone</param>
    /// <param name="Location">Location</param>
    public record Person(string Id, string FirstName, string LastName, bool Male, string Initials, DateTime BirthDay, string ImageUrl, string Email, string JobTitle, string Department, string Company, string Phone, string Location)
    {
        /// <summary>
        /// Gets the image as a base64 string
        /// </summary>
        public string ImageBase64 => IMG_PREFIX_JPG + ResourceReader.EmbeddedPicture($"AI.FaceAI-{RandomNumber(1, 50)}.jpg").ToBase64();

        /// <summary>
        /// Gets the age of the person
        /// </summary>
        public int Age => DateTime.Today.Year - BirthDay.Year;
    }
}
