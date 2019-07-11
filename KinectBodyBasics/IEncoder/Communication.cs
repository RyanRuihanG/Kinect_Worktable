/// <summary>
/// 封装上下位机通信接口
/// </summary>
namespace TDCommunication
{
	/// <summary>
	/// 数据包编码接口
	/// </summary>
    public interface IEncoder
    {
		/// <summary>
		/// 数据包编码
		/// </summary>
		/// <param name="inputPackage">输入数据包</param>
		/// <returns>byte数组</returns>
		byte[] Encode(DataPackage inputPackage);
    }

	/// <summary>
	/// 数据包解码接口
	/// </summary>
	public interface IDecoder
	{
		/// <summary>
		/// 数据包解码
		/// </summary>
		/// <param name="inputPackage">输入数据包</param>
		/// <returns>解码后的数据包</returns>
		DataPackage Decode(byte[] inputPackage);
	}

	/// <summary>
	/// 数据包封装类
	/// </summary>
	public class DataPackage
	{
		double _dataX;
		double _dataY;
		double _dataZ;
		double _dataF;

		byte _command;

		public DataPackage()
		{
			_dataX = 0;
			_dataY = 0;
			_dataZ = 0;
			_dataF = 0;
			_command = 0;
		}

		public double DataX { get { return _dataX; } set { _dataX = value; } }
		public double DataY { get { return _dataY; } set { _dataY = value; } }
		public double DataZ { get { return _dataZ; } set { _dataZ = value; } }
		public double DataF { get { return _dataF; } set { _dataF = value; } }
		public byte Command { get { return _command; } set { _command = value; } }

		public DataPackage Copy()
		{
			DataPackage temp = new DataPackage();

			temp.DataX = _dataX;
			temp.DataY = _dataY;
			temp.DataZ = _dataZ;
			temp.DataF = _dataF;
			temp.Command = _command;

			return temp;
		}
	}

	/// <summary>
	/// command静态类，表示各个指令对应的byte值
	/// </summary>
	public static class CommandClass
	{
		public const byte _RUNNING_ = 0x33;
		public const byte _FINISHED_ = 0x55;

		public const byte _TO_ZERO_ = 0x03;
		public const byte _PAUSE_ = 0x05;
		public const byte _SET_POSITION_ = 0x0A;
	}
}
