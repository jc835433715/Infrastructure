using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Interface;
using Infrastructure.Plc.Interface;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Infrastructure.Plc.ManualSimulator
{
    public class PlcManualSimulator : IPlc
    {
        public void Initialize()
        {
            this.form = new MainForm(this);
            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(nameof(PlcManualSimulator));

            ThreadHelper.StartThread(delegate (object o)
              {
                  Application.EnableVisualStyles();
                  Application.SetCompatibleTextRenderingDefault(false);

                  Application.Run(this.form);
              }, null, ApartmentState.STA);

            connectionStateChangedEventManager.OnConnectionStateChanged(ConnectionStateChanged, this, new ConnectionStateChangedEventArgs() { IsConnected = true });
        }

        public event EventHandler<ReadindgEventArgs> Readindg;

        public event EventHandler<WritingEventArgs> Writing;
        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public IEnumerable<TValue> Read<TValue>(DataAddress address)
        {
            var result = new TValue[] { };
            var re = new ReadindgEventArgs() { Address = address };

            Readindg(this, re);

            re.ManualResetEvent.WaitOne();

            result = re.Result.Select(e => (TValue)Convert.ChangeType(e, typeof(TValue))).ToArray();

            return result;
        }

        public void Write<TValue>(DataAddress address, IEnumerable<TValue> values)
        {
            var ea = new WritingEventArgs()
            {
                Address = address
            };

            values.ToList().ForEach(e => ea.Values.Add(e));

            Writing(this, ea);
        }

        public void Close()
        {

        }

        private MainForm form;
        private ConnectionStateChangedEventManager connectionStateChangedEventManager;
    }
}
