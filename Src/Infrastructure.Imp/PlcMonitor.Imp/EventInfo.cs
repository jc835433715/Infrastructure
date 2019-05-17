using System;
using System.Collections.Generic;

namespace Infrastructure.PlcMonitor.Imp
{
    public class EventInfo<TValue>
    {
        public EventInfo()
        {
            this.Event = new object();
            this.CurrentValue = GetDefaultValue();
            this.LastValue = GetDefaultValue();

            this.CallbackList = new List<dynamic>();
        }

        public TValue CurrentValue { get; set; }

        public TValue LastValue { get; set; }

        public object Event { get; set; }

        public List<dynamic> CallbackList { get; private set; }

        public override bool Equals(object obj)
        {
            bool result = false;
            EventInfo<TValue> other = obj as EventInfo<TValue>;

            result = object.ReferenceEquals(this, other) || (Event.Equals(other.Event));

            return result;
        }

        public override int GetHashCode()
        {
            return Event.GetHashCode();
        }

        public TValue GetDefaultValue()
        {
            if (typeof(TValue) == typeof(bool))
            {
                return (TValue)Convert.ChangeType(false, typeof(TValue));
            }
            else if (typeof(TValue) == typeof(short))
            {
                return (TValue)Convert.ChangeType(short.MaxValue, typeof(TValue));
            }
            else if (typeof(TValue) == typeof(ushort))
            {
                return (TValue)Convert.ChangeType(ushort.MaxValue, typeof(TValue));
            }
            else if (typeof(TValue) == typeof(int))
            {
                return (TValue)Convert.ChangeType(int.MaxValue, typeof(TValue));
            }
            else if (typeof(TValue) == typeof(float))
            {
                return (TValue)Convert.ChangeType(float.MaxValue, typeof(TValue));
            }
            else if (typeof(TValue) == typeof(string))
            {
                return (TValue)Convert.ChangeType("DefaultValue", typeof(TValue));
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
