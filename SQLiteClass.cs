using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using System.Data;

namespace Bank
{
    public class SQLiteClass
    {
        string dbFileName;

        public SQLiteClass(string dbFileName)
        {
            this.dbFileName = dbFileName;
        }
        public void DBCreate()
        {
            try
            {
                dbFileName += ".sqlite";
                if (!File.Exists(dbFileName))
                {
                    SQLiteConnection.CreateFile(dbFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка создания файла!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        SQLiteConnection con;
        SQLiteCommand cmd;

        public void SqliteExecute(string query)
        {
            try
            {
                con = new SQLiteConnection("Data source=" + dbFileName);
                con.Open();
                cmd = new SQLiteCommand();
                cmd.CommandText = query;
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
               // MessageBox.Show("Операция выполнена");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка выполнения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        public int SQLiteScalar(string query)
        {
            try
            {
                con = new SQLiteConnection("Data source=" + dbFileName);
                con.Open();
                cmd = new SQLiteCommand();
                cmd.CommandText = query;
                cmd.Connection = con;
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка выполнения команды!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            finally
            {
                con.Close();
            }
        }

        DataTable dTable;
        SQLiteDataAdapter adapter;

        public void SQLiteTable(string query, DataGridView dgv)
        {
            try
            {
                con = new SQLiteConnection("Data source=" + dbFileName);
                con.Open();
                dTable = new DataTable();
                adapter = new SQLiteDataAdapter(query, con);
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    dgv.Rows.Clear();
                    for (int i = 0; i < dTable.Rows.Count; i++)
                        dgv.Rows.Add(dTable.Rows[i].ItemArray);
                }
                else
                {
                    MessageBox.Show("База данных пустая");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка заполнения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        public void SQLiteComboBox(string query, ComboBox cb)
        {
            try
            {
                con = new SQLiteConnection("Data source=" + dbFileName);
                con.Open();
                adapter = new SQLiteDataAdapter(query, con);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                cb.DataSource = ds.Tables[0];
                cb.DisplayMember = (ds.Tables[0].Columns[0]).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка заполнения!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
