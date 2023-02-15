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
    public partial class frmPrint : Form
    {
        public int id, height, cur;
        DataTable dt;
        int tot = 0;

        public frmPrint()
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

            //lbTotal.Text = tot.ToString();
        }

        private void frmPrint_Load(object sender, EventArgs e)
        {
            dt.Clear();

            DataTable d = new DataTable();
            DB.FillData("Select * from sale where invoice = " + id + ";", d);

            int l = 0, p = 0, q = 0;
            string de = "";

            foreach (DataRow dr in d.Rows)
            {
                DataTable d1 = new DataTable();
                DB.FillData("Select * from item where id = " + dr[0].ToString() + ";", d1);

                foreach (DataRow dr1 in d1.Rows)
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        Bitmap bmp;

        private void btnPrint_Click(object sender, EventArgs e)
        {
            height = dgNew.RowCount * dgNew.RowTemplate.Height + dgNew.ColumnHeadersHeight;
            cur = 0;

            for (int i = 0; i < dgNew.Rows.Count; i++)
            {
                dgNew.Rows[i].Visible = false;
            }
            printDocument1.Print();

            this.Close();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            bmp = new Bitmap(e.PageBounds.Width, e.PageBounds.Height);

            int tr = e.PageBounds.Height / (dgNew.RowTemplate.Height);
            tr--;

            for (int i = 0; i < tr && cur + i < dgNew.Rows.Count; i++)
            {
                dgNew.Rows[cur+i].Visible = true;
            }

            dgNew.Height = e.PageBounds.Height;
            dgNew.Width = e.PageBounds.Width;

            dgNew.DrawToBitmap(bmp, new Rectangle(0, 0, e.PageBounds.Width, e.PageBounds.Height));

            for (int i = 0; i < tr && cur + i < dgNew.Rows.Count; i++)
            {
                dgNew.Rows[cur+i].Visible = false;
            }

            e.Graphics.DrawImage(bmp, 0, 0);
            cur += tr;

            e.HasMorePages = (cur < dgNew.Rows.Count);
        }
    }
}
