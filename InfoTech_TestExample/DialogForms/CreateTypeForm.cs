using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;

namespace InfoTech_TestExample.DialogForms
{
    public partial class CreateTypeForm : Form
    {
        const string quote = InfoTech_TestExample.Form1.quote;

        string filestring = "";

        private string connectionString;
        public string ConnectionString { get => connectionString; set { connectionString = value; } }

        private Form1 form1;
        public Form1 Form1 { set { form1 = value; } }

        public CreateTypeForm()
        {
            InitializeComponent();
        }

        public CreateTypeForm(string _ConnectionString, Form1 form)
        {
            InitializeComponent();
            ConnectionString = _ConnectionString;
            Form1 = form;
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string filestring = "";
            using (OdbcConnection connection = new OdbcConnection(ConnectionString))
            {
                //Подключение к БД
                connection.Open();

                //Запрос нового  ID
                string CommandText =
                $"SELECT {quote}TypeID{quote} " +
                $"FROM public.{quote}FileTypes{quote}" +
                $"  ORDER BY {quote}TypeID{quote} DESC ";

                OdbcCommand FolderReaderCommand = new OdbcCommand(CommandText, connection);

                int NewID = (int)FolderReaderCommand.ExecuteScalar() + 1;

                //Размещаем новую запись в БД
                string InsertText =
                $"INSERT INTO public.{quote}Types{quote} ({quote}TypeID{quote},{quote}Type{quote},{quote}Icon{quote})" +
                $"VALUES ({NewID},'''{textBox1.Text}''',{filestring})";

                OdbcCommand FolderInsertCommand = new OdbcCommand(InsertText, connection);

                int i = FolderInsertCommand.ExecuteNonQuery();
            }
            form1.AskRefresh();

            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
