using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    public partial class frmItem : Form
    {
        public frmItem()
        {
            InitializeComponent();
            LoadData();
        }

        public void LoadData()
        {
            dgItems.Rows.Clear();
            
            DataTable dt = new DataTable();
            dt = DB.FillData("Select * from item;", dt);

            foreach (DataRow dr in dt.Rows)
            {
                dgItems.Rows.Add(dr[0], dr[2], dr[1], dr[3]);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int l = 0, p = 0, i = 0;
            string d = txtDesc.Text;
            if (lbID.Text == "")
            {
                if (txtLookup.Text == "" || !int.TryParse(txtLookup.Text, out l) || !int.TryParse(txtPrice.Text, out p) || txtDesc.Text == "" || txtPrice.Text == "")
                {
                    MessageBox.Show("Invalid or Empty Values!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DB.InsertData("Insert into item (desc, lookup, price) values (\"" + d + "\", " + l + ", " + p + ");");
                btnClear.PerformClick();
                LoadData();
            }
            else
            {
                i = int.Parse(lbID.Text);
                if (txtLookup.Text == "" || !int.TryParse(txtLookup.Text, out l) || !int.TryParse(txtPrice.Text, out p) || txtDesc.Text == "" || txtPrice.Text == "")
                {
                    MessageBox.Show("Invalid or Empty Values!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DB.InsertData("Update item set desc = \"" + d + "\", lookup = " + l + ", price = " + p + " where id = " + i + ";");
                btnClear.PerformClick();
                LoadData();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lbID.Text = "";
            txtDesc.Text = "";
            txtLookup.Text = "";
            txtPrice.Text = "";
            txtLookup.Focus();
        }

        private void dgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgItems.Columns[e.ColumnIndex].Name;
            if (col == "Modify")
            {
                lbID.Text = dgItems[0, e.RowIndex].Value.ToString();
                txtLookup.Text = dgItems[1, e.RowIndex].Value.ToString();
                txtDesc.Text = dgItems[2, e.RowIndex].Value.ToString();
                txtPrice.Text = dgItems[3, e.RowIndex].Value.ToString();
                txtLookup.Focus();
            }
            else if (col == "Delete")
            {
                DB.InsertData("Delete from item where id = " + dgItems[0, e.RowIndex].Value.ToString() + ";");
                LoadData();
                txtLookup.Focus();
            }
        }
    }
}
