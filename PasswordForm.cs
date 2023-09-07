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
    public partial class PasswordForm : Form
    {
        SQLiteClass db = new SQLiteClass("Bank");
        
        public PasswordForm()
        {
            InitializeComponent();
            db.DBCreate();
            if (!File.Exists("Bank.sqlite"))
            {
                string query = "Create table bank(id integer NOT NULL Primary key autoincrement unique, number text NOT NULL unique, PIN text NOT NULL)";
                db.SqliteExecute(query);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string number = Logintb.Text;
            string pin = Passwordtb.Text;
            string query=String.Format("select count(*) from bank where number='{0}' and PIN='{1}'", number,pin);
            int result=db.SQLiteScalar(query);
          if(result==1)
            {
                Data.number= number;
                Data.pin= pin;
                UserForm form= new UserForm();
                this.Hide();
                form.Show();
            }
            else
            {
                MessageBox.Show("Введен неверный PIN или номер карты");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
