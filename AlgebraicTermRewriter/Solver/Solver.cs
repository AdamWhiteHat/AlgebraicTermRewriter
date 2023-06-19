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
				//else if (sentence is Expression)
				//{
				//	SolveExpression(sentence as Expression);
				//}
			}
		}

		private void SolveEquation(Equation eq)
		{
			if (eq.OnlyArithmeticTokens())
			{
				Solutions.Add(IsArithmeticEquasionTrue(eq).ToString());
				return;
			}

			if (LeftHasVariables && RightHasVariables)
			{
				SolveForVariablesOnBothSide(eq);
				Solutions.Add(eq.ToString());
				return;
			}

			int iterationLimit = 50;
			bool result = true;
			do
			{
				iterationLimit--;

				eq.EnsureVariableOnLeft();
				if (!LeftHasVariables)
				{
					return;
				}

				if (Left.Variables.Count() > 1)
				{
					result = SolveForMultipleVariables(eq);
				}
				else if (!Left.IsVariableIsolated)
				{
					result = IsolateSingleVariable(eq);
				}
			}
			while (result == true && iterationLimit > 0);

			if (Left.IsVariableIsolated && Right.IsSimplified)
			{
				Solutions.Add(eq.ToString());
				AddSolvedVariable(Left.Variables.Single(), Right.Numbers.Single());
				return;
			}

			throw new Exception("Not sure what to do here. Equations should have been solved.");
		}

		public static bool IsolateSingleVariable(Equation eq)
		{
			Expression from = null;
			Expression to = null;

			eq.EnsureVariableOnLeft();

			from = eq.LeftHandSide;
			to = eq.RightHandSide;

			if (!from.Numbers.Any())
			{
				return false;
			}

			bool moveVariableMultiple = false;
			if ((from.Variables.Any() && to.Variables.Any()) /*|| eq.GetDistinctVariableCount() > 1*/)
			{
				if (
					!(from.Contains("-") || from.Contains("+"))
					&&
					from.Contains("*")
					&&
					from.Numbers.Any()
					&&
					to.Contains("*")
					&&
					to.Numbers.Any()
					)
				{
					moveVariableMultiple = true;
				}
			}

			bool result = false;
			if (moveVariableMultiple)
			{
				from = eq.RightHandSide;
				to = eq.LeftHandSide;
				IVariable variable = from.Variables.First();
				result = MoveVariableTerm(variable, from, to);
			}
			else
			{
				result = MoveNumberTerm(from, to);
			}

			IToken leadingToken = from.Tokens.First();
			if (leadingToken.Contents == "-")
			{
				to.SetToAdditiveInverse();
				from.SetToAdditiveInverse();
			}

			if (result)
			{
				from = SimplifyEquation.Simplify(from);
				to = SimplifyEquation.Simplify(to);
			}

			eq.EnsureVariableOnLeft();

			return result;
		}

		public static bool MoveVariableTerm(IVariable variable, Expression from, Expression to)
		{
			SubExpression variableGroup = from.GetVariableProductSubExpression(variable);
			OperatorExpressionPair extracted = from.Extract(new Operator('+'), variableGroup);

			if (extracted != null)
			{
				to.Insert(extracted);
				return true;
			}
			return false;
		}

		public static bool MoveNumberTerm(Expression from, Expression to)
		{
			List<Tuple<IOperator, INumber>> OperatorTermIndexList = from.GetOperatorTermIndexPairs();
			if (OperatorTermIndexList.Any())
			{
				Tuple<IOperator, INumber> next = OperatorTermIndexList.First();

				OperatorExpressionPair extracted = from.Extract(next.Item1, next.Item2);
				if (extracted != null)
				{
					to.Insert(extracted);
					return true;
				}
			}
			return false;
		}

		private bool SolveForMultipleVariables(Equation input)
		{
			//int leftVarsCount = Left.Variables.Count();
			//int rightVarsCount = Right.Variables.Count();

			//IVariable firstVar = Left.Variables.First();

			//	throw new NotImplementedException();

			// Same variable appears more than once.
			#region Same variable appears more than once.

			/*

			if (result.Variables.Count() != result.Variables.Distinct().Count())
			{
				var variables = result.Variables.ToList();

				List<SubExpression> toCombine = new List<SubExpression>();
				foreach (IVariable variable in variables)
				{
					toCombine.Add(result.GetVariableProductSubExpression(variable));
				}

				string debugSource = result.ToString();

				int leftIndex = result.IndexOf(toCombine[0].Last());
				int rightIndex = result.IndexOf(toCombine[1].First());

				var rightOfLeftToken = result.RightOfToken(result.TokenAt(leftIndex));
				var leftOfRightToken = result.LeftOfToken(result.TokenAt(rightIndex));

				if (rightOfLeftToken == leftOfRightToken)
				{
					if (rightOfLeftToken.Type == TokenType.Operator)
					{
						IOperator operationToken = (IOperator)rightOfLeftToken;

						if (operationToken.Symbol == '+')
						{

						}
						else if (operationToken.Symbol == '-')
						{

						}
						else if (operationToken.Symbol == '*')
						{

						}
						else if (operationToken.Symbol == '/')
						{

						}
					}
				}
			}

			*/

			#endregion

			return IsolateSingleVariable(input);
		}

		private bool SolveForVariablesOnBothSide(Equation input)
		{
			//IVariable firstVar = Left.Variables.First();

			//throw new NotImplementedException();

			return IsolateSingleVariable(input);
		}

		private void AddSolvedVariable(IVariable variable, INumber numericValue)
		{
			SolvedVariables.Add(variable.Symbol, numericValue.Value);
		}

		private bool IsArithmeticEquasionTrue(Equation input)
		{
			var left = input.LeftHandSide;
			var right = input.RightHandSide;

			if (!left.IsSimplified || !right.IsSimplified) throw new Exception("Expected both sides of the equation were arithmetic tokens only, but failed to simplify one or both sides.");

			switch (input.ComparisonOperator)
			{
				case ComparisonType.Equals:
					return left.Value == right.Value;
				case ComparisonType.GreaterThan:
					return left.Value > right.Value;
				case ComparisonType.LessThan:
					return left.Value < right.Value;
				case ComparisonType.GreaterThanOrEquals:
					return left.Value >= right.Value;
				case ComparisonType.LessThanOrEquals:
					return left.Value <= right.Value;
				default:
					throw new Exception();
			}
		}



		/* EXPRESSIONS */

		private bool SolveExpression(Expression input)
		{

			throw new NotImplementedException();

			// TODO: Rewrite using IRewriteRules


			//if (ex.OnlyArithmeticTokens())
			//{
			//	ex.Simplify();
			//	if (!ex.IsSimplified) throw new Exception("Expected the expression was arithmetic tokens only, but failed to simplify.");
			//	Solutions.Add(ex.ToString());
			//	PrintStatus();
			//}
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
