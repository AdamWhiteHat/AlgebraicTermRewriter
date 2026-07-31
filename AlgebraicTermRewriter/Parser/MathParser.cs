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

			Stack<char> stack = new Stack<char>(input.Replace(" ", "").Reverse());

			IEnumerable<IToken> tokens = ParseExpression(stack);

			Expression result = new Expression(tokens.ToArray());
			return result;
		}

		private static IEnumerable<IToken> ParseExpression(Stack<char> stack)
		{
			while (stack.Any())
			{
				char c = stack.Pop();

				if (Types.Numbers.Contains(c) || (c == '-' && Types.Numbers.Contains(stack.Peek())))
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

					yield return new Number(int.Parse(value));
				}
				else if (Types.Operators.Contains(c))
				{
					yield return new Operator(c);
				}
				else if (Types.Variables.Contains(c))
				{
					yield return new Variable(c);
				}
				else if (c == Types.Parenthesis[1])
				{
					break;
				}
				else if (c == Types.Parenthesis[0])
				{
					IEnumerable<IToken> tokens = ParseExpression(stack);

					yield return new SubExpression(tokens.ToArray());
				}
				else
				{
					throw new FormatException($"Unrecognized token: '{c}'.");
				}
			}

			yield break;
		}


	}
}
