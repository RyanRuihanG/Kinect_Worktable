using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPublicPlugInInterface;

namespace DrawCopy
{
	public class Class1 : IPlugInGetSetData
	{
		private double _coorMul;

		private DataTransfer myDT;
		public DataTransfer DataTransfer { get => myDT;}

		private List<List<Point3D>> _pointLists;
		public List<List<Point3D>> PointLists { get => _pointLists; }

		public event ChangeDataHandler ChangeData;

		public PlugInInfo WhoAmI()
		{
			PlugInInfo tempInfo = new PlugInInfo();
			tempInfo.Name = "手绘跟随";
			return tempInfo;
		}

		public void SetDataTransfer(DataTransfer inputDT)
		{
			myDT = inputDT;
		}

		public void Action(object sender, EventArgs e)
		{
			_coorMul = 1;
			_pointLists = new List<List<Point3D>>();

			FormE formE = new FormE();
			formE.ChangePic += new ChangePicHandler(picChanged);
			formE.Show();
		}

		private void picChanged(List<List<Point>> ptlists,double icoorMul)
		{
			_pointLists.Clear();
			_coorMul = icoorMul;

			foreach (List<Point> pl in ptlists)
			{
				_pointLists.Add(new List<Point3D>());

				//屏幕坐标变换到实际坐标
				foreach (Point p in pl)
				{
					_pointLists[_pointLists.Count-1].Add(new Point3D());
					_pointLists[_pointLists.Count - 1][_pointLists[_pointLists.Count - 1].Count-1] = pointToPoint3D(p,_coorMul);
				}	
			}
			ChangeData?.Invoke(_pointLists);//执行委托实例  
		}

		private Point3D pointToPoint3D(Point inputPt, double icoorMul)
		{
			Point3D temp3dPt = new Point3D();
			temp3dPt.Z = 0;
			temp3dPt.X = inputPt.X * icoorMul;
			temp3dPt.Y = inputPt.Y * icoorMul;

			return temp3dPt;
		}
	}
}
