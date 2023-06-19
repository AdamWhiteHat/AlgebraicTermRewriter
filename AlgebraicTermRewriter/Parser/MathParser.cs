using System;
using System.Linq;
using System.Collections.Generic;

namespace AlgebraicTermRewriter
{
	public static class MathParser
	{
		public static ISentence ParseSentence(string input)
		{
			if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException();

			if (input.Any(c => Types.Comparison.Contains(c)))
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

			if (!input.Any(c => Types.Comparison.Contains(c)))
			{
				throw new ArgumentException("An Equation contains comparison symbols. You want an Expression.");
			}

			int index = input.IndexOfAny(Types.Comparison.ToArray());

			string leftExpression = input.Substring(0, index);

			string comparison = input.ElementAt(index).ToString();

			if (Types.Comparison.Contains(input.ElementAt(index + 1)))
			{
				comparison += input.ElementAt(index + 1).ToString();
				index += 1;
			}

			string rightExpression = input.Substring(index + 1);

			ComparisonType compareType = ConvertTo.ComparisonTypeEnum(comparison);
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

			if (input.Any(c => Types.Comparison.Contains(c)))
			{
				throw new ArgumentException("An expression contains no comparison symbols. You want an Equation.");
			}

			List<IToken> tokens = new List<IToken>();

			Stack<char> stack = new Stack<char>(input.Replace(" ", "").Reverse());
			while (stack.Any())
			{
				IToken newToken = null;

				char c = stack.Pop();

				if (Types.Numbers.Contains(c) || (tokens.Count() == 0 && c == '-'))
				{
					string value = c.ToString();
					while (stack.Any() && Types.Numbers.Contains(stack.Peek()))
					{
						c = stack.Pop();
						value += c;
					}

					// Handle negation
					//if (tokens.Any())
					//{
					//	int index = tokens.Count - 1;
					//	if (tokens[index].Contents == "-")
					//	{
					//		tokens[index] = new Operator('+');
					//		value = value.Insert(0, "-");
					//	}
					//}

					newToken = new Number(int.Parse(value));
				}
				else if (Types.Operators.Contains(c))
				{
					newToken = new Operator(c);
				}
				else if (Types.Variables.Contains(c))
				{
					newToken = new Variable(c);
				}
				else
				{
					throw new FormatException($"Unrecognized token: '{c}'.");
				}

				tokens.Add(newToken);
			}

			Expression result = new Expression(tokens.ToArray());
			return result;
		}
	}
}
