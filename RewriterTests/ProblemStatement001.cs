using System;
using System.Linq;
using AbstractTermRewriter;
using Microsoft.VisualStudio.TestTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace RewriterTests
{
	[TestClass]
	public class ProblemStatement001
	{
		public Expression Clone(Expression exp)
		{
			return new Expression(exp.ToString());
		}


		static string[] problemStatement =
			   new string[]
			   {
					" 2 * x + 4 = 10",
					" 3 * y ^ 2 - 2 = 20",
			   };

		static Problem testProblem;
		static Equation testEquation;
		static Expression exp;

		[ClassInitializeAttribute]
		public static void Initialize(TestContext context)
		{
			testProblem = new Problem(problemStatement);
			testEquation = (Equation)testProblem.Statements[0];
			exp = testEquation.LeftHandSide;
		}

		[TestMethod]
		public void TestInitializedSucceeded()
		{
			Assert.IsNotNull(testProblem);
			Assert.IsNotNull(testEquation);
			Assert.IsNotNull(exp);
			Print(problemStatement[0]);
			Print("=");
			Print(testEquation.ToString());
			Print();
			Print(exp.ToString());
			Print();
		}

		[TestMethod]
		public void TestExpressionExtensionMethods()
		{
			IElement elm = exp.ElementAt(2);
			Assert.AreEqual(ElementType.Variable, elm.Type);
			Variable var = (Variable)elm;

			Print(exp.ToString());
			Print("---");
			Print();
			Print("Selected element: " + var.ToString());
			Print();
			Print("RightOfElement: " + exp.RightOfElement(var).ToString());
			Print("LeftOfElement: " + exp.LeftOfElement(var).ToString());
			Print();
			Print("---");
			Print();

			IElement firstElm = exp.ElementAt(0);
			IElement lastElm = exp.ElementAt(exp.ElementCount - 1);

			Print(exp.LeftOfElement(firstElm).ToString());
			Print(exp.RightOfElement(lastElm).ToString());
		}

		[TestMethod]
		public void TestExtractElement()
		{
			Expression e1 = Clone(exp);
			Expression e2 = Clone(exp);
			Expression e3 = Clone(exp);


			Print(ExtractInfo(e1, 0));
			Print();
			Print(ExtractInfo(e2, 4));
			Print();
			Print(ExtractInfo(e3, 2));
			Print();
		}

		string ExtractInfo(Expression e, int index)
		{
			return
			$"Index: {index}{Environment.NewLine}{Environment.NewLine}" +
			$"Before: {e.ToString()}{Environment.NewLine}" +
			//$"Extract: {index.AsString()}{Environment.NewLine}" +
			$"After: {e.ToString()}{Environment.NewLine}" +
			$"---{Environment.NewLine}";
		}

		[TestMethod]
		public void TestTypeResolution()
		{
			IElement isNum = new Number(3);
			IElement isOp = new Operator('*');
			IElement isVar = new Variable('x');

			INumber number = new Number(2);
			IElement isElement = Element.None;

			Assert.AreEqual(isNum.Type, ElementType.Number, "isNum.Type, ElementType.Number  ");
			Assert.AreEqual(isOp.Type, ElementType.Operator, "isOp.Type,  ElementType.Operator");
			Assert.AreEqual(isVar.Type, ElementType.Variable, "isVar.Type, ElementType.Variable");


			IElement elementFromNumber = number as IElement;
			Assert.IsNotNull(elementFromNumber, "IElement elementFromNumber = number as IElement");


			Assert.IsTrue(typeof(IElement).IsAssignableFrom(typeof(INumber)), "	typeof(IElement).IsAssignableFrom(typeof(INumber))");
			Assert.IsTrue(typeof(IElement).IsAssignableFrom(typeof(IOperator)), "typeof(IElement).IsAssignableFrom(typeof(IOperator)) ");
			Assert.IsTrue(typeof(IElement).IsAssignableFrom(typeof(IVariable)), "typeof(IElement).IsAssignableFrom(typeof(IVariable))");
			Assert.IsTrue(typeof(IElement).IsAssignableFrom(typeof(ITerm)), "typeof(IElement).IsAssignableFrom(typeof(ITerm))");
			Assert.IsTrue(typeof(ITerm).IsAssignableFrom(typeof(INumber)), "typeof(ITerm).IsAssignableFrom(typeof(INumber))");
			Assert.IsTrue(typeof(ITerm).IsAssignableFrom(typeof(IVariable)), "typeof(ITerm).IsAssignableFrom(typeof(IVariable))");


			IOperator opMulti = new Operator('*');
			IVariable varY = new Variable('y');
			INumber numSeven = new Number(7);

			List<IElement> mixedElements = new List<IElement>();
			mixedElements.Add(opMulti);
			mixedElements.Add(varY);
			mixedElements.Add(numSeven);

			Assert.IsTrue(mixedElements[0] is IOperator, "elements[0] is IOperator");
			Assert.IsTrue(mixedElements[1] is IVariable, "elements[1] is IVariable");
			Assert.IsTrue(mixedElements[2] is INumber, "elements[2] is INumber  ");
			Assert.IsTrue(mixedElements[2] is IElement, "elements[2] is IElement  ");


			mixedElements.Add(number);
			mixedElements.Add(isElement);
			mixedElements.Add(isNum);
			mixedElements.Add(isOp);
			mixedElements.Add(isVar);

			int termCount = mixedElements.Where(e => e is ITerm).Count();

			Assert.AreEqual(5, termCount);


			IEnumerable<IElement> col = mixedElements.ToList();

			int complexity = exp.RankComplexity();
			Print($"Complexity: {complexity}");
		}



		//}

		//[TestMethod]
		//public void TestSerialize()
		//{
		//}		










		public void Print(string message = " ") { Print("{0}", message); }
		public void Print(string message, params object[] args) { TestContext.WriteLine(message, args); }

		private TestContext testContextInstance;
		public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }
	}
}
