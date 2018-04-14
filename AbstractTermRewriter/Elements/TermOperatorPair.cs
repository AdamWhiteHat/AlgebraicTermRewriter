using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AbstractTermRewriter
{
	public enum InsertOrientation
	{
		Either,
		Right,
		Left
	}
	public class TermOperatorPair
	{
		public ITerm Term { get; private set; }
		public IOperator Operator { get; private set; }
		public InsertOrientation Orientation { get; private set; }

		public TermOperatorPair(ITerm term, IOperator oper, InsertOrientation orientation)
		{
			Term = term;
			Operator = oper;
			Orientation = orientation;
		}

		public override string ToString()
		{
			if (Orientation == InsertOrientation.Left)
			{
				return $"{Term}{Operator}";
			}
			else
			{
				return $"{Operator}{Term}";
			}
		}
	}
}
