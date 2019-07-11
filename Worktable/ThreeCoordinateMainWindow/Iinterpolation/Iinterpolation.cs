using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPublicPlugInInterface;

namespace Iinterpolation
{
	/// <summary>
	/// 插补操作类的接口
	/// </summary>
    public interface IInterpolation
    {
		List<StepMark> LineInterpolation(Point3D start, Point3D end);
		List<StepMark> CircleInterpolation(Point3D center,Point3D start,Point3D end,int direction);
    }

	/// <summary>
	/// 象限
	/// </summary>
	public enum Quadrant
	{
		First,
		Second,
		Third,
		Forth
	}

	/// <summary>
	/// 插补操作算子
	/// </summary>
	public enum StepState
	{
		Zero,
		OneStepPositive,
		OneStepNegative,
		ToEnd
	};

	/// <summary>
	/// 标记存储插补结果类
	/// </summary>
	public class StepMark
	{
		StepState _x;
		StepState _y;
		StepState _z;

		/// <summary>
		/// X轴插补状态，只读
		/// </summary>
		public StepState X { get { return _x; } }
		/// <summary>
		/// Y轴插补状态，只读
		/// </summary>
		public StepState Y { get { return _y; } }
		/// <summary>
		/// Z轴插补状态，只读
		/// </summary>
		public StepState Z { get { return _z; } }

		/// <summary>
		/// 构造函数
		/// </summary>
		public StepMark()
		{
			_x = StepState.Zero;
			_y = StepState.Zero;
			_z = StepState.Zero;
		}

		/// <summary>
		/// 设定X轴正步进一次
		/// </summary>
		public void StepOnXPositive()
		{
			_x = StepState.OneStepPositive;
			_y = StepState.Zero;
			_z = StepState.Zero;
		}

		/// <summary>
		/// 设定Y轴正步进一次
		/// </summary>
		public void StepOnYPositive()
		{
			_x = StepState.Zero;
			_y = StepState.OneStepPositive;
			_z = StepState.Zero;
		}
		
		/// <summary>
		/// 设定Z轴正步进一次
		/// </summary>
		public void StepOnZPositive()
		{
			_x = StepState.Zero;
			_y = StepState.Zero;
			_z = StepState.OneStepPositive;
		}

		/// <summary>
		/// 设定X轴负向步进一次
		/// </summary>
		public void StepOnXNegative()
		{
			_x = StepState.OneStepNegative;
			_y = StepState.Zero;
			_z = StepState.Zero;
		}

		/// <summary>
		/// 设定Y轴负向步进一次
		/// </summary>
		public void StepOnYNegative()
		{
			_x = StepState.Zero;
			_y = StepState.OneStepNegative;
			_z = StepState.Zero;
		}

		/// <summary>
		/// 设定Z轴负向步进一次
		/// </summary>
		public void StepOnZNegative()
		{
			_x = StepState.Zero;
			_y = StepState.Zero;
			_z = StepState.OneStepNegative;
		}

		/// <summary>
		/// 设定X轴到底
		/// </summary>
		public void ToEndX()
		{
			_x = StepState.ToEnd;
			_y = StepState.Zero;
			_z = StepState.Zero;
		}

		/// <summary>
		/// 设定Y轴到底
		/// </summary>
		public void ToEndY()
		{
			_x = StepState.Zero;
			_y = StepState.ToEnd;
			_z = StepState.Zero;
		}

		/// <summary>
		/// 设定Z轴到底
		/// </summary>
		public void ToEndZ()
		{
			_x = StepState.Zero;
			_y = StepState.Zero;
			_z = StepState.ToEnd;
		}
	}
}
