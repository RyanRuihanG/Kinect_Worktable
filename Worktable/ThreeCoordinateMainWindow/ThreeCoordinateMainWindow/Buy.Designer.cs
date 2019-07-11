namespace ThreeCoordinateMainWindow
{
	partial class Buy
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
			this.label1 = new System.Windows.Forms.Label();
			this.rbAliPay = new System.Windows.Forms.RadioButton();
			this.rbWeChat = new System.Windows.Forms.RadioButton();
			this.picAliPay = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.picAliPay)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "感谢支持！";
			// 
			// rbAliPay
			// 
			this.rbAliPay.AutoSize = true;
			this.rbAliPay.Location = new System.Drawing.Point(237, 6);
			this.rbAliPay.Name = "rbAliPay";
			this.rbAliPay.Size = new System.Drawing.Size(59, 16);
			this.rbAliPay.TabIndex = 2;
			this.rbAliPay.TabStop = true;
			this.rbAliPay.Text = "支付宝";
			this.rbAliPay.UseVisualStyleBackColor = true;
			// 
			// rbWeChat
			// 
			this.rbWeChat.AutoSize = true;
			this.rbWeChat.Location = new System.Drawing.Point(184, 6);
			this.rbWeChat.Name = "rbWeChat";
			this.rbWeChat.Size = new System.Drawing.Size(47, 16);
			this.rbWeChat.TabIndex = 3;
			this.rbWeChat.TabStop = true;
			this.rbWeChat.Text = "微信";
			this.rbWeChat.UseVisualStyleBackColor = true;
			this.rbWeChat.CheckedChanged += new System.EventHandler(this.rbWeChat_CheckedChanged);
			// 
			// picAliPay
			// 
			this.picAliPay.Location = new System.Drawing.Point(12, 26);
			this.picAliPay.Name = "picAliPay";
			this.picAliPay.Size = new System.Drawing.Size(308, 307);
			this.picAliPay.TabIndex = 0;
			this.picAliPay.TabStop = false;
			// 
			// Buy
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(330, 345);
			this.Controls.Add(this.rbWeChat);
			this.Controls.Add(this.rbAliPay);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.picAliPay);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Buy";
			this.Text = "欢迎投食";
			this.Load += new System.EventHandler(this.Buy_Load);
			((System.ComponentModel.ISupportInitialize)(this.picAliPay)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox picAliPay;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton rbAliPay;
		private System.Windows.Forms.RadioButton rbWeChat;
	}
}