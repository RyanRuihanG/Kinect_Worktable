using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IPublicPlugInInterface;
using TDCommunication;
using GCodeInterpreter;
using ResLib;
using LineInter;
using Iinterpolation;
using System.Drawing.Text;
using System.IO.Ports;
using System.Threading;
using EncoderDecoder;

namespace ThreeCoordinateMainWindow
{
	public partial class MainWindow : Form
	{
		#region 私有变量
		/// <summary>
		/// 非主动向宿主传数据的插件列表
		/// </summary>
		List<IPlugInGetData> _plugInGetData = new List<IPlugInGetData>();
		/// <summary>
		/// 主动向宿主传数据的插件列表
		/// </summary>
		List<IPlugInGetSetData> _plugInGetSetData = new List<IPlugInGetSetData>();
		/// <summary>
		/// 成功加载的插件数
		/// </summary>
		int _countPlugAdded = 0;
		/// <summary>
		/// 向插件传递数据用的
		/// </summary>
		DataTransfer _mainWindowDT;
		/// <summary>
		/// 加载失败的插件数
		/// </summary>
		int _countPlugAddError = 0;
		/// <summary>
		/// G代码指令列
		/// </summary>
		List<string> GCodeLineList = new List<string>();
		/// <summary>
		/// G代码解释结果指令列
		/// </summary>
		List<GCodeClass> GCodeInterpreted = new List<GCodeClass>();
		/// <summary>
		/// G代码解释器操作子
		/// </summary>
		GCodeInterpreterClass _gCodeInterpreterOperator = new GCodeInterpreterClass();
		/// <summary>
		/// G代码插补运算操作子
		/// </summary>
		OperatorInter _gCodeInterOperator;
		/// <summary>
		/// 标志程序运行状态
		/// </summary>
		private AppWorkState _appWorkState;

		static AutoResetEvent _autoEventAction;//用于串口发送线程和接受线程同步
		static AutoResetEvent _controlThreadSuspendMonitor;//用于监视控制线程是否被挂起
		//static AutoResetEvent _askMonitorTick;//控制线程通知监视线程判断是否可以发脉冲

		bool _isControlThreadSuspended;//控制线程是否挂起的标志

		private Font _coordinateNumberFont;//坐标显示字体
		private int _keyControlMul = 1;
		private int _wheelControlMul = 1;

		Point3D _absoluteCoordinate;//绝对坐标
		Point3D _relativeCoordinate;//相对坐标
		Point3D _deltaCoordinate;//绝对坐标相对坐标之差，等于绝对 - 相对
		bool _isDisplayAbsolute;//是否正在显示绝对坐标

		bool _isG23Absolute = false;//G2/G3不是绝对圆心，是相对的
		double _gWorkStep = 1;//G代码插补步长，单位毫米

		//模态量保存
		GWorkState _GModal;
		MWorkState _MModal;
		int _FModal;
		double _IModal;
		double _JModal;

		//全局通信协议编码解码
		EncoderDecoder.Encoder _globalEncoder;
		EncoderDecoder.Decoder _globalDecoder;
		List<byte> _dataReceived;

		List<byte> _receivedPackage;//串口接收字符数组

		//线程定义
		Thread _controlTableThread;
		Thread _monitorControlThread;
		Thread _receivingProcess;
		Queue<List<byte>> _receivingDataQueue;
		#endregion

#if DEBUG
		int countSerialPortReceived = 0;
#endif

		public MainWindow()
		{
			InitializeComponent();
		}

		//初始化相关变量
		private void MainWindow_Load(object sender, EventArgs e)
		{
			#region 初始化字段
			_receivingDataQueue = new Queue<List<byte>>();//串口消息队列

			_receivingProcess = new Thread(new ThreadStart(DataProssing));//初始化串口处理线程
			/**/_receivingProcess.IsBackground = true;
			_receivingProcess.Name = "串口数据处理";
			_receivingProcess.Start();

			_mainWindowDT = new DataTransfer();
			_autoEventAction = new AutoResetEvent(false);
			_controlThreadSuspendMonitor = new AutoResetEvent(false);
			//_askMonitorTick = new AutoResetEvent(false);

			_isControlThreadSuspended = false;

			//_controlTableThread = new Thread(new ThreadStart(ControlTableThreading));
			//_controlTableThread.Name = "TableControl";

			_appWorkState = AppWorkState.Stop;//设定当前工作状态位停止

			_gCodeInterOperator = new OperatorInter(_gWorkStep);//初始化G代码插补计算操作子

			_absoluteCoordinate = new Point3D();//绝对坐标
			_relativeCoordinate = new Point3D();//相对坐标
			_deltaCoordinate = new Point3D();//绝对 - 相对
			_deltaCoordinate = _absoluteCoordinate - _relativeCoordinate;//绝对 - 相对

			_isDisplayAbsolute = false;//是否正在显示绝对坐标

			_GModal = GWorkState.G00;//G模态量
			_MModal = MWorkState.M30;//M模态量
			_FModal = 0;//速度模态量
			_IModal = 0;//I向量模态量
			_JModal = 0;//J向量模态量

			//通信相关实例化
			_globalEncoder = new EncoderDecoder.Encoder();
			_globalDecoder = new EncoderDecoder.Decoder();
			_receivedPackage = new List<byte>();
			_dataReceived = new List<byte>();
			#endregion

			#region 初始化控件

#if DEBUG
			tbPre.Visible = true;
			msg_ShowThread.Visible = true;
			btnDebugSet.Visible = true;
#endif

			//btnPicStart.Enabled = false;//禁用开始按钮防止误操作
			serialPort1.Close();
			ckbIsWheelControl.Enabled = false;
			ckbIsKeyControl.Enabled = false;

			//贴图
			btnSerialSwitch.Image = ResLibClass.GetSwitchImage(false);
			btnPicEStop.Image = ResLibClass.GetButtonImageEStop("normal");
			btnPicStart.Image = ResLibClass.GetButtonImageStart("normal");
			btnPicStop.Image = ResLibClass.GetButtonImageEnd("normal");
			btnPicPause.Image = ResLibClass.GetButtonImagePause("normal");
			#endregion

			areaWheelControl.MouseWheel += areaWheel_MouseWheel;//向鼠标滚轮控制添加事件处理

			string tpath;//文件操作的路径字符串
			#region 载入字体
			try
			{
				//路径             
				tpath = Path.Combine(Environment.CurrentDirectory + @"\Fonts\consolab.ttf");
				//读取字体文件             
				PrivateFontCollection pfc = new PrivateFontCollection();
				pfc.AddFontFile(tpath);
				//实例化字体             
				_coordinateNumberFont = new Font(pfc.Families[0], 16);//第二个数字是大小
				

			}
			catch
			{
				_coordinateNumberFont = new Font("宋体", 16);
			}
			#endregion

			#region 载入插件
			//插件列表
			List<string> list = new List<string>();

			//读取插件
			tpath = null;
			tpath = Path.Combine(Environment.CurrentDirectory + @"\PlugIn\");
			DirectoryInfo dir = new DirectoryInfo(tpath);
			FileInfo[] fil = dir.GetFiles();
			foreach (FileInfo f in fil)
			{
				if (f.Extension.Equals(".dll"))
				{
					list.Add(f.FullName);
				}
			}

			
			//组装插件
			if (list.Count != 0)
			{
				foreach (string tp in list)
				{
					try
					{
						Assembly asd = Assembly.LoadFile(tp);
						Type[] t = asd.GetTypes();
						foreach (Type tin in t)
						{
							if (tin.GetInterface("IPlugInGetData") != null)
							{
								IPlugInGetData tempIG;

								tempIG = (IPlugInGetData)Activator.CreateInstance(asd.GetType(tin.FullName));
								tempIG.SetDataTransfer(_mainWindowDT);
								_plugInGetData.Add(tempIG);

								//创建菜单
								ToolStripMenuItem newItem = new ToolStripMenuItem();
								newItem.Text = (tempIG.WhoAmI()).Name;
								newItem.Click += tempIG.Action;

								((ToolStripMenuItem)(mainMenu.Items[3])).DropDownItems.Add(newItem);

								//计数累加
								_countPlugAdded++;
								//break;
							}
							else if (tin.GetInterface("IPlugInGetSetData") != null)
							{
								IPlugInGetSetData tempIGS;

								tempIGS = (IPlugInGetSetData)Activator.CreateInstance(asd.GetType(tin.FullName));
								tempIGS.ChangeData += new ChangeDataHandler(PlugInDataChanged);//设定传入事件
								tempIGS.SetDataTransfer(_mainWindowDT);
								_plugInGetSetData.Add(tempIGS);

								//创建菜单
								ToolStripMenuItem newItem = new ToolStripMenuItem();
								newItem.Text = (tempIGS.WhoAmI()).Name;
								newItem.Click += tempIGS.Action;

								((ToolStripMenuItem)(mainMenu.Items[3])).DropDownItems.Add(newItem);

								//计数累加
								_countPlugAdded++;
								//break;
							}
						}
					}
					catch (Exception)
					{
						_countPlugAddError++;
					}
				}
			}
			else
			{
				ToolStripMenuItem newItem = new ToolStripMenuItem();
				newItem.Text = "No Plug-In";
				newItem.Enabled = false;
				((ToolStripMenuItem)(mainMenu.Items[3])).DropDownItems.Add(newItem);
			}
			#endregion
		}

		#region 菜单操作
		//菜单
		private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AboutBox AB = new AboutBox();
			AB.Show();
		}

