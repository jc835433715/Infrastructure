using Infrastructure.Log.Interface;
using Infrastructure.Plc.Interface;
using Infrastructure.PlcMonitor.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.PlcMonitor.Imp
{
    public class PlcMonitorImp : IPlcMonitor
    {
        static PlcMonitorImp()
        {
            IsPlcManualSimulator = false;
            MaxPollingCount = 10;
        }

        public PlcMonitorImp(IPlc plc, ILoggerFactory loggerFactory = null)
        {
            this.Plc = plc;
            this.loggerFactory = loggerFactory;
            this.plcMonitorWorks = new List<IPlcMonitorWork>();
        }

        public static bool IsPlcManualSimulator { get; set; }

        public static int MaxPollingCount { get; set; }

        public IPlc Plc
        {
            get;
            private set;
        }

        public bool Start()
        {
            return plcMonitorWorks.All(e => e.Start());
        }

        public void Register<TEvent>(TEvent @event, Action<TEvent> callback)
            where TEvent : EventBase, IEvent, new()
        {
            IPlcMonitorWork plcMonitorWork = null;

            if (@event.ValueType == typeof(bool))
            {
                plcMonitorWork = AddPlcMonitorWork<bool>();
            }
            else if (@event.ValueType == typeof(short))
            {
                plcMonitorWork = AddPlcMonitorWork<short>();
            }
            else if (@event.ValueType == typeof(ushort))
            {
                plcMonitorWork = AddPlcMonitorWork<ushort>();
            }
            else if (@event.ValueType == typeof(int))
            {
                plcMonitorWork = AddPlcMonitorWork<int>();
            }
            else if (@event.ValueType == typeof(float))
            {
                plcMonitorWork = AddPlcMonitorWork<float>();
            }
            else if (@event.ValueType == typeof(string))
            {
                plcMonitorWork = AddPlcMonitorWork<string>();
            }
            else
            {
                throw new NotImplementedException();
            }

            plcMonitorWork.Register(@event, callback);
        }

        public void Unregister<TEvent>(TEvent @event)
            where TEvent : EventBase, IEvent, new()
        {
            if (@event.ValueType == typeof(bool))
            {
                Unregister<bool, TEvent>(@event);
            }
            else if (@event.ValueType == typeof(short))
            {
                Unregister<short, TEvent>(@event);
            }
            else if (@event.ValueType == typeof(ushort))
            {
                Unregister<ushort, TEvent>(@event);
            }
            else if (@event.ValueType == typeof(int))
            {
                Unregister<int, TEvent>(@event);
            }
            else if (@event.ValueType == typeof(float))
            {
                Unregister<float, TEvent>(@event);
            }
            else if (@event.ValueType == typeof(string))
            {
                Unregister<string, TEvent>(@event);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Clear()
        {
            plcMonitorWorks.ForEach(e => e.Clear());
        }

        public void Stop()
        {
            plcMonitorWorks.ForEach(e => e.Stop());
            plcMonitorWorks.RemoveAll(e => e.RegisterEventCount == 0);
        }

        private IPlcMonitorWork AddPlcMonitorWork<TValue>()
        {
            IPlcMonitorWork plcMonitorWork = GetPlcMonitorWork<TValue>();

            if (plcMonitorWork == null)
            {
                plcMonitorWork = new PlcMonitorWork<TValue>(Plc, loggerFactory);

                plcMonitorWorks.Add(plcMonitorWork);
            }

            return plcMonitorWork;
        }

        private IPlcMonitorWork GetPlcMonitorWork<TValue>()
        {
            return plcMonitorWorks.SingleOrDefault(e => e.ValueType == typeof(TValue) && e.GetPollingCount() < MaxPollingCount);
        }

        private void Unregister<TValue, TEvent>(TEvent @event)
            where TEvent : EventBase, IEvent, new()
        {
            plcMonitorWorks.Where(e => e.ValueType == typeof(TValue)).ToList()
                .ForEach(e =>
                {
                    e.Unregister(@event);
                });
        }

        private List<IPlcMonitorWork> plcMonitorWorks;
        private readonly ILoggerFactory loggerFactory;
    }
}
