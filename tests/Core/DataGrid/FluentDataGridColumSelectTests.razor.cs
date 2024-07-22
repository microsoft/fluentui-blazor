// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using Bunit;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.DataGrid;

public partial class FluentDataGridColumSelectTests
{
    public FluentDataGridColumSelectTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(LibraryConfiguration.ForUnitTests);

        // Register Service
        var keycodeService = new KeyCodeService();
        Services.AddScoped<IKeyCodeService>(factory => keycodeService);
    }

    private record Person(int PersonId, string Name, DateOnly BirthDate)
    {
        public bool Selected { get; set; }
    };

    private readonly IQueryable<Person> People = new[]
    {
        new Person(1, "Jean Martin", new DateOnly(1985, 3, 16)),
        new Person(2, "Kenji Sato", new DateOnly(2004, 1, 9)),
        new Person(3, "Julie Smith", new DateOnly(1958, 10, 10)),
    }.AsQueryable();
}
