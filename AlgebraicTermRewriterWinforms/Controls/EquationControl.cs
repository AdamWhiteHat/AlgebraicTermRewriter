using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AlgebraicTermRewriter;

namespace AlgebraicTermRewriterWinforms.Controls
{
	public partial class EquationControl : UserControl
	{
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Browsable(true)]
		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				if (_text != value)
				{
					_text = value;
					TextChanged();
				}
			}
		}
		private string _text = "";

		public Equation Equation
		{
			get
			{
				return _equation;
			}
			set
			{
				_equation = value;
				PopulateControl();
			}
		}
		private Equation _equation = Equation.Empty;

		public EquationControl()
		{
			InitializeComponent();
		}

		public void Refresh()
		{
			if (_equation.CanSimplify())
			{
				Equation newEquation = _equation.Simplify();
				Equation = newEquation;
			}
			else
			{
				PopulateControl();
			}
		}

		private void TextChanged()
		{
			if (string.IsNullOrWhiteSpace(Text))
			{
				this.Equation = Equation.Empty;
				return;
			}

			if (Equation.Equals(this.Equation, Equation.Empty) || !string.Equals(this.Equation.ToString(), Text))
			{
				var parsed = Equation.Parse(Text);
				this.Equation = parsed;
			}
		}

		protected virtual void PopulateControl()
		{
			ClearControl(this.flowLHS);
			ClearControl(this.flowRHS);

			if (Equation.Equals(this.Equation, Equation.Empty))
			{
				this.Visible = false;
				return;
			}
			else
			{
				this.Visible = true;
			}
			_text = Equation.ToString();

			PopulateControl(this.flowLHS, Equation.LeftHandSide);
			PopulateControl(this.flowRHS, Equation.RightHandSide);

			/*
			this.flowLHS.Tag = lhs;
			this.flowRHS.Tag = rhs;

			int leftWidth = this.flowLHS.MinimumSize.Width + this.flowLHS.Padding.Left + this.flowLHS.Padding.Right;
			foreach (var token in lhs.Tokens)
			{
				if (token is IVariable)
				{
					VariableControl variableControl = new VariableControl(token as Variable);
					this.flowLHS.Controls.Add(variableControl);
					leftWidth += variableControl.Width;
					leftWidth += this.flowLHS.Padding.Left + this.flowLHS.Padding.Right;
				}
				else if (token is INumber)
				{
					NumberControl numberControl = new NumberControl(token as Number);
					this.flowLHS.Controls.Add(numberControl);
					leftWidth += numberControl.Width;
					leftWidth += this.flowLHS.Padding.Left + this.flowLHS.Padding.Right;
				}
				else
				{
					TokenControl tokenControl = new TokenControl(token);
					this.flowLHS.Controls.Add(tokenControl);
					leftWidth += tokenControl.Width;
					leftWidth += this.flowLHS.Padding.Left + this.flowLHS.Padding.Right;
				}
			}
			this.flowLHS.Width = leftWidth;


			int rightWidth = this.flowRHS.MinimumSize.Width + this.flowRHS.Padding.Left + this.flowRHS.Padding.Right;
			foreach (var token in rhs.Tokens)
			{
				if (token is IVariable)
				{
					VariableControl variableControl = new VariableControl(token as Variable);
					this.flowRHS.Controls.Add(variableControl);
					rightWidth += variableControl.Width;
					rightWidth += this.flowRHS.Padding.Left + this.flowRHS.Padding.Right;
				}
				else if (token is INumber)
				{
					NumberControl numberControl = new NumberControl(token as Number);
					this.flowRHS.Controls.Add(numberControl);
					rightWidth += numberControl.Width;
					rightWidth += this.flowRHS.Padding.Left + this.flowRHS.Padding.Right;
				}
				else
				{
					TokenControl tokenControl = new TokenControl(token);
					this.flowRHS.Controls.Add(tokenControl);
					rightWidth += tokenControl.Width;
					rightWidth += this.flowRHS.Padding.Left + this.flowRHS.Padding.Right;
				}
			}
			this.flowRHS.Width = rightWidth;
			*/
		}

		protected virtual void ClearControl(FlowLayoutPanel flowPanel)
		{
			flowPanel.Controls.Clear();
			flowPanel.Width = flowPanel.MinimumSize.Width;
			flowPanel.Tag = null;
		}

		protected virtual void PopulateControl(FlowLayoutPanel flowPanel, Expression expression)
		{
			flowPanel.Tag = expression;

			int width = flowPanel.MinimumSize.Width;
			foreach (var token in expression.Tokens)
			{
				if (token is IVariable)
				{
					VariableControl variableControl = new VariableControl(token as Variable);
					flowPanel.Controls.Add(variableControl);
					width += variableControl.Width;
				}
				else if (token is INumber)
				{
					NumberControl numberControl = new NumberControl(token as Number);
					flowPanel.Controls.Add(numberControl);
					width += numberControl.Width;
				}
				else if (token is SubExpression)
				{
					SubExpressionControl subExpressionControl = new SubExpressionControl(token as SubExpression);
					flowPanel.Controls.Add(subExpressionControl);
					width += subExpressionControl.Width;
				}
				else
				{
					TokenControl tokenControl = new TokenControl(token);
					flowPanel.Controls.Add(tokenControl);
					width += tokenControl.Width;
				}
				width += flowPanel.Padding.Left + flowPanel.Padding.Right;
			}
			flowPanel.Width = width;
		}
	}
}
