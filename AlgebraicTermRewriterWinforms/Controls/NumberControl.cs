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
	public partial class NumberControl : UserControl
	{
		[DefaultValue(null)]
		public Number Number
		{
			get
			{
				return _number;
			}
			set
			{
				_number = value;
				PopulateControl();
			}
		}
		private Number _number = null;

		public NumberControl()
		{
			InitializeComponent();
		}

		public NumberControl(Number number)
			: this()
		{
			Number = number;
		}

		protected virtual void PopulateControl()
		{
			if (_number == null)
			{
				labelNumber.Text = "";
				labelNumber.Tag = null;
				return;
			}

			labelNumber.Text = _number.ToString();
			labelNumber.Tag = _number;
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

			OperatorExpressionPair opVariablePair = parentExpression.Extract(Number);

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
