using System.Windows.Forms;

namespace AlgebraicTermRewriterWinforms.Controls
{
	partial class VariableControl
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
			components = new System.ComponentModel.Container();
			labelVariable = new Label();
			contextMenu = new ContextMenuStrip(components);
			menuItem_MoveToOtherSide = new ToolStripMenuItem();
			contextMenu.SuspendLayout();
			SuspendLayout();
			// 
			// labelVariable
			// 
			labelVariable.AutoSize = true;
			labelVariable.ContextMenuStrip = contextMenu;
			labelVariable.Location = new System.Drawing.Point(0, 0);
			labelVariable.Margin = new Padding(0);
			labelVariable.MinimumSize = new System.Drawing.Size(6, 35);
			labelVariable.Name = "labelVariable";
			labelVariable.Size = new System.Drawing.Size(15, 35);
			labelVariable.TabIndex = 0;
			labelVariable.Text = "X";
			labelVariable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// contextMenu
			// 
			contextMenu.Items.AddRange(new ToolStripItem[] { menuItem_MoveToOtherSide });
			contextMenu.Name = "contextMenu";
			contextMenu.ShowImageMargin = false;
			contextMenu.Size = new System.Drawing.Size(164, 48);
			contextMenu.Click += menuItem_MoveToOtherSide_Click;
			// 
			// menuItem_MoveToOtherSide
			// 
			menuItem_MoveToOtherSide.DisplayStyle = ToolStripItemDisplayStyle.Text;
			menuItem_MoveToOtherSide.Name = "menuItem_MoveToOtherSide";
			menuItem_MoveToOtherSide.Size = new System.Drawing.Size(163, 22);
			menuItem_MoveToOtherSide.Text = "&Move to other side";
			menuItem_MoveToOtherSide.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			menuItem_MoveToOtherSide.ToolTipText = "Move term to the other side of the equality symbol";
			menuItem_MoveToOtherSide.Click += menuItem_MoveToOtherSide_Click;
			// 
			// VariableControl
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			AutoSize = true;
			AutoSizeMode = AutoSizeMode.GrowAndShrink;
			ContextMenuStrip = contextMenu;
			Controls.Add(labelVariable);
			Margin = new Padding(0);
			Name = "VariableControl";
			Size = new System.Drawing.Size(15, 35);
			contextMenu.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label labelVariable;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem menuItem_MoveToOtherSide;
	}
}
