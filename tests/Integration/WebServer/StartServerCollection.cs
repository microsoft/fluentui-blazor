// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Microsoft.FluentUI.AspNetCore.Components.IntegrationTests.WebServer;

/// <summary>
/// Collection definition for the web server.
/// </summary>
[CollectionDefinition(StartServerCollection.Name)]
public class StartServerCollection : ICollectionFixture<StartServerFixture>
{
    /// <summary>
    /// The name of the collection.
    /// </summary>
    public const string Name = "Web Server Collection";
}
