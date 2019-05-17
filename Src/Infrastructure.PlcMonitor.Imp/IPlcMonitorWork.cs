using Infrastructure.PlcMonitor.Interface;
using System;

namespace Infrastructure.PlcMonitor.Imp
{
    public interface IPlcMonitorWork : IPlcMonitor
    {
        Type ValueType { get; }

        int RegisterEventCount { get; }

        int GetPollingCount();
    }
}