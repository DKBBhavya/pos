using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS
{
    internal class DB
    {
        private static SQLiteConnection con;
        private static SQLiteCommand cmd;
        private static SQLiteDataAdapter adap;

        private static void OpenCon()
        {
            try
            {
                con = new SQLiteConnection("Data Source=database.db;");
                con.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error1", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void CloseCon()
        {
            try
            {
                if (con != null && con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static DataTable FillData(string query, DataTable dt)
        {
            try
            {
                OpenCon();
                adap = new SQLiteDataAdapter(query, con);
                adap.Fill(dt);
                adap.Dispose();
                CloseCon();
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public static void InsertData(string query)
        {
            try
            {
                OpenCon();
                cmd = con.CreateCommand();
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                CloseCon();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
