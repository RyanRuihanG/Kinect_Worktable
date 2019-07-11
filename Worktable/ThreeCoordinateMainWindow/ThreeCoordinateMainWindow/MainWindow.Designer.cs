namespace ThreeCoordinateMainWindow
{
	partial class MainWindow
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要修改
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.mainMenu = new System.Windows.Forms.MenuStrip();
			this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.通信ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.通信设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.设定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.圆弧插补设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.插补步长设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.plugInsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.帮助ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
			this.tbGCodeOpened = new System.Windows.Forms.TextBox();
			this.labelX = new System.Windows.Forms.Label();
			this.labelY = new System.Windows.Forms.Label();
			this.labelZ = new System.Windows.Forms.Label();
			this.coorXShow = new System.Windows.Forms.Label();
			this.coorYShow = new System.Windows.Forms.Label();
			this.coorZShow = new System.Windows.Forms.Label();
			this.btnChangeCoordinate = new System.Windows.Forms.Button();
			this.tbGCodeInput = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.msgModelValue = new System.Windows.Forms.Label();
			this.btnToZero = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.keyControlMul = new System.Windows.Forms.Label();
			this.ckbIsKeyControl = new System.Windows.Forms.CheckBox();
			this.wheelControlMul = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbCoordinateZ = new System.Windows.Forms.RadioButton();
			this.rbCoordinateY = new System.Windows.Forms.RadioButton();
			this.rbCoordinateX = new System.Windows.Forms.RadioButton();
			this.msgGCodeInputCheck = new System.Windows.Forms.Label();
			this.ckbIsWheelControl = new System.Windows.Forms.CheckBox();
			this.msg_GErrorWarnning = new System.Windows.Forms.Label();
			this.btnSerialSwitch = new System.Windows.Forms.PictureBox();
			this.areaWheelControl = new System.Windows.Forms.PictureBox();
			this.btnPicEStop = new System.Windows.Forms.PictureBox();
			this.btnPicStart = new System.Windows.Forms.PictureBox();
			this.btnPicStop = new System.Windows.Forms.PictureBox();
			this.btnPicPause = new System.Windows.Forms.PictureBox();
			this.tbPre = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.msg_ShowThread = new System.Windows.Forms.Label();
			this.btnDebugSet = new System.Windows.Forms.Button();
			this.mainMenu.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.btnSerialSwitch)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.areaWheelControl)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.btnPicEStop)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.btnPicStart)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.btnPicStop)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.btnPicPause)).BeginInit();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.通信ToolStripMenuItem,
            this.设定ToolStripMenuItem,
            this.plugInsToolStripMenuItem,
            this.帮助ToolStripMenuItem});
			this.mainMenu.Location = new System.Drawing.Point(0, 0);
			this.mainMenu.Name = "mainMenu";
			this.mainMenu.Size = new System.Drawing.Size(1264, 25);
			this.mainMenu.TabIndex = 0;
			this.mainMenu.Text = "menuStrip1";
			// 
			// 文件ToolStripMenuItem
			// 
			this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.退出ToolStripMenuItem});
			this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
			this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
			this.文件ToolStripMenuItem.Text = "文件";
			// 
			// 打开ToolStripMenuItem
			// 
			this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
			this.打开ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
			this.打开ToolStripMenuItem.Text = "打开";
			this.打开ToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(97, 6);
			// 
			// 退出ToolStripMenuItem
			// 
			this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
			this.退出ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
			this.退出ToolStripMenuItem.Text = "退出";
			this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
			// 
			// 通信ToolStripMenuItem
			// 
			this.通信ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.通信设置ToolStripMenuItem});
			this.通信ToolStripMenuItem.Name = "通信ToolStripMenuItem";
			this.通信ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
			this.通信ToolStripMenuItem.Text = "通信";
			// 
			// 通信设置ToolStripMenuItem
			// 
			this.通信设置ToolStripMenuItem.Name = "通信设置ToolStripMenuItem";
			this.通信设置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.通信设置ToolStripMenuItem.Text = "通信设置";
			this.通信设置ToolStripMenuItem.Click += new System.EventHandler(this.通信设置ToolStripMenuItem_Click);
			// 
			// 设定ToolStripMenuItem
			// 
			this.设定ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.圆弧插补设置ToolStripMenuItem,
            this.插补步长设置ToolStripMenuItem});
			this.设定ToolStripMenuItem.Name = "设定ToolStripMenuItem";
			this.设定ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
			this.设定ToolStripMenuItem.Text = "设定";
			// 
			// 圆弧插补设置ToolStripMenuItem
			// 
			this.圆弧插补设置ToolStripMenuItem.Name = "圆弧插补设置ToolStripMenuItem";
			this.圆弧插补设置ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
			this.圆弧插补设置ToolStripMenuItem.Text = "圆弧插补设置";
			this.圆弧插补设置ToolStripMenuItem.Click += new System.EventHandler(this.圆弧插补设置ToolStripMenuItem_Click);
			// 
			// 插补步长设置ToolStripMenuItem
			// 
			this.插补步长设置ToolStripMenuItem.Name = "插补步长设置ToolStripMenuItem";
			this.插补步长设置ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
			this.插补步长设置ToolStripMenuItem.Text = "插补步长设置";
			this.插补步长设置ToolStripMenuItem.Click += new System.EventHandler(this.插补步长设置ToolStripMenuItem_Click);
			// 
			// plugInsToolStripMenuItem
			// 
			this.plugInsToolStripMenuItem.Name = "plugInsToolStripMenuItem";
			this.plugInsToolStripMenuItem.Size = new System.Drawing.Size(67, 21);
			this.plugInsToolStripMenuItem.Text = "Plug-Ins";
			// 
			// 帮助ToolStripMenuItem
			// 
			this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.帮助ToolStripMenuItem1,
            this.toolStripMenuItem2,
            this.关于ToolStripMenuItem});
			this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
			this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
			this.帮助ToolStripMenuItem.Text = "帮助";
			// 
			// 帮助ToolStripMenuItem1
			// 
			this.帮助ToolStripMenuItem1.Name = "帮助ToolStripMenuItem1";
			this.帮助ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
			this.帮助ToolStripMenuItem1.Text = "帮助";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(97, 6);
			// 
			// 关于ToolStripMenuItem
			// 
			this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
			this.关于ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
			this.关于ToolStripMenuItem.Text = "关于";
			this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// serialPort1
			// 
			this.serialPort1.ReceivedBytesThreshold = 4;
			this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
			// 
			// tbGCodeOpened
			// 
			this.tbGCodeOpened.Font = new System.Drawing.Font("宋体", 15F);
			this.tbGCodeOpened.Location = new System.Drawing.Point(252, 28);
			this.tbGCodeOpened.Multiline = true;
			this.tbGCodeOpened.Name = "tbGCodeOpened";
			this.tbGCodeOpened.ReadOnly = true;
			this.tbGCodeOpened.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbGCodeOpened.Size = new System.Drawing.Size(493, 412);
			this.tbGCodeOpened.TabIndex = 5;
			// 
			// labelX
			// 
			this.labelX.AutoSize = true;
			this.labelX.Font = new System.Drawing.Font("Consolas", 39F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelX.Location = new System.Drawing.Point(778, 105);
			this.labelX.Name = "labelX";
			this.labelX.Size = new System.Drawing.Size(85, 61);
			this.labelX.TabIndex = 6;
			this.labelX.Text = "X:";
			// 
			// labelY
			// 
			this.labelY.AutoSize = true;
			this.labelY.Font = new System.Drawing.Font("Consolas", 39F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelY.Location = new System.Drawing.Point(778, 234);
			this.labelY.Name = "labelY";
			this.labelY.Size = new System.Drawing.Size(85, 61);
			this.labelY.TabIndex = 7;
			this.labelY.Text = "Y:";
			// 
			// labelZ
			// 
			this.labelZ.AutoSize = true;
			this.labelZ.Font = new System.Drawing.Font("Consolas", 39F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelZ.Location = new System.Drawing.Point(778, 358);
			this.labelZ.Name = "labelZ";
			this.labelZ.Size = new System.Drawing.Size(85, 61);
			this.labelZ.TabIndex = 8;
			this.labelZ.Text = "Z:";
			// 
			// coorXShow
			// 
			this.coorXShow.AutoSize = true;
			this.coorXShow.Cursor = System.Windows.Forms.Cursors.Hand;
			this.coorXShow.Font = new System.Drawing.Font("Consolas", 39F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.coorXShow.Location = new System.Drawing.Point(859, 105);
			this.coorXShow.Name = "coorXShow";
			this.coorXShow.Size = new System.Drawing.Size(288, 61);
			this.coorXShow.TabIndex = 9;
			this.coorXShow.Text = "0000.0000";
			this.coorXShow.DoubleClick += new System.EventHandler(this.coorXShow_DoubleClick);
			// 
			// coorYShow
			// 
			this.coorYShow.AutoSize = true;
			this.coorYShow.Cursor = System.Windows.Forms.Cursors.Hand;
			this.coorYShow.Font = new System.Drawing.Font("Consolas", 39F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.coorYShow.Location = new System.Drawing.Point(859, 234);
			this.coorYShow.Name = "coorYShow";
			this.coorYShow.Size = new System.Drawing.Size(288, 61);
			this.coorYShow.TabIndex = 10;
			this.coorYShow.Text = "0000.0000";
			this.coorYShow.DoubleClick += new System.EventHandler(this.coorYShow_DoubleClick);
			// 
			// coorZShow
			// 
			this.coorZShow.AutoSize = true;
			this.coorZShow.Cursor = System.Windows.Forms.Cursors.Hand;
			this.coorZShow.Font = new System.Drawing.Font("Consolas", 39F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.coorZShow.Location = new System.Drawing.Point(859, 358);
			this.coorZShow.Name = "coorZShow";
			this.coorZShow.Size = new System.Drawing.Size(288, 61);
			this.coorZShow.TabIndex = 11;
			this.coorZShow.Text = "0000.0000";
			this.coorZShow.DoubleClick += new System.EventHandler(this.coorZShow_DoubleClick);
			// 
			// btnChangeCoordinate
			// 
			this.btnChangeCoordinate.Font = new System.Drawing.Font("微软雅黑", 20F);
			this.btnChangeCoordinate.Location = new System.Drawing.Point(1177, 181);
			this.btnChangeCoordinate.Name = "btnChangeCoordinate";
			this.btnChangeCoordinate.Size = new System.Drawing.Size(66, 181);
			this.btnChangeCoordinate.TabIndex = 12;
			this.btnChangeCoordinate.Text = "绝对坐标";
			this.btnChangeCoordinate.UseVisualStyleBackColor = true;
			this.btnChangeCoordinate.Visible = false;
			this.btnChangeCoordinate.Click += new System.EventHandler(this.btnChangeCoordinate_Click);
			// 
			// tbGCodeInput
			// 
			this.tbGCodeInput.Font = new System.Drawing.Font("Consolas", 20F);
			this.tbGCodeInput.Location = new System.Drawing.Point(252, 473);
			this.tbGCodeInput.Name = "tbGCodeInput";
			this.tbGCodeInput.Size = new System.Drawing.Size(792, 39);
			this.tbGCodeInput.TabIndex = 13;
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("微软雅黑", 15F);
			this.button1.Location = new System.Drawing.Point(1050, 475);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(97, 37);
			this.button1.TabIndex = 14;
			this.button1.Text = "发送";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("微软雅黑", 15F);
			this.label4.Location = new System.Drawing.Point(247, 443);
			this.label4.Name = "label4";
			this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label4.Size = new System.Drawing.Size(137, 27);
			this.label4.TabIndex = 15;
			this.label4.Text = "手动指令输入:";
			// 
			// msgModelValue
			// 
			this.msgModelValue.AutoSize = true;
			this.msgModelValue.Location = new System.Drawing.Point(787, 31);
			this.msgModelValue.Name = "msgModelValue";
			this.msgModelValue.Size = new System.Drawing.Size(53, 12);
			this.msgModelValue.TabIndex = 16;
			this.msgModelValue.Text = "模态量：";
			// 
			// btnToZero
			// 
			this.btnToZero.Font = new System.Drawing.Font("微软雅黑", 15F);
			this.btnToZero.Location = new System.Drawing.Point(252, 554);
			this.btnToZero.Name = "btnToZero";
			this.btnToZero.Size = new System.Drawing.Size(103, 97);
			this.btnToZero.TabIndex = 17;
			this.btnToZero.Text = "回  零";
			this.btnToZero.UseVisualStyleBackColor = true;
			this.btnToZero.Click += new System.EventHandler(this.btnToZero_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("微软雅黑", 15F);
			this.label1.Location = new System.Drawing.Point(483, 524);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 27);
			this.label1.TabIndex = 18;
			this.label1.Text = "点 动：";
			// 
			// keyControlMul
			// 
			this.keyControlMul.AutoSize = true;
			this.keyControlMul.Cursor = System.Windows.Forms.Cursors.Hand;
			this.keyControlMul.Font = new System.Drawing.Font("Consolas", 25F);
			this.keyControlMul.Location = new System.Drawing.Point(536, 580);
			this.keyControlMul.Name = "keyControlMul";
			this.keyControlMul.Size = new System.Drawing.Size(55, 40);
			this.keyControlMul.TabIndex = 19;
			this.keyControlMul.Text = "×1";
			this.keyControlMul.DoubleClick += new System.EventHandler(this.keyControlMul_DoubleClick);
			// 
			// ckbIsKeyControl
			// 
			this.ckbIsKeyControl.AutoSize = true;
			this.ckbIsKeyControl.Location = new System.Drawing.Point(567, 541);
			this.ckbIsKeyControl.Name = "ckbIsKeyControl";
			this.ckbIsKeyControl.Size = new System.Drawing.Size(72, 16);
			this.ckbIsKeyControl.TabIndex = 20;
			this.ckbIsKeyControl.Text = "开启点动";
			this.ckbIsKeyControl.UseVisualStyleBackColor = true;
			// 
			// wheelControlMul
			// 
			this.wheelControlMul.AutoSize = true;
			this.wheelControlMul.Cursor = System.Windows.Forms.Cursors.Hand;
			this.wheelControlMul.Font = new System.Drawing.Font("Consolas", 25F);
			this.wheelControlMul.Location = new System.Drawing.Point(957, 541);
			this.wheelControlMul.Name = "wheelControlMul";
			this.wheelControlMul.Size = new System.Drawing.Size(55, 40);
			this.wheelControlMul.TabIndex = 22;
			this.wheelControlMul.Text = "×1";
			this.wheelControlMul.DoubleClick += new System.EventHandler(this.wheelControlMul_DoubleClick);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("微软雅黑", 15F);
			this.label3.Location = new System.Drawing.Point(784, 524);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 27);
			this.label3.TabIndex = 23;
			this.label3.Text = "鼠标控制：";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbCoordinateZ);
			this.groupBox1.Controls.Add(this.rbCoordinateY);
			this.groupBox1.Controls.Add(this.rbCoordinateX);
			this.groupBox1.Location = new System.Drawing.Point(957, 580);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(85, 89);
			this.groupBox1.TabIndex = 24;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "坐标选择";
			// 
			// rbCoordinateZ
			// 
			this.rbCoordinateZ.AutoSize = true;
			this.rbCoordinateZ.Location = new System.Drawing.Point(7, 67);
			this.rbCoordinateZ.Name = "rbCoordinateZ";
			this.rbCoordinateZ.Size = new System.Drawing.Size(29, 16);
			this.rbCoordinateZ.TabIndex = 2;
			this.rbCoordinateZ.TabStop = true;
			this.rbCoordinateZ.Text = "Z";
			this.rbCoordinateZ.UseVisualStyleBackColor = true;
			// 
			// rbCoordinateY
			// 
			this.rbCoordinateY.AutoSize = true;
			this.rbCoordinateY.Location = new System.Drawing.Point(7, 42);
			this.rbCoordinateY.Name = "rbCoordinateY";
			this.rbCoordinateY.Size = new System.Drawing.Size(29, 16);
			this.rbCoordinateY.TabIndex = 1;
			this.rbCoordinateY.TabStop = true;
			this.rbCoordinateY.Text = "Y";
			this.rbCoordinateY.UseVisualStyleBackColor = true;
			// 
			// rbCoordinateX
			// 
			this.rbCoordinateX.AutoSize = true;
			this.rbCoordinateX.Location = new System.Drawing.Point(7, 18);
			this.rbCoordinateX.Name = "rbCoordinateX";
			this.rbCoordinateX.Size = new System.Drawing.Size(29, 16);
			this.rbCoordinateX.TabIndex = 0;
			this.rbCoordinateX.TabStop = true;
			this.rbCoordinateX.Text = "X";
			this.rbCoordinateX.UseVisualStyleBackColor = true;
			// 
			// msgGCodeInputCheck
			// 
			this.msgGCodeInputCheck.AutoSize = true;
			this.msgGCodeInputCheck.Location = new System.Drawing.Point(390, 454);
			this.msgGCodeInputCheck.Name = "msgGCodeInputCheck";
			this.msgGCodeInputCheck.Size = new System.Drawing.Size(23, 12);
			this.msgGCodeInputCheck.TabIndex = 25;
			this.msgGCodeInputCheck.Text = "...";
			// 
			// ckbIsWheelControl
			// 
			this.ckbIsWheelControl.AutoSize = true;
			this.ckbIsWheelControl.Location = new System.Drawing.Point(889, 532);
			this.ckbIsWheelControl.Name = "ckbIsWheelControl";
			this.ckbIsWheelControl.Size = new System.Drawing.Size(84, 16);
			this.ckbIsWheelControl.TabIndex = 26;
			this.ckbIsWheelControl.Text = "开鼠标控制";
			this.ckbIsWheelControl.UseVisualStyleBackColor = true;
			// 
			// msg_GErrorWarnning
			// 
			this.msg_GErrorWarnning.AutoSize = true;
			this.msg_GErrorWarnning.ForeColor = System.Drawing.Color.Red;
			this.msg_GErrorWarnning.Location = new System.Drawing.Point(751, 70);
			this.msg_GErrorWarnning.Name = "msg_GErrorWarnning";
			this.msg_GErrorWarnning.Size = new System.Drawing.Size(0, 12);
			this.msg_GErrorWarnning.TabIndex = 28;
			// 
			// btnSerialSwitch
			// 
			this.btnSerialSwitch.BackColor = System.Drawing.Color.Transparent;
			this.btnSerialSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnSerialSwitch.Location = new System.Drawing.Point(60, 154);
			this.btnSerialSwitch.Name = "btnSerialSwitch";
			this.btnSerialSwitch.Size = new System.Drawing.Size(128, 39);
			this.btnSerialSwitch.TabIndex = 27;
			this.btnSerialSwitch.TabStop = false;
			this.btnSerialSwitch.Click += new System.EventHandler(this.btnSerialSwitch_Click);
			// 
			// areaWheelControl
			// 
			this.areaWheelControl.BackColor = System.Drawing.SystemColors.ControlDark;
			this.areaWheelControl.Location = new System.Drawing.Point(789, 554);
			this.areaWheelControl.Name = "areaWheelControl";
			this.areaWheelControl.Size = new System.Drawing.Size(162, 115);
			this.areaWheelControl.TabIndex = 21;
			this.areaWheelControl.TabStop = false;
			// 
			// btnPicEStop
			// 
			this.btnPicEStop.BackColor = System.Drawing.Color.Transparent;
			this.btnPicEStop.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnPicEStop.Location = new System.Drawing.Point(12, 211);
			this.btnPicEStop.Name = "btnPicEStop";
			this.btnPicEStop.Size = new System.Drawing.Size(176, 56);
			this.btnPicEStop.TabIndex = 4;
			this.btnPicEStop.TabStop = false;
			this.btnPicEStop.Click += new System.EventHandler(this.btnPicEStop_Click);
			this.btnPicEStop.MouseEnter += new System.EventHandler(this.btnPicEStop_MouseEnter);
			this.btnPicEStop.MouseLeave += new System.EventHandler(this.btnPicEStop_MouseLeave);
			// 
			// btnPicStart
			// 
			this.btnPicStart.BackColor = System.Drawing.Color.Transparent;
			this.btnPicStart.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnPicStart.Location = new System.Drawing.Point(12, 273);
			this.btnPicStart.Name = "btnPicStart";
			this.btnPicStart.Size = new System.Drawing.Size(176, 128);
			this.btnPicStart.TabIndex = 3;
			this.btnPicStart.TabStop = false;
			this.btnPicStart.Click += new System.EventHandler(this.btnPicStart_Click);
			this.btnPicStart.MouseEnter += new System.EventHandler(this.btnPicStart_MouseEnter);
			this.btnPicStart.MouseLeave += new System.EventHandler(this.btnPicStart_MouseLeave);
			// 
			// btnPicStop
			// 
			this.btnPicStop.BackColor = System.Drawing.Color.Transparent;
			this.btnPicStop.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnPicStop.Location = new System.Drawing.Point(12, 407);
			this.btnPicStop.Name = "btnPicStop";
			this.btnPicStop.Size = new System.Drawing.Size(176, 128);
			this.btnPicStop.TabIndex = 2;
			this.btnPicStop.TabStop = false;
			this.btnPicStop.Click += new System.EventHandler(this.btnPicStop_Click);
			this.btnPicStop.MouseEnter += new System.EventHandler(this.btnPicStop_MouseEnter);
			this.btnPicStop.MouseLeave += new System.EventHandler(this.btnPicStop_MouseLeave);
			// 
			// btnPicPause
			// 
			this.btnPicPause.BackColor = System.Drawing.Color.Transparent;
			this.btnPicPause.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnPicPause.Location = new System.Drawing.Point(12, 541);
			this.btnPicPause.Name = "btnPicPause";
			this.btnPicPause.Size = new System.Drawing.Size(176, 128);
			this.btnPicPause.TabIndex = 1;
			this.btnPicPause.TabStop = false;
			this.btnPicPause.Click += new System.EventHandler(this.btnPicPause_Click);
			this.btnPicPause.MouseEnter += new System.EventHandler(this.btnPicPause_MouseEnter);
			this.btnPicPause.MouseLeave += new System.EventHandler(this.btnPicPause_MouseLeave);
			// 
			// tbPre
			// 
			this.tbPre.Location = new System.Drawing.Point(12, 28);
			this.tbPre.Multiline = true;
			this.tbPre.Name = "tbPre";
			this.tbPre.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbPre.Size = new System.Drawing.Size(234, 120);
			this.tbPre.TabIndex = 29;
			this.tbPre.Visible = false;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(408, 561);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(41, 12);
			this.label2.TabIndex = 30;
			this.label2.Text = "W - Y+";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(475, 580);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(41, 12);
			this.label5.TabIndex = 31;
			this.label5.Text = "A - X-";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(408, 598);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(41, 12);
			this.label6.TabIndex = 32;
			this.label6.Text = "Q - Z+";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(475, 561);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(41, 12);
			this.label7.TabIndex = 33;
			this.label7.Text = "S - Y-";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(408, 580);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(41, 12);
			this.label8.TabIndex = 34;
			this.label8.Text = "D - X+";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(475, 598);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(41, 12);
			this.label9.TabIndex = 35;
			this.label9.Text = "E - Z-";
			// 
			// msg_ShowThread
			// 
			this.msg_ShowThread.AutoSize = true;
			this.msg_ShowThread.Location = new System.Drawing.Point(366, 660);
			this.msg_ShowThread.Name = "msg_ShowThread";
			this.msg_ShowThread.Size = new System.Drawing.Size(47, 12);
			this.msg_ShowThread.TabIndex = 36;
			this.msg_ShowThread.Text = "label10";
			this.msg_ShowThread.Visible = false;
			// 
			// btnDebugSet
			// 
			this.btnDebugSet.Location = new System.Drawing.Point(657, 649);
			this.btnDebugSet.Name = "btnDebugSet";
			this.btnDebugSet.Size = new System.Drawing.Size(75, 23);
			this.btnDebugSet.TabIndex = 37;
			this.btnDebugSet.Text = "set";
			this.btnDebugSet.UseVisualStyleBackColor = true;
			this.btnDebugSet.Visible = false;
			this.btnDebugSet.Click += new System.EventHandler(this.btnDebugSet_Click);
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1264, 681);
			this.Controls.Add(this.btnDebugSet);
			this.Controls.Add(this.msg_ShowThread);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbPre);
			this.Controls.Add(this.msg_GErrorWarnning);
			this.Controls.Add(this.btnSerialSwitch);
			this.Controls.Add(this.ckbIsWheelControl);
			this.Controls.Add(this.msgGCodeInputCheck);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.wheelControlMul);
			this.Controls.Add(this.areaWheelControl);
			this.Controls.Add(this.ckbIsKeyControl);
			this.Controls.Add(this.keyControlMul);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnToZero);
			this.Controls.Add(this.msgModelValue);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.tbGCodeInput);
			this.Controls.Add(this.btnChangeCoordinate);
			this.Controls.Add(this.coorZShow);
			this.Controls.Add(this.coorYShow);
			this.Controls.Add(this.coorXShow);
			this.Controls.Add(this.labelZ);
			this.Controls.Add(this.labelY);
			this.Controls.Add(this.labelX);
			this.Controls.Add(this.tbGCodeOpened);
			this.Controls.Add(this.btnPicEStop);
			this.Controls.Add(this.btnPicStart);
			this.Controls.Add(this.btnPicStop);
			this.Controls.Add(this.btnPicPause);
			this.Controls.Add(this.mainMenu);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.KeyPreview = true;
			this.MainMenuStrip = this.mainMenu;
			this.MaximizeBox = false;
			this.Name = "MainWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "三坐标工作台上位机";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
			this.Load += new System.EventHandler(this.MainWindow_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainWindow_KeyDown);
			this.mainMenu.ResumeLayout(false);
			this.mainMenu.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.btnSerialSwitch)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.areaWheelControl)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.btnPicEStop)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.btnPicStart)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.btnPicStop)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.btnPicPause)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenu;
		private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 通信ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem plugInsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 通信设置ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 设定ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem1;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.IO.Ports.SerialPort serialPort1;
		private System.Windows.Forms.PictureBox btnPicPause;
		private System.Windows.Forms.PictureBox btnPicStop;
		private System.Windows.Forms.PictureBox btnPicStart;
		private System.Windows.Forms.PictureBox btnPicEStop;
		private System.Windows.Forms.TextBox tbGCodeOpened;
		private System.Windows.Forms.Label labelX;
		private System.Windows.Forms.Label labelY;
		private System.Windows.Forms.Label labelZ;
		private System.Windows.Forms.Label coorXShow;
		private System.Windows.Forms.Label coorYShow;
		private System.Windows.Forms.Label coorZShow;
		private System.Windows.Forms.Button btnChangeCoordinate;
		private System.Windows.Forms.TextBox tbGCodeInput;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label msgModelValue;
		private System.Windows.Forms.Button btnToZero;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label keyControlMul;
		private System.Windows.Forms.CheckBox ckbIsKeyControl;
		private System.Windows.Forms.PictureBox areaWheelControl;
		private System.Windows.Forms.Label wheelControlMul;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbCoordinateZ;
		private System.Windows.Forms.RadioButton rbCoordinateY;
		private System.Windows.Forms.RadioButton rbCoordinateX;
		private System.Windows.Forms.Label msgGCodeInputCheck;
		private System.Windows.Forms.CheckBox ckbIsWheelControl;
		private System.Windows.Forms.PictureBox btnSerialSwitch;
		private System.Windows.Forms.ToolStripMenuItem 圆弧插补设置ToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 插补步长设置ToolStripMenuItem;
		private System.Windows.Forms.Label msg_GErrorWarnning;
		private System.Windows.Forms.TextBox tbPre;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label msg_ShowThread;
		private System.Windows.Forms.Button btnDebugSet;
	}
}

