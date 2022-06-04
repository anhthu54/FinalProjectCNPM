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
    public partial class ReportReceived : Form
    {
        string strReceive;
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable dt = new DataTable();
        public ReportReceived()
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

        private void ReportReceived_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.ReportEmbeddedResource = "FinalProject.ReportReceived.rdlc";
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            MessageBox.Show(strReceive);



            cmd.CommandText = "SELECT * FROM [Goods Received] WHERE CodeReceived=@strReceive";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@strReceive",strReceive);
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
