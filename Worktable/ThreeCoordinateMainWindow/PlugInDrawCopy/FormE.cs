using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IPublicPlugInInterface;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DrawCopy
{
	public delegate void ChangePicHandler(List<List<Point>> ptlist,double coorMul);  //定义委托

	public partial class FormE : Form
	{
		bool _mouseLeftDown = false;

        //存储绘制的点
        List<List<Point>> _drawPoints;

		int _drawCount = 0;
		int pointNumber = 0;
		Point myMousePosition;

		public event ChangePicHandler ChangePic;  //定义事件

		public FormE()
		{
			InitializeComponent();
		}

		public void SetForm()
		{

		}

		private void FormE_Load(object sender, EventArgs e)
		{
			_drawPoints = new List<List<Point>>();
			myMousePosition = new Point();
		}

		private void FormE_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_mouseLeftDown = true;
				List<Point> tempList = new List<Point>();
				_drawPoints.Add(tempList);
			}
		}

		private void FormE_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_mouseLeftDown = false;
				_drawCount++;
			}
		}

		private void FormE_MouseMove(object sender, MouseEventArgs e)
		{
			if (_mouseLeftDown == true)
			{
				Point temp = new Point(e.Location.X, e.Location.Y);
				_drawPoints[_drawCount].Add(temp);
				
				pointNumber++;
				Text = "点数：" + pointNumber.ToString();
			}

			myMousePosition.X = e.Location.X;
			myMousePosition.Y = e.Location.Y;

			this.Invalidate();
		}

		private void FormE_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawLine(new Pen(Color.Black), 0, myMousePosition.Y, this.Size.Width, myMousePosition.Y);
			e.Graphics.DrawLine(new Pen(Color.Black), myMousePosition.X, 0, myMousePosition.X, this.Size.Height);
			foreach (List<Point> LP in _drawPoints)
			{
				for (int i = 0; i < LP.Count - 1; i++)
				{
					e.Graphics.DrawLine(new Pen(Color.Blue, 2), LP[i], LP[i + 1]);
				}
			}
		}

		private void FormE_FormClosed(object sender, FormClosedEventArgs e)
		{
			_drawPoints.Clear();
			GC.Collect();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			_drawPoints.Clear();
			GC.Collect();
			_drawCount = 0;
			pointNumber = 0;
			Text = "点数：" + pointNumber.ToString();
			Invalidate();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			//try
			//{
				double tempMul = Convert.ToDouble(tbMul.Text);

				List<List<Point>> tempPtList = new List<List<Point>>();

				foreach (List<Point> ptl in _drawPoints)
				{
					tempPtList.Add(new List<Point>());

					//反转Y坐标
					foreach (Point pt in ptl)
					{
						Point tpt = new Point();
						tpt.Y = this.Height - pt.Y - 37;
						tpt.X = pt.X;
						tempPtList[tempPtList.Count - 1].Add(tpt);
					}
				}
				ChangePic?.Invoke(tempPtList, tempMul);//执行委托实例  
			//}
			//catch
			//{
			//	MessageBox.Show("数据传递出错");
			//}
		}

        private void btnSave_Click(object sender, EventArgs e)
        {
            saveDialog.InitialDirectory = Application.StartupPath;
            saveDialog.Filter = "All files (*.*)|*.*|point files (*.3ctpl)|*.3ctpl";
            saveDialog.FilterIndex = 2;
            saveDialog.RestoreDirectory = true;
            saveDialog.FileName = "untitled.3ctpl";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream fileStream = new FileStream(saveDialog.FileName, FileMode.Create);
                BinaryFormatter binaryFormatter = new BinaryFormatter();

                //数据打包
                PointsListsPack tempDataPack = new PointsListsPack();
                tempDataPack.Data_Height = this.Height;
                tempDataPack.Data_Width = this.Width;
                tempDataPack.Data_PointLists = _drawPoints;

                //序列化文件
                binaryFormatter.Serialize(fileStream, tempDataPack);
                fileStream.Close();
            }
        }
    }
}
