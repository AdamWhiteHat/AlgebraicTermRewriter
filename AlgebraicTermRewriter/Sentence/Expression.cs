using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
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

		public List<IToken> Tokens = new List<IToken>();

		public int TokenCount { get { return Tokens.Count; } }

		public bool IsSimplified { get { return TokenCount == 1 || TokenCount == 2; } }
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
			Tokens = new List<IToken>();
		}

		public Expression(IToken[] tokens)
		{
			Tokens = tokens.ToList();
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
			Tokens.Add(newToken);
		}

		public void Insert(TermOperatorPair pair)
		{
			if (pair == null) throw new ArgumentNullException();

			if (pair.Orientation == InsertOrientation.Left)
			{
				Tokens.Insert(0, pair.Operator);
				Tokens.Insert(0, pair.Term);
			}
			else
			{
				Tokens.Add(pair.Operator);
				Tokens.Add(pair.Term);
			}
		}

		public Expression Clone()
		{
			return new Expression(this.Tokens.ToArray());
		}

		public override string ToString()
		{
			return string.Join(" ", Tokens.Select(e => e.Contents));
		}
	}
}
