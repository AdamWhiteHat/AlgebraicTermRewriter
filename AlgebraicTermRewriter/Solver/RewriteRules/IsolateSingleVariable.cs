using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgebraicTermRewriter
{
	public class IsolateSingleVariable : IRewriteRule
	{
		public static int ApplyOrder => 0;

		public static bool ShouldApplyRule(Equation equation)
		{
			if (!equation.LeftHandSide.Variables.Any() && !equation.RightHandSide.Variables.Any())
			{
				return false;
			}
			if (equation.LeftHandSide.IsVariableIsolated || equation.RightHandSide.IsVariableIsolated)
			{
				return false;
			}
			return true;
		}

		public static Equation ApplyRule(Equation equation)
		{
			// 1. Choose variable to isolate
			(IVariable Selected, Expression From, Expression To) = ChooseVariableToIsolate(equation);

			// 2. Move other terms to other side
			//   a) Move other variables
			//   b) Move addition/subtraction OperatorExpressionPair
			//   c) Move multiplication/division OperatorExpressionPair

			foreach (IVariable removeVar in From.Variables.Except(new IVariable[] { Selected }))
			{
				Solver.MoveVariableTerm(removeVar, From, To);
			}

			while (Solver.MoveNumberTerm(From, To))
			{
			}



			// 3. Combine like terms and simplify


			throw new NotImplementedException();
		}

		private static (IVariable Selected, Expression From, Expression To) ChooseVariableToIsolate(Equation equation)
		{
			List<(Expression Expr, RelativeDirection Side)> sides = new List<(Expression Expr, RelativeDirection Side)>()
			{
				(equation.LeftHandSide, RelativeDirection.Left),
				(equation.RightHandSide, RelativeDirection.Right)
			};

			var variableSide = sides.Where(t => t.Expr.Variables.Count() == 0).FirstOrDefault();
			var singleVarSide = sides.Where(t => t.Expr.Variables.Count() == 1).OrderBy(o => o.Expr.RankIsolationComplexity()).FirstOrDefault();
			var elseVarSide = sides.OrderBy(o => o.Expr.RankIsolationComplexity()).FirstOrDefault();

			if (variableSide == default((Expression, RelativeDirection)))
			{
				variableSide = sides.Where(t => t.Expr.Variables.Count() == 1).OrderBy(o => o.Expr.RankIsolationComplexity()).FirstOrDefault();
			}
			if (variableSide == default((Expression, RelativeDirection)))
			{
				variableSide = sides.OrderBy(o => o.Expr.RankIsolationComplexity()).FirstOrDefault();
			}
			if (variableSide == default((Expression, RelativeDirection)))
			{
				throw new Exception($"{nameof(sides)} is empty? How did OrderBy fail?");
			}

			RelativeDirection side = variableSide.Side;
			Expression from = variableSide.Expr;
			Expression to = sides.Where(t => t.Side != side).Single().Expr;
			IVariable selected = from.Variables.OrderBy(v => RankVariable(from, v)).First();

			if (side != RelativeDirection.Left)
			{
				equation.SwapLeftRight();
			}

			return
				(
					selected,
					from,
					to
				);
		}

		private static int RankVariable(Expression expr, IToken token)
		{
			int score = 0;
			IToken rightOperator = expr.RightOfToken(token);
			if (rightOperator.Type == TokenType.None)
			{
				return 6;
			}
			if (rightOperator.Type != TokenType.Operator)
			{
				throw new Exception("Token right of variable not an operator?");
			}

			IOperator operation = (IOperator)rightOperator;
			switch (operation.Symbol)
			{
				case '^':
					score += 7;
					break;
				case '*':
				case '/':
					score += 0;
					break;
				case '+':
				case '-':
					score += 1;
					break;
			}

			IToken rightOperand = expr.RightOfToken(rightOperator);
			switch (rightOperand.Type)
			{
				case TokenType.Number:
					score += 2;
					break;
				case TokenType.Variable:
					score += 4;
					break;
			}

			return score;
		}
	}
}
