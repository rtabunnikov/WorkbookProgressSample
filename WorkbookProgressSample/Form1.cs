using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Spreadsheet;

namespace WorkbookProgressSample {
    public partial class Form1 : Form {
        CancellationTokenSource cancellationSource;

        public Form1() {
            InitializeComponent();
        }

        private async void RunCancel_Click(object sender, EventArgs e) {
            if (cancellationSource != null) {
                cancellationSource.Cancel();
            }
            else {
                pbLoad.Value = 0;
                pbExport.Value = 0;
                butRunCancel.Text = "Cancel";
                cancellationSource = new CancellationTokenSource();
                try {
                    using (Workbook workbook = new Workbook()) {
                        await workbook.LoadDocumentAsync("document.xlsx",
                            cancellationSource.Token,
                            new Progress<int>((progress) => {
                                pbLoad.Value = progress;
                                pbLoad.Refresh();
                            }));
                        await workbook.ExportToPdfAsync("result.pdf",
                            cancellationSource.Token,
                            new Progress<int>((progress) => {
                                pbExport.Value = progress;
                                pbExport.Refresh();
                            }));
                    }
                }
                catch (OperationCanceledException) {
                    pbLoad.Value = 0;
                    pbExport.Value = 0;
                }
                finally {
                    cancellationSource.Dispose();
                    cancellationSource = null;
                    butRunCancel.Text = "Run";
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            cancellationSource?.Cancel();
        }
    }
}
