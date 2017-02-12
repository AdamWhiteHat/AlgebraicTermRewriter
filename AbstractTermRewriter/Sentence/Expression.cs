using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AbstractTermRewriter
{
	public enum EqualitySide
	{
		Left,
		Right
	}

	// Examples of an expression:
	//  5 * 3
	//  5 * y

	/// <summary>
	/// An expression consists of a mathematical statement with or without variables, but does not contain an equality or inequality symbol.
	/// 
	/// </summary>
	public class Expression : ISentence
	{
		//public int Exponent { get; private set; }
		//public int Coefficient { get; private set; }		
		public Element[] Operator { get { return Elements.Where(e => e.Type == ElementType.Operator).ToArray(); } }
		public Element[] Constants { get { return Elements.Where(e => e.Type == ElementType.Number).ToArray(); } }
		public Element[] Variables { get { return Elements.Where(e => e.Type == ElementType.Variable).ToArray(); } }

		public bool IsFullyReduced { get; private set; } = false;
		public bool HasVariables { get { return Variables.Any(); } }

		public EqualitySide SideOfEqualitySymbol { get; internal set; }

		public List<Element> Elements { get; private set; }

		public Expression(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				throw new ArgumentException();
			}

			foreach (char c in input)
			{
				AddElement(c);
			}
		}

		internal void AddElement(char c)
		{
			Element newElement = new Element(c);
			Elements.Add(newElement);
			newElement.SetParentExpression(this);
		}
	}
}
