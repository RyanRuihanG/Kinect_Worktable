using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDCommunication;


namespace EncoderDecoder
{
   
    public class Encoder : IEncoder
    {
        byte[] _array;
        public Encoder()
        {
            _array = new byte[19];
         
        }
        public byte[] Encode(DataPackage inputPackage)
        {
            _array = new byte[19];
			
            _array[0] = 0xAA;
            _array[1] = zhengshubuma(inputPackage.DataX)[1];//取X坐标整数高位
            _array[2] = zhengshubuma(inputPackage.DataX)[0];//取X坐标整数低位
            _array[3] = xiaoshutiqu(inputPackage.DataX)[1];//取X坐标小数高位
            _array[4] = xiaoshutiqu(inputPackage.DataX)[0];//取X坐标小数高位

            _array[5] = zhengshubuma(inputPackage.DataY)[1];
            _array[6] = zhengshubuma(inputPackage.DataY)[0];
            _array[7] = xiaoshutiqu(inputPackage.DataY)[1];
            _array[8] = xiaoshutiqu(inputPackage.DataY)[0];

            _array[9] = zhengshubuma(inputPackage.DataZ)[1];
            _array[10] = zhengshubuma(inputPackage.DataZ)[0];
            _array[11] = xiaoshutiqu(inputPackage.DataZ)[1];
            _array[12] = xiaoshutiqu(inputPackage.DataZ)[0];

            _array[13] = velocityGet(inputPackage.DataF)[0];//获取速度
            _array[14] = velocityGet(inputPackage.DataF)[1];

            _array[15] = inputPackage.Command;//获取特殊命令
            _array[16] = yihuojiaoyan(_array);//异或校验位
            _array[17] = 0xA5;
            _array[18] = 0x5A;
            return _array;
        }

        private byte yihuojiaoyan(byte[] _array)
        {
            byte result = _array[1];
            for(int i = 2; i <= 15; i++) //从1位到15位依次异或
            {
                result ^=  _array[i];
            }
            return result;
        }

        private byte[] velocityGet(double dataF) //获得速度
        {
            byte[] _velocity = new byte[2];
			int _intF = (int)dataF;
            _velocity[0] = Convert.ToByte(_intF / 100);
			_velocity[1] = Convert.ToByte((_intF - _velocity[0] * 100));
            return _velocity;
        }

        private byte[] xiaoshutiqu(double dataX)
        {
			byte[] _decimalData = new byte[2];
			int _intDecimal = 0;
			if (dataX > 0)
			{
				double _decimal = dataX - (int)dataX;//小数部分

				_intDecimal = (int)(_decimal * 10000);//变成整数
			}
			else if (dataX < 0)//-5.2  -6 + 0.8   5.2
			{
				dataX = -dataX;
				double _decimal = dataX - (int)dataX;//小数部分
				_decimal = 1 - _decimal;

				_intDecimal = (int)(_decimal * 10000);//变成整数
			}

			_decimalData[0] = Convert.ToByte((_intDecimal - (int)(_intDecimal / 100) * 100));
			_decimalData[1] = Convert.ToByte((_intDecimal - _decimalData[0]) / 100);
            //int index = dataX.ToString().IndexOf(".");//找到小数点位置
            //string decimalDataX = dataX.ToString().Substring(index + 1, index + 5);//截取string
            //int decimalDataX_int = Convert.ToInt16(decimalDataX);//string转化为int16
            //_decimalData[0] = Convert.ToByte(decimalDataX_int);//取低8位
            //_decimalData[1] = Convert.ToByte(decimalDataX_int >> 8);//取高8位
            return _decimalData;

        }

        private byte[] zhengshubuma(double dataX)
        {
			if (dataX < 0)
			{
				dataX -= 1;
			}
            byte[] _intData = new byte[2];
            int _dataX_int = (int)dataX;
            short _dataX_intbu = (short)(~_dataX_int + 0x01);//取补码
			_intData[0] = (byte)_dataX_int;
			_intData[1] = (byte)(_dataX_int >> 8);
            //_intData[0] = Convert.ToByte(_dataX_intbu);
            //_intData[1] = Convert.ToByte(_dataX_intbu >> 8);
            return _intData;
        }
    }

    public class Decoder : IDecoder
    {        
        public Decoder()
        {
        }
        public  DataPackage Decode(byte[] inputPackage)
        {
			if (inputPackage.Length == 4)//19)
			{
				DataPackage tempPackage = new DataPackage();
				//if (!rightYihuo(inputPackage))
				//{
				//	return null;
				//}
				//else
				//{
					//tempPackage.DataX = getX(inputPackage);
					//tempPackage.DataY = getY(inputPackage);
					//tempPackage.DataZ = getZ(inputPackage);
					//tempPackage.DataF = getF(inputPackage);
					tempPackage.Command = getCommand(inputPackage);
					return tempPackage;
				//}
			}
			else
			{
				return null;
			}
        }

        private byte getCommand(byte[] inputPackage)
        {
			return inputPackage[1];
        }

        private double getF(byte[] inputPackage)
        {
			return 0;
        }

        private double getZ(byte[] inputPackage)
        {
			byte[] buf = new byte[2];
			buf[1] = inputPackage[9];
			buf[0] = inputPackage[10];
			int tempInt = BitConverter.ToInt16(buf, 0);

			double tempDec = (double)(inputPackage[11] * 100 + inputPackage[12]) / 10000;

			return (tempDec + tempInt);
		}

        private double getY(byte[] inputPackage)
        {
			byte[] buf = new byte[2];
			buf[1] = inputPackage[5];
			buf[0] = inputPackage[6];
			int tempInt = BitConverter.ToInt16(buf, 0);

			double tempDec = (double)(inputPackage[7] * 100 + inputPackage[8]) / 10000;

			return (tempDec + tempInt);
		}

        private double getX(byte[] inputPackage)
        {
			byte[] buf = new byte[2];
			buf[1] = inputPackage[1];
			buf[0] = inputPackage[2];
			int tempInt = BitConverter.ToInt16(buf, 0);

			double tempDec = (double)(inputPackage[3] * 100 + inputPackage[4]) / 10000;

			return (tempDec + tempInt);
        }

        private bool rightYihuo(byte[] inputPackage)
        {
            byte result = inputPackage[1];
            for (int i = 2; i <= 15; i++) //从1位到15位依次异或
            {
                result ^= inputPackage[i];
            }
			if (result == inputPackage[16])//判断是否正确异或
			{
				return true;
			}
			else
			{
//#if DEBUG
				return true;
//#else
				//return false;
//#endif
			}
        }
    }
}
