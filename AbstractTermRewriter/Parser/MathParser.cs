using System;
using System.Linq;
using System.Collections.Generic;

namespace AbstractTermRewriter
{
	public static class MathParser
	{
		public static ISentence ParseSentence(string input)
		{
			if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException();

			if (input.Any(c => Types.Comparative.Contains(c)))
			{
				return (ISentence)MathParser.ParseEquation(input);
			}
			else
			{
				return (ISentence)MathParser.ParseExpression(input);
			}
		}

		public static Equation ParseEquation(string input)
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

			ComparativeType compareType = ConvertTo.ComparativeTypeEnum(comparative);
			Expression lhs = MathParser.ParseExpression(leftExpression);
			Expression rhs = MathParser.ParseExpression(rightExpression);

			Equation result = new Equation(lhs, compareType, rhs);
			return result;
		}

		public static Expression ParseExpression(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				throw new ArgumentException($"{nameof(input)} cannot be null, empty or white space.");
			}

			if (input.Any(c => Types.Comparative.Contains(c)))
			{
				throw new ArgumentException("An expression contains no comparative symbols. You want an Equation.");
			}

			Stack<char> stack = new Stack<char>(input.Replace(" ", "").Reverse());

			List<IElement> elements = new List<IElement>();
			while (stack.Any())
			{
				IElement newElement = null;

				char c = stack.Pop();

				if (Types.Numbers.Contains(c))
				{
					string value = c.ToString();
					while (stack.Any() && Types.Numbers.Contains(stack.Peek()))
					{
						c = stack.Pop();
						value += c;
					}

					newElement = new Number(int.Parse(value));
				}
				else if (Types.Operators.Contains(c))
				{
					newElement = new Operator(c);
				}
				else if (Types.Variables.Contains(c))
				{
					newElement = new Variable(c);
				}

				elements.Add(newElement);
			}

			Expression result = new Expression(elements.ToArray());
			return result;
		}
	}
}
