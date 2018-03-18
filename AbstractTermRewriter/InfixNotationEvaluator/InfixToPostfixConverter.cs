using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbstractTermRewriter
{
	public static class InfixToPostfixConverter
	{
		private static void AddToOutput(List<char> output, params char[] chars)
		{
			if (chars.Length < 1) throw new ArgumentOutOfRangeException();

			output.AddRange(chars);
			output.Add(' ');
		}

		public static string Convert(string infixNotationString)
		{
			if (string.IsNullOrWhiteSpace(infixNotationString)) throw new ArgumentException("Argument infixNotationString must not be null, empty or whitespace.", "infixNotationString");

			string number = string.Empty;
			List<string> enumerableInfixTokens = new List<string>();
			string inputString = new string(infixNotationString.Where(c => ParserTokens.AllowedCharacters.Contains(c)).ToArray());
			foreach (char c in inputString)
			{
				if (ParserTokens.Operators.Contains(c) || "()".Contains(c))
				{
					if (number.Length > 0)
					{
						enumerableInfixTokens.Add(number);
						number = string.Empty;
					}
					enumerableInfixTokens.Add(c.ToString());
				}
				else if (ParserTokens.Numbers.Contains(c))
				{
					number += c.ToString();
				}
				else throw new Exception(string.Format("Unexpected character '{0}'.", c));
			}

			if (number.Length > 0)
			{
				enumerableInfixTokens.Add(number);
				number = string.Empty;
			}

			List<char> outputQueue = new List<char>();
			Stack<char> operatorStack = new Stack<char>();
			foreach (string token in enumerableInfixTokens)
			{
				if (ParserTokens.IsNumeric(token))
				{
					AddToOutput(outputQueue, token.ToArray());
				}
				else if (token.Length == 1)
				{
					char c = token[0];

					if (ParserTokens.Numbers.Contains(c))
					{
						AddToOutput(outputQueue, c);
					}
					else if (ParserTokens.Operators.Contains(c))
					{
						if (operatorStack.Count > 0)
						{
							char o = operatorStack.Peek();
							if (
									(ParserTokens.AssociativityDictionary[c] == ParserTokens.Associativity.Left
											&& ParserTokens.PrecedenceDictionary[c] <= ParserTokens.PrecedenceDictionary[o])
									||
									(ParserTokens.AssociativityDictionary[c] == ParserTokens.Associativity.Right
											&& ParserTokens.PrecedenceDictionary[c] < ParserTokens.PrecedenceDictionary[o])
								)
							{
								AddToOutput(outputQueue, operatorStack.Pop());
							}
						}
						operatorStack.Push(c);
					}
					else if (c == '(')
					{
						operatorStack.Push(c);
					}
					else if (c == ')')
					{
						bool leftParenthesisFound = false;
						while (operatorStack.Count > 0)
						{
							char o = operatorStack.Peek();
							if (o == '(')
							{
								operatorStack.Pop();
								leftParenthesisFound = true;
								break;
							}
							AddToOutput(outputQueue, operatorStack.Pop());
						}

						if (!leftParenthesisFound) throw new FormatException("The algebraic string contains mismatched parentheses (missing a left parenthesis).");
					}
					else throw new Exception("Unrecognized character " + c.ToString());
				}
				else throw new Exception(token + " is not numeric or has a length greater than 1.");
			} // end foreach

			while (operatorStack.Count > 0)
			{
				char o = operatorStack.Pop();
				if ("()".Contains(o)) throw new FormatException("The algebraic string contains mismatched parentheses (extra " + (o == '(' ? "left" : "right") + " parenthesis).");

				AddToOutput(outputQueue, o);
			}

			return new string(outputQueue.ToArray()).TrimEnd();
		}
	}
}
