using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Web;

namespace Microsoft.Fast.Components.FluentUI
{
    public partial class FluentDataGridRow<TItem>
    {
        // FAST Attributes
        [Parameter]
        public int? RowIndex { get; set; }

        [Parameter]
        public string? GridTemplateColumns { get; set; } = null;
        [Parameter]
        public DataGridRowType? RowType { get; set; } = DataGridRowType.Default;
        // FAST Properties
        [Parameter]
        public TItem? RowData { get; set; }

        [Parameter]
        public IEnumerable<ColumnDefinition<TItem>>? ColumnDefinitions { get; set; }

        // General Blazor parameters
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object>? AdditionalAttributes { get; set; }
    }
}