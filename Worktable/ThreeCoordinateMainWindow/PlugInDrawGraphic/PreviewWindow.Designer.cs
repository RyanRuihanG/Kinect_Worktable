namespace PlugInDrawGraphic
{
	partial class PreviewWindow
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.btnResetView = new System.Windows.Forms.Button();
			this.timerRepaint = new System.Windows.Forms.Timer(this.components);
			this.msgShowPosition = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnResetView
			// 
			this.btnResetView.Location = new System.Drawing.Point(298, 2);
			this.btnResetView.Name = "btnResetView";
			this.btnResetView.Size = new System.Drawing.Size(75, 23);
			this.btnResetView.TabIndex = 0;
			this.btnResetView.Text = "重置视图";
			this.btnResetView.UseVisualStyleBackColor = true;
			this.btnResetView.Click += new System.EventHandler(this.btnResetView_Click);
			// 
			// timerRepaint
			// 
			this.timerRepaint.Interval = 40;
			this.timerRepaint.Tick += new System.EventHandler(this.timerRepaint_Tick);
			// 
			// msgShowPosition
			// 
			this.msgShowPosition.AutoSize = true;
			this.msgShowPosition.Location = new System.Drawing.Point(4, 378);
			this.msgShowPosition.Name = "msgShowPosition";
			this.msgShowPosition.Size = new System.Drawing.Size(41, 12);
			this.msgShowPosition.TabIndex = 1;
			this.msgShowPosition.Text = "坐标：";
			// 
			// PreviewWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(456, 393);
			this.Controls.Add(this.msgShowPosition);
			this.Controls.Add(this.btnResetView);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PreviewWindow";
			this.Text = "预览";
			this.Load += new System.EventHandler(this.PreviewWindow_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.PreviewWindow_Paint);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PreviewWindow_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PreviewWindow_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PreviewWindow_MouseUp);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnResetView;
		private System.Windows.Forms.Timer timerRepaint;
		private System.Windows.Forms.Label msgShowPosition;
	}
}