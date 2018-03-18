using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AbstractTermRewriter
{
	/// <summary>
	/// An equation consists of two expressions with an equality or inequality symbol between them 
	/// </summary>
	public class Equation : ISentence
	{
		public Expression LeftHandSide { get; set; } = null;
		public ComparativeType ComparativeOperator { get; private set; }
		public Expression RightHandSide { get; set; } = null;

		private Equation(Expression lhs, ComparativeType comparative, Expression rhs)
		{
			LeftHandSide = lhs;
			ComparativeOperator = comparative;
			RightHandSide = rhs;
		}

		public Equation(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				throw new ArgumentException($"{nameof(input)} cannot be null, empty or white space.");
			}

			if (!input.Any(c => Types.Comparative.Contains(c)))
			{
				throw new ArgumentException("An Equation contains comparative symbols. You want an Expression.");
			}

			int index = input.IndexOfAny(Types.Comparative.ToArray());

			string leftExpression = input.Substring(0, index);

			string comparative = input.ElementAt(index).ToString();

			if (Types.Comparative.Contains(input.ElementAt(index + 1)))
			{
				comparative += input.ElementAt(index + 1).ToString();
				index += 1;
			}

			string rightExpression = input.Substring(index + 1);

			ComparativeOperator = ConvertTo.ComparativeTypeEnum(comparative);
			LeftHandSide = new Expression(leftExpression);
			RightHandSide = new Expression(rightExpression);
		}

		public override string ToString()
		{
			return LeftHandSide.ToString() + ComparativeOperator.AsString() + RightHandSide.ToString();
		}
	}
}
