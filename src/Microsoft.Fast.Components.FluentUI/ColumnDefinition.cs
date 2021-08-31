namespace Microsoft.Fast.Components.FluentUI
{
    public class ColumnDefinition
    {
        public string ColumnDataKey { get; set; }
        public string Title { get; set; }

        public string HeaderCellTemplate { get; set; }
        public bool HeaderCellInternalFocusQueue { get; set; }
        public int HeaderCellFocusTargetCallback { get; set; }


        public string CellTemplate { get; set; }
        public bool CellInternalFocusQueue { get; set; }
        public int CellInternalFocusTargetCallback { get; }
    }
}
