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
    public partial class ListReceived : Form
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adapter = new SqlDataAdapter();
        public ListReceived()
        {
            InitializeComponent();
            conn.ConnectionString = Properties.Settings.Default.conn;
            conn.Open();
            cmd.Connection = conn;
        }
        void LoadData()
        {
            cmd.CommandText = "SELECT * FROM [ReceivedManager]";
            cmd.Parameters.Clear();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void ListReceived_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                Library.codeReceived = dataGridView1.SelectedRows[0].Cells["CodeReceived"].Value.ToString();
                Close();
            }
        }
    }
}
