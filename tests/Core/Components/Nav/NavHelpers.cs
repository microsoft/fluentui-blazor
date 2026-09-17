// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Components.Nav;

public class DerivedFluentNav : FluentNav
{
    public DerivedFluentNav(LibraryConfiguration configuration) : base(configuration)
    {
    }
}

public class DerivedFluentNavCategory : FluentNavCategory
{
    public DerivedFluentNavCategory(LibraryConfiguration configuration) : base(configuration)
    {
        Owner = new DerivedFluentNav(configuration);
    }
}
