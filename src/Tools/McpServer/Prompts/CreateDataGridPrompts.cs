// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.McpServer.Prompts;

/// <summary>
/// MCP prompts for creating DataGrid components with Fluent UI Blazor.
/// </summary>
[McpServerPromptType]
public class CreateDataGridPrompts
{
    /// <summary>
    /// Generates a prompt to help create a DataGrid with Fluent UI Blazor.
    /// </summary>
    /// <param name="dataDescription">Description of the data to display.</param>
    /// <param name="features">Comma-separated list of features to include.</param>
    /// <param name="displayMode">Display mode: 'grid' or 'table'.</param>
    [McpServerPrompt(Name = "create_datagrid")]
    [Description("Generates code for creating a FluentDataGrid component with sorting, pagination, and other features.")]
    public static ChatMessage CreateDataGrid(
        [Description("A description of the data to display (e.g., 'list of users with name, email, role')")]
        string dataDescription,
        [Description("Comma-separated list of features: 'sorting', 'pagination', 'selection', 'virtualization'. Default is 'sorting,pagination'.")]
        string features = "sorting,pagination",
        [Description("Display mode: 'grid' (default) or 'table'. Use 'table' with virtualization.")]
        string displayMode = "grid")
    {
        var featureList = features.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var sb = new StringBuilder();
        sb.AppendLine("# Create a Fluent UI Blazor DataGrid");
        sb.AppendLine();
        sb.AppendLine(CultureInfo.InvariantCulture, $"Create a DataGrid to display: **{dataDescription}**");
        sb.AppendLine();

        AppendFeatures(sb, featureList);
        AppendDisplayMode(sb, displayMode);
        AppendBasicStructure(sb, featureList, displayMode);
        AppendCodeBehind(sb, featureList);
        AppendColumnTypes(sb);
        AppendAdapters(sb);

        return new ChatMessage(ChatRole.User, sb.ToString());
    }

    private static void AppendFeatures(StringBuilder sb, HashSet<string> featureList)
    {
        sb.AppendLine("### Features:");
        if (featureList.Contains("SORTING"))
        {
            sb.AppendLine("- ✓ **Sorting**");
        }

        if (featureList.Contains("PAGINATION"))
        {
            sb.AppendLine("- ✓ **Pagination**");
        }

        if (featureList.Contains("SELECTION"))
        {
            sb.AppendLine("- ✓ **Row Selection**");
        }

        if (featureList.Contains("VIRTUALIZATION"))
        {
            sb.AppendLine("- ✓ **Virtualization**");
        }

        sb.AppendLine();
    }

    private static void AppendDisplayMode(StringBuilder sb, string displayMode)
    {
        var mode = displayMode.Equals("table", StringComparison.OrdinalIgnoreCase) ? "Table" : "Grid";
        sb.AppendLine(CultureInfo.InvariantCulture, $"**Display Mode:** {mode}");
        sb.AppendLine();
    }

    private static void AppendBasicStructure(StringBuilder sb, HashSet<string> featureList, string displayMode)
    {
        sb.AppendLine("## Basic Structure");
        sb.AppendLine();
        sb.AppendLine("The `FluentDataGrid` component should be configured with:");
        sb.AppendLine();
        sb.AppendLine("- `Items` parameter bound to an `IQueryable<T>` data source");

        if (featureList.Contains("PAGINATION"))
        {
            sb.AppendLine("- `Pagination` parameter linked to a `PaginationState` instance");
            sb.AppendLine("- A `FluentPaginator` component to display pagination controls");
        }

        if (displayMode.Equals("table", StringComparison.OrdinalIgnoreCase))
        {
            sb.AppendLine("- `DisplayMode` set to `DataGridDisplayMode.Table`");
        }

        if (featureList.Contains("VIRTUALIZATION"))
        {
            sb.AppendLine("- `Virtualize` set to `true` with appropriate `ItemSize`");
        }

        if (featureList.Contains("SELECTION"))
        {
            sb.AppendLine("- A `SelectColumn` for row selection checkboxes");
        }

        sb.AppendLine("- `PropertyColumn` components for data-bound columns");
        sb.AppendLine("- `TemplateColumn` components for custom content like action buttons");
        sb.AppendLine();
    }

    private static void AppendCodeBehind(StringBuilder sb, HashSet<string> featureList)
    {
        sb.AppendLine("## Code Behind");
        sb.AppendLine();
        sb.AppendLine("The `@code` block should include:");
        sb.AppendLine();

        if (featureList.Contains("PAGINATION"))
        {
            sb.AppendLine("- A `PaginationState` instance with `ItemsPerPage` configuration");
        }

        if (featureList.Contains("SELECTION"))
        {
            sb.AppendLine("- A collection to track selected items");
        }

        sb.AppendLine("- An `IQueryable<T>` data source for the grid items");
        sb.AppendLine("- Event handlers for actions like Edit, Delete, etc.");
        sb.AppendLine();
    }

    private static void AppendColumnTypes(StringBuilder sb)
    {
        sb.AppendLine("## Column Types");
        sb.AppendLine();
        sb.AppendLine("- **PropertyColumn** - Binds to a property, supports sorting");
        sb.AppendLine("- **TemplateColumn** - Custom content with RenderFragment");
        sb.AppendLine("- **SelectColumn** - Row selection checkboxes");
        sb.AppendLine();
    }

    private static void AppendAdapters(StringBuilder sb)
    {
        sb.AppendLine("## EF Core Adapter");
        sb.AppendLine();
        sb.AppendLine("For Entity Framework Core integration:");
        sb.AppendLine();
        sb.AppendLine("- Install the `Microsoft.FluentUI.AspNetCore.Components.DataGrid.EntityFrameworkAdapter` package");
        sb.AppendLine("- Register the adapter with `builder.Services.AddDataGridEntityFrameworkAdapter();`");
        sb.AppendLine();
        sb.AppendLine("**Important:** Use the available MCP tools to retrieve component documentation and code examples for the DataGrid implementation.");
    }
}