		private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog1.InitialDirectory = Application.StartupPath;
			openFileDialog1.Filter = "All files (*.*)|*.*|txt files (*.txt)|*.txt|NC files (*.nc)|*.nc";
			openFileDialog1.FilterIndex = 3;
			openFileDialog1.RestoreDirectory = true;
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				string  _tpath = openFileDialog1.FileName.ToString();

				StreamReader file = new StreamReader(_tpath,System.Text.Encoding.Default);
				string line;
				int lineCounter = 0;
				GCodeLineList.Clear();
				while ((line = file.ReadLine()) != null)
				{
					GCodeLineList.Add(line);
					lineCounter++;
				}
				file.Close();

				tbGCodeOpened.Text = "";
				msg_GErrorWarnning.Text = "";
				GCodeInterpreted.Clear();
				int tcounter = 0;//行数计数器

				_appWorkState = AppWorkState.ReadGCode;//标记G代码读取完毕

				_mainWindowDT.OperationList.Clear();

				foreach (string s in GCodeLineList)
				{
					tcounter++;

					tbGCodeOpened.Text += s;
					tbGCodeOpened.Text += "\r\n";//UI显示

					GCodeClass tempResult = _gCodeInterpreterOperator.Interpreter(s);//解释
					if (tempResult.IsError == false)//如果没有错
					{
						GCodeInterpreted.Add(_gCodeInterpreterOperator.Interpreter(s));//加入解释完毕列表
					}
					else//如果有错
					{
						//写报警UI
						msg_GErrorWarnning.Text = "G代码错误，位置：" + tcounter.ToString() + "行";
						_appWorkState = AppWorkState.Stop;
						GCodeInterpreted.Clear();
						break;//跳出
					}
				}

