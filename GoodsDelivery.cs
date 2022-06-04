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
    public partial class GoodsDelivery : Form
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlCommand cmd2 = new SqlCommand();
        SqlDataAdapter adapter = new SqlDataAdapter();
        SqlDataAdapter adapter2 = new SqlDataAdapter();
        DataTable dtDetail = new DataTable();
        DataTable dtForm = new DataTable();
        public GoodsDelivery()
        {

            InitializeComponent();
            conn.ConnectionString = Properties.Settings.Default.conn;
            conn.Open();
            cmd.Connection = conn;
            cmd2.Connection = conn;
        }

        void Detail(string num)
        {
            cmd.CommandText = "SELECT *,CAST(quantity*PriceDelivery as BIGINT) as Total FROM [Goods Delivery] WHERE CodeDelivery=@num";
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
                double count = Convert.ToDouble(dtDetail.Rows[i]["quantity"]);
                double price = Convert.ToDouble(dtDetail.Rows[i]["PriceDelivery"]);
                Total += count * price;
            }
            txtTotal.Text = Total.ToString();
        }
        void Delivery(string num)
        {
            cmd2.CommandText = "SELECT * From DeliveryManager Where CodeDelivery=@num ";
            cmd2.Parameters.Clear();
            cmd2.Parameters.AddWithValue("@num", num);
            adapter2.SelectCommand = cmd2;
            DataTable dt = new DataTable();
            adapter2.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtID.Text = dt.Rows[0]["CodeDelivery"].ToString();
                txtManager.Text = dt.Rows[0]["Manager"].ToString();
                txtCustomer.Text = dt.Rows[0]["CustomerName"].ToString();
                dateTimePicker1.Value = Convert.ToDateTime(dt.Rows[0]["DateDelivery"]);
                txtMethod.Text = dt.Rows[0]["paymethod"].ToString();
                txtStatus.Text = dt.Rows[0]["StatusDelivery"].ToString();
            }

        }

        void ListDelivery()
        {
            cmd2.CommandText = "Select * FROM [DeliveryManager]";
            cmd2.Parameters.Clear();
            adapter2.SelectCommand = cmd2;
            dtForm.Clear();
            adapter2.Fill(dtForm);
        }

        private void GoodsDelivery_Load(object sender, EventArgs e)
        {
            ListDelivery();
            if (dtForm.Rows.Count > 0)
            {
                Detail(txtID.Text = "Delivery201");
            }

        }

        private void btnSee_Click(object sender, EventArgs e)
        {
            ListDelivery();
            if (dtForm.Rows.Count > 0)
            {
                dataGridView1.Refresh();
                Detail(txtID.Text);
            }
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            ListDelivery frm = new ListDelivery();
            frm.ShowDialog();
            if (Library.codeDelivery != "")
            {
                Delivery(Library.codeDelivery);
                Detail(Library.codeDelivery);

            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cmd2.CommandText = "INSERT INTO [DeliveryManager](CodeDelivery,Manager,CustomerName,DateDelivery,paymethod,StatusDelivery)" +
                "Values(@CodeDelivery,@Manager,@CustomerName,@DateDelivery,@paymethod,@StatusDelivery)";
            cmd2.Parameters.Clear();
            cmd2.Parameters.AddWithValue("@CodeDelivery", txtID.Text);
            cmd2.Parameters.AddWithValue("@Manager", txtManager.Text);
            cmd2.Parameters.AddWithValue("@CustomerName", txtCustomer.Text);
            cmd2.Parameters.AddWithValue("@DateDelivery", dateTimePicker1.Value);
            cmd2.Parameters.AddWithValue("@paymethod", txtMethod.Text);
            cmd2.Parameters.AddWithValue("@StatusDelivery", txtStatus.Text);
            cmd2.ExecuteNonQuery();
            MessageBox.Show("Create Success" +
                "Please Fill This Detail");
            ListDelivery();
            Detail(txtID.Text);
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {

            dataGridView1.Rows[e.Row.Index - 1].Cells["CodeDelivery"].Value = txtID.Text;
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                double count = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["quantity"].Value);
                double price = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["PriceDelivery"].Value);
                double total = count * price;
                dataGridView1.Rows[e.RowIndex].Cells["Total"].Value = total;
                SqlCommandBuilder cb = new SqlCommandBuilder(adapter);
                adapter.Update(dtDetail);
                double Total = 0;
                for (int i = 0; i < dtDetail.Rows.Count; i++)
                {
                    double count1 = Convert.ToDouble(dtDetail.Rows[i]["quantity"]);
                    double price1 = Convert.ToDouble(dtDetail.Rows[i]["PriceDelivery"]);
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
            ReportDelivery rp = new ReportDelivery();
            rp.Message = txtID.Text;
         
            rp.ShowDialog();
        }
    }
}
