using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractTermRewriter
{
	public static class ExpressionExtensionMethods_Checks
	{
		public static void CombineLikeValues(this Expression source)
		{
			// // If two adjacent variables with acting operators of like precedence
			// If two or more values separated by only operators with like precedence			
		}

		public static void Substitute(this Expression source, IVariable variable, INumber value)
		{
			if (source.Elements.Any(e => e.Symbol == variable.Symbol))
			{
				int index = -1;
				int elementCount = source.ElementCount;
				while (index++ < elementCount)
				{
					IElement currentElement = source.Elements.ElementAt(index);

					if (currentElement.Symbol == variable.Symbol)
					{
						source.Elements.RemoveAt(index);
						source.Elements.Insert(index, variable);
					}
				}
			}
		}
		public static int ArithmeticEvaluation(this Expression source)
		{
			if (!source.OnlyArithmeticElements())
			{
				throw new ArithmeticException("Expression elements not arithmetic only.");
			}

			string infixExpression = source.ToString();
			return InfixNotationEvaluator.Evaluate(infixExpression);
		}
		public static bool OnlyArithmeticElements(this Expression source)
		{
			return (!source.Elements.Any(e => e.Type == ElementType.Variable)
				&& source.Elements.Any(e => e.Type == ElementType.Number));
		}
		public static bool OperatorsAllSame(this Expression source)
		{
			var operators = source.Elements.Where(e => e is IOperator);

			int count = operators.Select(o => o.Type).Distinct().Count();

			return (count == 1 && operators.Count() != 1);
		}
		public static bool TermsAllSame(this Expression source, TermType type)
		{
			ElementType elementType = type.GetElementType();
			return source.Elements.Select(e => e.Type).Same();
		}
		public static int CountType(this Expression source, ElementType type)
		{
			return source.Elements.Where(e => e.Type == type).Count();
		}
		public static int RankComplexity(this Expression source)
		{
			int elementCount = source.Elements.Where(e => e is ITerm).Count();

			if (elementCount == 1)
			{
				ElementType singletonType = source.Elements.First().Type;
				if (singletonType == ElementType.Number) return 1;
				else if (singletonType == ElementType.Variable) return 2;
			}
			else if (elementCount == 2)
			{
				ElementType firstType = source.Elements[0].Type;
				ElementType secondType = source.Elements[1].Type;

				if (firstType == secondType)
				{
					if (firstType == ElementType.Number) return 1;
					else return 3;
				}
				else return 2;
			}
			else if (elementCount == 3)
			{
				int variables = source.CountType(ElementType.Variable);
				if (variables >= 2) return 4;
				else return 3;
			}
			return 5;
		}
	}

	public static class ExpressionExtensionMethods_Manipulations
	{
		public static IElement LeftOfElement(this Expression source, IElement element)
		{
			int index = source.Elements.IndexOf(element);
			if (index == 0) return Element.None;
			else return source.ElementAt(index - 1);
		}

		public static IElement RightOfElement(this Expression source, IElement element)
		{
			int index = source.Elements.IndexOf(element);
			if (index == source.ElementCount - 1) return Element.None;
			else return source.ElementAt(index + 1);
		}

		public static TermOperatorPair Extract(this Expression source, IElement element)
		{
			return source.Extract(source.Elements.IndexOf(element));
		}

		public static TermOperatorPair Extract(this Expression source, int elementIndex)
		{
			if (elementIndex < 0 || elementIndex > source.Elements.Count - 1) throw new IndexOutOfRangeException(nameof(elementIndex));

			int operationIndex = elementIndex - 1;
			if (elementIndex == 0)
			{
				operationIndex = 1;
			}

			IOperator oper = (IOperator)source.ElementAt(operationIndex);
			ITerm term = (ITerm)source.ElementAt(elementIndex);

			source.Elements.Remove(oper);
			source.Elements.Remove(term);

			oper = Operator.GetInverse(oper);


			bool operatorLeft = false;
			if (operationIndex < elementIndex)
			{
				operatorLeft = true;
			}

			if (operatorLeft)
			{
				return new TermOperatorPair(term, oper, InsertOrientation.Left);
			}
			else
			{
				return new TermOperatorPair(term, oper, InsertOrientation.Right);
			}
		}
	}
}
