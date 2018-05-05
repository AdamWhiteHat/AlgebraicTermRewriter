using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgebraicTermRewriter
{
	public static class ExpressionExtensionMethods_Checks
	{
		public static void CombineArithmeticElements(this Expression source)
		{
			Tuple<int, int> range = source.Elements.GetLongestArithmeticRange();

			if (range == null)
			{
				return;
			}

			List<IElement> arithmeticExpression = source.Elements.GetRange(range.Item1, range.Item2);

			string toEvaluate = string.Join("", arithmeticExpression.Select(e => e.Symbol));

			INumber newValue = new Number(InfixNotationEvaluator.Evaluate(toEvaluate));

			source.Elements.RemoveRange(range.Item1, range.Item2);

			int insertIndex = range.Item1 == 0 ? 0 : range.Item1 - 1;

			if (newValue.Value == 0)
			{
				if (source.ElementCount == 0)
				{
					source.Elements.Insert(0, new Number(0));
					return;
				}
				IElement op = source.ElementAt(insertIndex);
				if (op.Symbol != "*")
				{
					source.Elements.RemoveAt(insertIndex);
				}
			}
			else
			{
				source.Elements.Insert(insertIndex, newValue);
			}
		}

		public static void Substitute(this Expression source, IVariable variable, IElement[] expression)
		{
			if (source.Elements.Any(e => e.Symbol == variable.Symbol))
			{
				int index = -1;
				int elementCount = source.ElementCount;
				while (index++ < elementCount)
				{
					IElement currentElement = source.ElementAt(index);

					if (currentElement.Symbol == variable.Symbol)
					{
						source.Elements.RemoveAt(index);
						source.Elements.InsertRange(index, expression);
					}
				}
			}
		}

		public static bool OnlyArithmeticElements(this Expression source)
		{
			return (!source.Variables.Any()
				&& source.Numbers.Any());
		}

		public static int RankComplexity(this Expression source)
		{
			int elementCount = source.Terms.Count();

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
				int variables = source.Variables.Count();
				if (variables >= 2) return 4;
				else return 3;
			}
			return 5;
		}

		public static TermOperatorPair Extract(this Expression source, int elementIndex)
		{
			if (elementIndex < 0 || elementIndex > source.ElementCount - 1) throw new IndexOutOfRangeException(nameof(elementIndex));

			int operationIndex = elementIndex - 1;
			//if (elementIndex == 0)
			//{
			//	operationIndex = 1;
			//}

			ITerm term = source.ElementAt(elementIndex) as ITerm;
			IOperator oper = null;

			if (operationIndex == -1)
			{
				oper = source.RightOfElement(term) as IOperator;
				if (oper.Symbol == "-")
				{
					oper = new Operator('+');
				}
			}
			else
			{
				oper = source.ElementAt(operationIndex) as IOperator;
			}

			source.Elements.Remove(term);
			source.Elements.Remove(oper);


			InsertOrientation orientation = InsertOrientation.Either;
			if (operationIndex < elementIndex)
			{
				//if (oper.Symbol == "+" || oper.Symbol == "*")
				//{
				oper = Operator.GetInverse(oper);
				orientation = InsertOrientation.Right;
				//}
			}
			else// if (elementIndex == 0)
			{
				oper = Operator.GetInverse(oper);
				orientation = InsertOrientation.Right;
			}
			//if (operationIndex < elementIndex)
			//{
			//	orientation = InsertOrientation.Left;
			//}
			//else
			//{
			//	orientation = InsertOrientation.Right;
			//}


			return new TermOperatorPair(term, oper, orientation);
		}

		public static void SetToMultiplicativeInverse2(this Expression source)
		{
			string expression = source.ToString()?.Replace(" ", "") ?? "";

			if (expression.StartsWith("0"))
			{
				source.Elements.RemoveAt(0);
				source.Elements.RemoveAt(0);
				return;
			}

			if (expression.StartsWith("-"))
			{
				source.Elements.RemoveAt(0);
			}
			else
			{
				if (source.ElementCount > 0)
				{
					INumber num = source.Elements[0] as INumber;
					if (num != null)
					{
						int newNum = -num.Value;
						source.Elements[0] = new Number(newNum);
						return;
					}
				}
			}
		}

		public static void SetToMultiplicativeInverse(this Expression source)
		{
			bool isNegative = false;
			IElement first = null;
			IElement second = null;

			first = source.Elements.FirstOrDefault();
			if (first != null && first.Symbol == "0")
			{
				second = source.Elements.Skip(1).FirstOrDefault();
				if (second != null && second.Symbol == "-")
				{
					isNegative = true;
				}
			}


			if (!isNegative)
			{
				first = source.Elements.FirstOrDefault();
				if (first != null && first.Symbol == "-")
				{
					second = source.Elements.Skip(1).FirstOrDefault();
					if (second != null && second is IVariable)
					{
						isNegative = true;
					}
				}
			}

			if (isNegative)
			{
				source.Elements.Remove(first);
				source.Elements.Remove(second);
			}

		}
	}


	#region Unused / Dead code
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
	}
	#endregion

	//public static void CombineVariables(this Expression source)
	//{
	//	var vars = source.Variables.Select(e => (e as IVariable).Value).ToList();
	//	var distinctVars = vars.Distinct();

	//	if (distinctVars.Count() == vars.Count())
	//	{
	//		return;
	//	}

	//	string elements = string.Join("", source.Elements.Select(e => e.Symbol[0]));

	//	var found = new List<Tuple<char, List<int>>>();
	//	foreach (char distinct in distinctVars)
	//	{
	//		List<int> indices = Enumerable.Range(0, elements.Length - 1).Where(i => distinct.Equals(elements[i])).ToList();

	//		if (indices.Count > 1)
	//		{
	//			found.Add(new Tuple<char, List<int>>(distinct, indices));
	//		}
	//	}

	//	if (!found.Any()) return;

	//	found = found.OrderBy(tup => tup.Item2.Count).ToList();

	//	Tuple<char, List<int>> first = found.First();

	//	int value = 0;
	//	int insertPosition = -1;
	//	foreach (int index in first.Item2)
	//	{
	//		if (index == 0)
	//		{
	//			value = 1;
	//			insertPosition = 0;
	//			continue;
	//		}

	//		IElement element = source.ElementAt(index);

	//		IOperator left = source.LeftOfElement(element) as IOperator;

	//		if (left.Symbol == "+")
	//		{
	//			value += 1;
	//			insertPosition = index;

	//		}
	//		else if (left.Symbol == "-")
	//		{
	//			value -= 1;
	//			insertPosition = index - 1;
	//		}
	//		else if (left.Symbol == "*")
	//		{
	//			IElement leftLeft = source.LeftOfElement(left);
	//			if (leftLeft != Element.None && leftLeft.Type == ElementType.Number)
	//			{
	//				int multiplyer = (leftLeft as INumber).Value;

	//				IElement leftLeftLeft = source.LeftOfElement(leftLeft);

	//				if (leftLeftLeft.Symbol == "-")
	//				{
	//					value -= multiplyer;
	//					insertPosition = index - 3;
	//				}
	//				else //if (leftLeftLeft == Element.None || leftLeftLeft.Symbol == "+")
	//				{
	//					value += multiplyer;
	//					insertPosition = index - 2;
	//				}
	//			}
	//			else
	//			{
	//				value += 1;
	//				insertPosition = index;
	//			}
	//		}
	//		else
	//		{
	//			value += 1;
	//			insertPosition = index;
	//		}


	//	}

	//}
}
