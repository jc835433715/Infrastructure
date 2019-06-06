using Infrastructure.Log.Factory;
using Infrastructure.Log.Interface;
using Infrastructure.Utils;
using NUnit.Framework;
using System;

namespace Infrastructure.UT.Infrastructure.Utils.Tests
{
    [TestFixture()]
    class MessageWritedEventHelperTests
    {
        [Test]
        public void MessageWritedDemoTest()
        {
            //UI层代码
            {
                MessageWritedEventHelper.MessageWrited += (s, e) =>
                {
                    if (e.LoggerName == typeof(MessageWritedEventHelperTests).FullName)
                    {
                        var items = e.Message.Split(',');

                        //添加到DataGridView，控制最大行数，避免一直添加
                        //参考代码：
                        //if (dataGridView.Rows.Count >= MaxShowRecordCount)
                        //{
                        //    dataGridView.Rows.RemoveAt(0);
                        //}

                        //dataGridView.Rows.Add(x);

                        //dataGridView.FirstDisplayedScrollingRowIndex = dataGridView.RowCount - 1;


                        Assert.AreEqual(7, items.Length);
                    }
                };
            }

            {
                LogManager.GetLogger<MessageWritedEventHelperTests>().Info($"{DateTime.Now:HH:mm:ss},离线,调用Mes时间(ms):0,无,无,无,无");
            }
        }
    }
}
