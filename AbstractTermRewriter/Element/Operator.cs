using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTermRewriter
{
	public enum OperatorType
	{
		Addition,
		Subtraction,
		Multiplication,
		Division
	}

	public class Operator : IElement
	{
		public char Symbol { get; }
		public OperatorType Type { get; private set; }

		public Operator(char symbol)
		{
			Symbol = symbol;
		}
	}
}
