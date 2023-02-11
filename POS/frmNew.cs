using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    public partial class frmNew : Form
    {
        DataTable dt;

        public frmNew()
        {
            InitializeComponent();
            dt = new DataTable();
            dt.Columns.Add(new DataColumn());
            dt.Columns.Add(new DataColumn());
            dt.Columns.Add(new DataColumn());
            dt.Columns.Add(new DataColumn());
            dt.Columns.Add(new DataColumn());
            dt.Columns.Add(new DataColumn());
        }

        public void LoadData()
        {
            dgNew.Rows.Clear();

            int i = 1, tot = 0;

            foreach (DataRow dr in dt.Rows)
            {
                dgNew.Rows.Add(i, dr[0], dr[1], dr[2], dr[3], dr[4], dr[5]);
                tot += int.Parse(dr[5].ToString());
                i++;
            }

            lbTotal.Text = tot.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lbSNo.Text = "";
            txtLookup.Text = "";
            txtPrice.Text = "";
            txtQuantity.Text = "";
            txtLookup.Focus();
        }

        private void btmAdd_Click(object sender, EventArgs e)
        {
            int l = 0, p = 0, q = 0, i = 0;
            if (lbSNo.Text == "")
            {
                if (txtLookup.Text == "" || !int.TryParse(txtLookup.Text, out l) || !int.TryParse(txtPrice.Text, out p) ||  txtPrice.Text == "" || !int.TryParse(txtQuantity.Text, out q) || txtQuantity.Text == "")
                {
                    MessageBox.Show("Invalid or Empty Values!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataTable d = new DataTable();
                DB.FillData("Select * from item where lookup = " + l + ";", d);

                if (d.Rows.Count > 0)
                {
                    foreach (DataRow dr in d.Rows)
                    {
                        dt.Rows.Add(dr[0], dr[2], dr[1], p, q, p*q);
                    }

                    LoadData();
                }
                else
                {
                    MessageBox.Show("Invalid or Empty Values!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                i = int.Parse(lbSNo.Text);
                if (txtLookup.Text == "" || !int.TryParse(txtLookup.Text, out l) || !int.TryParse(txtPrice.Text, out p) || txtPrice.Text == "" || !int.TryParse(txtQuantity.Text, out q) || txtQuantity.Text == "")
                {
                    MessageBox.Show("Invalid or Empty Values!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataTable d = new DataTable();
                DB.FillData("Select * from item where lookup = " + l + ";", d);

                if (d.Rows.Count > 0)
                {
                    foreach (DataRow dr in d.Rows)
                    {
                        dt.Rows[i - 1][0] = dr[0];
                        dt.Rows[i - 1][1] = dr[2];
                        dt.Rows[i - 1][2] = dr[1];
                        dt.Rows[i - 1][3] = p;
                        dt.Rows[i - 1][4] = q;
                        dt.Rows[i - 1][5] = p * q;
                    }

                    LoadData();
                }
                else
                {
                    MessageBox.Show("Invalid or Empty Values!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            btnClear.PerformClick();
        }

        private void dgNew_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgNew.Columns[e.ColumnIndex].Name;
            if (col == "Delete")
            {
                dt.Rows.RemoveAt(e.RowIndex);
                LoadData();
                txtLookup.Focus();
            }
            else if (col == "Modify")
            {
                lbSNo.Text = dgNew[0, e.RowIndex].Value.ToString();
                txtLookup.Text = dgNew[2, e.RowIndex].Value.ToString();
                txtPrice.Text = dgNew[4, e.RowIndex].Value.ToString();
                txtQuantity.Text = dgNew[5, e.RowIndex].Value.ToString();
                txtPrice.Focus();
            }
        }
    }
}
