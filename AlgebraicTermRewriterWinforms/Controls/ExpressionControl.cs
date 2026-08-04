using AlgebraicTermRewriter;
using AlgebraicTermRewriterWinforms.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlgebraicTermRewriterWinforms.Controls
{
	public partial class ExpressionControl : UserControl
	{
		[DefaultValue(null)]
		public Expression Expression
		{
			get
			{
				return _expression;
			}
			set
			{
				_expression = value;
				PopulateControl();
			}
		}
		private Expression _expression = null;

		public Equation ParentEquation { get; set; }

		public ExpressionControl()
			: base()
		{
			InitializeComponent();
		}

		public ExpressionControl(Expression expression)
			: this()
		{
			Expression = expression;
			ParentEquation = expression.Parent;
		}

		public void ReDraw()
		{
			if (_expression != null)
			{
				if (_expression.CanSimplify())
				{
					Expression newExpression = _expression.Simplify();
					newExpression.Parent = this.ParentEquation;
					Expression = newExpression;
				}
				else
				{
					PopulateControl();
				}
			}
			//this.Invalidate(true);
		}

		public void ClearControl()
		{
			ClearControl(flowExpression);
		}

		protected virtual void ClearControl(FlowLayoutPanel flowPanel)
		{
			flowPanel.Controls.Clear();
			flowPanel.Tag = null;
		}

		protected virtual void PopulateControl()
		{
			ClearControl(flowExpression);

			if (this.Expression == null || Expression.Equals(this.Expression, Expression.Empty))
			{
				this.Visible = false;
				return;
			}
			else
			{
				this.Visible = true;
			}

			if (Expression != null)
			{
				PopulateControl(flowExpression, Expression);
			}
		}

		protected virtual void PopulateControl(FlowLayoutPanel flowPanel, Expression expression)
		{
			flowPanel.Tag = expression;

			foreach (var token in expression.Tokens)
			{
				if (token is IVariable)
				{
					VariableControl variableControl = new VariableControl(token as Variable);
					flowPanel.Controls.Add(variableControl);
				}
				else if (token is INumber)
				{
					NumberControl numberControl = new NumberControl(token as Number);
					flowPanel.Controls.Add(numberControl);
				}
				else if (token is SubExpression)
				{
					SubExpression subExpression = token as SubExpression;
					subExpression.Parent = this.ParentEquation;
					SubExpressionControl subExpressionControl = new SubExpressionControl(subExpression);
					flowPanel.Controls.Add(subExpressionControl);
				}
				else
				{
					TokenControl tokenControl = new TokenControl(token);
					flowPanel.Controls.Add(tokenControl);
				}
			}
			this.Invalidate(true);
		}
	}
}
