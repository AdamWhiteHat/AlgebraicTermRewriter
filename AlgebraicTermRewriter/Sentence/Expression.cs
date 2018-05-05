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

		public Expression(IElement[] elements)
		{
			Elements = elements.ToList();
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

		public Expression Clone()
		{
			return new Expression(this.Elements.ToArray());
		}

		public override string ToString()
		{
			return string.Join(" ", Elements.Select(e => e.Symbol));
		}
	}
}
