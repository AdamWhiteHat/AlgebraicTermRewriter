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
	public partial class VariableControl : UserControl
	{
		[DefaultValue(null)]
		public Variable Variable
		{
			get
			{
				return _variable;
			}
			set
			{
				_variable = value;
				PopulateControl();
			}
		}
		private Variable _variable = null;

		public VariableControl()
		{
			InitializeComponent();
		}

		public VariableControl(Variable variable)
			: this()
		{
			Variable = variable;
		}

		protected virtual void PopulateControl()
		{
			if (_variable == null)
			{
				labelVariable.Text = "";
				labelVariable.Tag = null;
				return;
			}

			labelVariable.Text = _variable.ToString();
			labelVariable.Tag = _variable;
		}

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			SetContextMenuText();
		}

		private string otherSide = string.Empty;
		private string menuItemText = "&Move to other side";
		private static string menuItemTextFormat = "&Move to {0}HS";

		private Control parentControl = null;
		private Expression parentExpression = null;
		private EquationControl parentEquationControl = null;
		private Equation parentEquation = null;
		private RelativeDirection? thisExpressionLocation = null;

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

			thisExpressionLocation = parentExpression.SideOfEquality;
			if (!thisExpressionLocation.HasValue)
			{
				return;
			}
		}

		private void menuItem_MoveToOtherSide_Click(object sender, EventArgs e)
		{
			SetPrivateVariables();

			if (parentExpression == null)
			{
				return;
			}
			if (parentEquation == null)
			{
				return;
			}
			if (parentEquationControl == null)
			{
				return;
			}
			if (!thisExpressionLocation.HasValue)
			{
				return;
			}

			OperatorExpressionPair opVariablePair = parentExpression.Extract(Variable);

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
