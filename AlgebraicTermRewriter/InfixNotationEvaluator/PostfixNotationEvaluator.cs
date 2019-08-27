using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgebraicTermRewriter
{
	public static class PostfixNotationEvaluator
	{
		private static string AllowedCharacters = ParserTokens.Numbers + ParserTokens.Operators + " ";
		public static int Evaluate(string postfixNotationString)
		{
			if (string.IsNullOrWhiteSpace(postfixNotationString)) throw new ArgumentException("Argument postfixNotationString must not be null, empty or whitespace.", "postfixNotationString");

			Stack<string> stack = new Stack<string>();
			string sanitizedString = new string(postfixNotationString.Where(c => AllowedCharacters.Contains(c)).ToArray());
			List<string> enumerablePostfixTokens = sanitizedString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

			foreach (string token in enumerablePostfixTokens)
			{
				if (token.Length > 0)
				{
					if (token.Length > 1)
					{
						if (ParserTokens.IsNumeric(token))
						{
							stack.Push(token);
						}
						else { throw new Exception("Operators and operands must be separated by a space."); }
					}
					else
					{
						char tokenChar = token[0];

						if (ParserTokens.Numbers.Contains(tokenChar))
						{
							stack.Push(tokenChar.ToString());
						}
						else if (ParserTokens.Operators.Contains(tokenChar))
						{
							if (stack.Count < 2)
							{
								if (tokenChar == '-')
								{
									string val = $"-{stack.Pop()}";
									stack.Push(val);
									continue;
								}
								else { throw new FormatException("The algebraic string has not sufficient values in the expression for the number of operators."); }
							}

							string r = stack.Pop();
							string l = stack.Pop();

							int rhs = int.MinValue;
							int lhs = int.MinValue;

							bool parseSuccess = int.TryParse(r, out rhs);
							parseSuccess &= int.TryParse(l, out lhs);

							if (!parseSuccess) { throw new Exception("Unable to parse valueStack characters to Int32."); }

							int value = int.MinValue;
							if (tokenChar == '+')
							{
								value = lhs + rhs;
							}
							else if (tokenChar == '-')
							{
								value = lhs - rhs;
							}
							else if (tokenChar == '*')
							{
								value = lhs * rhs;
							}
							else if (tokenChar == '/')
							{
								value = lhs / rhs;
							}
							else if (tokenChar == '^')
							{
								value = (int)Math.Pow(lhs, rhs);
							}
							else { throw new Exception(string.Format("Unrecognized token '{0}'.", tokenChar)); }


							if (value != int.MinValue)
							{
								stack.Push(value.ToString());
							}
							else { throw new Exception("Value never got set."); }
						}
						else { throw new Exception(string.Format("Unrecognized character '{0}'.", tokenChar)); }
					}
				}
				else { throw new Exception("Token length is less than one."); }
			}

			if (stack.Count == 1)
			{
				int result = 0;
				if (!int.TryParse(stack.Pop(), out result)) { throw new Exception("Last value on stack could not be parsed into an integer."); }
				return result;
			}
			else { throw new Exception("The input has too many values for the number of operators."); }

		} // method
	}
}
