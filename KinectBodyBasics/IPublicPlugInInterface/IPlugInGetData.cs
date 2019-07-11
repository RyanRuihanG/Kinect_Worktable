using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPublicPlugInInterface;

/// <summary>
/// 封装插件接口，相关数据传递类和G代码指令列
/// </summary>
namespace IPublicPlugInInterface
{
	public delegate void ChangeDataHandler(List<List<Point3D>> ptlists);  //定义委托

	/// <summary>
	/// 只从宿主获取数据，宿主主动索取数据
	/// </summary>
	public interface IPlugInGetData
	{
		DataTransfer DataTransfer { get; }
		/// <summary>
		/// 用于返回插件相关信息
		/// </summary>
		/// <returns></returns>
		PlugInInfo WhoAmI();
		/// <summary>
		/// 设置数据传递实例
		/// </summary>
		/// <param name="inputDT">引用传递数据传递实例</param>
		void SetDataTransfer(DataTransfer inputDT);
		/// <summary>
		/// 插件程序入口点
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Action(object sender, EventArgs e);
		/// <summary>
		/// 保存插件内点列计算结果，如不需要可不实现
		/// </summary>
		List<List<Point3D>> PointLists { get; }
	}

	/// <summary>
	/// 建立事件通知宿主取数据
	/// </summary>
	public interface IPlugInGetSetData
	{
		DataTransfer DataTransfer { get; }
		/// <summary>
		/// 用于返回插件相关信息
		/// </summary>
		/// <returns></returns>
		PlugInInfo WhoAmI();
		/// <summary>
		/// 设置数据传递实例
		/// </summary>
		/// <param name="inputDT">引用传递数据传递实例</param>
		void SetDataTransfer(DataTransfer inputDT);
		/// <summary>
		/// 插件程序入口点
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Action(object sender, EventArgs e);
		/// <summary>
		/// 保存插件内点列计算结果，如不需要可不实现
		/// </summary>
		List<List<Point3D>> PointLists { get; }

		/// <summary>
		/// 通知宿主程序取数据的事件
		/// </summary>
		event ChangeDataHandler ChangeData;  //定义事件
	}

	/// <summary>
	/// 数据传递操作子
	/// </summary>
	public class DataTransfer
	{
		double _coordinateX;
		double _coordinateY;
		double _coordinateZ;
		double _workF;

		List<OperationListNode> _operationList;

		public DataTransfer()
		{
			_coordinateX = 0;
			_coordinateY = 0;
			_coordinateZ = 0;
			_workF = 0;

			_operationList = new List<OperationListNode>();
		}

		public double CoordinateX { get => _coordinateX; set => _coordinateX = value; }
		public double CoordinateY { get => _coordinateY; set => _coordinateY = value; }
		public double CoordinateZ { get => _coordinateZ; set => _coordinateZ = value; }
		public double WorkF { get => _workF; set => _workF = value; }
		public List<OperationListNode> OperationList { get => _operationList; set => _operationList = value; }
	}

	/// <summary>
	/// 插件信息
	/// </summary>
	public class PlugInInfo
	{
		string _name;

		public string Name { get => _name; set => _name = value; }
	}

	/// <summary>
	/// 3坐标double点
	/// </summary>
	public class Point3D
	{
		double _x;
		double _y;
		double _z;

		public Point3D()
		{
			_x = 0;
			_y = 0;
			_z = 0;
		}

		public Point3D(double x, double y, double z)
		{
			_x = x;
			_y = y;
			_z = z;
		}

		public double X { get => _x; set => _x = value; }
		public double Y { get => _y; set => _y = value; }
		public double Z { get => _z; set => _z = value; }

		/// <summary>
		/// 重载+运算符
		/// </summary>
		/// <param name="input1"></param>
		/// <param name="input2"></param>
		/// <returns></returns>
		public static Point3D operator +(Point3D input1, Point3D input2)
		{
			Point3D result = new Point3D();
			result.X = input1.X + input2.X;
			result.Y = input1.Y + input2.Y;
			result.Z = input1.Z + input2.Z;
			return result;
		}
		
		/// <summary>
		/// 重载-运算符
		/// </summary>
		/// <param name="input1"></param>
		/// <param name="input2"></param>
		/// <returns></returns>
		public static Point3D operator -(Point3D input1, Point3D input2)
		{
			Point3D result = new Point3D();
			result.X = input1.X - input2.X;
			result.Y = input1.Y - input2.Y;
			result.Z = input1.Z - input2.Z;
			return result;
		}
	}

	/// <summary>
	/// 用于表示G代码解释结果的枚举
	/// </summary>
	public enum GWorkState
	{
		NULL,
		G00,
		G01,
		G02,
		G03,
		Others
	};

	/// <summary>
	/// G代码指令列，用于和插件通信使用
	/// </summary>
	public class OperationListNode
	{
		GWorkState command;
		Point3D start;
		Point3D end;
		Point3D center;

		public OperationListNode()
		{
			command = GWorkState.NULL;
			start = new Point3D();
			end = new Point3D();
			center = new Point3D();
		}

		public GWorkState Command { get => command; set => command = value; }
		public Point3D Start { get => start; set => start = value; }
		public Point3D End { get => end; set => end = value; }
		public Point3D Center { get => center; set => center = value; }
	}

    [Serializable]
    public class PointsListsPack
    {
        int data_Height;
        int data_Width;
        List<List<Point>> data_PointLists;

        /// <summary>
        /// 数据 - 点依附的画框高度
        /// </summary>
        public int Data_Height { get => data_Height; set => data_Height = value; }
        /// <summary>
        /// 数据 - 点依附的画框宽度
        /// </summary>
        public int Data_Width { get => data_Width; set => data_Width = value; }
        /// <summary>
        /// 数据 - 点列表
        /// </summary>
        public List<List<Point>> Data_PointLists { get => data_PointLists; set => data_PointLists = value; }
    }
}
