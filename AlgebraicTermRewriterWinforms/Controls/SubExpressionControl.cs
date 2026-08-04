using AlgebraicTermRewriter;
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
	public partial class SubExpressionControl : UserControl
	{
		[DefaultValue(null)]
		public SubExpression Subexpression
		{
			get
			{
				return _subExpression;
			}
			set
			{
				_subExpression = value;
				PopulateControl();
			}
		}
		private SubExpression _subExpression = null;

		public Equation ParentEquation { get; set; }

		private string otherSide = string.Empty;
		private string menuItemText = "&Move to other side";
		private static string menuItemTextFormat = "&Move to {0}HS";

		private SubExpression thisSubExpression = null;
		private Control parentControl = null;
		private Expression parentExpression = null;
		private EquationControl parentEquationControl = null;
		private Equation parentEquation = null;
		private RelativeDirection? thisExpressionLocation = null;

		public SubExpressionControl()
			: base()
		{
			InitializeComponent();
		}

		public SubExpressionControl(SubExpression subexpression)
			: this()
		{
			Subexpression = subexpression;
			ParentEquation = subexpression.Parent;
		}

		public void ReDraw()
		{
			if (_subExpression != null)
			{
				if (_subExpression.CanSimplify())
				{
					Expression newExpression = _subExpression.Simplify();
					SubExpression newSubexpression = new SubExpression(newExpression.AsEnumerable());
					newSubexpression.Parent = this.ParentEquation;
					Subexpression = newSubexpression;
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
			ClearControl(flowSubexpression);
		}

		protected virtual void ClearControl(FlowLayoutPanel flowPanel)
		{
			flowPanel.Controls.Clear();
			flowPanel.Tag = null;
		}

		protected virtual void PopulateControl()
		{
			ClearControl(this.flowSubexpression);

			if (this.Subexpression == null || SubExpression.Equals(this.Subexpression, SubExpression.Empty))
			{
				this.Visible = false;
				return;
			}
			else
			{
				this.Visible = true;
			}

			if (Subexpression != null)
			{
				PopulateControl(this.flowSubexpression, Subexpression);
			}
		}

		protected virtual void PopulateControl(FlowLayoutPanel flowPanel, SubExpression subexpression)
		{
			flowPanel.Tag = subexpression;

			foreach (var token in subexpression.Tokens)
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
			//this.Invalidate(true);
		}

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			SetContextMenuText();
		}

		private void SetContextMenuText()
		{
			SetPrivateVariables();

			if (thisExpressionLocation.HasValue)
			{
				if (thisExpressionLocation == RelativeDirection.Left)
				{
					otherSide = "R";
				}
				else if (thisExpressionLocation == RelativeDirection.Right)
				{
					otherSide = "L";
				}
				menuItemText = string.Format(menuItemTextFormat, otherSide);
			}

			menuItem_MoveToOtherSide.Text = menuItemText;
		}

		private void SetPrivateVariables()
		{
			thisSubExpression = flowSubexpression.Tag as SubExpression;
			if (thisSubExpression == null)
			{
				return;
			}

			Control parentControl = ControlsHelper.GetFirstParentControlOfType(this, new List<Type> { typeof(SubExpressionControl), typeof(ExpressionControl) });
			if (parentControl == null)
			{
				return;
			}

			SubExpressionControl subExprCtrl = parentControl as SubExpressionControl;
			ExpressionControl exprCtrl = parentControl as ExpressionControl;
			if (subExprCtrl != null)
			{
				parentExpression = subExprCtrl.Subexpression;
			}
			else if (exprCtrl != null)
			{
				parentExpression = exprCtrl.Expression;
			}

			parentEquationControl = ControlsHelper.GetParentControlOfType<EquationControl>(this);
			if (parentEquationControl != null)
			{
				parentEquation = parentEquationControl.Equation;
			}

			thisExpressionLocation = thisSubExpression.SideOfEquality;
			if (!thisExpressionLocation.HasValue)
			{
				return;
			}
		}

		private void menuItem_MoveToOtherSide_Click(object sender, EventArgs e)
		{
			SetPrivateVariables();

			if (parentEquationControl == null)
			{
				return;
			}
			if (thisSubExpression == null)
			{
				return;
			}
			if (parentEquation == null)
			{
				return;
			}
			if (!thisExpressionLocation.HasValue)
			{
				return;
			}

			OperatorExpressionPair opVariablePair = thisSubExpression.Extract(Subexpression);

			if (thisExpressionLocation == RelativeDirection.Left)
			{
				parentEquation.RightHandSide.Insert(opVariablePair);
			}
			else if (thisExpressionLocation == RelativeDirection.Right)
			{
				parentEquation.LeftHandSide.Insert(opVariablePair);
			}

			parentEquationControl.ReDraw();
		}
	}
}
