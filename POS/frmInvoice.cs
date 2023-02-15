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
    public partial class frmInvoice : Form
    {
        DataTable dt;
        int tot = 0;

        public frmInvoice()
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

            int i = 1;
            tot = 0;

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

        private void txtLookup_TextChanged(object sender, EventArgs e)
        {
            if (txtLookup.Text.Length == 1)
            {
                int l;

                if (txtLookup.Text == "" || !int.TryParse(txtLookup.Text, out l))
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
                        txtPrice.Text = dr[3].ToString();
                        txtQuantity.Text = "1";
                        txtPrice.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid or Empty Values!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void frmNew_Load(object sender, EventArgs e)
        {
            txtDate.Value = DateTime.Now;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i = 0;

            if (txtInvoice.Text == "" || !int.TryParse(txtInvoice.Text, out i))
            {
                MessageBox.Show("Invalid or Empty Values!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnDelete.PerformClick();
            DB.InsertData("Insert into invoice (id, total) values (" + i + ", " + tot + ");");

            foreach (DataRow dr in dt.Rows)
            {
                DB.InsertData("Insert into sale (item, invoice, price, quantity) values (" + dr[0] + ", " + i + ", " + dr[3] + ", " + dr[4] + ");");
            }

            btnClearall.PerformClick();
            txtInvoice.Text = i.ToString();
        }

        private void btnClearall_Click(object sender, EventArgs e)
        {
            txtInvoice.Text = "";
            dt.Clear();
            LoadData();
            btnClear.PerformClick();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            int i = 0;

            if (txtInvoice.Text == "" || !int.TryParse(txtInvoice.Text, out i))
            {
                MessageBox.Show("Invalid or Empty Values!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmPrint fp = new frmPrint();
            fp.id = i;

            fp.ShowDialog();

            btnClearall.PerformClick();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            int id = 0;

            if (txtInvoice.Text == "" || !int.TryParse(txtInvoice.Text, out id))
            {
                MessageBox.Show("Invalid or Empty Values!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dt.Clear();

            DataTable d = new DataTable();
            DB.FillData("Select * from sale where invoice = " + id + ";", d);

            int l = 0, p = 0, q = 0;
            string de = "";

            foreach (DataRow dr in d.Rows)
            {
                DataTable d1 = new DataTable();
                DB.FillData("Select * from item where id = " + dr[0].ToString() + ";", d1);

                foreach(DataRow dr1 in d1.Rows)
                {
                    de = dr1[1].ToString();
                    l = int.Parse(dr1[2].ToString());
                }

                p = int.Parse(dr[2].ToString());
                q = int.Parse(dr[3].ToString());

                dt.Rows.Add(dr[0].ToString(), l, de, p, q, p * q);
            }

            LoadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = 0;

            if (txtInvoice.Text == "" || !int.TryParse(txtInvoice.Text, out id))
            {
                MessageBox.Show("Invalid or Empty Values!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DB.InsertData("Delete from invoice where id = " + id + ";");
            DB.InsertData("Delete from sale where invoice = " + id + ";");
        }
    }
}
