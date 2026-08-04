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
	partial class ExpressionControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			flowExpression = new FlowLayoutPanel();
			tableLayoutPanel1 = new TableLayoutPanel();
			tableLayoutPanel1.SuspendLayout();
			SuspendLayout();
			// 
			// flowExpression
			// 
			flowExpression.AutoSize = true;
			flowExpression.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowExpression.Location = new Point(0, 0);
			flowExpression.Margin = new Padding(0);
			flowExpression.MinimumSize = new Size(6, 35);
			flowExpression.Name = "flowExpression";
			flowExpression.Size = new Size(6, 35);
			flowExpression.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.AutoSize = true;
			tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			tableLayoutPanel1.ColumnCount = 1;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel1.Controls.Add(flowExpression, 0, 0);
			tableLayoutPanel1.Location = new Point(0, 0);
			tableLayoutPanel1.Margin = new Padding(0);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 1;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
			tableLayoutPanel1.Size = new Size(6, 35);
			tableLayoutPanel1.TabIndex = 1;
			// 
			// ExpressionControl
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			Controls.Add(tableLayoutPanel1);
			Margin = new Padding(0);
			MinimumSize = new Size(6, 35);
			Name = "ExpressionControl";
			Size = new Size(6, 35);
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private FlowLayoutPanel flowExpression;
		private TableLayoutPanel tableLayoutPanel1;
	}
}
