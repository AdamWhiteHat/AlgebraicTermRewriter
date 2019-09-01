using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	public class Operator : IOperator
	{
		public char Symbol { get; private set; }
		public string Contents { get; private set; }
		public TokenType Type { get { return TokenType.Operator; } }

		public Operator(char symbol)
		{

			if (!Types.Operators.Contains(symbol))
			{
				throw new ArgumentException($"{nameof(symbol)} does not match any of the valid operator symbols.");
			}
			Symbol = symbol;
			Contents = symbol.ToString();
		}

		public Number ApplyOperation(Number lhs, Number rhs)
		{
			int l = lhs.Value;
			int r = rhs.Value;

			int total = 0;
			switch (Symbol)
			{
				case '+':
					total = l + r;
					break;
				case '-':
					total = l - r;
					break;
				case '*':
					total = l * r;
					break;
				case '/':
					total = l / r;
					break;
				case '^':
					total = (int)Math.Pow(l, r);
					break;
				default:
					throw new NotImplementedException(Symbol.ToString());
			}

			return new Number(total);
		}

		public static IOperator GetInverse(IOperator token)
		{
			switch (token.Symbol)
			{
				case '+':
					return new Operator('-');
				case '-':
					return new Operator('+');
				case '*':
					return new Operator('/');
				case '/':
					return new Operator('*');
				default:
					throw new ArgumentException();
			}
		}

		public override string ToString()
		{
			return Symbol.ToString();
		}
	}
}
