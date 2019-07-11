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

namespace PlugInDrawGraphic
{
	public partial class PreviewWindow : Form
	{
		DataTransfer _formDT;
		Point _zeroPosition;//坐标原点位置
		double _displayMul;//缩放比例
		List<Point> _originalGraphic;//原始图形

		Point _mousePosition;

		bool _mouseMidDown = false;
		Point _mouseDownPosition;

		Point _startPaintPt;

		public PreviewWindow(DataTransfer inputDT)
		{
			InitializeComponent();
			_formDT = inputDT;
			_zeroPosition = new Point((int)(0.5 * Width), (int)(0.5 * Height));//默认原点在中间
			_displayMul = 1;
			_originalGraphic = new List<Point>();
			_startPaintPt = new Point();
			_mouseDownPosition = new Point();
			timerRepaint.Enabled = true;
			_mouseDownPosition = new Point();

			this.MouseWheel += PreviewWindow_MouseWheel;
		}

		private int TransY(double inputY)
		{
			int pix_Y = 0;
			pix_Y = (int)(_zeroPosition.Y - inputY * _displayMul);
			return pix_Y;
		}

		private int TransX(double inputX)
		{
			int pix_X = 0;
			pix_X = (int)(inputX * _displayMul + _zeroPosition.X);
			return pix_X;
		}

		private Point3D deTransPt(Point pt)
		{
			Point3D result = new Point3D();
			result.Z = 0;
			Point tempPt = PointSub(pt, _zeroPosition);
			result.X = tempPt.X / _displayMul;
			result.Y = tempPt.Y / _displayMul * (-1);

			return result;
		}

		private Point TransPt(Point3D inputPt)
		{
			Point result = new Point();
			result.X = TransX(inputPt.X);
			result.Y = TransY(inputPt.Y);
			return result;
		}

		private void PreviewWindow_Load(object sender, EventArgs e)
		{
			//读取DT获取原始图形
			
		}

		/// <summary>
		/// 画圆弧
		/// </summary>
		/// <param name="p">画笔</param>
		/// <param name="center">圆心</param>
		/// <param name="start">起点</param>
		/// <param name="end">终点</param>
		/// <param name="direction">方向 0-顺时针 1-逆时针</param>
		/// <param name="g">Graphic 实例</param>
		private void Paint_Arc(Pen p, Point center, Point start, Point end, int direction, Graphics g)
		{
			//DrawArc 顺时针为正，x轴为0
			//计算相对起点和终点
			Point _relativeStart = PointSub(start,center);
			Point _relativeEnd = PointSub(end, center);
			//计算半径
			int _radio = (int)Math.Sqrt((_relativeStart.X * _relativeStart.X + _relativeStart.Y * _relativeStart.Y));
			//起始角
			double startAngle = (Math.Atan(_relativeStart.Y / (double)_relativeStart.X)) * 180.0 / Math.PI;
			//终止角
			double endAngle = (Math.Atan(_relativeEnd.Y / (double)_relativeEnd.X)) * 180.0 / Math.PI;
			//扫过角度
			double deltaAngle = 0;

			if (_relativeStart.X == 0 && _relativeStart.Y > 0)//在+Y轴上
			{
				startAngle = 90;
			}
			else if (_relativeStart.X == 0 && _relativeStart.Y < 0)//在-Y轴上
			{
				startAngle = 270;
			}
			else if (_relativeStart.Y == 0 && _relativeStart.X > 0)//在+X轴上
			{
				startAngle = 0;
			}
			else if (_relativeStart.Y == 0 && _relativeStart.X < 0)//在-X轴上
			{
				startAngle = 180;
			}
			else if (_relativeStart.Y == 0 && _relativeStart.X == 0)
			{
				startAngle = 0;
			}
			else
			{
				if (GetQuadrant(_relativeStart) == 2 || GetQuadrant(_relativeStart) == 3)
				{
					startAngle = 180 + startAngle;
				}
				else if (GetQuadrant(_relativeStart) == 4)
				{
					startAngle = 360 + startAngle;
				}
			}

			if (_relativeEnd.X == 0 && _relativeEnd.Y > 0)//在+Y轴上
			{
				endAngle = 90;
			}
			else if (_relativeEnd.X == 0 && _relativeEnd.Y < 0)//在-Y轴上
			{
				endAngle = 270;
			}
			else if (_relativeEnd.Y == 0 && _relativeEnd.X > 0)//在+X轴上
			{
				endAngle = 0;
			}
			else if (_relativeEnd.Y == 0 && _relativeEnd.X < 0)//在-X轴上
			{
				endAngle = 180;
			}
			else if (_relativeEnd.Y == 0 && _relativeEnd.X == 0)
			{
				endAngle = 0;
			}
			else
			{
				if (GetQuadrant(_relativeEnd) == 2 || GetQuadrant(_relativeEnd) == 3)
				{
					endAngle = 180 + endAngle;
				}
				else if (GetQuadrant(_relativeEnd) == 4)
				{
					endAngle = 360 + endAngle;
				}
			}

			if (startAngle >= endAngle)
			{
				if (direction == 0)//顺时针
				{
					deltaAngle = 360 - startAngle + endAngle;
				}
				else//逆时针
				{
					deltaAngle = startAngle - endAngle;
				}
			}
			else
			{
				if (direction == 0)//顺时针
				{
					deltaAngle = endAngle - startAngle;
				}
				else
				{
					deltaAngle = 360 - endAngle + startAngle;
				}
			}
			//if (GetQuadrant(_relativeEnd) == 2 || GetQuadrant(_relativeEnd)==3)
			//{
			//	endAngle = 180 + endAngle;
			//}
			//else if (GetQuadrant(_relativeEnd) == 4)
			//{
			//	endAngle = 360 + endAngle;
			//}

			if (_radio <= 0)
			{
				_radio = 1;
			}

			if (direction == 0)//顺时针
			{
				g.DrawArc(p, center.X - _radio, center.Y - _radio, 2 * _radio, 2 * _radio, (int)startAngle, (int)deltaAngle);
			}
			else//逆时针
			{
				g.DrawArc(p, center.X - _radio, center.Y - _radio, 2 * _radio, 2 * _radio, (int)endAngle, (int)deltaAngle);
			}
		}

