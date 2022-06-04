using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinalProject
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void goodsReceivedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoodsReceived goodsReceived = new GoodsReceived();
            goodsReceived.ShowDialog();
        }

        private void goodsDeliveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoodsDelivery goodsDelivery = new GoodsDelivery();
            goodsDelivery.ShowDialog();
        }

        private void statisticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Satistical  satistical = new Satistical();
            satistical.ShowDialog();
        }
    }
}
