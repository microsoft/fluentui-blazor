// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.PropertyColumn;
public partial class PropertyColumnFormatterTests
{
    private protected record Person(int PersonId, CustomFormattable Name, DateOnly? BirthDate, string NickName)
    {
        public bool Selected { get; set; }
    };

    private protected class CustomFormattable : IFormattable
    {
        private readonly string _value;
        public CustomFormattable(string value) => _value = value;
        public string ToString(string? format, IFormatProvider? provider) =>
            string.IsNullOrEmpty(format) ? _value : string.Format(format, _value);
    }

    private readonly IList<Person> _people =
    [
        new Person(1, new("Jean Martin"), new DateOnly(1985, 3, 16), string.Empty),
        new Person(2, new("Kenji Sato"), new DateOnly(2004, 1, 9), string.Empty),
        new Person(3, new("Julie Smith"), new DateOnly(1958, 10, 10), string.Empty),
    ];

    private protected IQueryable<Person> People => _people.AsQueryable();

    public PropertyColumnFormatterTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(LibraryConfiguration.ForUnitTests);

        var keycodeService = new KeyCodeService();
        Services.AddScoped<IKeyCodeService>(factory => keycodeService);
    }
}
