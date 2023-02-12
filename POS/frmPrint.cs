using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    public partial class frmPrint : Form
    {
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

            lbTotal.Text = tot.ToString();
        }

        private void frmPrint_Load(object sender, EventArgs e)
        {
            DataTable d = new DataTable();
            DB.FillData("Select max(id) from invoice;", d);

            int id = 0;

            foreach (DataRow dr in d.Rows)
            {
                id = int.Parse(dr[0].ToString());
            }

            lbInvoice.Text = id.ToString();

            dt.Clear();

            d = new DataTable();
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

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern long BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private Bitmap memoryImage;

        private void PrintScreen()
        {
            Graphics mygraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, mygraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            IntPtr dc1 = mygraphics.GetHdc();
            IntPtr dc2 = memoryGraphics.GetHdc();
            BitBlt(dc2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, dc1, 0, 0, 13369376);
            mygraphics.ReleaseHdc(dc1);
            memoryGraphics.ReleaseHdc(dc2);
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintScreen();
            printPreviewDialog1.ShowDialog();
            this.Close();
        }
    }
}
