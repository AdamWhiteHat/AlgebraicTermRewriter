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

namespace AlgebraicTermRewriterWinforms
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			equationControl1.Equation = Equation.Empty;
			errorProviderInput.SetIconAlignment(textBoxInput, ErrorIconAlignment.MiddleLeft);
			errorProviderInput.SetIconPadding(textBoxInput, 3);
		}

		private void buttonGo_Click(object sender, EventArgs e)
		{
			SetEquation();
		}

		private void textBoxInput_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				SetEquation();
			}
		}

		private void SetEquation()
		{
			ClearErrors();
			equationControl1.Equation = Equation.Empty;

			string input = textBoxInput.Text;
			if (string.IsNullOrWhiteSpace(input))
			{
				equationControl1.Equation = Equation.Empty;
				SetError("Equation is empty.");
				return;
			}

			if (!input.Contains('='))
			{
				SetError("The equation must contain an equality symbol (i.e. \"=\")");
				return;
			}

			Equation eq = Equation.Parse(input);
			equationControl1.Equation = eq;
			CenterControl(equationControl1);
		}

		private void CenterControl(Control control)
		{
			int formWidth = this.ClientSize.Width;
			int controlWidth = control.Width;

			if (controlWidth > formWidth)
			{
				this.ClientSize = new Size(controlWidth + this.Padding.Left + this.Padding.Right, ClientSize.Height);
				control.Left = 0 + this.Padding.Left;
				return;
			}

			int diff = formWidth - controlWidth;
			int left = diff / 2;
			control.Left = left;
		}

		private void SetError(string errorMessage)
		{
			errorProviderInput.SetError(this.textBoxInput, errorMessage);
		}

		private void ClearErrors()
		{
			errorProviderInput.SetError(this.textBoxInput, string.Empty);
		}
	}
}
