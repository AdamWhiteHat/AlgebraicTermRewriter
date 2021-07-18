using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AlgebraicTermRewriter
{
	public enum InsertOrientation
	{
		Either,
		Right,
		Left
	}
	public class OperatorExpressionPair
	{
		public SubExpression Expr { get; private set; }
		public IOperator Operator { get; private set; }
		public InsertOrientation Orientation { get; private set; }

		public OperatorExpressionPair(IOperator oper, SubExpression expr, InsertOrientation orientation)
		{
			Expr = expr;
			Operator = oper;
			Orientation = orientation;
		}

		public override string ToString()
		{
			if (Orientation == InsertOrientation.Left)
			{
				return $"{Expr}{Operator}";
			}
			else
			{
				return $"{Operator}{Expr}";
			}
		}
	}
}