				ChangeGCodeInterpertedToDT();
			}
		}

		private void 通信设置ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SerialSet serialSetWindow = new SerialSet(ref serialPort1);
			if (serialPort1.IsOpen == false)
			{
				serialSetWindow.ShowDialog();
			}
		}

		private void 圆弧插补设置ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			G23IsAbsoluteSetForm g23iasForm = new G23IsAbsoluteSetForm(_isG23Absolute);
			if (g23iasForm.ShowDialog() == DialogResult.OK)//如果用户点了是
			{
				_isG23Absolute = g23iasForm.IsG23Absolute;//获取结果
			}
			g23iasForm.Dispose();
		}

		private void 插补步长设置ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_appWorkState == AppWorkState.Stop || _appWorkState == AppWorkState.ReadGCode)
			{
				NumberInputForm<double> niform = new NumberInputForm<double>(false, true, _gWorkStep);//实例化数字输入窗体
				if (niform.ShowDialog() == DialogResult.OK)
				{
					_gWorkStep = niform.GetNumber;//获取用户输入
				}
				niform.Dispose();//释放资源

				_gCodeInterOperator.SetStep(_gWorkStep);//修改操作子插补步长
			}
			else
			{
				MessageBox.Show("现在不能修改插补步长，请停止正在运行的程序");
			}
		}
		#endregion

		#region 主按钮操作 包含串口开关
		private void btnPicStart_Click(object sender, EventArgs e)
		{
			if (_appWorkState != AppWorkState.ReadGCode && _appWorkState != AppWorkState.Pause)
			{
				MessageBox.Show("G代码未载入或程序不处于空闲！", "Error");
			}
			else
			{
				if (serialPort1.IsOpen == true)
				{
					if (_isControlThreadSuspended == true)//控制线程被挂起，则此时恢复运行
					{
						_isControlThreadSuspended = false;//非挂起
						_appWorkState = AppWorkState.RunningGCode;
						//_askMonitorTick.Set();
					}
					else//G代码载入完毕，线程未挂起，说明从头开始
					{
						_controlTableThread = new Thread(new ThreadStart(ControlTableThreading));
						_monitorControlThread = new Thread(new ThreadStart(MonitorOfControlThread));

						_controlTableThread.Name = "控制工作台";
						_monitorControlThread.Name = "监视";

						_appWorkState = AppWorkState.RunningGCode;

						_controlTableThread.Start();
						_monitorControlThread.Start();
					}
				}
			}
		}

		private void btnPicStop_Click(object sender, EventArgs e)
		{
			if (_appWorkState == AppWorkState.RunningGCode || _appWorkState == AppWorkState.Pause)
			{
				_controlTableThread.Abort();
				_monitorControlThread.Abort();

				_appWorkState = AppWorkState.Stop;//标志程序停止状态

				//清理已经读入的G代码
				GCodeLineList.Clear();
				GCodeInterpreted.Clear();
				tbGCodeOpened.Text = "";
			}
		}

		private void btnPicPause_Click(object sender, EventArgs e)
		{
			if (_appWorkState == AppWorkState.RunningGCode)
			{
				if (_controlTableThread.ThreadState == ThreadState.Running)//如果线程正在运行
				{
					_isControlThreadSuspended = true;//挂起
					_appWorkState = AppWorkState.Pause;//标记程序状态
				}
			}
		}

		private void btnPicEStop_Click(object sender, EventArgs e)
		{
			if (_appWorkState == AppWorkState.RunningGCode || _appWorkState == AppWorkState.Pause)
			{
				_controlTableThread.Abort();
				_monitorControlThread.Abort();

				_appWorkState = AppWorkState.Stop;//标记程序停止

				//清理已经读入的G代码
				GCodeLineList.Clear();
				GCodeInterpreted.Clear();
				tbGCodeOpened.Text = "";
			}
		}

		//串口开关
		private void btnSerialSwitch_Click(object sender, EventArgs e)
		{
			if (serialPort1.IsOpen)//如果串口已经打开说明是关闭串口
			{
				if (_appWorkState == AppWorkState.RunningGCode || _appWorkState == AppWorkState.PlugInControl)
				{
					MessageBox.Show("不能关闭串口");
				}
				else
				{
					serialPort1.Close();
					btnPicStart.Enabled = false;
					btnSerialSwitch.Image = ResLibClass.GetSwitchImage(false);
				}

				ckbIsKeyControl.Enabled = false;
				ckbIsWheelControl.Enabled = false;
			}
			else//开启串口
			{
				if (serialPort1.PortName == "")
				{
					MessageBox.Show("串口设置不正确，请检查“通信”->“通信设置”");
				}
				else
				{
					try
					{
						serialPort1.Open();
						btnPicStart.Enabled = true;
						btnSerialSwitch.Image = ResLibClass.GetSwitchImage(true);
					}
					catch
					{
						MessageBox.Show("串口设置不正确或串口被占用");
						btnPicStart.Enabled = false;
					}
				}

				ckbIsKeyControl.Enabled = true;
				ckbIsWheelControl.Enabled = true;
			}
		}

		/// <summary>
		/// 回零
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnToZero_Click(object sender, EventArgs e)
		{
			if (_appWorkState == AppWorkState.Stop || _appWorkState == AppWorkState.ReadGCode)
			{
				DataPackage tempPackage = new DataPackage();
				tempPackage.Command = CommandClass._TO_ZERO_;

				if (serialPort1.IsOpen == true)
				{
					byte[] tempByte = _globalEncoder.Encode(tempPackage);
					serialPort1.Write(tempByte, 0, tempByte.Length);//发包
				}
			}
		}

		/// <summary>
		/// 输入指令控制
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			if (_appWorkState == AppWorkState.Stop)
			{
				string oneLine = tbGCodeInput.Text;
				GCodeClass tempResult = _gCodeInterpreterOperator.Interpreter(oneLine);//解释
				if (tempResult.IsError == false)
				{
					GCodeInterpreted.Clear();
					tbGCodeOpened.Text = "";
					GCodeLineList.Clear();//清理G代码
					GCodeInterpreted.Add(tempResult);//加入指令列表

					GCodeClass tempM30 = new GCodeClass();
					tempM30.MWorkState = MWorkState.M30;//加入程序结束
					GCodeInterpreted.Add(tempM30);//加入指令列表

					_appWorkState = AppWorkState.ReadGCode;
					btnPicStart_Click(sender, e);//开始操作
				}
			}
		}

		#endregion

		private void keyControlMul_DoubleClick(object sender, EventArgs e)
		{
			NumberInputForm<int> niform = new NumberInputForm<int>(false, false);//实例化数字输入窗体
			if (niform.ShowDialog() == DialogResult.OK)
			{
				_keyControlMul = niform.GetNumber;//获取用户输入
				keyControlMul.Text = "×" + _keyControlMul.ToString();//处理UI显示
			}
			niform.Dispose();//释放资源
		}

		private void wheelControlMul_DoubleClick(object sender, EventArgs e)
		{
			NumberInputForm<int> niform = new NumberInputForm<int>(false, false);
			if (niform.ShowDialog() == DialogResult.OK)
			{
				_wheelControlMul = niform.GetNumber;//获取用户输入
				wheelControlMul.Text = "×" + _wheelControlMul.ToString();//处理UI显示
			}
			niform.Dispose();//释放资源
		}

		#region 相对坐标双击设定
		/// <summary>
		/// 修改X相对坐标
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void coorXShow_DoubleClick(object sender, EventArgs e)
		{
			if (_isDisplayAbsolute == false)//正在显示相对坐标，可以修改
			{
				NumberInputForm<double> niform = new NumberInputForm<double>(true, true);
				if (niform.ShowDialog() == DialogResult.OK)//获取用户输入
				{
					_relativeCoordinate.X = niform.GetNumber;
					coorXShow.Text = _relativeCoordinate.X.ToString("0000.0000");//处理UI显示
				}
				niform.Dispose();

				_deltaCoordinate = _absoluteCoordinate - _relativeCoordinate;//绝对 - 相对
			}
		}

		/// <summary>
		/// 修改Y相对坐标
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void coorYShow_DoubleClick(object sender, EventArgs e)
		{
			if (_isDisplayAbsolute == false)//正在显示相对坐标，可以修改
			{
				NumberInputForm<double> niform = new NumberInputForm<double>(true, true);
				if (niform.ShowDialog() == DialogResult.OK)//获取用户输入
				{
					_relativeCoordinate.Y = niform.GetNumber;
					coorYShow.Text = _relativeCoordinate.Y.ToString("0000.0000");//处理UI显示
				}
				niform.Dispose();

				_deltaCoordinate = _absoluteCoordinate - _relativeCoordinate;//绝对 - 相对
			}
		}

		/// <summary>
		/// 修改Z相对坐标
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void coorZShow_DoubleClick(object sender, EventArgs e)
		{
			if (_isDisplayAbsolute == false)//正在显示相对坐标，可以修改
			{
				NumberInputForm<double> niform = new NumberInputForm<double>(true, true);
				if (niform.ShowDialog() == DialogResult.OK)//获取用户输入
				{
					_relativeCoordinate.Z = niform.GetNumber;
					coorZShow.Text = _relativeCoordinate.Z.ToString("0000.0000");//处理UI显示
				}
				niform.Dispose();

				_deltaCoordinate = _absoluteCoordinate - _relativeCoordinate;//绝对 - 相对
			}
		}

		#endregion

		/// <summary>
		/// 改变坐标UI显示
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnChangeCoordinate_Click(object sender, EventArgs e)
		{
			if (_isDisplayAbsolute == false)//正在显示相对坐标
			{
				_isDisplayAbsolute = true;
				btnChangeCoordinate.Text = "相对坐标";

				//处理UI显示
				coorXShow.Text = _absoluteCoordinate.X.ToString("0000.0000");
				coorYShow.Text = _absoluteCoordinate.Y.ToString("0000.0000");
				coorZShow.Text = _absoluteCoordinate.Z.ToString("0000.0000");
			}
			else
			{
				_isDisplayAbsolute = false;
				btnChangeCoordinate.Text = "绝对坐标";

				//处理UI显示
				coorXShow.Text = _relativeCoordinate.X.ToString("0000.0000");
				coorYShow.Text = _relativeCoordinate.Y.ToString("0000.0000");
				coorZShow.Text = _relativeCoordinate.Z.ToString("0000.0000");
			}
		}

		#region 键盘鼠标控制代码
		//键盘点动控制
		private void MainWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if(ckbIsKeyControl.Checked == true)
			{
				#region 数据包封包
				DataPackage tempPackage = new DataPackage();
				if (e.KeyCode == Keys.W)
				{
					tempPackage.DataX = 0;
					tempPackage.DataY = _keyControlMul * _gWorkStep;
					tempPackage.DataZ = 0;
					tempPackage.DataF = 0;
					tempPackage.Command = 0;
				}
				else if (e.KeyCode == Keys.S)
				{
					tempPackage.DataX = 0;
					tempPackage.DataY = -1 * _keyControlMul * _gWorkStep;
					tempPackage.DataZ = 0;
					tempPackage.DataF = 0;
					tempPackage.Command = 0;
				}
				else if (e.KeyCode == Keys.A)
				{
					tempPackage.DataX = -1 * _keyControlMul * _gWorkStep;
					tempPackage.DataY = 0;
					tempPackage.DataZ = 0;
					tempPackage.DataF = 0;
					tempPackage.Command = 0;
				}
				else if (e.KeyCode == Keys.D)
				{
					tempPackage.DataX = _keyControlMul * _gWorkStep;
					tempPackage.DataY = 0;
					tempPackage.DataZ = 0;
					tempPackage.DataF = 0;
					tempPackage.Command = 0;
				}
				else if (e.KeyCode == Keys.Q)
				{
					tempPackage.DataX = 0;
					tempPackage.DataY = 0;
					tempPackage.DataZ =_keyControlMul * _gWorkStep;
					tempPackage.DataF = 0;
					tempPackage.Command = 0;
				}
				else if (e.KeyCode == Keys.E)
				{
					tempPackage.DataX = 0;
					tempPackage.DataY = 0;
					tempPackage.DataZ = -1 * _keyControlMul * _gWorkStep;
					tempPackage.DataF = 0;
					tempPackage.Command = 0;
				}
				else
				{
					return;
				}
				#endregion
				_relativeCoordinate.X += tempPackage.DataX;
				_relativeCoordinate.Y += tempPackage.DataY;
				_relativeCoordinate.Z += tempPackage.DataZ;

				coorXShow.Invoke(actionLabelDelegate, _relativeCoordinate.X.ToString("0000.0000"), coorXShow);
				coorYShow.Invoke(actionLabelDelegate, _relativeCoordinate.Y.ToString("0000.0000"), coorYShow);
				coorZShow.Invoke(actionLabelDelegate, _relativeCoordinate.Z.ToString("0000.0000"), coorZShow);

				byte[] package = _globalEncoder.Encode(tempPackage);
				if (serialPort1.IsOpen == true)
				{
					serialPort1.Write(package, 0, package.Length);
				}
			}
		}

		//鼠标滚轮控制
		public void areaWheel_MouseWheel(object sender, MouseEventArgs e)
		{
			if (ckbIsWheelControl.Checked == true)
			{
				//当e.Delta > 0时鼠标滚轮是向上滚动，e.Delta< 0时鼠标滚轮向下滚动。 
				int temp = e.Delta / 120;
				int coordinate = 0;//1-x 2-y 3-z
				if (rbCoordinateX.Checked == true)
				{
					coordinate = 1;
				}
				else if (rbCoordinateY.Checked == true)
				{
					coordinate = 2;
				}
				else if (rbCoordinateZ.Checked == true)
				{
					coordinate = 3;
				}
				else
				{
					coordinate = 0;
				}

				DataPackage tempPackage = new DataPackage();
				//数据包封包
				switch (coordinate)
				{
					case 1: tempPackage.DataX = temp * _gWorkStep * _wheelControlMul; break;
					case 2: tempPackage.DataY = temp * _gWorkStep * _wheelControlMul; break;
					case 3: tempPackage.DataZ = temp * _gWorkStep * _wheelControlMul; break;
				}
				_relativeCoordinate.X += tempPackage.DataX;
				_relativeCoordinate.Y += tempPackage.DataY;
				_relativeCoordinate.Z += tempPackage.DataZ;

				coorXShow.Invoke(actionLabelDelegate, _relativeCoordinate.X.ToString("0000.0000"), coorXShow);
				coorYShow.Invoke(actionLabelDelegate, _relativeCoordinate.Y.ToString("0000.0000"), coorYShow);
				coorZShow.Invoke(actionLabelDelegate, _relativeCoordinate.Z.ToString("0000.0000"), coorZShow);

				//发包
				if (serialPort1.IsOpen == true)
				{
					byte[] tempByte = _globalEncoder.Encode(tempPackage);
					serialPort1.Write(tempByte, 0, tempByte.Length);
				}
			}
		}
		#endregion

		#region 主按钮UI控制
		private void btnPicEStop_MouseEnter(object sender, EventArgs e)
		{
			btnPicEStop.Image = ResLibClass.GetButtonImageEStop("glow");
		}

		private void btnPicEStop_MouseLeave(object sender, EventArgs e)
		{
			btnPicEStop.Image = ResLibClass.GetButtonImageEStop("normal");
		}

		private void btnPicStart_MouseEnter(object sender, EventArgs e)
		{
			btnPicStart.Image = ResLibClass.GetButtonImageStart("glow");
		}

		private void btnPicStart_MouseLeave(object sender, EventArgs e)
		{
			btnPicStart.Image = ResLibClass.GetButtonImageStart("normal");
		}

		private void btnPicStop_MouseEnter(object sender, EventArgs e)
		{
			btnPicStop.Image = ResLibClass.GetButtonImageEnd("glow");
		}

		private void btnPicStop_MouseLeave(object sender, EventArgs e)
		{
			btnPicStop.Image = ResLibClass.GetButtonImageEnd("normal");
		}

		private void btnPicPause_MouseEnter(object sender, EventArgs e)
		{
			btnPicPause.Image = ResLibClass.GetButtonImagePause("glow");
		}

		private void btnPicPause_MouseLeave(object sender, EventArgs e)
		{
			btnPicPause.Image = ResLibClass.GetButtonImagePause("normal");
		}
		#endregion

		#region 子线程
		//=============
		//操作下位机线程
		//=============
		//定义跨线程访问的委托
		Action<string, TextBox> actionTBDelegate = (x, control) => { control.Text = x; };//跨线程操作textbox的text属性
		Action<string, Label> actionLabelDelegate = (x, control) => { control.Text = x; };//跨线程访问label的text属性
		Action<SerialPort, byte[]> actionSPWrite = (sp, package) => { sp.Write(package, 0, package.Length); };//跨线程操作串口写
		Action<string, Button> actionButtonDelegate = (x, button) => { button.Text = x; };
		Func<SerialPort, bool> funcSPIsOpen = (sp) => { return sp.IsOpen; };//跨线程访问serialport是否打开

		private void ControlTableThreading()
		{
			if (GCodeInterpreted.Count == 0 ||
				(_appWorkState != AppWorkState.RunningGCode))
			{
				_appWorkState = AppWorkState.Stop;
#if DEBUG
				btnToZero.Invoke(actionButtonDelegate, "手动退出", btnToZero);
#endif
				return;
			}
			else
			{
				foreach (GCodeClass oneLine in GCodeInterpreted)//取每一个G代码编译的指令
				{
					if (oneLine.MWorkState == MWorkState.M30)//程序结束
					{
						_appWorkState = AppWorkState.Stop;
#if DEBUG
						btnToZero.Invoke(actionButtonDelegate, "M30退出", btnToZero);
#endif
						return;
					}
					else if (oneLine.MWorkState == MWorkState.M1)//程序暂停
					{
						//挂起自己
						_appWorkState = AppWorkState.Pause;
						_isControlThreadSuspended = true;
						//_askMonitorTick.Set();
						_controlThreadSuspendMonitor.WaitOne();
						continue;
					}
					if (oneLine.GWorkState == GWorkState.NULL)//如果G指令为空，则取模态G指令
					{
						oneLine.GWorkState = _GModal;
					}
					else
					{
						_GModal = oneLine.GWorkState;//非空那么就更新模态G指令
						msgModelValue.Invoke(actionLabelDelegate, "模态量：" + _GModal.ToString(), msgModelValue);
					}

					if (oneLine.GWorkState == GWorkState.G00)//G00指令
					{
						//快速定位，获取坐标
						Point3D targetRelative = new Point3D();
						//复制当前坐标
						targetRelative.X = _relativeCoordinate.X;
						targetRelative.Y = _relativeCoordinate.Y;
						targetRelative.Z = _relativeCoordinate.Z;

						//读取G代码
						int coorCount = 0;
						foreach (CoordinateState c in oneLine.Coordinate)
						{
							if (c == CoordinateState.X)
							{
								targetRelative.X = oneLine.Value[coorCount];
							}
							else if (c == CoordinateState.Y)
							{
								targetRelative.Y = oneLine.Value[coorCount];
							}
							else if (c == CoordinateState.Z)
							{
								targetRelative.Z = oneLine.Value[coorCount];
							}
							coorCount++;
						}
						DataPackage tempPackage = new DataPackage();
						tempPackage.Command = CommandClass._SET_POSITION_;
						tempPackage.DataX = (targetRelative - _relativeCoordinate).X;//传送X偏移坐标
						tempPackage.DataY = (targetRelative - _relativeCoordinate).Y;//传送Y偏移坐标
						tempPackage.DataZ = (targetRelative - _relativeCoordinate).Z;//传送Z偏移坐标
						tempPackage.DataF = 0;
						byte[] packageSend = _globalEncoder.Encode(tempPackage);
						if (serialPort1.IsOpen == true)
						{
							serialPort1.Write(packageSend, 0, packageSend.Length);//发数据包
						}

						_relativeCoordinate = targetRelative;//更新相对坐标

						//更新UI
						coorXShow.Invoke(actionLabelDelegate, _relativeCoordinate.X.ToString("0000.0000"), coorXShow);
						coorYShow.Invoke(actionLabelDelegate, _relativeCoordinate.Y.ToString("0000.0000"), coorYShow);
						coorZShow.Invoke(actionLabelDelegate, _relativeCoordinate.Z.ToString("0000.0000"), coorZShow);

						_autoEventAction.WaitOne();//挂起等待收到完成
						//_askMonitorTick.Set();
						_controlThreadSuspendMonitor.WaitOne();//挂起或未挂起，由监视线程处理是否挂起
					}
					else if (oneLine.GWorkState == GWorkState.G01)//G01指令
					{
						//直线插补，获取坐标
						Point3D targetRelative = new Point3D();
						//复制当前坐标
						targetRelative.X = _relativeCoordinate.X;
						targetRelative.Y = _relativeCoordinate.Y;
						targetRelative.Z = _relativeCoordinate.Z;

						int targetF = _FModal;//保存当前速度

						//读取G代码
						int coorCount = 0;
						foreach (CoordinateState c in oneLine.Coordinate)
						{
							if (c == CoordinateState.X)
							{
								targetRelative.X = oneLine.Value[coorCount];
							}
							else if (c == CoordinateState.Y)
							{
								targetRelative.Y = oneLine.Value[coorCount];
							}
							else if (c == CoordinateState.Z)
							{
								targetRelative.Z = oneLine.Value[coorCount];
							}
							else if (c == CoordinateState.F)
							{
								targetF = (int)oneLine.Value[coorCount];
							}
							coorCount++;
						}

						//设置模态量
						_FModal = targetF;

						//执行插补计算
						List<StepMark> tempMark = _gCodeInterOperator.LineInterpolation(_relativeCoordinate, targetRelative);
						//封包执行
						Point3D distanceHaveDone = new Point3D();
						foreach (StepMark SM in tempMark)
						{
							DataPackage tempPackage = new DataPackage();

							if (SM.X == StepState.OneStepPositive)//x正向步进
							{
								tempPackage.DataF = targetF;
								tempPackage.DataX = _gWorkStep;
								tempPackage.DataY = 0;
								tempPackage.DataZ = 0;
								tempPackage.Command = 0;

								distanceHaveDone.X += _gWorkStep;

								//_relativeCoordinate.X += _gWorkStep;
							}
							else if (SM.X == StepState.OneStepNegative)//x负向步进
							{
								tempPackage.DataF = targetF;
								tempPackage.DataX = -1 * _gWorkStep;
								tempPackage.DataY = 0;
								tempPackage.DataZ = 0;
								tempPackage.Command = 0;

								distanceHaveDone.X -= _gWorkStep;

								//_relativeCoordinate.X -= _gWorkStep;
							}
							else if (SM.Y == StepState.OneStepPositive)//y正向步进
							{
								tempPackage.DataF = targetF;
								tempPackage.DataX = 0;
								tempPackage.DataY = _gWorkStep;
								tempPackage.DataZ = 0;
								tempPackage.Command = 0;

								distanceHaveDone.Y += _gWorkStep;

								//_relativeCoordinate.Y += _gWorkStep;
							}
							else if (SM.Y == StepState.OneStepNegative)//y负向步进
							{
								tempPackage.DataF = targetF;
								tempPackage.DataX = 0;
								tempPackage.DataY = -1 * _gWorkStep;
								tempPackage.DataZ = 0;
								tempPackage.Command = 0;

								distanceHaveDone.Y -= _gWorkStep;

								//_relativeCoordinate.Y -= _gWorkStep;
							}
							else if (SM.X == StepState.ToEnd)//x到头
							{
								tempPackage.DataF = targetF;
								tempPackage.DataX = (targetRelative - _relativeCoordinate - distanceHaveDone).X;
								tempPackage.DataY = 0;
								tempPackage.DataZ = 0;
								tempPackage.Command = 0;

								distanceHaveDone.X = (targetRelative - _relativeCoordinate).X;
							}
							else if (SM.Y == StepState.ToEnd)//y到头
							{
								tempPackage.DataF = targetF;
								tempPackage.DataX = 0;
								tempPackage.DataY = (targetRelative - _relativeCoordinate - distanceHaveDone).Y;
								tempPackage.DataZ = 0;
								tempPackage.Command = 0;

								distanceHaveDone.Y = (targetRelative - _relativeCoordinate).Y;
							}
							else if (SM.Z == StepState.ToEnd)//Z到头
							{
								tempPackage.DataF = targetF;
								tempPackage.DataX = 0;
								tempPackage.DataY = 0;
								tempPackage.DataZ = (targetRelative - _relativeCoordinate - distanceHaveDone).Z;
								tempPackage.Command = 0;

								distanceHaveDone.Z = (targetRelative - _relativeCoordinate).Z;
							}

							//编码
							byte[] packageSend = _globalEncoder.Encode(tempPackage);
							if (serialPort1.IsOpen == true)
							{
								serialPort1.Write(packageSend, 0, packageSend.Length);//发数据包
							}

							_autoEventAction.WaitOne();//挂起等待收到完成
							//_askMonitorTick.Set();
							_controlThreadSuspendMonitor.WaitOne();//挂起或未挂起，由监视线程处理是否挂起
						}

						_relativeCoordinate = targetRelative;//更新相对坐标

						//更新UI
						coorXShow.Invoke(actionLabelDelegate, _relativeCoordinate.X.ToString("0000.0000"), coorXShow);
						coorYShow.Invoke(actionLabelDelegate, _relativeCoordinate.Y.ToString("0000.0000"), coorYShow);
						coorZShow.Invoke(actionLabelDelegate, _relativeCoordinate.Z.ToString("0000.0000"), coorZShow);
					}
					else if (oneLine.GWorkState == GWorkState.G02 || oneLine.GWorkState == GWorkState.G03)//G02/G03
					{
						//G02 顺时针旋转
						//G03 逆时针旋转
						int direction = 0;
						if (oneLine.GWorkState == GWorkState.G02)//顺时针 1-顺 0-逆
						{
							direction = 1;
						}
						else//逆时针
						{
							direction = 0;
						}
						//圆弧插补，获取坐标
						Point3D targetRelative = new Point3D();
						//复制当前坐标
						targetRelative.X = _relativeCoordinate.X;
						targetRelative.Y = _relativeCoordinate.Y;
						targetRelative.Z = _relativeCoordinate.Z;

						int targetF = _FModal;//保存当前速度
						double targetI = _IModal;//保存当前I
						double targetJ = _JModal;//保存当前J

						//读取G代码
						int coorCount = 0;
						foreach (CoordinateState c in oneLine.Coordinate)
						{
							if (c == CoordinateState.X)
							{
								targetRelative.X = oneLine.Value[coorCount];
							}
							else if (c == CoordinateState.Y)
							{
								targetRelative.Y = oneLine.Value[coorCount];
							}
							else if (c == CoordinateState.Z)
							{
								targetRelative.Z = oneLine.Value[coorCount];
							}
							else if (c == CoordinateState.F)
							{
								targetF = (int)oneLine.Value[coorCount];
							}
							else if (c == CoordinateState.I)
							{
								targetI = (int)oneLine.Value[coorCount];
							}
							else if (c == CoordinateState.J)
							{
								targetJ = (int)oneLine.Value[coorCount];
							}
							else if (c == CoordinateState.K)
							{
								//没有三维圆弧插补，忽略
							}
							coorCount++;
						}

						//设置模态量
						_FModal = targetF;
						_IModal = targetI;
						_JModal = targetJ;

						//计算圆心
						Point3D tempCenterRelative = new Point3D();
						if (_isG23Absolute == true)//IJ 为绝对圆心
						{
							tempCenterRelative.X = targetI;
							tempCenterRelative.Y = targetJ;
						}
						else//IJ为相对圆心
						{
							tempCenterRelative.X = _relativeCoordinate.X + targetI;
							tempCenterRelative.Y = _relativeCoordinate.Y + targetJ;
						}

						//执行插补计算
						List<StepMark> tempMark = _gCodeInterOperator.CircleInterpolation(tempCenterRelative, _relativeCoordinate, targetRelative, direction);
						//封包执行
						Point3D distanceHaveDone = new Point3D();
						foreach (StepMark SM in tempMark)
						{
							DataPackage tempPackage = new DataPackage();

							if (SM.X == StepState.OneStepPositive)//x正向步进
							{
								tempPackage.DataF = targetF;
								tempPackage.DataX = _gWorkStep;
								tempPackage.DataY = 0;
								tempPackage.DataZ = 0;
								tempPackage.Command = 0;

								distanceHaveDone.X += _gWorkStep;

								//_relativeCoordinate.X += _gWorkStep;
							}
							else if (SM.X == StepState.OneStepNegative)//x负向步进
							{
								tempPackage.DataF = targetF;
								tempPackage.DataX = -1 * _gWorkStep;
								tempPackage.DataY = 0;
								tempPackage.DataZ = 0;
								tempPackage.Command = 0;

								distanceHaveDone.X -= _gWorkStep;

								//_relativeCoordinate.X -= _gWorkStep;
							}
							else if (SM.Y == StepState.OneStepPositive)//y正向步进
							{
								tempPackage.DataF = targetF;
								tempPackage.DataX = 0;
								tempPackage.DataY = _gWorkStep;
								tempPackage.DataZ = 0;
								tempPackage.Command = 0;

								distanceHaveDone.Y += _gWorkStep;

								//_relativeCoordinate.Y += _gWorkStep;
							}
							else if (SM.Y == StepState.OneStepNegative)//y负向步进
							{
								tempPackage.DataF = targetF;
								tempPackage.DataX = 0;
								tempPackage.DataY = -1 * _gWorkStep;
								tempPackage.DataZ = 0;
								tempPackage.Command = 0;

								distanceHaveDone.Y -= _gWorkStep;

								//_relativeCoordinate.Y -= _gWorkStep;
							}
							else if (SM.X == StepState.ToEnd)//x到头
							{
								tempPackage.DataF = targetF;
								tempPackage.DataX = (targetRelative - _relativeCoordinate - distanceHaveDone).X;
								tempPackage.DataY = 0;
								tempPackage.DataZ = 0;
								tempPackage.Command = 0;

								distanceHaveDone.X = (targetRelative - _relativeCoordinate).X;
							}
							else if (SM.Y == StepState.ToEnd)//y到头
							{
								tempPackage.DataF = targetF;
								tempPackage.DataX = 0;
								tempPackage.DataY = (targetRelative - _relativeCoordinate - distanceHaveDone).Y;
								tempPackage.DataZ = 0;
								tempPackage.Command = 0;

								distanceHaveDone.Y = (targetRelative - _relativeCoordinate).Y;
							}
							else if (SM.Z == StepState.ToEnd)//Z到头
							{
								tempPackage.DataF = targetF;
								tempPackage.DataX = 0;
								tempPackage.DataY = 0;
								tempPackage.DataZ = (targetRelative - _relativeCoordinate - distanceHaveDone).Z;
								tempPackage.Command = 0;

								distanceHaveDone.Z = (targetRelative - _relativeCoordinate).Z;
							}

							//编码
							byte[] packageSend = _globalEncoder.Encode(tempPackage);
							if (serialPort1.IsOpen == true)
							{
								serialPort1.Write(packageSend, 0, packageSend.Length);//发数据包
							}

							_autoEventAction.WaitOne();//挂起等待收到完成
							//_askMonitorTick.Set();
							_controlThreadSuspendMonitor.WaitOne();//挂起或未挂起，由监视线程处理是否挂起
						}
						//更新相对坐标
						_relativeCoordinate = targetRelative;

						//更新UI显示
						coorXShow.Invoke(actionLabelDelegate, _relativeCoordinate.X.ToString("0000.0000"), coorXShow);
						coorYShow.Invoke(actionLabelDelegate, _relativeCoordinate.Y.ToString("0000.0000"), coorYShow);
						coorZShow.Invoke(actionLabelDelegate, _relativeCoordinate.Z.ToString("0000.0000"), coorZShow);
					}
				}
			}
#if DEBUG
			btnToZero.Invoke(actionButtonDelegate, "foreach退出", btnToZero);
#endif
		}

		/// <summary>
		/// 监视Control线程是否被挂起
		/// </summary>
		private void MonitorOfControlThread()
		{
			while (true)
			{
				if (_controlTableThread.ThreadState == ThreadState.Stopped)
				{
					return;
				}

				if (_isControlThreadSuspended == false)//如果没有被挂起
				{
					_controlThreadSuspendMonitor.Set();//发送继续信号
				}
			}
		}


		List<byte> _ReceivedByteList = new List<byte>();
		/// <summary>
		/// 串口接收响应
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
#if DEBUG
			countSerialPortReceived++;
			Action<string> actionWindowText = (x) => { Text = "上位机" + x; };
			this.Invoke(actionWindowText, countSerialPortReceived.ToString());
