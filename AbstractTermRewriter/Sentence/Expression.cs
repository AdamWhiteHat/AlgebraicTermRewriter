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
			AddElement(newElement);
		}

		internal void AddElement(Element newElement)
		{
			Elements.Add(newElement);
			newElement.SetParentExpression(this);
		}

		public void Insert(Element[] elements)
		{
			if(elements == null || elements.Length != 2)
			{
				throw new ArgumentException();
			}

			bool operatorBefore = false;
			if (elements[0].IsType(ElementType.Operator))
			{
				operatorBefore = true;
			}
			else if(elements[1].IsType(ElementType.Operator))
			{
				operatorBefore = false;
			}

			Element[] toAdd = new Element[] { };

			if(!operatorBefore)
			{
				toAdd = elements.Reverse().ToArray();
			}
			else
			{
				toAdd = elements.ToArray();
			}

			foreach (Element e in toAdd)
			{
				AddElement(e);
			}
		}

		public Element[] Extract(Element element)
		{
			return Extract(Elements.IndexOf(element));
		}

		public Element[] Extract(int elementIndex)
		{
			if (elementIndex < 1 || elementIndex > Elements.Count - 1)
			{
				throw new IndexOutOfRangeException();
			}

			int operationIndex = -1;

			if (elementIndex == 0)
			{
				operationIndex = 1;
			}
			else
			{
				operationIndex = elementIndex - 1;
			}

			List<Element> result = new List<Element>();

			Element operation = Elements[operationIndex];
			Element element = Elements[elementIndex];

			Elements.Remove(operation);
			Elements.Remove(element);

			operation = Types.Operation.GetOpposite(operation);

			result.Add(element);

			if (operationIndex < elementIndex)
			{
				result.Insert(0, operation);
			}
			else
			{
				result.Add(element);
			}

			return result.ToArray();
		}
	}
}
