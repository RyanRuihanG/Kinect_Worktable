using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace ThreeCoordinateMainWindow
{
	public partial class SerialSet : Form
	{
		private SerialPort _mySP;//引用外部串口
		public SerialSet(ref SerialPort inputSP)
		{
			_mySP = inputSP;
			InitializeComponent();
		}

		private void SerialSet_Load(object sender, EventArgs e)//窗体初始化
		{
			if (_mySP.IsOpen == true)//如果串口已经打开了
			{
				cbSerial.Items.Clear();
				cbSerial.Items.Add(_mySP.PortName);
				cbSerial.SelectedIndex = 0;//设置串口号

				cbBaudRate.SelectedItem = _mySP.BaudRate;//设定波特率

				switch (_mySP.StopBits)//设定停止位
				{
					//case StopBits.None:cbStopBits.SelectedIndex = 0;break;
					case StopBits.One:cbStopBits.SelectedIndex = 0;break;
					case StopBits.OnePointFive:cbStopBits.SelectedIndex = 1;break;
					case StopBits.Two:cbStopBits.SelectedIndex = 2;break;
				}
				switch (_mySP.DataBits)//设定数据位
				{
					case 8:cbDataBits.SelectedIndex = 0;break;
					case 7:cbDataBits.SelectedIndex = 1;break;
					case 6:cbDataBits.SelectedIndex = 2;break;
					case 5:cbDataBits.SelectedIndex = 3;break;
				}
				switch (_mySP.Parity)//设定校验位
				{
					case Parity.None:cbParity.SelectedIndex = 0;break;
					case Parity.Odd:cbParity.SelectedIndex = 1;break;//奇校验
					case Parity.Even:cbParity.SelectedIndex = 2;break;//偶校验
				}
				//禁用一切控件
				btnOK.Enabled = false;
				cbSerial.Enabled = false;
				btnFind.Enabled = false;
				cbBaudRate.Enabled = false;
				cbStopBits.Enabled = false;
				cbDataBits.Enabled = false;
				cbParity.Enabled = false;
			}
			else
			{
				//btnFind_Click(sender, e);

				cbBaudRate.SelectedIndex = 13;//设定波特率
				switch (_mySP.StopBits)//设定停止位
				{
					//case StopBits.None:cbStopBits.SelectedIndex = 0;break;
					case StopBits.One: cbStopBits.SelectedIndex = 0; break;
					case StopBits.OnePointFive: cbStopBits.SelectedIndex = 1; break;
					case StopBits.Two: cbStopBits.SelectedIndex = 2; break;
				}
				switch (_mySP.DataBits)//设定数据位
				{
					case 8: cbDataBits.SelectedIndex = 0; break;
					case 7: cbDataBits.SelectedIndex = 1; break;
					case 6: cbDataBits.SelectedIndex = 2; break;
					case 5: cbDataBits.SelectedIndex = 3; break;
				}
				switch (_mySP.Parity)//设定校验位
				{
					case Parity.None: cbParity.SelectedIndex = 0; break;
					case Parity.Odd: cbParity.SelectedIndex = 1; break;//奇校验
					case Parity.Even: cbParity.SelectedIndex = 2; break;//偶校验
				}
			}
		}

		private void btnFind_Click(object sender, EventArgs e)
		{
			cbSerial.Items.Clear();
			string[] str = SerialPort.GetPortNames();
			if (str.Length == 0)
			{
				MessageBox.Show("没有发现串口", "Error");
				return;
			}

			foreach (string s in str)
			{
				cbSerial.Items.Add(s);
			}

			cbSerial.SelectedIndex = 0;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (_mySP.IsOpen == true)
			{
				btnOK.Enabled = false;
				MessageBox.Show("请关闭串口再进行操作。", "Error");
			}
			else
			{
				if (cbSerial.SelectedItem != null)
				{
					//获取相关设置
					int baudRate = Convert.ToInt32(cbBaudRate.SelectedItem);
					int dataBits = Convert.ToInt32(cbDataBits.SelectedItem);
					if (cbParity.SelectedIndex == 0)//无校验
					{
						_mySP.Parity = Parity.None;
					}
					else if (cbParity.SelectedIndex == 1)//奇校验
					{
						_mySP.Parity = Parity.Odd;
					}
					else if (cbParity.SelectedIndex == 2)//偶校验
					{
						_mySP.Parity = Parity.Even;
					}

					if (cbStopBits.SelectedIndex == 0)//1
					{
						_mySP.StopBits = StopBits.One;
					}
					else if (cbStopBits.SelectedIndex == 1)//1.5
					{
						_mySP.StopBits = StopBits.OnePointFive;
					}
					else if (cbStopBits.SelectedIndex == 2)//2
					{
						_mySP.StopBits = StopBits.Two;
					}

					_mySP.BaudRate = baudRate;
					_mySP.DataBits = dataBits;
					_mySP.PortName = cbSerial.SelectedItem.ToString();

					this.Close();
				}
				else
				{
					MessageBox.Show("请选择串口");
				}
			}
		}
	}
}
