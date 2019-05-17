﻿using Infrastructure.Plc.Interface;
using NUnit.Framework;
using System;
using System.Linq;

namespace Infrastructure.Plc.Opc.Tests
{
    [TestFixture()]
    public class PlcOpcByOpcUaHelperTests
    {

        [Test, Ignore("测试OPC读写方法")]
        public void ReadAndWrite()
        {
            try
            {
                var opc = new PlcOpcByOpcUaHelper("opc.tcp://10.201.88.173:4862");
                var value1 = "1";

                opc.Initialize();

                opc.Write<string>(new PlcAddress() { Value = "ns=1;s=t|MES_B4_HHJ_MES_LOGIN" }, new string[] { value1 });
                var value2 = opc.Read<string>(new PlcAddress() { Value = "ns=1;s=t|MES_B4_HHJ_MES_LOGIN" }).ToArray()[0];

                opc.Close();

                Assert.IsTrue(value1 == value2);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}