		private Point PointSub(Point p1, Point p2)
		{
			Point result = new Point();
			result.X = p1.X - p2.X;
			result.Y = p1.Y - p2.Y;
			return result;
		}

		private void PreviewWindow_Paint(object sender, PaintEventArgs e)
		{
			//e.Graphics.DrawLine(new Pen(Color.Black), 0, _mousePosition.Y, Width, _mousePosition.Y);
			//e.Graphics.DrawLine(new Pen(Color.Black), _mousePosition.X, 0, _mousePosition.X, Height);
			Paint_Basic(e.Graphics);

			//e.Graphics.DrawLine(new Pen(Color.Blue), TransPt(new Point3D(0, 0, 0)), TransPt(new Point3D(100, 100, 0)));

			//Paint_Arc(new Pen(Color.Red), TransPt(new Point3D(100,100,0)), TransPt(new Point3D(50, 100, 0)), TransPt(new Point3D(150, 100, 0)), 0, e.Graphics);
		}

		private int GetQuadrant(Point pt)
		{
			int tempQuadrant = 1;
			if (pt.X > 0 && pt.Y >= 0)
			{
				tempQuadrant = 1;
			}
			else if (pt.X <= 0 && pt.Y > 0)
			{
				tempQuadrant = 2;
			}
			else if (pt.X < 0 && pt.Y <= 0)
			{
				tempQuadrant = 3;
			}
			else if (pt.X >= 0 && pt.Y < 0)
			{
				tempQuadrant = 4;
			}
			else if (pt.X == 0 && pt.Y == 0)
			{
				tempQuadrant = 1;
			}
			return tempQuadrant;
		}

		private void Paint_Basic(Graphics g)
		{
			//画坐标轴
			Pen pCoor = new Pen(Color.Red);
			g.DrawLine(pCoor, new Point(0, _zeroPosition.Y), new Point(Width, _zeroPosition.Y));//画X轴
			pCoor.Color = Color.Green;
			g.DrawLine(pCoor, new Point(_zeroPosition.X, 0), new Point(_zeroPosition.X, Height));

			lock (_formDT)
			{
				Pen pBasic = new Pen(Color.Purple);
				foreach (OperationListNode OLN in _formDT.OperationList)
				{
					if (OLN.Command == GWorkState.G00)
					{
						_startPaintPt = TransPt(OLN.End);
					}
					//G02 顺时针旋转
					//G03 逆时针旋转
					else if (OLN.Command == GWorkState.G02)
					{
						Point center = TransPt(OLN.Center);
						Point end = TransPt(OLN.End);
						Paint_Arc(pBasic, center, _startPaintPt, end, 0, g);
						_startPaintPt = end;
					}
					else if (OLN.Command == GWorkState.G03)
					{
						Point center = TransPt(OLN.Center);
						Point end = TransPt(OLN.End);
						Paint_Arc(pBasic, center, _startPaintPt, end, 1, g);
						_startPaintPt = end;
					}
					else if (OLN.Command == GWorkState.G01)
					{
						Point end = TransPt(OLN.End);
						g.DrawLine(pBasic, _startPaintPt, end);
						_startPaintPt = end;
					}
				}
			}
		}

		private void PreviewWindow_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle)
			{
				_mouseMidDown = true;
				_mouseDownPosition.X = e.X;
				_mouseDownPosition.Y = e.Y;
			}
		}

		private void PreviewWindow_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle)
			{
				_mouseMidDown = false;
			}
		}

		private void PreviewWindow_MouseMove(object sender, MouseEventArgs e)
		{
			if (_mouseMidDown == true)
			{
				_zeroPosition.X += (e.X - _mouseDownPosition.X);
				_zeroPosition.Y += (e.Y - _mouseDownPosition.Y);

				_mouseDownPosition.X = e.X;
				_mouseDownPosition.Y = e.Y;
			}

			_mouseDownPosition = e.Location;

			Point3D realPosition = deTransPt(e.Location);
			msgShowPosition.Text = "坐标：(" + realPosition.X.ToString("f4") + ", " + realPosition.Y.ToString("f4") + ")";

			this.Invalidate();
		}

		private void PreviewWindow_MouseWheel(object sender, MouseEventArgs e)
		{
			int temp = e.Delta;
			if (temp > 0)
			{
				_displayMul *= 1.2;
			}
			else
			{
				if (_displayMul >= 0.012)
				{
					_displayMul /= 1.2;
				}
				else
				{
					_displayMul = 0.01;
				}
			}

			this.Invalidate();
		}

		private void btnResetView_Click(object sender, EventArgs e)
		{
			_zeroPosition = new Point((int)(0.5 * Width), (int)(0.5 * Height));//默认原点在中间
			_displayMul = 1;
		}

		private void timerRepaint_Tick(object sender, EventArgs e)
		{
			this.Invalidate();//重绘
		}
	}
}
