using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	/// <summary>
	/// An expression consists of a mathematical statement with or without variables, but does not contain an equality or inequality symbol.	/// 
	/// </summary>
	public class Expression : ISentence, ICloneable<Expression>
	{
		public static Expression Empty = new Expression();
		public IEnumerable<IOperator> Operators { get { return Tokens.Where(e => e.Type == TokenType.Operator).Select(e => (e as IOperator)); } }
		public IEnumerable<INumber> Numbers { get { return Tokens.Where(e => e.Type == TokenType.Number).Select(e => (e as INumber)); } }
		public IEnumerable<IVariable> Variables { get { return Tokens.Where(e => e.Type == TokenType.Variable).Select(e => (e as IVariable)); } }
		public IEnumerable<SubExpression> SubExpressions { get { return Tokens.Where(e => e.Type == TokenType.Subexpression).Select(e => (e as SubExpression)); } }
		public IEnumerable<ITerm> Terms { get { return Tokens.Where(e => e.Type == TokenType.Number || e.Type == TokenType.Variable).Select(e => (e as ITerm)); } }

		public IEnumerable<IToken> Tokens { get { return _tokens.AsEnumerable(); } }
		private List<IToken> _tokens = null;

		public Equation Parent { get; set; }

		public RelativeDirection? SideOfEquality
		{
			get
			{
				RelativeDirection? result = null;
				Equation parentEquation = this.Parent;
				if (parentEquation != null && !Equation.Equals(parentEquation, Equation.Empty))
				{

					if (Expression.Equals(this, parentEquation.LeftHandSide))
					{
						result = RelativeDirection.Left;
					}
					else if (Expression.Equals(this, parentEquation.RightHandSide))
					{
						result = RelativeDirection.Right;
					}
				}
				return result;
			}
		}

		public int TokenCount { get { return Tokens.Count(); } }

		public bool IsSimplified { get { if (TokenCount == 1) { return true; } else { if (TokenCount == 2) { return true; } else { return false; } } } }
		public bool IsVariableIsolated { get { return (IsSimplified && Tokens.First().Type == TokenType.Variable); } }

		public int Value
		{
			get
			{
				if (!IsSimplified) throw new Exception("Expression is not a single value.");
				if (!(Tokens.First() is INumber)) throw new Exception("Expression is not a numeric value.");
				else return (Tokens.Single() as INumber).Value;
			}
		}

		private Expression()
		{
			_tokens = new List<IToken>();
		}

		public Expression(IToken[] tokens)
			: this()
		{
			_tokens.AddRange(tokens);
		}

		public static Expression Parse(string expressionText)
		{
			if (string.IsNullOrWhiteSpace(expressionText))
			{
				throw new ArgumentException($"{nameof(expressionText)} cannot be null, empty or whitespace.");
			}

			if (expressionText.Any(c => Types.Comparison.Contains(c)))
			{
				throw new Exception($"{nameof(expressionText)} contains an equality or comparison operator (=, >, <, >=, <=). Perhaps you meant to parse it as an {nameof(Equation)} instead?");
			}
			else
			{
				return MathParser.ParseExpression(expressionText);
			}
		}

		public Expression Simplify()
		{
			if (this._tokens.CanSimplify())
			{
				return Expression.Parse(InfixNotationEvaluator.Evaluate(this.ToString()).ToString());
			}
			return this;
		}

		public bool CanSimplify()
		{
			return this._tokens.CanSimplify();
		}

		public bool Contains(IToken token)
		{
			return Tokens.Any(tok => IToken.Equals(tok, token));
		}

		public bool Contains(string contents)
		{
			return Tokens.Any(tok => tok.Contents == contents);
		}

		public IToken TokenAt(int index)
		{
			if (index < 0 || index > TokenCount - 1)
			{
				return Token.None;
			}

			return Tokens.ElementAt(index);
		}

		internal void AddToken(IToken newToken)
		{
			IToken clone = newToken.Clone();
			_tokens.Add(clone);
		}

		internal void ReplaceAt(int index, IToken replacementToken)
		{
			IToken clone = replacementToken.Clone();
			_tokens.RemoveAt(index);
			_tokens.Insert(index, clone);
		}

		internal void RemoveAt(int index)
		{
			_tokens.RemoveAt(index);
		}

		internal void Remove(IToken token)
		{
			int index = IndexOf(token);
			if (index != -1)
			{
				_tokens.RemoveAt(index);
			}
		}

		internal int RemoveAll(IToken token)
		{
			return _tokens.RemoveAll(t => IToken.Equals(t, token));
		}

		internal void RemoveRange(Range range)
		{
			RemoveRange(range.StartIndex, range.Count);
		}

		internal void RemoveRange(int start, int count)
		{
			int[] indices = Enumerable.Range(start, count).Reverse().ToArray();
			foreach (int index in indices)
			{
				RemoveAt(index);
			}
		}

		internal int IndexOf(IToken token)
		{
			return _tokens.FindIndex(t => IToken.Equals(t, token));
		}

		public void Insert(OperatorExpressionPair pair)
		{
			if (pair == null) throw new ArgumentNullException();

			OperatorExpressionPair clone = pair;

			if (clone.Orientation == InsertOrientation.Left)
			{
				Insert(0, clone.Operator);
				InsertRange(0, clone.Expr);
			}
			else
			{
				Add(clone.Operator);
				AddRange(clone.Expr);
			}
		}

		public void Add(IToken item)
		{
			_tokens.Add(item);
		}

		public void AddRange(SubExpression collection)
		{
			_tokens.AddRange(collection);
		}

		public void Insert(int index, IToken item)
		{
			IToken clone = item.Clone();
			_tokens.Insert(index, clone);
		}

		public void InsertRange(int index, SubExpression collection)
		{
			_tokens.InsertRange(index, collection.AsEnumerable());
		}

		/// <summary>
		/// Finds all numbers and their associated operations.
		/// For each number it creates a tuple with the operator's precedence and the number's index, in that order.
		/// Returns a list of such tuples, ordered by precedence in ascending order.
		/// </summary>
		/// <returns>A list of tuples of the form: (precedence, index), ordered by precedence in ascending order.</returns>
		public List<Tuple<IOperator, INumber>> GetOperatorTermIndexPairs()
		{
			var results = new List<Tuple<IOperator, INumber>>();

			foreach (INumber candidate in this.Numbers)
			{
				INumber term = candidate;

				int termIndex = this.IndexOf(term);

				IToken op = null;

				if (termIndex == 0)
				{
					op = this.RightOfToken(term);

					if (op.Contents == "/")
					{
						IToken alternative = this.RightOfToken(op);
						term = (INumber)alternative;
					}
					//else if (op.Contents == "+" || op.Contents == "-")
					//{
					//	op = new Operator('+');
					//	term.Negate();
					//}
				}
				else
				{
					op = this.LeftOfToken(term);
				}

				IOperator operation = op as IOperator;
				if (operation == null)
				{
					throw new Exception("Was expecting to find Operator.");
				}

				results.Add(new Tuple<IOperator, INumber>((IOperator)operation.Clone(), (INumber)term.Clone()));
			}

			return results.OrderBy(tup => GetOperatorSolveOrder(tup.Item1)).ToList();
		}

		private static int GetOperatorSolveOrder(IOperator operation)
		{
			int weight = ParserTokens.PrecedenceDictionary[operation.Symbol];

			if (operation.Symbol == '/') weight += 1; // Prefer other operations first
			if (operation.Symbol == '^') weight += 2; // One does not simply negate an exponent and move it to the other side...

			return weight;
		}

		public bool Equals(Expression other)
		{
			return Expression.Equals(this, other);
		}

		public static bool Equals(Expression left, Expression right)
		{
			if (left == null)
			{
				return (right == null);
			}
			else if (right == null)
			{
				return false;
			}

			if (left.TokenCount != right.TokenCount)
			{
				return false;
			}

			int index = 0;
			while (index < left.TokenCount)
			{
				if (!IToken.Equals(left.TokenAt(index), right.TokenAt(index)))
				{
					return false;
				}
				index++;
			}

			return true;
		}

		public Expression Clone()
		{
			return new Expression(this.Tokens.Select(tok => tok.Clone()).ToArray());
		}

		public override string ToString()
		{
			return string.Join(" ", _tokens.Select(e => e.ToString()));
		}
	}
}
