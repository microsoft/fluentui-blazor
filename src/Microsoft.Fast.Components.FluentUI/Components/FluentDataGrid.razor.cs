using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentDataGrid<TItem>
    {
        // FAST Attributes
        [Parameter]
        public GenerateHeaderOptions? GenerateHeader { get; set; } = GenerateHeaderOptions.Default;
        [Parameter]
        public string? GridTemplateColumns { get; set; } = "";
        //FAST Properties
        [Parameter]
        public List<TItem>? RowsData { get; set; }

        [Parameter]
        public IEnumerable<ColumnDefinition<TItem>>? ColumnDefinitions { get; set; }

        [Parameter]
        public RenderFragment<ColumnDefinition<TItem>>? HeaderCellTemplate { get; set; } = null;
        [Parameter]
        public RenderFragment<TItem>? RowItemTemplate { get; set; } = null;
        // General Blazor parameters
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object>? AdditionalAttributes { get; set; }
    }
}