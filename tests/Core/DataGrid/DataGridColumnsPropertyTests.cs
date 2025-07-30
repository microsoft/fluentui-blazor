using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Microsoft.FluentUI.AspNetCore.Components.Tests.DataGrid;

/// <summary>
/// Tests for the public Columns property in DataGridRow and DataGridCell classes.
/// </summary>
public class DataGridColumnsPropertyTests : TestBase
{
    public DataGridColumnsPropertyTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton(LibraryConfiguration.ForUnitTests);
        
        // Register KeyCode service
        var keycodeService = new KeyCodeService();
        Services.AddScoped<IKeyCodeService>(factory => keycodeService);
    }

    [Fact]
    public void DataGrid_Properties_CompileCorrectly()
    {
        // This test verifies that our new public properties compile correctly
        // by checking that they exist and are public using reflection
        
        var rowType = typeof(FluentDataGridRow<object>);
        var cellType = typeof(FluentDataGridCell<object>);
        
        // Verify that the Columns property exists on DataGridRow
        var columnsProperty = rowType.GetProperty("Columns");
        Assert.NotNull(columnsProperty);
        Assert.True(columnsProperty.CanRead);
        Assert.True(columnsProperty.GetMethod?.IsPublic == true);
        
        // Verify that the Column property exists on DataGridCell
        var columnProperty = cellType.GetProperty("Column");
        Assert.NotNull(columnProperty);
        Assert.True(columnProperty.CanRead);
        Assert.True(columnProperty.GetMethod?.IsPublic == true);
    }
}