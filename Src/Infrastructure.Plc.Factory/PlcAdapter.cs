using Infrastructure.Common.Interface;
using Infrastructure.Plc.Interface;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Plc.Factory
{
    /// <summary>
    /// PLC适配器
    /// </summary>
    public class PlcAdapter
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="plc">Plc对象</param>
        public void Initialize(IPlc plc)
        {
            this.Plc = plc;

            this.Plc.Initialize();
        }

        /// <summary>
        /// Plc对象
        /// </summary>
        public IPlc Plc { get; set; }

        /// <summary>
        /// 读数据寄存器位状态
        /// Public Function ReadRegest_TCP(ByVal regestName As String, ByRef regestBit() As String, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValue">地址名</param>
        /// <param name="values">地址数据</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public bool ReadUshort(string addressValue, ref string[] values, ref string errorInfo)
        {
            var result = new List<string>();
            var value = Plc.ReadSingle<ushort>(new DataAddress()
            {
                Value = addressValue,
                Type = DataAddressType.Ushort,
                Offset = 0
            });

            for (int bit = 0; bit < 16; bit++)
            {
                result.Add(((value >> bit) & 0x01).ToString());
            }

            values = result.ToArray();

            return true;
        }

        /// <summary>
        ///  读数据寄存器位状态(EM区)
        ///  Public Function ReadRegest_TCP(ByVal areaName As String, ByVal regestName As String, ByRef regestBit() As String, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValueArea">寄存器区</param>
        /// <param name="addressValue">地址名</param>
        /// <param name="values">地址数据</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public bool ReadUshort(string addressValueArea, string addressValue, ref string[] values, ref string errorInfo)
        {
            addressValue = GetAddressValue(addressValueArea) + addressValue;

            var result = new List<string>();
            var value = Plc.ReadSingle<ushort>(new DataAddress()
            {
                Value = addressValue,
                Type = DataAddressType.Ushort,
                Offset = 0
            });

            for (int bit = 0; bit < 16; bit++)
            {
                result.Add(((value >> bit) & 0x01).ToString());
            }

            values = result.ToArray();

            return true;
        }

        /// <summary>
        /// 读连续多个数据存储器(16位)
        /// Public Function ReadDate_TCP(ByVal regestName As String, ByVal regestCnt As Integer, ByRef regestBit() As Integer, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValue">地址名</param>
        /// <param name="count">数据个数</param>
        /// <param name="values">地址数据</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>    
        public bool ReadShort(string addressValue, int count, ref int[] values, ref string errorInfo)
        {
            var temp = new List<int>();
            var value = Plc.Read<short>(new DataAddress()
            {
                Value = addressValue,
                Type = DataAddressType.Short,
                Offset = count - 1
            });

            value.ToList().ForEach(e => { temp.Add(int.Parse(e.ToString())); });
            values = temp.ToArray();


            return true;
        }


        /// <summary>
        /// 读连续多个数据存储器(EM区16位)-PLC 返回整数
        /// Public Function ReadDate_TCP(ByVal areaName As String, ByVal regestName As String, ByVal regestCnt As Integer, ByRef regestBit() As Integer, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValueArea">寄存器区</param>
        /// <param name="addressValue">地址名</param>
        /// <param name="count">数据个数</param>
        /// <param name="values">地址数据</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public bool ReadShort(string addressValueArea, string addressValue, int count, ref int[] values, ref string errorInfo)
        {
            addressValue = GetAddressValue(addressValueArea) + addressValue;

            var value = Plc.Read<short>(new DataAddress()
            {
                Value = addressValue,
                Type = DataAddressType.Short,
                Offset = count - 1
            });

            for (int i = 0; i < value.Count(); i++)
            {
                values[i] = value.ToArray()[i];
            }

            return true;
        }

        /// <summary>
        /// 读连续多个数据存储器（32位）-PLC
        /// Public Function RWordDate_TCP(ByVal regestName As String, ByVal wordCnt As Integer, ByRef regestData() As Integer, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValue">地址名</param>
        /// <param name="count">数据个数</param>
        /// <param name="values">地址数据</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public bool ReadInt(string addressValue, int count, ref int[] values, ref string errorInfo)
        {
            var value = Plc.Read<int>(new DataAddress()
            {
                Value = addressValue,
                Type = DataAddressType.Int,
                Offset = count - 1
            });

            values = value.ToArray();

            return true;
        }

        /// <summary>
        /// 读连续多个数据存储器（EM区32位）-PLC
        /// Public Function RWordDate_TCP(ByVal areaName As String, ByVal regestName As String, ByVal wordCnt As Integer, ByRef regestData() As Integer, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValueArea">寄存器区</param>
        /// <param name="addressValue">地址名</param>
        /// <param name="count">数据个数</param>
        /// <param name="values">地址数据</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public bool ReadInt(string addressValueArea, string addressValue, int count, ref int[] values, ref string errorInfo)
        {
            addressValue = GetAddressValue(addressValueArea) + addressValue;

            var value = Plc.Read<int>(new DataAddress()
            {
                Value = addressValue,
                Type = DataAddressType.Int,
                Offset = count - 1
            });

            values = value.ToArray();

            return true;
        }


        /// <summary>
        /// 读连续多个浮点数
        /// Public Function Readfloat_Tcp(ByVal regestName As String, ByVal regestCnt As Integer, ByRef floatData() As Single, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValue">地址名</param>
        /// <param name="count">数据个数</param>
        /// <param name="values">地址数据</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        /// 
        public bool ReadFloat(string addressValue, int count, ref float[] values, ref string errorInfo)
        {
            var value = Plc.Read<float>(new DataAddress()
            {
                Value = addressValue,
                Type = DataAddressType.Float,
                Offset = count - 1
            });

            values = value.ToArray();

            return true;
        }

        /// <summary>
        /// 读连续浮点数(EM区)-PLC
        /// Public Function Rfloat_TCP(ByVal areaName As String, ByVal regestName As String, ByVal readCnt As Integer, ByRef regestBit() As Single, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValueArea">寄存器区</param>
        /// <param name="addressValue">地址名</param>
        /// <param name="count">数据个数</param>
        /// <param name="values">地址数据</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public bool ReadFloat(string addressValueArea, string addressValue, int count, ref float[] values, ref string errorInfo)
        {
            addressValue = GetAddressValue(addressValueArea) + addressValue;

            var value = Plc.Read<float>(new DataAddress()
            {
                Value = addressValue,
                Type = DataAddressType.Float,
                Offset = count - 1
            });

            values = value.ToArray();

            return true;
        }

        /// <summary>
        /// 读连续多个数据存储器(EM区16位)-PLC 返回ASCII
        /// Public Function ReadDate_TCP(ByVal areaName As String, ByVal regestName As String, ByVal regestCnt As Integer, ByRef regestBit() As String, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValueArea">寄存器区</param>
        /// <param name="addressValue">地址名</param>
        /// <param name="count">数据个数</param>
        /// <param name="values">地址数据</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public bool ReadString(string addressValueArea, string addressValue, int count, ref string[] values, ref string errorInfo)
        {
            var temp = new List<string>();
            addressValue = GetAddressValue(addressValueArea) + addressValue;

            var value = Plc.ReadSingle<string>(new DataAddress()
            {
                Value = addressValue,
                Type = DataAddressType.Short,
                Offset = (count * 2) - 1
            });

            value.ToList().ForEach(e => { temp.Add(e.ToString()); });
            values = temp.ToArray();

            return true;
        }

        /// <summary>
        /// 读条码
        /// Public Function Read_Barcode(ByVal regestName As String, ByVal Bar_len As Integer, ByRef barcode As String, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValue">地址名</param>
        /// <param name="length">条码长度</param>
        /// <param name="barcode">条码</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public bool ReadString(string addressValue, int length, ref string barcode, ref string errorInfo)
        {
            var value = Plc.ReadSingle<string>(new DataAddress()
            {
                Value = addressValue,
                Type = DataAddressType.String,
                Offset = length - 1
            });

            barcode = value;

            return true;
        }

        /// <summary>
        /// '读条码(按字长度)
        /// Public Function Rbarcoder_word(ByVal regestlen As String, ByVal regBarcoder As String, ByRef barcoder As String, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValueLength">条码长度地址</param>
        /// <param name="addressValueBarcode">条码地址</param>
        /// <param name="barcode">条码</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public bool ReadStringByWord(string addressValueLength, string addressValueBarcode, ref string barcode, ref string errorInfo)
        {
            var lengthValue = Plc.ReadSingle<int>(new DataAddress()
            {
                Value = addressValueLength,
                Type = DataAddressType.Int,
                Offset = 0
            });

            var value = Plc.ReadSingle<string>(new DataAddress()
            {
                Value = addressValueBarcode,
                Type = DataAddressType.String,
                Offset = (lengthValue * 2) - 1
            });

            barcode = value;

            return true;
        }

        /// <summary>
        /// 读条码(按字节条码位数读）
        /// Public Function Rbarcoder_byte(ByVal regestlen As String, ByVal regBarcoder As String, ByRef barcoder As String, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValueLength">条码长度地址</param>
        /// <param name="addressValueBarcode">条码地址</param>
        /// <param name="barcode">条码</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public bool ReadStringByBarcodeLength(string addressValueLength, string addressValueBarcode, ref string barcode, ref string errorInfo)
        {
            var lengthValue = Plc.ReadSingle<int>(new DataAddress()
            {
                Value = addressValueLength,
                Type = DataAddressType.Int,
                Offset = 0
            });

            var value = Plc.ReadSingle<string>(new DataAddress()
            {
                Value = addressValueBarcode,
                Type = DataAddressType.String,
                Offset = lengthValue - 1
            });

            barcode = value;

            return true;
        }

        /// <summary>
        /// 写条码
        /// Public Function Write_Barcode(ByVal regestName As String, ByVal BarcodeName As String, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValue">地址名</param>
        /// <param name="barcode">条码</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        /// 
        public bool WriteString(string addressValue, string barcode, ref string errorInfo)
        {
            Plc.WriteSingle<string>(new DataAddress()
            {
                Value = addressValue,
                Type = DataAddressType.String,
                Offset = barcode.Length - 1
            }, barcode);

            return true;
        }

        /// <summary>
        /// 写单个数据存储器->PLC
        /// Public Function WriteData_TCP(ByVal regestName As String, ByVal WriteData As Integer, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValue">地址名</param>
        /// <param name="value">地址值</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        /// 
        public bool WriteShort(string addressValue, int value, ref string errorInfo)
        {

            Plc.WriteSingle<short>(new DataAddress()
            {
                Value = addressValue,
                Type = DataAddressType.Int,
                Offset = 0
            }, (short)value);

            return true;
        }

        /// <summary>
        /// 写单个数据存储器(EM区)->PLC
        /// Public Function WriteData_TCP(ByVal areaName As String, ByVal regestName As String, ByVal WriteData As Integer, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValueArea">寄存器区</param>
        /// <param name="addressValueName">地址名</param>
        /// <param name="value">地址值</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public bool WriteShort(string addressValueArea, string addressValueName, int value, ref string errorInfo)
        {
            addressValueName = GetAddressValue(addressValueArea) + addressValueName;

            Plc.WriteSingle<short>(new DataAddress()
            {
                Value = addressValueName,
                Type = DataAddressType.Int,
                Offset = 0
            }, (short)value);

            return true;
        }

        /// <summary>
        /// 置位/复位一个数据存储器(16位)中的某一位
        /// Public Function SetRstBit(ByVal regestName As String, ByVal bitInd As String, ByVal BitData As Integer, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValue">地址名</param>
        /// <param name="index">位</param>
        /// <param name="value">写入数据</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        /// 
        public bool WriteBool(string addressValue, string index, int value, ref string errorInfo)
        {
            Plc.WriteSingle<bool>(new DataAddress()
            {
                Value = addressValue,
                Type = DataAddressType.Boolean,
                Offset = int.Parse(index)
            }, value == 1);

            return true;
        }

        /// <summary>
        /// 置位/复位一个数据寄存器(EM区16位)中的某一位->PLC
        /// Public Function Set_Rst_Bit_TCP(ByVal areaName As String, ByVal regestName As String, ByVal bitInd As Integer, ByVal BitData As Integer, ByRef Err As String) As Boolean
        /// </summary>
        /// <param name="addressValueArea">寄存器区</param>
        /// <param name="addressValueName">地址名</param>
        /// <param name="index">位置</param>
        /// <param name="value">写入数据</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public bool WriteBool(string addressValueArea, string addressValueName, string index, int value, ref string errorInfo)
        {
            addressValueName = GetAddressValue(addressValueArea) + addressValueName;

            Plc.WriteSingle<bool>(new DataAddress()
            {
                Value = addressValueName,
                Type = DataAddressType.Boolean,
                Offset = int.Parse(index)
            }, value == 1);

            return true;
        }

        private string GetAddressValue(string addressHead)
        {
            var addressValueArea = string.Empty;

            switch (addressHead)
            {
                case "E0":
                    addressValueArea = addressHead + "_";
                    break;
                case "E1":
                    addressValueArea = addressHead + "_";
                    break;
                case "E2":
                    addressValueArea = addressHead + "_";
                    break;
                case "E3":
                    addressValueArea = addressHead + "_";
                    break;
                default:
                    addressValueArea = addressHead;
                    break;
            }
            return addressValueArea;
        }
    }
}
