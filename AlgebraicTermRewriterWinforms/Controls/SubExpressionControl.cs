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

		private string otherSide = string.Empty;
		private string menuItemText = "&Move to other side";
		private static string menuItemTextFormat = "&Move to {0}HS";

		private Control? parentControl = null;
		private FlowLayoutPanel flowLayoutPanel = null;
		private Expression parentExpression = null;
		private Equation parentEquation = null;
		private RelativeDirection? parentExpressionLocation = null;


		public SubExpressionControl()
		{
			InitializeComponent();
		}

		public SubExpressionControl(SubExpression subexpression)
			: this()
		{
			Subexpression = subexpression;
		}

		protected virtual void PopulateControl()
		{
			labelSubExpression.Text = $"{_subExpression}";
			labelSubExpression.Tag = _subExpression;
		}

		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);

			SetContextMenuText();
		}

		private void SetContextMenuText()
		{
			SetPrivateVariables();

			if (parentExpressionLocation.HasValue)
			{
				if (parentExpressionLocation == RelativeDirection.Left)
				{
					otherSide = "R";
				}
				else if (parentExpressionLocation == RelativeDirection.Right)
				{
					otherSide = "L";
				}
				menuItemText = string.Format(menuItemTextFormat, otherSide);
			}

			menuItem_MoveToOtherSide.Text = menuItemText;
		}

		private void SetPrivateVariables()
		{
			parentControl = this.Parent;
			if (parentControl == null)
			{
				return;
			}

			flowLayoutPanel = parentControl as FlowLayoutPanel;
			if (flowLayoutPanel == null)
			{
				return;
			}

			parentExpression = flowLayoutPanel.Tag as Expression;
			if (parentExpression == null)
			{
				return;
			}

			parentEquation = parentExpression.Parent;
			if (parentEquation == null)
			{
				return;
			}

			parentExpressionLocation = parentExpression.SideOfEquality;
			if (!parentExpressionLocation.HasValue)
			{
				throw new Exception("Cannot find self (expression) in parent's LHS or RHS?!");
			}
		}

		private void menuItem_MoveToOtherSide_Click(object sender, EventArgs e)
		{
			SetPrivateVariables();

			if (flowLayoutPanel == null)
			{
				return;
			}
			if (parentExpression == null)
			{
				return;
			}
			if (parentEquation == null)
			{
				return;
			}
			if (!parentExpressionLocation.HasValue)
			{
				throw new Exception("Cannot find self (expression) in parent's LHS or RHS?!");
			}

			OperatorExpressionPair opVariablePair = parentExpression.Extract(Subexpression);

			if (parentExpressionLocation == RelativeDirection.Left)
			{
				parentEquation.RightHandSide.Insert(opVariablePair);
			}
			else if (parentExpressionLocation == RelativeDirection.Right)
			{
				parentEquation.LeftHandSide.Insert(opVariablePair);
			}

			Control tableLayoutPanel = flowLayoutPanel.Parent;
			Control equationControl = tableLayoutPanel.Parent;

			EquationControl equationControl1 = equationControl as EquationControl;
			if (equationControl1 != null)
			{
				equationControl1.Refresh();
			}
		}

	}
}
