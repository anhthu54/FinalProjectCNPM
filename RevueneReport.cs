using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;

namespace FinalProject
{
    public partial class RevueneReport : Form
    {
        string strReceive;
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();  
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable dt = new DataTable();
        public RevueneReport()
        {
            InitializeComponent();
            conn.ConnectionString = Properties.Settings.Default.conn;
            conn.Open();
            cmd.Connection = conn;
        }
        public string Message
        {
            get { return strReceive; }
            set { strReceive = value; }
        }

        private void RevueneReport_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.ReportEmbeddedResource = "FinalProject.RevueneReport.rdlc";
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";


            int i = int.Parse(strReceive);

            cmd.CommandText = "SELECT * FROM RevenueReport WHERE MONTH(DateDelivery) = " + i + " OR MONTH(DateReceived)=" + i + "";
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                reportDataSource.Value = dt;
                reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            }
            else
            {
                MessageBox.Show("No data");
            }
            this.reportViewer1.RefreshReport();

        }
    }
}
