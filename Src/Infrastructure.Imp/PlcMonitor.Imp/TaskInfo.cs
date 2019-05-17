namespace Infrastructure.PlcMonitor.Imp
{
    class TaskInfo
    {
        public TaskInfo(object @event, dynamic callback)
        {
            this.Event = @event;
            this.Callback = callback;
        }

        public object Event { get; set; }

        public dynamic Callback { get; set; }
    }
}
