using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Plc.Interface
{
    /// <summary>
    /// 地址排序帮助者
    /// </summary>
    public static class PlcAddressSortHelper
    {
        /// <summary>
        /// 排序,须为同类型地址
        /// </summary>
        /// <typeparam name="TValue">地址类型</typeparam>
        /// <param name="distinctPlcAddresses">去重后的地址</param>
        /// <returns>排序结果</returns>
        public static SortResult Sort<TValue>(IEnumerable<PlcAddress> distinctPlcAddresses)
        {
            var result = new SortResult();
            var query = from address in distinctPlcAddresses
                        group address by address.Value.Substring(0, 2) into g
                        select new
                        {
                            Value = from item in g
                                    let value = int.Parse(item.Value.Replace(g.Key, string.Empty).Replace("_", string.Empty))
                                    orderby value ascending
                                    select new { Address = item, Value = value }
                        };

            foreach (var item in query)
            {
                var value = item.Value.ToList();
                PlcAddressSegment plcAddressSegment = null;

                for (int compareTime = 0; compareTime < value.Count() - 1; compareTime++)
                {
                    plcAddressSegment = plcAddressSegment ?? new PlcAddressSegment();

                    if (value[compareTime + 1].Value - value[compareTime].Value == GetIntervalWordCount(typeof(TValue)))
                    {
                        if (plcAddressSegment.StartAddress.Equals(PlcAddress.Empty))
                        {
                            plcAddressSegment.StartAddress = value[compareTime].Address;
                            plcAddressSegment.AllAddressesByDes.Add(value[compareTime].Address);
                        }

                        plcAddressSegment.AllAddressesByDes.Add(value[compareTime + 1].Address);
                    }

                    if (value[compareTime + 1].Value - value[compareTime].Value != GetIntervalWordCount(typeof(TValue))
                        || compareTime == value.Count() - 2)
                    {
                        if (plcAddressSegment.AllAddressesByDes.Any())
                        {
                            result.AddressSegments.Add(new PlcAddressSegment()
                            {
                                StartAddress = plcAddressSegment.StartAddress,
                                AllAddressesByDes = plcAddressSegment.AllAddressesByDes
                            });

                            plcAddressSegment = null;
                        }
                    }
                }
            }

            result.AddressNotSegment.Addresses.AddRange(distinctPlcAddresses.Except(new Func<IEnumerable<PlcAddress>>(
               () =>
                               {
                                   var r = new List<PlcAddress>();

                                   result.AddressSegments.ForEach(e => r.AddRange(e.AllAddressesByDes));

                                   return r;
                               }).Invoke()));


            return result;
        }

        /// <summary>
        /// 获取字间隔
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>间隔数</returns>
        public static int GetIntervalWordCount(Type type)
        {
            var result = int.MaxValue;

            if (type == typeof(bool))
            {
                result = int.MaxValue;
            }
            else if (type == typeof(short))
            {
                result = 1;
            }
            else if (type == typeof(ushort))
            {
                result = 1;
            }
            else if (type == typeof(int))
            {
                result = 2;
            }
            else if (type == typeof(float))
            {
                result = 2;
            }
            else if (type == typeof(string))
            {
                result = int.MaxValue;
            }
            else
            {
                throw new NotImplementedException();
            }

            return result;
        }
    }

    /// <summary>
    /// 排序结果
    /// </summary>
    public class SortResult
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public SortResult()
        {
            this.AddressSegments = new List<PlcAddressSegment>();
            this.AddressNotSegment = new PlcAddressNotSegment();
        }

        /// <summary>
        /// 连续段地址
        /// </summary>
        public List<PlcAddressSegment> AddressSegments { get; set; }

        /// <summary>
        /// 非连续段地址
        /// </summary>
        public PlcAddressNotSegment AddressNotSegment { get; set; }
    }

    /// <summary>
    /// 连续段地址
    /// </summary>
    public class PlcAddressSegment
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public PlcAddressSegment()
        {
            this.StartAddress = PlcAddress.Empty;
            this.AllAddressesByDes = new List<PlcAddress>();
        }

        /// <summary>
        /// 开始地址
        /// </summary>
        public PlcAddress StartAddress { get; set; }

        /// <summary>
        /// 所有地址
        /// </summary>
        public List<PlcAddress> AllAddressesByDes { get; set; }
    }

    /// <summary>
    /// 非连续段地址
    /// </summary>
    public class PlcAddressNotSegment
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public PlcAddressNotSegment()
        {
            this.Addresses = new List<PlcAddress>();
        }

        /// <summary>
        /// 地址
        /// </summary>
        public List<PlcAddress> Addresses { get; set; }

        /// <summary>
        /// 地址数
        /// </summary>
        public int Count() => Addresses.Count;
    }
}
