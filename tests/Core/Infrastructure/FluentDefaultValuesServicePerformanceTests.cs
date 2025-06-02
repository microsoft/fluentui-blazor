// ------------------------------------------------------------------------
// MIT License - Copyright (c) Microsoft Corporation. All rights reserved.
// ------------------------------------------------------------------------

using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components.Infrastructure;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.Infrastructure;

/// <summary>
/// Performance tests for FluentDefaultValuesService to ensure it can handle
/// scenarios with hundreds of components efficiently.
/// </summary>
public class FluentDefaultValuesServicePerformanceTests
{
    public FluentDefaultValuesServicePerformanceTests()
    {
        // Reset service state before each test
        FluentDefaultValuesService.Reset();
    }

    /// <summary>
    /// Test component with no defaults defined - should be very fast
    /// </summary>
    private class TestComponentWithoutDefaults : ComponentBase
    {
        [Parameter] public string? TestProperty { get; set; }
        [Parameter] public int NumberProperty { get; set; }
    }

    /// <summary>
    /// Test component with defaults defined
    /// </summary>
    private class TestComponentWithDefaults : ComponentBase
    {
        [Parameter] public string? TestProperty { get; set; }
        [Parameter] public int NumberProperty { get; set; }
    }

    /// <summary>
    /// Defaults for TestComponentWithDefaults
    /// </summary>
    public static class TestDefaults
    {
        [FluentDefault("TestComponentWithDefaults")]
        public static string? TestProperty => "default-value";

        [FluentDefault("TestComponentWithDefaults")]
        public static int NumberProperty => 42;
    }

    [Fact]
    public void ApplyDefaults_WithoutDefaults_ShouldBeVeryFast()
    {
        // Create many components without any defaults defined
        var components = Enumerable.Range(0, 1000)
            .Select(_ => new TestComponentWithoutDefaults())
            .ToArray();

        var stopwatch = Stopwatch.StartNew();
        
        foreach (var component in components)
        {
            FluentDefaultValuesService.ApplyDefaults(component);
        }
        
        stopwatch.Stop();

        // Should be very fast since these components have no defaults
        // Even 1000 components should take less than 50ms
        Assert.True(stopwatch.ElapsedMilliseconds < 50, 
            $"Processing 1000 components without defaults took {stopwatch.ElapsedMilliseconds}ms, expected < 50ms");
    }

    [Fact]
    public void ApplyDefaults_WithDefaults_ShouldBeCached()
    {
        // First, make sure defaults are loaded
        FluentDefaultValuesService.ApplyDefaults(new TestComponentWithDefaults());

        // Create many components with defaults defined
        var components = Enumerable.Range(0, 500)
            .Select(_ => new TestComponentWithDefaults())
            .ToArray();

        var stopwatch = Stopwatch.StartNew();
        
        foreach (var component in components)
        {
            FluentDefaultValuesService.ApplyDefaults(component);
        }
        
        stopwatch.Stop();

        // Should be reasonably fast even with defaults due to caching
        // 500 components with defaults should take less than 100ms
        Assert.True(stopwatch.ElapsedMilliseconds < 100, 
            $"Processing 500 components with defaults took {stopwatch.ElapsedMilliseconds}ms, expected < 100ms");

        // Verify defaults were actually applied
        Assert.Equal("default-value", components[0].TestProperty);
        Assert.Equal(42, components[0].NumberProperty);
    }

    [Fact]
    public void ApplyDefaults_MixedComponents_ShouldHandleEfficiently()
    {
        // Mix of components with and without defaults
        var componentsWithDefaults = Enumerable.Range(0, 100)
            .Select(_ => new TestComponentWithDefaults())
            .ToArray();
            
        var componentsWithoutDefaults = Enumerable.Range(0, 400)
            .Select(_ => new TestComponentWithoutDefaults())
            .ToArray();

        var stopwatch = Stopwatch.StartNew();
        
        // Apply defaults to mixed components
        foreach (var component in componentsWithDefaults)
        {
            FluentDefaultValuesService.ApplyDefaults(component);
        }
        
        foreach (var component in componentsWithoutDefaults)
        {
            FluentDefaultValuesService.ApplyDefaults(component);
        }
        
        stopwatch.Stop();

        // Should handle mixed scenario efficiently
        Assert.True(stopwatch.ElapsedMilliseconds < 75, 
            $"Processing 500 mixed components took {stopwatch.ElapsedMilliseconds}ms, expected < 75ms");
    }

    [Fact]
    public void ApplyDefaults_RepeatedCalls_ShouldBenefitFromCaching()
    {
        var component = new TestComponentWithDefaults();
        
        // First call to establish cache
        FluentDefaultValuesService.ApplyDefaults(component);
        
        // Measure repeated calls to same component type
        var stopwatch = Stopwatch.StartNew();
        
        for (int i = 0; i < 1000; i++)
        {
            var newComponent = new TestComponentWithDefaults();
            FluentDefaultValuesService.ApplyDefaults(newComponent);
        }
        
        stopwatch.Stop();

        // Repeated calls should be very fast due to caching
        Assert.True(stopwatch.ElapsedMilliseconds < 50, 
            $"1000 repeated calls took {stopwatch.ElapsedMilliseconds}ms, expected < 50ms");
    }
}