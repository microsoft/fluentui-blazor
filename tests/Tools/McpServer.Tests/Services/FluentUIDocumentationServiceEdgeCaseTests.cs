// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using Microsoft.FluentUI.AspNetCore.McpServer.Services;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Tests.Services;

/// <summary>
/// Additional edge case tests for FluentUIDocumentationService.
/// </summary>
public class FluentUIDocumentationServiceEdgeCaseTests
{
    #region FindEnumForType Tests (via GetEnumsForComponent)

    [Fact]
    public void GetEnumsForComponent_WithNullableEnumType_ShouldFindEnum()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"test_nullable_{Guid.NewGuid()}.json");
        var json = """
        {
            "metadata": { "assemblyVersion": "1.0.0", "generatedDateUtc": "2024-01-01", "componentCount": 1, "enumCount": 1 },
            "components": [{
                "name": "TestComponent",
                "fullName": "Test.TestComponent",
                "summary": "Test",
                "category": "Test",
                "isGeneric": false,
                "properties": [{
                    "name": "TestProp",
                    "type": "TestEnum?",
                    "description": "Test property",
                    "isParameter": true,
                    "isInherited": false,
                    "enumValues": ["Value1", "Value2"]
                }],
                "events": [],
                "methods": []
            }],
            "enums": [{
                "name": "TestEnum",
                "fullName": "Test.TestEnum",
                "description": "Test enum",
                "values": [{ "name": "Value1", "value": 0, "description": "First" }, { "name": "Value2", "value": 1, "description": "Second" }]
            }]
        }
        """;
        File.WriteAllText(tempPath, json);

        try
        {
            var service = new FluentUIDocumentationService(tempPath);

            // Act
            var enums = service.GetEnumsForComponent("TestComponent");

            // Assert
            Assert.True(enums.ContainsKey("TestProp"));
            Assert.Equal("TestEnum", enums["TestProp"].Name);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void GetEnumsForComponent_WithEnumFromProperty_ShouldFindEnum()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"test_prop_enum_{Guid.NewGuid()}.json");
        var json = """
        {
            "metadata": { "assemblyVersion": "1.0.0", "generatedDateUtc": "2024-01-01", "componentCount": 1, "enumCount": 1 },
            "components": [{
                "name": "TestComponent",
                "fullName": "Test.TestComponent",
                "summary": "Test",
                "category": "Test",
                "isGeneric": false,
                "properties": [{
                    "name": "PropEnum",
                    "type": "PropEnumType",
                    "description": "Property enum",
                    "isParameter": false,
                    "isInherited": false,
                    "enumValues": []
                }],
                "events": [],
                "methods": []
            }],
            "enums": [{
                "name": "PropEnumType",
                "fullName": "Test.PropEnumType",
                "description": "Property enum type",
                "values": [{ "name": "A", "value": 0, "description": "A" }]
            }]
        }
        """;
        File.WriteAllText(tempPath, json);

        try
        {
            var service = new FluentUIDocumentationService(tempPath);

            // Act
            var enums = service.GetEnumsForComponent("TestComponent");

            // Assert
            Assert.True(enums.ContainsKey("PropEnum"));
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void GetEnumsForComponent_WithPartialEnumMatch_ShouldFindEnum()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"test_partial_{Guid.NewGuid()}.json");
        var json = """
        {
            "metadata": { "assemblyVersion": "1.0.0", "generatedDateUtc": "2024-01-01", "componentCount": 1, "enumCount": 1 },
            "components": [{
                "name": "TestComponent",
                "fullName": "Test.TestComponent",
                "summary": "Test",
                "category": "Test",
                "isGeneric": false,
                "properties": [{
                    "name": "MyProp",
                    "type": "MyEnum",
                    "description": "My property",
                    "isParameter": true,
                    "isInherited": false,
                    "enumValues": []
                }],
                "events": [],
                "methods": []
            }],
            "enums": [{
                "name": "MyEnum",
                "fullName": "Some.Namespace.MyEnum",
                "description": "My enum",
                "values": [{ "name": "X", "value": 0, "description": "X" }]
            }]
        }
        """;
        File.WriteAllText(tempPath, json);

        try
        {
            var service = new FluentUIDocumentationService(tempPath);

            // Act
            var enums = service.GetEnumsForComponent("TestComponent");

            // Assert
            Assert.True(enums.ContainsKey("MyProp"));
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void GetEnumsForComponent_WithDuplicateEnumForDifferentProps_ShouldNotDuplicate()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"test_dup_{Guid.NewGuid()}.json");
        var json = """
        {
            "metadata": { "assemblyVersion": "1.0.0", "generatedDateUtc": "2024-01-01", "componentCount": 1, "enumCount": 1 },
            "components": [{
                "name": "TestComponent",
                "fullName": "Test.TestComponent",
                "summary": "Test",
                "category": "Test",
                "isGeneric": false,
                "properties": [{
                    "name": "Prop1",
                    "type": "SharedEnum",
                    "description": "Prop 1",
                    "isParameter": true,
                    "isInherited": false,
                    "enumValues": []
                }, {
                    "name": "Prop2",
                    "type": "SharedEnum",
                    "description": "Prop 2",
                    "isParameter": true,
                    "isInherited": false,
                    "enumValues": []
                }],
                "events": [],
                "methods": []
            }],
            "enums": [{
                "name": "SharedEnum",
                "fullName": "Test.SharedEnum",
                "description": "Shared enum",
                "values": [{ "name": "A", "value": 0, "description": "A" }]
            }]
        }
        """;
        File.WriteAllText(tempPath, json);

        try
        {
            var service = new FluentUIDocumentationService(tempPath);

            // Act
            var enums = service.GetEnumsForComponent("TestComponent");

            // Assert
            Assert.True(enums.ContainsKey("Prop1"));
            Assert.True(enums.ContainsKey("Prop2"));
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    #endregion

    #region GetComponentDetails Edge Cases

    [Fact]
    public void GetComponentDetails_WithComponentNotInCache_ShouldReturnNull()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"test_cache_{Guid.NewGuid()}.json");
        var json = """
        {
            "metadata": { "assemblyVersion": "1.0.0", "generatedDateUtc": "2024-01-01", "componentCount": 0, "enumCount": 0 },
            "components": [],
            "enums": []
        }
        """;
        File.WriteAllText(tempPath, json);

        try
        {
            var service = new FluentUIDocumentationService(tempPath);

            // Act
            var details = service.GetComponentDetails("NonExistent");

            // Assert
            Assert.Null(details);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void GetComponentDetails_ShouldConvertAllProperties()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"test_convert_{Guid.NewGuid()}.json");
        var json = """
        {
            "metadata": { "assemblyVersion": "1.0.0", "generatedDateUtc": "2024-01-01", "componentCount": 1, "enumCount": 0 },
            "components": [{
                "name": "FullComponent",
                "fullName": "Test.FullComponent",
                "summary": "Full component",
                "category": "Test",
                "isGeneric": true,
                "baseClass": "BaseComponent",
                "properties": [{
                    "name": "Prop1",
                    "type": "string",
                    "description": "Property 1",
                    "isParameter": true,
                    "isInherited": false,
                    "defaultValue": "default",
                    "enumValues": []
                }, {
                    "name": "Prop2",
                    "type": "int",
                    "description": "Property 2",
                    "isParameter": false,
                    "isInherited": true,
                    "enumValues": []
                }],
                "events": [{
                    "name": "OnChange",
                    "type": "EventCallback<ChangeEventArgs>",
                    "description": "Change event",
                    "isInherited": false
                }],
                "methods": [{
                    "name": "DoSomething",
                    "returnType": "void",
                    "description": "Does something",
                    "parameters": ["string arg"],
                    "isInherited": false
                }]
            }],
            "enums": []
        }
        """;
        File.WriteAllText(tempPath, json);

        try
        {
            var service = new FluentUIDocumentationService(tempPath);

            // Act
            var details = service.GetComponentDetails("FullComponent");

            // Assert
            Assert.NotNull(details);
            Assert.Equal("FullComponent", details!.Component.Name);
            Assert.True(details.Component.IsGeneric);
            Assert.Equal("BaseComponent", details.Component.BaseClass);
            Assert.Single(details.Parameters);
            Assert.Equal("Prop1", details.Parameters[0].Name);
            Assert.Equal(2, details.Properties.Count);
            Assert.Single(details.Events);
            Assert.Equal("OnChange", details.Events[0].Name);
            Assert.Single(details.Methods);
            Assert.Equal("DoSomething", details.Methods[0].Name);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    #endregion

    #region Search Edge Cases

    [Fact]
    public void SearchComponents_WithPartialMatch_ShouldFindComponents()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"test_search_{Guid.NewGuid()}.json");
        var json = """
        {
            "metadata": { "assemblyVersion": "1.0.0", "generatedDateUtc": "2024-01-01", "componentCount": 2, "enumCount": 0 },
            "components": [{
                "name": "FluentButton",
                "fullName": "Test.FluentButton",
                "summary": "A button component",
                "category": "Buttons",
                "isGeneric": false,
                "properties": [], "events": [], "methods": []
            }, {
                "name": "FluentIconButton",
                "fullName": "Test.FluentIconButton",
                "summary": "An icon button",
                "category": "Buttons",
                "isGeneric": false,
                "properties": [], "events": [], "methods": []
            }],
            "enums": []
        }
        """;
        File.WriteAllText(tempPath, json);

        try
        {
            var service = new FluentUIDocumentationService(tempPath);

            // Act
            var results = service.SearchComponents("Button");

            // Assert
            Assert.Equal(2, results.Count);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    [Fact]
    public void SearchComponents_InSummary_ShouldFindComponents()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"test_search_sum_{Guid.NewGuid()}.json");
        var json = """
        {
            "metadata": { "assemblyVersion": "1.0.0", "generatedDateUtc": "2024-01-01", "componentCount": 1, "enumCount": 0 },
            "components": [{
                "name": "FluentDialog",
                "fullName": "Test.FluentDialog",
                "summary": "A modal popup dialog component",
                "category": "Dialogs",
                "isGeneric": false,
                "properties": [], "events": [], "methods": []
            }],
            "enums": []
        }
        """;
        File.WriteAllText(tempPath, json);

        try
        {
            var service = new FluentUIDocumentationService(tempPath);

            // Act
            var results = service.SearchComponents("popup");

            // Assert
            Assert.Single(results);
            Assert.Equal("FluentDialog", results[0].Name);
        }
        finally
        {
            File.Delete(tempPath);
        }
    }

    #endregion
}
