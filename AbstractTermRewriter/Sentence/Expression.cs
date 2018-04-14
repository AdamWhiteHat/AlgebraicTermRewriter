using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AbstractTermRewriter
{
	/// <summary>
	/// An expression consists of a mathematical statement with or without variables, but does not contain an equality or inequality symbol.	/// 
	/// </summary>
	public class Expression : ISentence
	{
		public static Expression Empty = new Expression();
		public IEnumerable<IOperator> Operators { get { return Elements.Where(e => e.Type == ElementType.Operator).Select(e => (e as IOperator)); } }
		public IEnumerable<INumber> Numbers { get { return Elements.Where(e => e.Type == ElementType.Number).Select(e => (e as INumber)); } }
		public IEnumerable<IVariable> Variables { get { return Elements.Where(e => e.Type == ElementType.Variable).Select(e => (e as IVariable)); } }
		public IEnumerable<ITerm> Terms { get { return Elements.Where(e => e.Type == ElementType.Number || e.Type == ElementType.Variable).Select(e => (e as ITerm)); } }

		public List<IElement> Elements = new List<IElement>();

		public int ElementCount { get { return Elements.Count; } }

		public bool IsSimplified { get { return ElementCount == 1 || ElementCount == 2; } }
		public bool IsVariableIsolated { get { return (IsSimplified && Elements.First().Type == ElementType.Variable); } }

		public int Value
		{
			get
			{
				if (!IsSimplified) throw new Exception("Expression is not a single value.");
				if (!(Elements.First() is INumber)) throw new Exception("Expression is not a numeric value.");
				else return (Elements.Single() as INumber).Value;
			}
		}


		private Expression()
		{
			Elements = new List<IElement>();
		}

		public Expression(string input)
		{
			Elements = ExpressionStringParser(input).ToList();
		}

		private static IElement[] ExpressionStringParser(string expression)
		{
			if (string.IsNullOrWhiteSpace(expression))
			{
				throw new ArgumentException($"{nameof(expression)} cannot be null, empty or white space.");
			}

			if (expression.Any(c => Types.Comparative.Contains(c)))
			{
				throw new ArgumentException("An expression contains no comparative symbols. You want an Equation.");
			}

			Stack<char> stack = new Stack<char>(expression.Replace(" ", "").Reverse());

			List<IElement> result = new List<IElement>();

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

				result.Add(newElement);
			}

			return result.ToArray();
		}


		public IElement ElementAt(int index)
		{
			if (index < 0 || index > ElementCount - 1)
			{
				return Element.None;
			}

			return Elements.ElementAt(index);
		}

		internal void AddElement(IElement newElement)
		{
			Elements.Add(newElement);
		}

		public void Insert(TermOperatorPair pair)
		{
			if (pair == null) throw new ArgumentNullException();

			if (pair.Orientation == InsertOrientation.Left)
			{
				Elements.Insert(0, pair.Operator);
				Elements.Insert(0, pair.Term);
			}
			else
			{
				Elements.Add(pair.Operator);
				Elements.Add(pair.Term);
			}
		}

		public override string ToString()
		{
			return string.Join(" ", Elements.Select(e => e.Symbol));
		}
	}
}
