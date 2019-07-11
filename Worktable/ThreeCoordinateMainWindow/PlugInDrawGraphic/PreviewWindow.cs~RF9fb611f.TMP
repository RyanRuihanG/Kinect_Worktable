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
		Point _zeroPosition;
		double _displayMul;
		List<Point> _originalGraphic;

		Point _startPaintPt;

		public PreviewWindow(DataTransfer inputDT)
		{
			InitializeComponent();
			_formDT = inputDT;
			_zeroPosition = new Point((int)(0.5 * Width), (int)(0.5 * Height));//默认原点在中间
			_displayMul = 1;
			_originalGraphic = new List<Point>();
			_startPaintPt = new Point();
		}

		private int TransY(double inputY)
		{
			int pix_Y = 0;
			pix_Y = (int)(Height - inputY * _displayMul - _zeroPosition.Y);
			return pix_Y;
		}

		private int TransX(double inputX)
		{
			int pix_X = 0;
			pix_X = (int)(inputX * _displayMul + _zeroPosition.X);
			return pix_X;
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
			lock(_formDT)
			{
				foreach (OperationListNode OLN in _formDT.OperationList)
				{
					if (OLN.Command == GWorkState.G00)
					{
						_startPaintPt = TransPt(OLN.Start);
					}
					//G02 顺时针旋转
					//G03 逆时针旋转
					else if (OLN.Command == GWorkState.G02)
					{

					}
				}
			}
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

			if (GetQuadrant(_relativeStart) == 2 || GetQuadrant(_relativeStart) == 3)
			{
				startAngle = 180 + startAngle;
			}
			else if (GetQuadrant(_relativeStart) == 4)
			{
				startAngle = 360 + startAngle;
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
			if (GetQuadrant(_relativeEnd) == 2 || GetQuadrant(_relativeEnd)==3)
			{
				endAngle = 180 + endAngle;
			}
			else if (GetQuadrant(_relativeEnd) == 4)
			{
				endAngle = 360 + endAngle;
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
			e.Graphics.DrawArc(new Pen(Color.Black), 100, 100, 200, 200, 90, 135);
			Paint_Arc(new Pen(Color.Red), new Point(100, 100), new Point(50, 100), new Point(150, 100), 0, e.Graphics);
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
	}
}
