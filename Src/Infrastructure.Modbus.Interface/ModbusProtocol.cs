namespace Infrastructure.Modbus.Interface
{
    /// <summary>
    /// Modbus类型
    /// </summary>
    public enum ModbusType
    {
        /// <summary>
        /// Modbus主站Tcp
        /// </summary>
        ModbusMasterTcp,
        /// <summary>
        /// Modbus主站Rtu
        /// </summary>
        ModbusMasterRtu,
        /// <summary>
        /// Modbus主站Ascii
        /// </summary>
        ModbusMasterAscii
    }
}
