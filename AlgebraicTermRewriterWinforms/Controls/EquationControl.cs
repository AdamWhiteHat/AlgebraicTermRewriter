using AlgebraicTermRewriter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace AlgebraicTermRewriterWinforms.Controls
{
	public partial class EquationControl : UserControl
	{
		[DefaultValue(null)]
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
		private Equation _equation = null;

		public EquationControl()
			: base()
		{
			InitializeComponent();
		}

		public EquationControl(Equation equation)
			: this()
		{
			this.Equation = equation;
		}

		public void ReDraw()
		{
			if (this.Equation != null)
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
			//this.Invalidate(true);
		}

		protected virtual void ClearControl(FlowLayoutPanel flowPanel)
		{
			flowPanel.Controls.Clear();
			flowPanel.Tag = null;
		}

		protected virtual void PopulateControl()
		{
			ClearControl(flowEquation);
			this.expressionLHS.ClearControl();
			this.expressionRHS.ClearControl();

			if (this.Equation == null || Equation.Equals(this.Equation, Equation.Empty))
			{
				//this.Visible = false;
				return;
			}

			if (this.Equation != null)
			{
				PopulateControl(flowEquation, Equation);
			}
		}

		protected virtual void PopulateControl(FlowLayoutPanel flowPanel, Equation equation)
		{
			flowPanel.Tag = equation;

			this.expressionLHS.Expression = equation.LeftHandSide;
			this.expressionLHS.ParentEquation = equation;
			Label labelEquals = CreateEqualsLabel();
			this.expressionRHS.Expression = equation.RightHandSide;
			this.expressionRHS.ParentEquation = equation;

			flowPanel.Controls.Add(this.expressionLHS);
			flowPanel.Controls.Add(labelEquals);
			flowPanel.Controls.Add(this.expressionRHS);

			//this.Invalidate(true);
		}

		private Label CreateEqualsLabel()
		{
			Label result = new Label();
			result.Name = "labelEquals";
			result.MinimumSize = new Size(11, 35);
			result.Margin = new Padding(0);
			result.Padding = new Padding(0);
			result.TextAlign = ContentAlignment.MiddleCenter;
			result.Text = "=";
			result.TabStop = false;
			result.AutoSize = true;
			return result;
		}
	}
}
