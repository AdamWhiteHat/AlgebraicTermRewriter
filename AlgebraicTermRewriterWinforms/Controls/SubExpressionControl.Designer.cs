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
	partial class SubExpressionControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
			components = new Container();
			contextMenu = new ContextMenuStrip(components);
			menuItem_MoveToOtherSide = new ToolStripMenuItem();
			flowSubexpression = new FlowLayoutPanel();
			tableLayoutPanel1 = new TableLayoutPanel();
			labelRightParen = new Label();
			labelLeftParen = new Label();
			contextMenu.SuspendLayout();
			tableLayoutPanel1.SuspendLayout();
			SuspendLayout();
			// 
			// contextMenu
			// 
			contextMenu.Items.AddRange(new ToolStripItem[] { menuItem_MoveToOtherSide });
			contextMenu.Name = "contextMenu";
			contextMenu.ShowImageMargin = false;
			contextMenu.Size = new Size(164, 48);
			// 
			// menuItem_MoveToOtherSide
			// 
			menuItem_MoveToOtherSide.DisplayStyle = ToolStripItemDisplayStyle.Text;
			menuItem_MoveToOtherSide.Name = "menuItem_MoveToOtherSide";
			menuItem_MoveToOtherSide.Size = new Size(163, 22);
			menuItem_MoveToOtherSide.Text = "&Move to other side";
			menuItem_MoveToOtherSide.TextAlign = ContentAlignment.MiddleLeft;
			menuItem_MoveToOtherSide.ToolTipText = "Move term to the other side of the equality symbol";
			menuItem_MoveToOtherSide.Click += menuItem_MoveToOtherSide_Click;
			// 
			// flowSubexpression
			// 
			flowSubexpression.AutoSize = true;
			flowSubexpression.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			flowSubexpression.ContextMenuStrip = contextMenu;
			flowSubexpression.Location = new Point(11, 0);
			flowSubexpression.Margin = new Padding(0);
			flowSubexpression.MinimumSize = new Size(6, 35);
			flowSubexpression.Name = "flowSubexpression";
			flowSubexpression.Size = new Size(6, 35);
			flowSubexpression.TabIndex = 0;
			flowSubexpression.WrapContents = false;
			// 
			// tableLayoutPanel1
			// 
			tableLayoutPanel1.AutoSize = true;
			tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			tableLayoutPanel1.ColumnCount = 3;
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 11F));
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
			tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 11F));
			tableLayoutPanel1.Controls.Add(labelRightParen, 2, 0);
			tableLayoutPanel1.Controls.Add(flowSubexpression, 1, 0);
			tableLayoutPanel1.Controls.Add(labelLeftParen, 0, 0);
			tableLayoutPanel1.Location = new Point(0, 0);
			tableLayoutPanel1.Margin = new Padding(0);
			tableLayoutPanel1.Name = "tableLayoutPanel1";
			tableLayoutPanel1.RowCount = 1;
			tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
			tableLayoutPanel1.Size = new Size(28, 35);
			tableLayoutPanel1.TabIndex = 1;
			// 
			// labelRightParen
			// 
			labelRightParen.Location = new Point(17, 0);
			labelRightParen.Margin = new Padding(0);
			labelRightParen.MinimumSize = new Size(6, 35);
			labelRightParen.Name = "labelRightParen";
			labelRightParen.Size = new Size(11, 35);
			labelRightParen.TabIndex = 3;
			labelRightParen.Text = ")";
			labelRightParen.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// labelLeftParen
			// 
			labelLeftParen.Location = new Point(0, 0);
			labelLeftParen.Margin = new Padding(0);
			labelLeftParen.MinimumSize = new Size(6, 35);
			labelLeftParen.Name = "labelLeftParen";
			labelLeftParen.Size = new Size(11, 35);
			labelLeftParen.TabIndex = 2;
			labelLeftParen.Text = "(";
			labelLeftParen.TextAlign = ContentAlignment.MiddleCenter;
			// 
			// SubExpressionControl
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			Controls.Add(tableLayoutPanel1);
			Margin = new Padding(0);
			MinimumSize = new Size(13, 35);
			Name = "SubExpressionControl";
			Size = new Size(28, 35);
			contextMenu.ResumeLayout(false);
			tableLayoutPanel1.ResumeLayout(false);
			tableLayoutPanel1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem menuItem_MoveToOtherSide;
		private System.Windows.Forms.FlowLayoutPanel flowSubexpression;
		private TableLayoutPanel tableLayoutPanel1;
		private Label labelLeftParen;
		private Label labelRightParen;
	}
}
