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
        sb.AppendLine("```razor");

        if (featureList.Contains("PAGINATION"))
        {
            sb.AppendLine("<FluentPaginator State=\"@pagination\" />");
        }

        sb.Append("<FluentDataGrid Items=\"@items\"");

        if (featureList.Contains("PAGINATION"))
        {
            sb.Append(" Pagination=\"@pagination\"");
        }

        if (displayMode.Equals("table", StringComparison.OrdinalIgnoreCase))
        {
            sb.Append(" DisplayMode=\"DataGridDisplayMode.Table\"");
        }

        if (featureList.Contains("VIRTUALIZATION"))
        {
            sb.Append(" Virtualize=\"true\" ItemSize=\"46\"");
        }

        sb.AppendLine(">");

        if (featureList.Contains("SELECTION"))
        {
            sb.AppendLine("    <SelectColumn TGridItem=\"YourModel\" />");
        }

        sb.AppendLine("    <PropertyColumn Property=\"@(item => item.Name)\" Sortable=\"true\" />");
        sb.AppendLine("    <TemplateColumn Title=\"Actions\">");
        sb.AppendLine("        <FluentButton OnClick=\"@(() => Edit(context))\">Edit</FluentButton>");
        sb.AppendLine("    </TemplateColumn>");
        sb.AppendLine("</FluentDataGrid>");
        sb.AppendLine("```");
        sb.AppendLine();
    }

    private static void AppendCodeBehind(StringBuilder sb, HashSet<string> featureList)
    {
        sb.AppendLine("## Code Behind");
        sb.AppendLine();
        sb.AppendLine("```csharp");
        sb.AppendLine("@code {");

        if (featureList.Contains("PAGINATION"))
        {
            sb.AppendLine("    private PaginationState pagination = new() { ItemsPerPage = 10 };");
        }

        if (featureList.Contains("SELECTION"))
        {
            sb.AppendLine("    private IEnumerable<YourModel> selectedItems = new List<YourModel>();");
        }

        sb.AppendLine("    private IQueryable<YourModel> items = GetData().AsQueryable();");
        sb.AppendLine("    private void Edit(YourModel item) { }");
        sb.AppendLine("}");
        sb.AppendLine("```");
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
        sb.AppendLine("```bash");
        sb.AppendLine("dotnet add package Microsoft.FluentUI.AspNetCore.Components.DataGrid.EntityFrameworkAdapter");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("Then: `builder.Services.AddDataGridEntityFrameworkAdapter();`");
        sb.AppendLine();
        sb.AppendLine("Please generate a complete implementation with the model class and sample data.");
    }
}
