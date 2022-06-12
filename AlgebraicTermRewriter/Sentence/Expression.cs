using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
		public IEnumerable<ITerm> Terms { get { return Tokens.Where(e => e.Type == TokenType.Number || e.Type == TokenType.Variable).Select(e => (e as ITerm)); } }

		public IEnumerable<IToken> Tokens { get { return SubExpressions.SelectMany(s => s).ToArray(); } }

		public List<SubExpression> SubExpressions = new List<SubExpression>();

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
			SubExpressions = new List<SubExpression>();
		}

		public Expression(IToken[] tokens)
			: this()
		{
			SubExpressions.Add(new SubExpression(tokens));
		}

		public bool Contains(IToken token)
		{
			return Contains(token.Contents);
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
			if (SubExpressions.Count == 1)
			{
				SubExpressions[0].Add(newToken);
			}
			else
			{
				SubExpressions.Add(new SubExpression() { newToken });
			}
		}

		internal void RemoveAt(int index)
		{
			Tuple<SubExpression, int> result = GetSubExpressionAtIndex(index);
			int subIndex = result.Item2;
			SubExpression found = result.Item1;

			if (found != null)
			{
				found.RemoveAt(subIndex);
			}
		}

		private Tuple<SubExpression, int> GetSubExpressionAtIndex(int index)
		{
			int subIndex = 0;
			int globalIndex = 0;
			foreach (SubExpression subExpr in SubExpressions)
			{
				subIndex = 0;
				foreach (IToken token in subExpr)
				{
					if (globalIndex == index)
					{
						return new Tuple<SubExpression, int>(subExpr, subIndex);
					}
					subIndex++;
					globalIndex++;
				}
			}
			throw new IndexOutOfRangeException();
		}

		internal void Remove(IToken token)
		{
			int index = 0;
			int maxIndex = SubExpressions.Count;
			while (index < maxIndex)
			{
				if (SubExpressions[index].Contains(token))
				{
					SubExpressions[index].Remove(token);
				}
				index++;
			}
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
			return Tokens.ToList().IndexOf(token);
		}

		public void Insert(OperatorExpressionPair pair)
		{
			if (pair == null) throw new ArgumentNullException();

			if (pair.Orientation == InsertOrientation.Left)
			{
				Insert(0, pair.Operator);
				InsertRange(0, pair.Expr);
			}
			else
			{
				Add(pair.Operator);
				AddRange(pair.Expr);
			}
		}

		public void Add(IToken item)
		{
			SubExpressions.Last().Add(item);
		}

		public void AddRange(SubExpression collection)
		{
			SubExpressions.Last().AddRange(collection);
		}

		public void Insert(int index, IToken item)
		{
			if (SubExpressions.Count == 1)
			{
				SubExpressions[0].Insert(index, item);
			}
			else
			{
				Tuple<SubExpression, int> result = GetSubExpressionAtIndex(index);
				int subIndex = result.Item2;
				SubExpression found = result.Item1;
				if (found != null)
				{
					found.Insert(subIndex, item);
				}
			}
		}

		public void InsertRange(int index, SubExpression collection)
		{
			if (SubExpressions.Count == 1)
			{
				//List<IToken> newTokens = Tokens.ToList();
				//newTokens.InsertRange(index, collection);
				//SubExpressions = new List<SubExpression>() { new SubExpression(newTokens.ToArray()) };
				SubExpressions[0].InsertRange(index, collection);
			}
			else
			{
				Tuple<SubExpression, int> result = GetSubExpressionAtIndex(index);
				int subIndex = result.Item2;
				SubExpression found = result.Item1;
				if (found != null)
				{
					found.InsertRange(subIndex, collection);
				}
			}
		}

		public Expression Clone()
		{
			return new Expression(this.Tokens.ToArray());
		}

		public override string ToString()
		{
			if (SubExpressions.Count == 1)
			{
				return SubExpressions.Single().ToString();
			}
			return string.Join(" ", SubExpressions.Select(e => e.Count == 1 ? e.ToString() : $"({e})"));
		}
	}
}