#endif
			byte[] receivedByte = new byte[serialPort1.BytesToRead];
			serialPort1.Read(receivedByte,0, receivedByte.Length);
			_ReceivedByteList.AddRange(new List<byte>(receivedByte));

			serialPort1.DiscardInBuffer();
		}

		/// <summary>
		/// 串口接收处理线程
		/// </summary>
		private void DataProssing()
		{
			while (true)
			{
#if DEBUG
				//msg_ShowThread.Invoke(actionLabelDelegate, "", msg_ShowThread);
				if (_controlTableThread != null)
				{
					msg_ShowThread.Invoke(actionLabelDelegate, "控制工作台：" + _controlTableThread.ThreadState.ToString(), msg_ShowThread);
				}
				if (_monitorControlThread != null)
				{
					msg_ShowThread.Invoke(actionLabelDelegate, msg_ShowThread.Text + "  监视：" + _monitorControlThread.ThreadState.ToString(), msg_ShowThread);
				}
#endif
				if (_ReceivedByteList.Count >= 4)
				{
					int head = 0;
					int end = 0;
					bool ifCanRead = false;

					for (int i = 0; i < _ReceivedByteList.Count - 1; i++)
					{
						if (_ReceivedByteList[i] == 0xAA)
						{
							head = i;
						}
						if (_ReceivedByteList[i] == 0xA5)
						{
							if (_ReceivedByteList[i + 1] == 0x5A)
							{
								end = i + 1;
								ifCanRead = true;
								break;
							}
						}
					}


					if (ifCanRead == true)
					{
						List<byte> tempByteNode = new List<byte>();
						tempByteNode.AddRange(_ReceivedByteList.GetRange(head, end - head + 1));
						_receivingDataQueue.Enqueue(tempByteNode);

						_ReceivedByteList.RemoveRange(0, end + 1);
					}
				}

				if (_receivingDataQueue.Count >= 1)//如果队列有东西
				{
					_receivedPackage = _receivingDataQueue.Dequeue();//出队

					string tempString = "";

					foreach (byte b in _receivedPackage)
					{
						tempString += (b.ToString("X2") + "  ");
					}

					tbPre.Invoke(actionTBDelegate, tempString, tbPre);

					if (_receivedPackage.Count >= 4)//如果大于19个字节
					{
						if (_receivedPackage[3] == 0x5A &&
							_receivedPackage[2] == 0xA5 &&
							_receivedPackage[0] == 0xAA)//头尾满足要求
						{
							DataPackage tempDataPackage = new DataPackage();
							tempDataPackage = _globalDecoder.Decode(_receivedPackage.ToArray());//解码
							if (tempDataPackage != null)//表示没有传错
							{
								////获得绝对坐标
								//_absoluteCoordinate.X = tempDataPackage.DataX;
								//_absoluteCoordinate.Y = tempDataPackage.DataY;
								//_absoluteCoordinate.Z = tempDataPackage.DataZ;
								////计算相对坐标
								//_relativeCoordinate = _absoluteCoordinate - _deltaCoordinate;//绝对 - 差值 = 相对

								////UI显示
								//if (_isDisplayAbsolute == true)//如果是绝对显示
								//{
								//	coorXShow.Invoke(actionLabelDelegate, _absoluteCoordinate.X.ToString("0000.0000"), coorXShow);
								//	coorYShow.Invoke(actionLabelDelegate, _absoluteCoordinate.Y.ToString("0000.0000"), coorYShow);
								//	coorZShow.Invoke(actionLabelDelegate, _absoluteCoordinate.Z.ToString("0000.0000"), coorZShow);
								//}
								//else
								//{
								//	coorXShow.Invoke(actionLabelDelegate, _relativeCoordinate.X.ToString("0000.0000"), coorXShow);
								//	coorYShow.Invoke(actionLabelDelegate, _relativeCoordinate.Y.ToString("0000.0000"), coorYShow);
								//	coorZShow.Invoke(actionLabelDelegate, _relativeCoordinate.Z.ToString("0000.0000"), coorZShow);
								//}

								//解读附加
								if (tempDataPackage.Command == CommandClass._FINISHED_)//下位机运行完了上一个插补
								{
									if (_appWorkState == AppWorkState.RunningGCode)
									{
										_receivingDataQueue.Clear();
										_autoEventAction.Set();//通知控制线程可以进行下一步
															   //_isTableFinished = true;
									}
								}
							}
						}
					}
					_receivedPackage.Clear();
				}
			}
		}

		#endregion

		/// <summary>
		/// 插件数据传入响应函数
		/// </summary>
		/// <param name="inputPtL">3d点坐标列</param>
		private void PlugInDataChanged(List<List<Point3D>> inputPtL)
		{
			if (_appWorkState != AppWorkState.Stop && _appWorkState != AppWorkState.ReadGCode)//不等于stop且不等于已经都好了G代码
			{
				//说明程序忙
				MessageBox.Show("程序正忙，数据传入失败");
				return;
			}
			else
			{
				//数据正常传入
				GCodeInterpreted.Clear();

				GCodeClass tempGCode = new GCodeClass();
				tempGCode.IsError = false;
				tempGCode.MWorkState = MWorkState.NULL;
				tempGCode.GWorkState = GWorkState.G00;
				tempGCode.SetCoordinate(CoordinateState.Z, 5);
				tempGCode.SetCoordinate(CoordinateState.X, 0);
				tempGCode.SetCoordinate(CoordinateState.Y, 0);//抬到z5X0Y0处

				GCodeInterpreted.Add(tempGCode);

				for (int i = 0; i < inputPtL.Count; i++)
				{
					tempGCode = new GCodeClass();
					GC.Collect();
					tempGCode.GWorkState = GWorkState.G00;
					tempGCode.SetCoordinate(CoordinateState.Z, 10);//抬到Z10

					GCodeInterpreted.Add(tempGCode);
					tempGCode = new GCodeClass();
					GC.Collect();

					tempGCode.SetCoordinate(CoordinateState.X, inputPtL[i][0].X);
					tempGCode.SetCoordinate(CoordinateState.Y, inputPtL[i][0].Y);//移动到第一个点上方

					GCodeInterpreted.Add(tempGCode);
					tempGCode = new GCodeClass();
					GC.Collect();

					tempGCode.GWorkState = GWorkState.G01;
					tempGCode.SetCoordinate(CoordinateState.Z, 0);
					tempGCode.SetCoordinate(CoordinateState.F, 100);//下到Z0

					GCodeInterpreted.Add(tempGCode);
					tempGCode = new GCodeClass();
					GC.Collect();

					for (int j = 1; j < inputPtL[i].Count; j++)
					{
						tempGCode.SetCoordinate(CoordinateState.X, inputPtL[i][j].X);
						tempGCode.SetCoordinate(CoordinateState.Y, inputPtL[i][j].Y);

						GCodeInterpreted.Add(tempGCode);
						tempGCode = new GCodeClass();
						GC.Collect();
					}
				}

				tempGCode = new GCodeClass();
				GC.Collect();
				tempGCode.GWorkState = GWorkState.G00;
				tempGCode.SetCoordinate(CoordinateState.Z, 10);//抬到Z10

				GCodeInterpreted.Add(tempGCode);

				tempGCode = new GCodeClass();
				GC.Collect();
				tempGCode.MWorkState = MWorkState.M30;//程序结束

				GCodeInterpreted.Add(tempGCode);

				//至此G代码已经成功传入
				_appWorkState = AppWorkState.ReadGCode;
				tbGCodeOpened.Text = "";
				GCodeLineList.Clear();
				//写入DT
				ChangeGCodeInterpertedToDT();
			}
		}
		
		/// <summary>
		/// 将解释完毕的G代码写入DT中
		/// </summary>
		private void ChangeGCodeInterpertedToDT()
		{
			_mainWindowDT.OperationList.Clear();

			if (_appWorkState == AppWorkState.ReadGCode)//说明G代码顺利读取
			{
				Point3D nowPosition = new Point3D();//当前位置
				Point3D centerVector = new Point3D();//圆心向量
				GWorkState modalG = new GWorkState();
				modalG = GWorkState.NULL;

				foreach (GCodeClass GCC in GCodeInterpreted)
				{
					OperationListNode tempNode = new OperationListNode();

					//写入节点G指令
					if (GCC.GWorkState == GWorkState.G00)
					{
						tempNode.Command = GWorkState.G00;
					}
					else if (GCC.GWorkState == GWorkState.G01)
					{
						tempNode.Command = GWorkState.G01;
					}
					else if (GCC.GWorkState == GWorkState.G02)
					{
						tempNode.Command = GWorkState.G02;
					}
					else if (GCC.GWorkState == GWorkState.G03)
					{
						tempNode.Command = GWorkState.G03;
					}
					else if (GCC.GWorkState == GWorkState.Others)
					{
						tempNode.Command = modalG;
					}
					else if (GCC.GWorkState == GWorkState.NULL)
					{
						tempNode.Command = modalG;
					}
					modalG = tempNode.Command;

					tempNode.Start.X = nowPosition.X;//写入起始点
					tempNode.Start.Y = nowPosition.Y;
					tempNode.Start.Z = nowPosition.Z;

					//读取其他坐标
					int count = 0;
					foreach (CoordinateState coor in GCC.Coordinate)
					{
						if (coor == CoordinateState.X)
						{
							nowPosition.X = GCC.Value[count];
						}
						else if (coor == CoordinateState.Y)
						{
							nowPosition.Y = GCC.Value[count];
						}
						else if (coor == CoordinateState.Z)
						{
							nowPosition.Z = GCC.Value[count];
						}
						else if (coor == CoordinateState.I)
						{
							centerVector.X = GCC.Value[count];
						}
						else if (coor == CoordinateState.J)
						{
							centerVector.Y = GCC.Value[count];
						}
						else if (coor == CoordinateState.K)
						{
							centerVector.Z = GCC.Value[count];
						}
						count++;
					}
					//写入中点
					tempNode.End.X = nowPosition.X;
					tempNode.End.Y = nowPosition.Y;
					tempNode.End.Z = nowPosition.Z;

					if (tempNode.Command == GWorkState.G02 || tempNode.Command == GWorkState.G03)
					{
						Point3D tempCenter = new Point3D();
						//写入圆心
						if (_isG23Absolute == true)//如果G23为绝对
						{
							tempCenter.X = centerVector.X;
							tempCenter.Y = centerVector.Y;
							tempCenter.Z = centerVector.Z;
						}
						else//G23为相对
						{
							tempCenter.X = tempNode.Start.X + centerVector.X;
							tempCenter.Y = tempNode.Start.Y + centerVector.Y;
							tempCenter.Z = tempNode.Start.Z + centerVector.Z;
						}
						//写入圆心
						tempNode.Center = tempCenter;
					}

					_mainWindowDT.OperationList.Add(tempNode);
				}
			}
		}

		/// <summary>
		/// 主窗体关闭之前结束子线程
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			_receivingProcess.Abort();

			if (_controlTableThread != null)
			{
				if (_controlTableThread.ThreadState == ThreadState.Running || _controlTableThread.ThreadState == ThreadState.Suspended)
				{
					_controlTableThread.Abort();
				}
			}

			if (_monitorControlThread != null)
			{
				if (_monitorControlThread.ThreadState == ThreadState.Running || _monitorControlThread.ThreadState == ThreadState.Suspended)
				{
					_monitorControlThread.Abort();
				}
			}
		}

		private void btnDebugSet_Click(object sender, EventArgs e)
		{
			_autoEventAction.Set();
		}
	}
}
