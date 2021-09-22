using System;
using System.Linq.Expressions;

namespace Microsoft.Fast.Components.FluentUI
{
	public class ColumnDefinition<TItem>
	{
		public string ColumnDataKey { get; set; }
		public string Title { get; set; }

		public string HeaderCellTemplate { get; set; }
		public bool HeaderCellInternalFocusQueue { get; set; }
		public int HeaderCellFocusTargetCallback { get; set; }


		public string CellTemplate { get; set; }
		public bool CellInternalFocusQueue { get; set; }
		public int CellInternalFocusTargetCallback { get; }

		public Func<TItem, object>? FieldSelector { get; set; }

		public Expression<Func<TItem, object>>? FieldSelectorExpression { get; set; }

		public ColumnDefinition(string fieldName, Expression<Func<TItem, object>> fieldSelectorExpression)
		{
			ColumnDataKey = fieldName;
			Title = fieldName;
			FieldSelectorExpression = fieldSelectorExpression;
			FieldSelector = fieldSelectorExpression.Compile();
		}
	}
}
