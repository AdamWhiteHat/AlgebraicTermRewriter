using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgebraicTermRewriter
{
	public class Solver
	{
		private Equation currentEquation;
		private Problem equations;
		private Action<string> LoggingMethod;

		public List<string> Solutions { get; private set; }
		public Dictionary<char, int> SolvedVariables { get; private set; }


		private Expression Left { get { return (currentEquation == null) ? Expression.Empty : currentEquation.LeftHandSide; } }
		private Expression Right { get { return (currentEquation == null) ? Expression.Empty : currentEquation.RightHandSide; } }

		private bool LeftHasVariables { get { return Left.Variables.Any(); } }
		private bool RightHasVariables { get { return Right.Variables.Any(); } }
		public Solver(Problem problem)
			: this(problem, null)
		{
		}

		public Solver(Problem problem, Action<string> loggingMethod)
		{
			LoggingMethod = loggingMethod;
			Solutions = new List<string>();
			SolvedVariables = new Dictionary<char, int>();
			equations = problem;
		}

		public void Solve()
		{
			foreach (ISentence sentence in equations.Statements)
			{
				currentEquation = sentence as Equation;
				if (sentence is Equation)
				{
					SolveEquation(sentence as Equation);
				}
				else if (sentence is Expression)
				{
					SolveExpression(sentence as Expression);
				}
			}
		}

		private void SolveEquation(Equation eq)
		{
			if (eq.OnlyArithmeticTokens())
			{
				Solutions.Add(IsArithmeticEquasionTrue(eq).ToString());
				return;
			}

			//if (eq.RightHandSide.Variables.Any())
			//{
			//	eq.OrientEquation();
			//}

			//Expression left = eq.LeftHandSide;
			//Expression right = eq.RightHandSide;

			//bool leftHasVariables = Left.Variables.Any();
			//bool rightHasVariables = Right.Variables.Any();

			if (LeftHasVariables && RightHasVariables)
			{
				SolveForVariablesOnBothSide(eq);
				return;
			}

			eq.EnsureVariableOnLeft();

			if (LeftHasVariables)
			{
				if (Left.Variables.Count() > 1)
				{
					SolveForMultipleVariables(Left);
				}

				if (!Left.IsVariableIsolated)
				{
					IsolateSingleVariable(eq);
				}

				if (!Left.IsVariableIsolated)
				{
					throw new Exception("Failed to isolate LeftHandSide.");
				}

				if (!Right.IsSimplified)
				{
					throw new Exception("Failed to simplify RightHandSide.");
				}

				Solutions.Add(eq.ToString());
				AddSolvedVariable(Left.Variables.Single(), Right.Numbers.Single());
				return;
			}
			//else if (RightHasVariables)
			//{
			//	if (Right.Variables.Count() > 1)
			//	{
			//		SolveForMultipleVariables(Left);
			//	}

			//	if (!Right.IsVariableIsolated)
			//	{
			//		IsolateSingleVariable(eq, SideOfEquation.Right);
			//	}

			//	if (!Right.IsVariableIsolated)
			//	{
			//		throw new Exception("Failed to isolate RightHandSide.");
			//	}

			//	if (!Left.IsSimplified)
			//	{
			//		throw new Exception("Failed to simplify LeftHandSide.");
			//	}

			//	Solutions.Add(eq.ToString());
			//	AddSolvedVariable(Right.Variables.Single(), Left.Numbers.Single());
			//	return;
			//}

			throw new Exception("Not sure what to do here. Equations should have been solved.");
		}


		/// <summary>
		/// Finds all numbers and their associated operations.
		/// For each number it creates a tuple with the operator's precedence and the number's index, in that order.
		/// Returns a list of such tuples, ordered by precedence in ascending order.
		/// </summary>
		/// <returns>A list of tuples of the form: (precedence, index), ordered by precedence in ascending order.</returns>
		private static List<Tuple<int, int>> GetPrecedenceIndexPairs(Expression from)
		{
			List<Tuple<int, int>> results = new List<Tuple<int, int>>();

			foreach (INumber candidate in from.Numbers)
			{
				int index = from.Tokens.IndexOf(candidate);

				IToken op = null;

				if (index == 0)
				{
					op = from.RightOfToken(candidate);
					if (op.Contents == "/")
					{
						IToken alternative = from.RightOfToken(op);
						index = from.Tokens.IndexOf(alternative);
					}
				}
				else
				{
					op = from.LeftOfToken(candidate);
				}

				IOperator operation = op as IOperator;
				if (operation == null)
				{
					throw new Exception("Was expecting to find Operator.");
				}

				int precedence = ParserTokens.PrecedenceDictionary[operation.Symbol];

				if (operation.Symbol == '/') precedence += 1; // Prefer other operations first
				if (operation.Symbol == '^') continue; // One does not simply negate an exponent and move it to the other side...

				results.Add(new Tuple<int, int>(precedence, index));
			}

			return results.OrderBy(tup => tup.Item1).ToList();
		}

		private static void IsolateSingleVariable(Equation eq)
		{
			Expression from = null;
			Expression to = null;

			eq.EnsureVariableOnLeft();

			while (true)
			{
				from = eq.LeftHandSide;
				to = eq.RightHandSide;

				if (!from.Numbers.Any())
				{
					break;
				}

				List<Tuple<int, int>> precedenceIndexList = GetPrecedenceIndexPairs(from);
				if (!precedenceIndexList.Any())
				{
					break;
				}

				int extractIndex = precedenceIndexList.First().Item2;
				IToken extractTerm = from.Tokens.ElementAtOrDefault(extractIndex);
				if (extractTerm == null)
				{
					throw new Exception("Index does not exist! GetPrecedenceIndexPairs() returned an invalid index positions.");
				}

				IToken op = null;

				if (extractIndex == 0)
				{
					op = from.RightOfToken(extractTerm);
					if (op.Contents == "/")
					{
						extractTerm = from.RightOfToken(op);
						extractIndex = from.Tokens.IndexOf(extractTerm);
					}
				}
				else
				{
					op = from.LeftOfToken(extractTerm);
				}

				TermOperatorPair extracted = from.Extract(extractIndex);
				to.Insert(extracted);

				//if (op.Symbol == "-")
				//{
				//	TermOperatorPair extracted = from.Extract(extractIndex);

				//	TermOperatorPair termPair = new TermOperatorPair(toExtract, new Operator('-'), InsertOrientation.Right);
				//	from.Tokens.InsertRange(1, new IToken[] { termPair.Operator, termPair.Term });
				//	to.Insert(termPair);
				//}
				//else if (op.Symbol == "/")
				//{
				//	TermOperatorPair extracted = from.Extract(extractIndex);
				//	to.Insert(extracted);

				//	//TermOperatorPair pair = from.Extract(2);
				//	//to.Insert(pair);

				//	//Expression temp = to;
				//	//to = from;
				//	//from = temp;
				//}
				//else
				//{
				//	TermOperatorPair extracted = from.Extract(extractIndex);
				//	to.Insert(extracted);
				//	//toExtract = from.TokenAt(0) as ITerm;

				//	//TermOperatorPair pair = from.Extract(0);
				//	//to.Insert(pair);

				//	//Expression temp = to;
				//	//to = from;
				//	//from = temp;
				//}

				to.CombineArithmeticTokens();
				from.CombineArithmeticTokens();

				IToken leadingToken = from.Tokens.First();
				if (leadingToken.Contents == "-")
				{
					to.SetToMultiplicativeInverse2();
					from.SetToMultiplicativeInverse2();
				}

				eq.EnsureVariableOnLeft();
			}


		}

		private void SolveForMultipleVariables(Expression ex)
		{
			throw new NotImplementedException();
		}

		private void SolveForVariablesOnBothSide(Equation eq)
		{
			throw new NotImplementedException();
		}

		private void AddSolvedVariable(IVariable variable, INumber numericValue)
		{
			SolvedVariables.Add(variable.Value, numericValue.Value);
		}

		private bool IsArithmeticEquasionTrue(Equation eq)
		{
			eq.LeftHandSide.CombineArithmeticTokens();
			eq.RightHandSide.CombineArithmeticTokens();

			var left = eq.LeftHandSide;
			var right = eq.RightHandSide;

			if (!left.IsSimplified || !right.IsSimplified) throw new Exception("Expected both sides of the equation were arithmetic tokens only, but failed to simplify one or both sides.");

			switch (eq.ComparativeOperator)
			{
				case ComparativeType.Equals:
					return left.Value == right.Value;
				case ComparativeType.GreaterThan:
					return left.Value > right.Value;
				case ComparativeType.LessThan:
					return left.Value < right.Value;
				case ComparativeType.GreaterThanOrEquals:
					return left.Value >= right.Value;
				case ComparativeType.LessThanOrEquals:
					return left.Value <= right.Value;
				default:
					throw new Exception();
			}
		}



		/* EXPRESSIONS */

		private void SolveExpression(Expression ex)
		{
			if (ex.OnlyArithmeticTokens())
			{
				ex.CombineArithmeticTokens();
				if (!ex.IsSimplified) throw new Exception("Expected the expression was arithmetic tokens only, but failed to simplify.");
				Solutions.Add(ex.ToString());
				PrintStatus();
			}
		}

		private void PrintStatus()
		{
			if (LoggingMethod != null)
			{
				LoggingMethod.Invoke(equations.ToString());
			}
		}

	}
}
