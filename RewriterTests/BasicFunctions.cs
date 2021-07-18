using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlgebraicTermRewriter;

namespace RewriterTests
{
	[TestClass]
	public class BasicFunctions
	{
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
			IToken elm = exp.TokenAt(2);
			Assert.AreEqual(TokenType.Variable, elm.Type);
			Variable var = (Variable)elm;

			Print(exp.ToString());
			Print("---");
			Print();
			Print("Selected token: " + var.ToString());
			Print();
			Print("RightOfToken: " + exp.RightOfToken(var).ToString());
			Print("LeftOfToken: " + exp.LeftOfToken(var).ToString());
			Print();
			Print("---");
			Print();

			IToken firstElm = exp.TokenAt(0);
			IToken lastElm = exp.TokenAt(exp.TokenCount - 1);

			Print(exp.LeftOfToken(firstElm).ToString());
			Print(exp.RightOfToken(lastElm).ToString());
		}

		[TestMethod]
		public void TestSimplify_AddVariableGroups()
		{
			// "5 * x + 3 * x"
			string expected = "8 * x";
			Expression expr = new Expression(new IToken[] { new Number(5), new Operator('*'), new Variable('x'), new Operator('+'), new Number(3), new Operator('*'), new Variable('x') });

			expr.Simplify();

			TestAssertResults(expr, expected);
		}

		[TestMethod]
		public void TestSimplify_UnitaryOperations()
		{
			string expected = "14";
			Expression expr = new Expression(new IToken[] { new Operator('+'), new Number(14) });

			expr.Simplify();

			TestAssertResults(expr, expected);
		}

		[TestMethod]
		public void TestExtractToken()
		{
			Expression e1 = exp.Clone();
			Expression e2 = exp.Clone();
			Expression e3 = exp.Clone();


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
			IToken isNum = new Number(3);
			IToken isOp = new Operator('*');
			IToken isVar = new Variable('x');

			INumber number = new Number(2);
			IToken isToken = Token.None;

			Assert.AreEqual(isNum.Type, TokenType.Number, "isNum.Type, TokenType.Number  ");
			Assert.AreEqual(isOp.Type, TokenType.Operator, "isOp.Type,  TokenType.Operator");
			Assert.AreEqual(isVar.Type, TokenType.Variable, "isVar.Type, TokenType.Variable");


			IToken tokenFromNumber = number as IToken;
			Assert.IsNotNull(tokenFromNumber, "IToken tokenFromNumber = number as IToken");


			Assert.IsTrue(typeof(IToken).IsAssignableFrom(typeof(INumber)), "	typeof(IToken).IsAssignableFrom(typeof(INumber))");
			Assert.IsTrue(typeof(IToken).IsAssignableFrom(typeof(IOperator)), "typeof(IToken).IsAssignableFrom(typeof(IOperator)) ");
			Assert.IsTrue(typeof(IToken).IsAssignableFrom(typeof(IVariable)), "typeof(IToken).IsAssignableFrom(typeof(IVariable))");
			Assert.IsTrue(typeof(IToken).IsAssignableFrom(typeof(ITerm)), "typeof(IToken).IsAssignableFrom(typeof(ITerm))");
			Assert.IsTrue(typeof(ITerm).IsAssignableFrom(typeof(INumber)), "typeof(ITerm).IsAssignableFrom(typeof(INumber))");
			Assert.IsTrue(typeof(ITerm).IsAssignableFrom(typeof(IVariable)), "typeof(ITerm).IsAssignableFrom(typeof(IVariable))");


			IOperator opMulti = new Operator('*');
			IVariable varY = new Variable('y');
			INumber numSeven = new Number(7);

			List<IToken> mixedTokens = new List<IToken>();
			mixedTokens.Add(opMulti);
			mixedTokens.Add(varY);
			mixedTokens.Add(numSeven);

			Assert.IsTrue(mixedTokens[0] is IOperator, "tokens[0] is IOperator");
			Assert.IsTrue(mixedTokens[1] is IVariable, "tokens[1] is IVariable");
			Assert.IsTrue(mixedTokens[2] is INumber, "tokens[2] is INumber  ");
			Assert.IsTrue(mixedTokens[2] is IToken, "tokens[2] is IToken  ");


			mixedTokens.Add(number);
			mixedTokens.Add(isToken);
			mixedTokens.Add(isNum);
			mixedTokens.Add(isOp);
			mixedTokens.Add(isVar);

			int termCount = mixedTokens.Where(e => e is ITerm).Count();

			Assert.AreEqual(5, termCount);


			IEnumerable<IToken> col = mixedTokens.ToList();

			int complexity = exp.RankComplexity();
			Print($"Complexity: {complexity}");
		}

		private void TestAssertResults(Expression expr, string expected)
		{
			string actual = expr.ToString();

			Print($"Result: [{actual}]           Expecting: ({expected})");
			Print();
			Print("-----");
			Print();

			Assert.AreEqual(expected, actual);
		}

		public void Print(string message = " ") { Print("{0}", message); }
		public void Print(string message, params object[] args) { TestContext.WriteLine(message, args); }

		private TestContext testContextInstance;

		public TestContext TestContext { get { return testContextInstance; } set { testContextInstance = value; } }

	}


}
