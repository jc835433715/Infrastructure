using Infrastructure.BarcodeReader.Interface;
using Infrastructure.Common.Interface;
using Infrastructure.ComPort.Interface;
using Infrastructure.Config.Interface;
using Infrastructure.Serialization.Interface;
using Infrastructure.UI;
using Infrastructure.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Infrastructure.BarcodeReader.Imp
{
    public partial class UIBarcodeReader : UserControl, IBarcodeReader
    {
        class State
        {
            public State()
            {
                this.Enabled = true;
                this.Barcodes = new string[] { };
            }

            public bool Enabled { get; set; }

            public string[] Barcodes { get; set; }
        }

        class CollectionDecorator : ICollection<string>
        {
            public CollectionDecorator(ICollection<string> collection, DataGridView dataGridView, Action saveStateAction)
            {
                this.collection = collection;
                this.dataGridView = dataGridView;
                this.saveStateAction = saveStateAction;
            }

            public int Count => collection.Count;

            public bool IsReadOnly => collection.IsReadOnly;

            public void Add(string item)
            {
                collection.Add(item);

                saveStateAction();
                RefreshDataGrid();
            }

            public bool Remove(string item)
            {
                var result = collection.Remove(item);

                saveStateAction();
                RefreshDataGrid();

                return result;
            }

            public void Clear()
            {
                collection.Clear();

                saveStateAction();
                RefreshDataGrid();
            }

            public bool Contains(string item)
            {
                return collection.Contains(item);
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                collection.CopyTo(array, arrayIndex);
            }

            public IEnumerator<string> GetEnumerator()
            {
                return collection.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return collection.GetEnumerator();
            }

            private void RefreshDataGrid()
            {
                dataGridView.InvokeEx(new Action(() =>
                {
                    dataGridView.Rows.Clear();

                    for (int index = 0; index < collection.Count; index++)
                    {
                        dataGridView.Rows.Add((index + 1).ToString("00"), collection.ToArray()[index]);
                    }

                    if (dataGridView.RowCount > 0)
                    {
                        dataGridView.FirstDisplayedScrollingRowIndex = dataGridView.RowCount - 1;
                    }
                }));
            }

            private readonly ICollection<string> collection;
            private readonly DataGridView dataGridView;
            private readonly Action saveStateAction;
        }

        static UIBarcodeReader()
        {
            converter = new Infrastructure.Serialization.Newtonsoft.JsonConverter();
        }

        public UIBarcodeReader()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            this.terminator = (char)0x0D;

            this.connectionStateChangedEventManager = new ConnectionStateChangedEventManager(string.Empty);
        }

        public UIBarcodeReader(string name, char terminator = (char)0x0D, string regex = null) : this()
        {
            this.Name = name;
            this.terminator = terminator;
            this.regex = new Regex(regex ?? string.Empty);
            this.barcodes = new CollectionDecorator(new List<string>(), dgvBarcode, SaveState);
            this.connectionStateChangedEventManager.ConnectionName = name;
            this.filePath = $@".\AppData\UIBarcodeReader-{Name}.sta";

            if (AppConfigHelper.GetCurrentDomainData(ConfigConst.LocalDataDirectory) != null)
            {
                filePath = $@"{AppConfigHelper.GetCurrentDomainData(ConfigConst.LocalDataDirectory)}\UIBarcodeReader-{Name}.sta";
            }

            this.Load += (s, e) => LoadState();
        }

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        public event EventHandler<BarcodeReaderReceivedEventArgs> BarcoderReceived;

        public string BarcodeReaderName
        {
            get
            {
                return this.gb.Text;
            }
            set
            {
                this.gb.Text = value;
            }
        }

        public string Read()
        {
            var result = barcodes.FirstOrDefault();

            if (string.IsNullOrEmpty(result))
            {
                result = BarcodeConst.ErrorBarcode;
            }
            else
            {
                barcodes.Remove(result);
            }

            if (!cbEnabled.Checked)
            {
                result = BarcodeConst.EmptyBarcode;
            }

            return result;
        }

        private void tbxBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == terminator)
            {
                var barcoderReceivedEventArgs = new BarcodeReaderReceivedEventArgs();

                barcoderReceivedEventArgs.Barcode = tbxBarcode.Text.TrimEnd(terminator);

                try
                {
                    if (regex != null && !regex.IsMatch(barcoderReceivedEventArgs.Barcode)) barcoderReceivedEventArgs.Barcode = BarcodeConst.ErrorBarcode;
                }
                catch
                {
                    barcoderReceivedEventArgs.Barcode = BarcodeConst.ErrorBarcode;
                }

                OnBarcoderReceived(this, barcoderReceivedEventArgs);

                if (barcoderReceivedEventArgs.ConfirmResult == ConfirmResult.OK)
                {
                    barcodes.Add(barcoderReceivedEventArgs.Barcode);
                }

                if (barcoderReceivedEventArgs.ConfirmResult == ConfirmResult.NG)
                {
                    MessageBoxWrapper.ShowDialog(this, $"条码校验出错,错误信息：“{barcoderReceivedEventArgs.ConfirmMessage  }”,请确认！", MessageCaption.Information, MessageBoxButtons.OK);
                }

                tbxBarcode.Text = string.Empty;
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.tbxBarcode.Text))
            {
                tbxBarcode_KeyPress(this.tbxBarcode, new KeyPressEventArgs(terminator));
            }
        }

        private void OnBarcoderReceived(object sender, EventArgs e)
        {
            if (BarcoderReceived != null)
            {
                foreach (var del in BarcoderReceived.GetInvocationList())
                {
                    del.DynamicInvoke(this, e);
                }
            }
        }

        private void tsmDelete_Click(object sender, EventArgs e)
        {
            var rows = dgvBarcode.SelectedRows;

            if (rows.Count > 0)
            {
                barcodes.Remove(rows[0].Cells[1].Value.ToString());
            }
        }

        private void cbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            this.tbxBarcode.Enabled = cbEnabled.Checked;

            if (!cbEnabled.Checked)
            {
                barcodes.Clear();
            }

            connectionStateChangedEventManager.OnConnectionStateChanged(ConnectionStateChanged, this, new ConnectionStateChangedEventArgs() { IsConnected = cbEnabled.Checked });

            SaveState();
        }

        private void SaveState()
        {
            FileSystemHelper.CreateDirectory(filePath);

            File.WriteAllText(filePath, converter.Serialize(new State()
            {
                Enabled = cbEnabled.Checked,
                Barcodes = barcodes.ToArray()
            }));
        }

        private void LoadState()
        {
            var state = new State();

            if (FileSystemHelper.IsFileExists(filePath))
            {
                string value = File.ReadAllText(filePath);

                state = converter.Deserialize<State>(value);
            }

            cbEnabled.Checked = state.Enabled;

            state.Barcodes.ToList().ForEach(e => barcodes.Add(e));
        }

        private static IConverter converter;
        private string filePath;
        private ICollection<string> barcodes;
        private Regex regex;
        private char terminator;
        private ConnectionStateChangedEventManager connectionStateChangedEventManager;
    }
}
