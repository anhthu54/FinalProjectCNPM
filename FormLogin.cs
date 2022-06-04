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
    public partial class FormLogin : Form
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter adapter = new SqlDataAdapter();
        public FormLogin()
        {
            InitializeComponent();
            conn.ConnectionString = Properties.Settings.Default.conn;
            conn.Open();
            cmd.Connection = conn;
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            cmd.CommandText = "SELECT Username, Password FROM LoginManager WHERE " +
                        "Username='" + txtUser.Text + "' and Password='" + txtPass.Text + "'";
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            Main main = new Main();
            if(dt.Rows.Count > 0)
            {
                MessageBox.Show("Successful");
                main.ShowDialog();
            }
            else
            {
                MessageBox.Show("Invalid");
            }
            conn.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Thank you. Click OK to exit.");
            Application.Exit();
        }
    }
}
