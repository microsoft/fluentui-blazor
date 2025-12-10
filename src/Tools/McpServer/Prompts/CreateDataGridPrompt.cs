// ------------------------------------------------------------------------
// This file is licensed to you under the MIT License.
// ------------------------------------------------------------------------

using System.ComponentModel;
using System.Text;
using Microsoft.FluentUI.AspNetCore.Components.McpServer.Services;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Server;

namespace Microsoft.FluentUI.AspNetCore.Components.McpServer.Prompts;

/// <summary>
/// MCP Prompt for generating Fluent UI Blazor DataGrid components.
/// </summary>
[McpServerPromptType]
public class CreateDataGridPrompt
{
    private readonly FluentUIDocumentationService _documentationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateDataGridPrompt"/> class.
    /// </summary>
    public CreateDataGridPrompt(FluentUIDocumentationService documentationService)
    {
        _documentationService = documentationService;
    }

    [McpServerPrompt(Name = "create_datagrid")]
    [Description("Generate a Fluent UI Blazor DataGrid with specified columns and features.")]
    public ChatMessage CreateDataGrid(
        [Description("Describe the data and columns (e.g., 'products with name, price, category, stock count')")] string dataDescription,
        [Description("Optional: features to include (e.g., 'sorting, filtering, pagination, row selection')")] string? features = null,
        [Description("Optional: the item type name (e.g., 'Product')")] string? itemType = null)
    {
        var gridDetails = _documentationService.GetComponentDetails("FluentDataGrid");

        var sb = new StringBuilder();
        sb.AppendLine("# Create a Fluent UI Blazor DataGrid");
        sb.AppendLine();

        if (gridDetails != null)
        {
            sb.AppendLine("## FluentDataGrid Component");
            sb.AppendLine();
            sb.AppendLine($"{gridDetails.Component.Summary}");
            sb.AppendLine();

            sb.AppendLine("### Key Parameters");
            sb.AppendLine();

            var keyParams = new[] { "Items", "ItemsProvider", "Pagination", "RowsDataSize", "SelectionMode", "ShowHover" };
            foreach (var param in gridDetails.Parameters.Where(p => keyParams.Contains(p.Name)))
            {
                sb.AppendLine($"- `{param.Name}` ({param.Type}): {param.Description}");
            }

            sb.AppendLine();
        }

        sb.AppendLine("## DataGrid Requirements");
        sb.AppendLine();
        sb.AppendLine($"**Data**: {dataDescription}");

        if (!string.IsNullOrEmpty(features))
        {
            sb.AppendLine($"**Features**: {features}");
        }

        if (!string.IsNullOrEmpty(itemType))
        {
            sb.AppendLine($"**Item Type**: {itemType}");
        }

        sb.AppendLine();
        sb.AppendLine("## Task");
        sb.AppendLine();
        sb.AppendLine("Generate a complete FluentDataGrid implementation that includes:");
        sb.AppendLine("1. A model class for the data items");
        sb.AppendLine("2. Sample data generation");
        sb.AppendLine("3. PropertyColumn definitions for each field");
        sb.AppendLine("4. Appropriate column formatting (dates, currency, etc.)");

        if (!string.IsNullOrEmpty(features))
        {
            if (features.Contains("sort", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine("5. Sortable columns configuration");
            }

            if (features.Contains("filter", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine("6. Column filtering");
            }

            if (features.Contains("pagination", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine("7. Pagination with FluentPaginator");
            }

            if (features.Contains("selection", StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine("8. Row selection handling");
            }
        }

        sb.AppendLine();
        sb.AppendLine("Include both the Razor markup and the code-behind.");

        return new ChatMessage(ChatRole.User, sb.ToString());
    }
}
