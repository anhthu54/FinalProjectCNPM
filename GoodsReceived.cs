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
    public partial class GoodsReceived : Form
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlCommand cmd2 = new SqlCommand();
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlDataAdapter adapter2 = new SqlDataAdapter();
        DataTable dtDetail = new DataTable();
        DataTable dtForm = new DataTable();
        public GoodsReceived()
        {
            InitializeComponent();
            conn.ConnectionString = Properties.Settings.Default.conn;
            conn.Open();
            cmd.Connection = conn;
            cmd2.Connection = conn;
        }

        void Detail(string num)
        {
            try { 
            cmd.CommandText = "SELECT *,quantityReceived*PriceReceived as Total FROM [Goods Received] WHERE CodeReceived=@num";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@num", num);
            adapter.SelectCommand = cmd;
            dtDetail.Clear();
            adapter.Fill(dtDetail);
            dataGridView1.DataSource = dtDetail;
            //Calculate total price
            double Total = 0;
            for (int i = 0; i < dtDetail.Rows.Count; i++)
            {
                double count = Convert.ToDouble(dtDetail.Rows[i]["quantityReceived"]);
                double price = Convert.ToDouble(dtDetail.Rows[i]["PriceReceived"]);
                Total += count * price;
            }
            txtTotal.Text = Total.ToString();
            }
            catch { }
        }

        void Received(string num)
        {
            cmd2.CommandText = "SELECT * From ReceivedManager Where CodeReceived=@num ";
            cmd2.Parameters.Clear();
            cmd2.Parameters.AddWithValue("@num", num);
            adapter2.SelectCommand = cmd2;
            DataTable dt = new DataTable();
            adapter2.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtID.Text = dt.Rows[0]["CodeReceived"].ToString();
                txtManager.Text = dt.Rows[0]["Manager"].ToString();
                dateTimePicker1.Value = Convert.ToDateTime(dt.Rows[0]["DateReceived"]);

            }

        }

        void ListReceived()
        {
            cmd2.CommandText = "Select * FROM [ReceivedManager]";
            cmd2.Parameters.Clear();
            adapter2.SelectCommand = cmd2;
            dtForm.Clear();
            adapter2.Fill(dtForm);
        }

        private void GoodsReceived_Load(object sender, EventArgs e)
        {
            ListReceived();
            if(dtForm.Rows.Count > 0)
            {
                Detail(txtID.Text="Code101");
            }  

        }

        private void btnSee_Click(object sender, EventArgs e)
        {
            ListReceived();
            if(dtForm.Rows.Count > 0)
            {
                dataGridView1.Refresh();
                Detail(txtID.Text);
            }
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            ListReceived frm = new ListReceived();
            frm.ShowDialog();
            if(Library.codeReceived != "")
            {
                Received(Library.codeReceived);
                Detail(Library.codeReceived);

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cmd2.CommandText = "INSERT INTO [ReceivedManager](CodeReceived,Manager,DateReceived)" +
                "Values(@CodeReceived,@Manager,@DateReceived)";
            cmd2.Parameters.Clear();
            cmd2.Parameters.AddWithValue("@CodeReceived", txtID.Text);
            cmd2.Parameters.AddWithValue("@Manager", txtManager.Text);
            cmd2.Parameters.AddWithValue("@DateReceived",dateTimePicker1.Value);
            cmd2.ExecuteNonQuery();
            MessageBox.Show("Create Success" +
                "Please Fill This Detail");
            ListReceived();
            Detail(txtID.Text);
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            dataGridView1.Rows[e.Row.Index - 1].Cells["CodeReceived"].Value = txtID.Text;
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
       
                double count = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["quantityReceived"].Value);
                double price = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["PriceReceived"].Value);
                double total = count * price;
                dataGridView1.Rows[e.RowIndex].Cells["Total"].Value = total;         
                SqlCommandBuilder cb = new SqlCommandBuilder(adapter);
                adapter.Update(dtDetail);
                double Total = 0;
                for (int i = 0; i < dtDetail.Rows.Count; i++)
                {
                    double count1 = Convert.ToDouble(dtDetail.Rows[i]["quantityReceived"]);
                    double price1 = Convert.ToDouble(dtDetail.Rows[i]["PriceReceived"]);
                    Total += count1 * price1;
                }
                txtTotal.Text = Total.ToString();
            }
            catch { }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Succesfull");
            Detail(txtID.Text);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ReportReceived rp = new ReportReceived();
            rp.Message = txtID.Text;
            rp.ShowDialog();
        }
    }
}
