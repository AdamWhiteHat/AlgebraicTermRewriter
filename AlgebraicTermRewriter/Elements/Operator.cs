using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	public class Operator : IOperator
	{
		public char Value { get; private set; }
		public string Symbol { get; private set; }
		public ElementType Type { get { return ElementType.Operator; } }
		public Operator(char symbol)
		{
			
			if (!Types.Operators.Contains(symbol))
			{
				throw new ArgumentException($"{nameof(symbol)} does not match any of the valid operator symbols.");
			}
			Value = symbol;
			Symbol = symbol.ToString(); ;
		}

		public Number ApplyOperation(Number lhs, Number rhs)
		{
			int l = lhs.Value;
			int r = rhs.Value;

			int total = 0;
			switch (Symbol)
			{
				case "+":
					total = l + r;
					break;
				case "-":
					total = l - r;
					break;
				case "*":
					total = l * r;
					break;
				case "/":
					total = l / r;
					break;
			}

			return new Number(total);
		}

		public static IOperator GetInverse(IOperator element)
		{
			switch (element.Symbol)
			{
				case "+":
					return new Operator('-');
				case "-":
					return new Operator('+');
				case "*":
					return new Operator('/');
				case "/":
					return new Operator('*');
				default:
					throw new ArgumentException();
			}
		}

		public override string ToString()
		{
			return Symbol;
		}
	}
}
