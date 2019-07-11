using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Windows.Media.Animation;

namespace SerialControlNS
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class SerialControl : UserControl
    {
        public delegate void QueueAddedHandler(object sender, EventArgs e);
        public event QueueAddedHandler QueueAdded;

        /// <summary>
        /// 串口
        /// </summary>
        private SerialPort _myPort = new SerialPort();

        private List<byte> bytesReceived = new List<byte>();
        private Queue<List<byte>> dataQueue = new Queue<List<byte>>();

        public SerialControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 控件初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _myPort.ReceivedBytesThreshold = 1;
            _myPort.DataReceived += SerialPort_DataReceived;
        }

        /// <summary>
        /// 获取控件内串口
        /// </summary>
        public SerialPort MyPort { get { return _myPort; } }

        /// <summary>
        /// 获取消息队列
        /// </summary>
        public Queue<List<byte>> DataQueue { get { return dataQueue; } }

        /// <summary>
        /// 查找串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// 串口开关
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSwitch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_myPort.IsOpen == true)//如果已经打开
            {
                _myPort.Close();
                cbBaudRate.IsEnabled = true;
                cbDataBits.IsEnabled = true;
                cbParity.IsEnabled = true;
                cbStopBits.IsEnabled = true;
                cbSerial.IsEnabled = true;
                btnFind.IsEnabled = true;

                
                //播放动画
                BeginStoryboard((Storyboard)FindResource("AniSwitchOff"));

            }
            else
            {
                if (_myPort.PortName == "")
                {
                    MessageBox.Show("串口设置不正确");
                }
                else
                {

                    try
                    {
                        if (cbSerial.SelectedItem != null)
                        {
                            //获取相关设置
                            _myPort.PortName = cbSerial.SelectedItem.ToString();

                            int baudRate = Convert.ToInt32(((ListBoxItem)(cbBaudRate.SelectedItem)).Content.ToString());
                            int dataBits = Convert.ToInt32(((ListBoxItem)(cbDataBits.SelectedItem)).Content.ToString());

                            if (cbParity.SelectedIndex == 0)//无校验
                            {
                                _myPort.Parity = Parity.None;
                            }
                            else if (cbParity.SelectedIndex == 1)//奇校验
                            {
                                _myPort.Parity = Parity.Odd;
                            }
                            else if (cbParity.SelectedIndex == 2)//偶校验
                            {
                                _myPort.Parity = Parity.Even;
                            }

                            if (cbStopBits.SelectedIndex == 0)//1
                            {
                                _myPort.StopBits = StopBits.One;
                            }
                            else if (cbStopBits.SelectedIndex == 1)//1.5
                            {
                                _myPort.StopBits = StopBits.OnePointFive;
                            }
                            else if (cbStopBits.SelectedIndex == 2)//2
                            {
                                _myPort.StopBits = StopBits.Two;
                            }

                            _myPort.BaudRate = baudRate;
                            _myPort.DataBits = dataBits;

                            _myPort.Open();//尝试打开串口


                            cbBaudRate.IsEnabled = false;
                            cbDataBits.IsEnabled = false;
                            cbParity.IsEnabled = false;
                            cbStopBits.IsEnabled = false;
                            cbSerial.IsEnabled = false;
                            btnFind.IsEnabled = false;

                            BeginStoryboard((Storyboard)FindResource("AniSwitchOn"));
                        }
                    }
                    catch
                    {
                        MessageBox.Show("串口设置不正确或串口被占用");
                    }
                }
            }
        }

        /// <summary>
        /// 串口接收到了数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[_myPort.BytesToRead];
            _myPort.Read(buffer, 0, buffer.Length);

            if (buffer.Length > 0)
            {
                bytesReceived.AddRange(buffer);
            }

            _myPort.DiscardInBuffer();

            if (bytesReceived.Count > 0)
            {
                while (bytesReceived[0] != 0xAA)
                {
                    bytesReceived.RemoveAt(0);
                    if (bytesReceived.Count == 0)
                    {
                        break;
                    }
                }
            }

            if (bytesReceived.Count >= 4)
            {
                //Action<string> ade = (x) => { testTB.Text += x; };
                //testTB.Dispatcher.Invoke(ade,"");
                //testTB.Dispatcher.Invoke(ade, bytesReceived[0].ToString("X2") + " ");
                //testTB.Dispatcher.Invoke(ade, bytesReceived[0].ToString("X2") + " ");
                //testTB.Dispatcher.Invoke(ade, bytesReceived[0].ToString("X2") + " ");
                //testTB.Dispatcher.Invoke(ade, bytesReceived[0].ToString("X2"));

                if (bytesReceived[2] == 0xA5 && bytesReceived[3] == 0x5A)
                {
                    dataQueue.Enqueue(new List<byte>(bytesReceived.GetRange(0, 4).ToArray()));//入队

                    QueueAdded(sender, e);//触发事件
                }
                else
                {
                    bytesReceived.Clear();
                }
            }
        }

        /// <summary>
        /// 写串口
        /// </summary>
        /// <param name="inputBytes">要写的字节数组</param>
        /// <param name="offset">初始偏移</param>
        /// <param name="count">字节数</param>
        public void WriteBytes(byte[] inputBytes, int offset, int count)
        {
            _myPort.Write(inputBytes, offset, count);
        }
    }
}
