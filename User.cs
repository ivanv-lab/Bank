using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace Bank
{
    public partial class UserForm : Form
    {
        SQLiteClass db = new SQLiteClass("Bank");
        public UserForm()
        {
            InitializeComponent();
            db.DBCreate();
            UserBalanceUpdate();
            UserOpersCount();
        }

        public void UserBalanceUpdate()
        {
            string query = String.Format("select cash from server where number='{0}'",Data.number);
            textBox1.Text= db.SQLiteScalar(query).ToString();
        }
        
        public void UserOpersCount()
        {
            string query = String.Format("select operations from server where number='{0}'", Data.number);
            label4.Text= db.SQLiteScalar(query).ToString();
        }

        public void OperationsUpdate()
        {
            string query = String.Format("select operations from server where number='{0}'", Data.number);
            int ops = db.SQLiteScalar(query);
            ops++;
            query = String.Format("update server set operations='{0}' where number ='{1}'", ops, Data.number);
            db.SqliteExecute(query);
        }

       public void ListUpdate()
        {
                string[] text = File.ReadLines(Data.number + ".txt").ToArray();
            listBox1.Items.Add(text[text.Length-1]);
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            UserBalanceUpdate();
            OperationsUpdate();
            UserOpersCount();
            string[] text = File.ReadLines(Data.number + ".txt").ToArray();
            for(int i=text.Length-5;i<text.Length;i++)
            {
                listBox1.Items.Add(text[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int cashstring = Convert.ToInt32(textBox2.Text);
                if (cashstring <= 0)
                {
                    MessageBox.Show("Введена неверная сумма");
                    return;
                }
                string query = String.Format("select cash from server where number = '{0}'", Data.number);
                int cashnow = db.SQLiteScalar(query);
                if (cashstring > cashnow)
                {
                    MessageBox.Show("Ошибка. Недостаточно средств на счету");
                    return;
                }
                else
                {
                    int cashnew = cashnow - cashstring;
                    query = String.Format("update server set cash='{0}' where number='{1}'", cashnew, Data.number);
                    db.SqliteExecute(query);

                    if (!File.Exists(Data.number + ".txt"))
                        File.Create(Data.number + ".txt");
                    File.AppendAllText(Data.number + ".txt", "- " + cashstring.ToString() + Environment.NewLine);
                    ListUpdate();
                    UserBalanceUpdate();
                    OperationsUpdate();
                    UserOpersCount();

                }
            }
            catch
            {
                MessageBox.Show("Введена неверная сумма");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int cashstring = Convert.ToInt32(textBox2.Text);
                if (cashstring <= 0)
                {
                    MessageBox.Show("Введена неверная сумма");
                    return;
                }
                string query = String.Format("select cash from server where number = '{0}'", Data.number);
                int cashnow = db.SQLiteScalar(query);
                cashnow += cashstring;
                string query1 = String.Format("update server set cash='{0}' where number='{1}'", cashnow, Data.number);
                db.SqliteExecute(query1);

                if (!File.Exists(Data.number + ".txt"))
                    File.Create(Data.number + ".txt");
                File.AppendAllText(Data.number + ".txt", "+ " + cashstring.ToString() + Environment.NewLine);
                ListUpdate();
                UserBalanceUpdate();
                OperationsUpdate();
                UserOpersCount();
            }
            catch
            {
                MessageBox.Show("Введена неверная сумма");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                int cashstring = Convert.ToInt32(textBox2.Text);
                if (cashstring <= 0)
                {
                    MessageBox.Show("Введена неверная сумма");
                    return;
                }
                string query = String.Format("select cash from server where number = '{0}'", Data.number);
                int cashnow = db.SQLiteScalar(query);
                if (cashstring > cashnow)
                {
                    MessageBox.Show("Ошибка. Недостаточно средств на счету");
                    return;
                }
                else
                {
                    CardNum form = new CardNum();
                    form.ShowDialog();
                    query = String.Format("select cash from server where number='{0}'", Data.number1);
                    int cashout = db.SQLiteScalar(query);
                    cashout += cashstring;
                    query = String.Format("update server set cash='{0}' where number='{1}'", cashout, Data.number1);
                    db.SqliteExecute(query);
                    cashnow -= cashstring;
                    query = String.Format("update server set cash='{0}' where number='{1}'", cashnow, Data.number);
                    db.SqliteExecute(query);

                    if (!File.Exists(Data.number + ".txt"))
                        File.Create(Data.number + ".txt");
                    File.AppendAllText(Data.number + ".txt", "- " + cashstring.ToString() + Environment.NewLine);
                    if (!File.Exists(Data.number1 + ".txt"))
                        File.Create(Data.number1 + ".txt");
                    File.AppendAllText(Data.number1 + ".txt", "+ " + cashstring.ToString() + Environment.NewLine);
                    ListUpdate();
                    UserBalanceUpdate();
                    OperationsUpdate();
                    UserOpersCount();
                }
            }
            catch
            {
                MessageBox.Show("Введена неверная сумма");

            }
        }
    }
}
