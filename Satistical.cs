using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace FinalProject
{
    public partial class Satistical : Form
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd1 = new SqlCommand();
        SqlCommand cmd2 = new SqlCommand();
        SqlDataAdapter adapter1 = new SqlDataAdapter();
        SqlDataAdapter adapter2 = new SqlDataAdapter();
        DataTable dtReceived = new DataTable();
        DataTable dtDelivery = new DataTable();
        public Satistical()
        {
            InitializeComponent();
            conn.ConnectionString = Properties.Settings.Default.conn;
            conn.Open();
            cmd1.Connection = conn;
            cmd2.Connection = conn;
        }

        private void Satistical_Load(object sender, EventArgs e)
        {
            cmd1.CommandText = "SELECT * FROM ReceivedManager";
            cmd2.CommandText = "SELECT * FROM DeliveryManager";
            adapter1.SelectCommand = cmd1;
            adapter2.SelectCommand = cmd2;
            dtDelivery.Clear();
            dtReceived.Clear();
            adapter1.Fill(dtReceived);
            adapter2.Fill(dtDelivery);
            dataGridView1.DataSource = dtReceived;
            dataGridView2.DataSource = dtDelivery;

        }
        private Boolean checkValue()
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Please Choose Month", "EMPTY", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }


        private void btnReport_Click(object sender, EventArgs e)
        {

            if (checkValue())
            {
                RevueneReport rp = new RevueneReport();
                rp.Message = comboBox1.Text;
                rp.ShowDialog();
            }
        }
    }
}
