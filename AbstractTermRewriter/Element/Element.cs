using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AbstractTermRewriter
{
	public class Element
	{
		public char Symbol { get; private set; }
		public ElementType Type { get; private set; }

		public Element Previous { get; internal set; } = null;
		public Element Next { get; internal set; } = null;

		private Expression ParentExpression { get; set; } = null;

		public Element(char symbol)
		{
			Symbol = symbol;
			Type = Types.Convert.ToElementType(symbol);
		}

		internal void SetParentExpression(Expression parentExpression)
		{
			ParentExpression = parentExpression;
			int index = ParentExpression.Elements.Count - 1;

			Previous = ParentExpression.Elements[index - 1];
			Previous.Next = this;
		}
	}
}
