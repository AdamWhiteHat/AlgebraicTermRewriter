namespace AlgebraicTermRewriterWinforms.Controls
{
	partial class NumberControl
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
			labelNumber = new System.Windows.Forms.Label();
			contextMenu = new System.Windows.Forms.ContextMenuStrip(components);
			menuItem_MoveToOtherSide = new System.Windows.Forms.ToolStripMenuItem();
			contextMenu.SuspendLayout();
			SuspendLayout();
			// 
			// labelNumber
			// 
			labelNumber.AutoSize = true;
			labelNumber.ContextMenuStrip = contextMenu;
			labelNumber.Location = new System.Drawing.Point(0, 0);
			labelNumber.Margin = new System.Windows.Forms.Padding(0);
			labelNumber.MinimumSize = new System.Drawing.Size(6, 35);
			labelNumber.Name = "labelNumber";
			labelNumber.Size = new System.Drawing.Size(14, 35);
			labelNumber.TabIndex = 0;
			labelNumber.Text = "0";
			labelNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// contextMenu
			// 
			contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { menuItem_MoveToOtherSide });
			contextMenu.Name = "contextMenu";
			contextMenu.ShowImageMargin = false;
			contextMenu.Size = new System.Drawing.Size(164, 48);
			// 
			// menuItem_MoveToOtherSide
			// 
			menuItem_MoveToOtherSide.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			menuItem_MoveToOtherSide.Name = "menuItem_MoveToOtherSide";
			menuItem_MoveToOtherSide.Size = new System.Drawing.Size(163, 22);
			menuItem_MoveToOtherSide.Text = "&Move to other side";
			menuItem_MoveToOtherSide.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			menuItem_MoveToOtherSide.ToolTipText = "Move term to the other side of the equality symbol";
			menuItem_MoveToOtherSide.Click += menuItem_MoveToOtherSide_Click;
			// 
			// NumberControl
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			AutoSize = true;
			AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			ContextMenuStrip = contextMenu;
			Controls.Add(labelNumber);
			Margin = new System.Windows.Forms.Padding(0);
			Name = "NumberControl";
			Size = new System.Drawing.Size(14, 35);
			contextMenu.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label labelNumber;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem menuItem_MoveToOtherSide;
	}
}
