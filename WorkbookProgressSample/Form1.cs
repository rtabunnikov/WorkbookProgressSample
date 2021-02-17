using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Spreadsheet;

namespace WorkbookProgressSample {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private async void butRun_Click(object sender, EventArgs e) {
            lblDone.Visible = false;
            pbLoad.Value = 0;
            pbExport.Value = 0;
            using (Workbook workbook = new Workbook()) {
                await workbook.LoadDocumentAsync("document.xlsx",
                    new Progress<int>((progress) => {
                        pbLoad.Value = progress;
                        pbLoad.Refresh();
                    }));
                await workbook.ExportToPdfAsync("result.pdf",
                    new Progress<int>((progress) => {
                        pbExport.Value = progress;
                        pbExport.Refresh();
                    }));
            }
            lblDone.Visible = true;
        }
    }
}
