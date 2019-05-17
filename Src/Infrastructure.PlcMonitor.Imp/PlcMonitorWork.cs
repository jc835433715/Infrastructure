using Infrastructure.Log.Interface;
using Infrastructure.Plc.Interface;
using Infrastructure.PlcMonitor.Interface;
using Infrastructure.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.PlcMonitor.Imp
{
    public class PlcMonitorWork<TValue> : IPlcMonitorWork
    {
        public PlcMonitorWork(IPlc plc, ILoggerFactory loggerFactory)
        {
            this.Plc = plc;
            this.loggerFactory = loggerFactory;
            this.eventInfoes = new List<EventInfo<TValue>>();
            this.tasks = new ConcurrentQueue<TaskInfo>();
            this.tokenSource = new CancellationTokenSource();
        }

        public IPlc Plc { get; private set; }

        public Type ValueType { get { return typeof(TValue); } }

        public int RegisterEventCount => eventInfoes.Count;

        public int GetPollingCount()
        {
            var value = PlcAddressSortHelper.Sort<TValue>(eventInfoes.Select(e => (e.Event as EventBase).PlcAddress).Distinct());

            return value.AddressSegments.Count() + value.AddressNotSegment.Count();
        }

        public bool Start()
        {
            tokenSource = new CancellationTokenSource();

            StartProducerThread();
            StartConsumerThread();

            return true;
        }

        public void Register<TEvent>(TEvent @event, Action<TEvent> callback)
            where TEvent : EventBase, IEvent, new()
        {
            var eventInfo = new EventInfo<TValue>()
            {
                Event = @event
            };

            eventInfo.CallbackList.Add(callback);

            if (eventInfoes.Any(e => e == eventInfo))
            {
                eventInfoes.Single(e => e == eventInfo)
                    .CallbackList.Add(callback);
            }
            else
            {
                eventInfoes.Add(eventInfo);
            }
        }

        public void Unregister<TEvent>(TEvent @event)
            where TEvent : EventBase, IEvent, new()
        {
            var eventInfo = new EventInfo<TValue>()
            {
                Event = @event
            };

            eventInfoes.RemoveAll(e => e.Equals(eventInfo));
        }

        public void Clear()
        {
            eventInfoes.Clear();
        }

        public void Stop()
        {
            tokenSource.Cancel();
        }

        private void StartProducerThread()
        {
            var valueReadEvents = eventInfoes.Where(e => e.Event is ValueReadEvent<TValue>).ToList();
            var notValueReadEvents = eventInfoes.Where(e => !(e.Event is ValueReadEvent<TValue>)).ToList();

            if (!PlcMonitorImp.IsPlcManualSimulator)
            {
                Task.Factory.StartNew(() =>
                {
                    var eventInfoes = notValueReadEvents;

                    while (!tokenSource.Token.IsCancellationRequested)
                    {
                        try
                        {
                            ProduceTaskAction(Plc, eventInfoes).ForEach(e => tasks.Enqueue(e));
                        }
                        catch (Exception e)
                        {
                            loggerFactory?.GetLogger<PlcMonitorWork<TValue>>().Error(e.ToString());
                        }
                        finally
                        {
                            ThreadHelper.Sleep(1);
                        }
                    }
                }, TaskCreationOptions.LongRunning);
            }
            else
            {
                notValueReadEvents.ForEach(notValueReadEvent =>
                {
                    Task.Factory.StartNew(state =>
                    {
                        var eventInfoes = new List<EventInfo<TValue>> { state as EventInfo<TValue> };

                        while (!tokenSource.Token.IsCancellationRequested)
                        {
                            try
                            {
                                ProduceTaskAction(Plc, eventInfoes).ForEach(e => tasks.Enqueue(e));
                            }
                            catch (Exception e)
                            {
                                loggerFactory?.GetLogger<PlcMonitorWork<TValue>>().Error(e.ToString());
                            }
                        }
                    }, notValueReadEvent, TaskCreationOptions.LongRunning);
                });
            }

            valueReadEvents.ForEach(valueReadEvent =>
            {
                Task.Factory.StartNew(state =>
                {
                    var eventInfoes = new List<EventInfo<TValue>> { state as EventInfo<TValue> };

                    while (!tokenSource.Token.IsCancellationRequested)
                    {
                        try
                        {
                            ProduceTaskAction(Plc, eventInfoes).ForEach(e => tasks.Enqueue(e));
                        }
                        catch (Exception e)
                        {
                            loggerFactory?.GetLogger<PlcMonitorWork<TValue>>().Error(e.ToString());
                        }
                        finally
                        {
                            ThreadHelper.Sleep((int)(eventInfoes.Single().Event as ValueReadEvent<TValue>).Period.TotalMilliseconds);
                        }
                    }
                }, valueReadEvent, TaskCreationOptions.LongRunning);
            });
        }

        private void StartConsumerThread()
        {
            Task.Factory.StartNew(() =>
            {
                while (!tokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        ConsumeTaskAction(tasks);
                    }
                    catch (Exception e)
                    {
                        loggerFactory?.GetLogger<PlcMonitorWork<TValue>>().Error(e.ToString());
                    }
                    finally
                    {
                        ThreadHelper.Sleep(1);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        private List<TaskInfo> ProduceTaskAction(IPlc plc, List<EventInfo<TValue>> eventInfoes)
        {
            var result = new List<TaskInfo>();
            List<Tuple<PlcAddress, TValue>> values = ScanAddress(plc, eventInfoes);

            eventInfoes.ForEach(eventInfo =>
            {
                eventInfo.CurrentValue = values.Single(e => e.Item1.Equals((eventInfo.Event as EventBase).PlcAddress)).Item2;

                result.AddRange(ProduceTask(eventInfo));
            });

            return result;
        }

        private void ConsumeTaskAction(ConcurrentQueue<TaskInfo> tasks)
        {
            while (tasks.TryDequeue(out TaskInfo callbackInfo))
            {
                Task.Factory.StartNew(o =>
                {
                    TaskInfo ci = o as TaskInfo;

                    if (!(callbackInfo.Event is IEventStatus))
                    {
                        ExcuteTask(ci);
                    }
                    else
                    {
                        var @event = ci.Event as IEventStatus;

                        while (!tokenSource.Token.IsCancellationRequested && !@event.IsHandleCompleted)
                        {
                            ExcuteTask(ci);
                        }
                    }
                },
                callbackInfo, TaskCreationOptions.LongRunning | TaskCreationOptions.PreferFairness).ContinueWith(t =>
                {
                    loggerFactory?.GetLogger<PlcMonitorWork<TValue>>().Error(t.Exception.GetBaseException().ToString());
                }, TaskContinuationOptions.OnlyOnFaulted);
            }
        }

        private List<Tuple<PlcAddress, TValue>> ScanAddress(IPlc plc, List<EventInfo<TValue>> eventInfoes)
        {
            var result = new List<Tuple<PlcAddress, TValue>>();
            SortResult sortResult = InMemoryCache.GetOrAdd<SortResult>(eventInfoes.GetHashCode().ToString(), key =>
            {
                var r = new SortResult();
                var distinctPlcAddresses = eventInfoes.Select(e => (e.Event as EventBase).PlcAddress).Distinct();

                r.AddressNotSegment.Addresses = distinctPlcAddresses.ToList();

                if (!PlcMonitorImp.IsPlcManualSimulator)
                {
                    r = PlcAddressSortHelper.Sort<TValue>(distinctPlcAddresses);
                }

                return r;
            });

            sortResult.AddressSegments.ForEach(addressSegment =>
            {
                var startAddress = addressSegment.StartAddress;
                List<TValue> values;

                startAddress.Offset = addressSegment.AllAddressesByDes.Count - 1;

                values = plc.Read<TValue>(startAddress).ToList();

                for (var index = 0; index < values.Count(); index++)
                {
                    result.Add(new Tuple<PlcAddress, TValue>(addressSegment.AllAddressesByDes[index], values[index]));
                }
            });

            sortResult.AddressNotSegment.Addresses.ForEach(address =>
            {
                var value = plc.ReadSingle<TValue>(address);

                result.Add(new Tuple<PlcAddress, TValue>(address, value));
            });

            return result;
        }

        private List<TaskInfo> ProduceTask(EventInfo<TValue> eventInfo)
        {
            var result = new List<TaskInfo>();
            var valueReadEvent = eventInfo.Event as ValueReadEvent<TValue>;

            result.AddRange(ProduceTask<EndValueReadEvent<TValue>>(eventInfo, evt => !eventInfo.LastValue.Equals(evt.EndValue) && eventInfo.CurrentValue.Equals(evt.EndValue)));
            result.AddRange(ProduceTask<EndValueReadEventStatus<TValue>>(eventInfo, evt => !eventInfo.LastValue.Equals(evt.EndValue) && eventInfo.CurrentValue.Equals(evt.EndValue)));
            result.AddRange(ProduceTask<FromStartValueToEndValueReadEvent<TValue>>(eventInfo, evt => eventInfo.LastValue.Equals(evt.StartValue) && eventInfo.CurrentValue.Equals(evt.EndValue)));
            result.AddRange(ProduceTask<NotStartValueReadEvent<TValue>>(eventInfo, evt =>
            {
                var r = eventInfo.LastValue.Equals(evt.StartValue) && !eventInfo.CurrentValue.Equals(evt.StartValue);

                evt.CurrentValue = eventInfo.CurrentValue;

                return r;
            }));
            result.AddRange(ProduceTask<ValueReadChangedEvent<TValue>>(eventInfo, evt =>
            {
                var r = !eventInfo.LastValue.Equals(eventInfo.CurrentValue);

                evt.CurrentValue = eventInfo.CurrentValue;

                return r;
            }));
            result.AddRange(ProduceTask<ValueReadEvent<TValue>>(eventInfo, evt => evt.Predicate(eventInfo.CurrentValue)));

            eventInfo.LastValue = eventInfo.CurrentValue;

            return result;
        }

        private List<TaskInfo> ProduceTask<TEvent>(EventInfo<TValue> eventInfo, Func<TEvent, bool> func)
            where TEvent : EventBase, IEvent, new()
        {
            var result = new List<TaskInfo>();
            var evt = eventInfo.Event as TEvent;

            if (evt != null && func(evt))
            {
                eventInfo.CallbackList.ForEach(e => result.Add(new TaskInfo(evt.Clone(), e)));
            }

            return result;
        }

        private void ExcuteTask(TaskInfo ci)
        {
            ExcuteTask<EndValueReadEvent<TValue>>(ci);
            ExcuteTask<EndValueReadEventStatus<TValue>>(ci);
            ExcuteTask<FromStartValueToEndValueReadEvent<TValue>>(ci);
            ExcuteTask<NotStartValueReadEvent<TValue>>(ci);
            ExcuteTask<ValueReadChangedEvent<TValue>>(ci);
            ExcuteTask<ValueReadEvent<TValue>>(ci);
        }

        private void ExcuteTask<TEvent>(TaskInfo ci)
            where TEvent : EventBase, IEvent, new()
        {
            var evt = ci.Event as TEvent;

            if (evt != null) ci.Callback(evt);
        }

        private CancellationTokenSource tokenSource;
        private ConcurrentQueue<TaskInfo> tasks;
        private List<EventInfo<TValue>> eventInfoes;
        private readonly ILoggerFactory loggerFactory;
    }
}