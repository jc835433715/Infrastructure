using Infrastructure.Common.Interface;
using Infrastructure.Plc.Interface;
using Infrastructure.UI;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Infrastructure.Plc.ManualSimulator
{
    partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(PlcManualSimulator plc) : this()
        {
            plc.Readindg += Plc_Reading;
            plc.Writing += Plc_Writing;

            dgvRead.CellClick += (s, e) =>
            {
                try
                {
                    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                    {
                        var row = dgvRead.Rows[e.RowIndex];
                        var readindgEventArgs = row.Tag as ReadindgEventArgs;

                        if (dgvRead[e.ColumnIndex, e.RowIndex].Value.Equals("发送"))
                        {
                            var values = row.Cells[2].Value.ToString().Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries);

                            readindgEventArgs.Result.Clear();

                            foreach (var value in values)
                            {
                                switch (readindgEventArgs.Address.Type)
                                {
                                    case DataAddressType.Boolean:
                                        {
                                            readindgEventArgs.Result.Add(value == "1");
                                        }
                                        break;
                                    case DataAddressType.Short:
                                        {
                                            readindgEventArgs.Result.Add(short.Parse(value));
                                        }
                                        break;
                                    case DataAddressType.Ushort:
                                        {
                                            readindgEventArgs.Result.Add(ushort.Parse(value));
                                        }
                                        break;
                                    case DataAddressType.Int:
                                        {
                                            readindgEventArgs.Result.Add(int.Parse(value));
                                        }
                                        break;
                                    case DataAddressType.Float:
                                        {
                                            readindgEventArgs.Result.Add(float.Parse(value));
                                        }
                                        break;
                                    case DataAddressType.String:
                                        {
                                            readindgEventArgs.Result.Add(value);
                                        }
                                        break;
                                    default: throw new NotImplementedException();
                                }
                            }

                            if (readindgEventArgs.Address.Type == DataAddressType.Boolean)
                            {
                                if (readindgEventArgs.Result.Count != 1) throw new ApplicationException("请重新输入，输入元素数须唯一");
                            }
                            else if (readindgEventArgs.Address.Type == DataAddressType.String)
                            {
                                if (readindgEventArgs.Result.Count != 1) throw new ApplicationException("请重新输入，输入元素数须唯一");
                            }
                            else
                            {
                                if (readindgEventArgs.Result.Count != readindgEventArgs.Address.Offset + 1) throw new ApplicationException($"请重新输入，输入元素数有误，当前元素数为：{readindgEventArgs.Result.Count}");
                            }

                            dgvRead.Rows.Remove(row);

                            readindgEventArgs.ManualResetEvent.Set();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxWrapper.ShowDialog(this, ex.Message, MessageCaption.Information, MessageBoxButtons.OK);
                }
            };
        }

        private void Plc_Reading(object sender, ReadindgEventArgs e)
        {
            this.InvokeEx(new Action(() =>
            {
                int index = dgvRead.Rows.Add(DateTime.Now.ToString("MM-dd HH:mm:ss:fff"), e.Address, string.Empty, "发送");

                dgvRead.Rows[index].Tag = e;

                dgvRead.FirstDisplayedScrollingRowIndex = dgvRead.RowCount - 1;
            }));
        }

        private void Plc_Writing(object sender, WritingEventArgs e)
        {
            this.InvokeEx(new Action(() =>
            {
                dgvWrite.Rows.Add(DateTime.Now.ToString("MM-dd HH:mm:ss:fff"), e.Address, string.Join(",", e.Values.Select(x => x.ToString()).ToArray()));

                dgvWrite.FirstDisplayedScrollingRowIndex = dgvWrite.RowCount - 1;
            }));
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
