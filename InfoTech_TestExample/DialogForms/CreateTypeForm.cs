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
using System.IO;
using InfoTech_TestExample;

namespace InfoTech_TestExample.DialogForms
{
    public partial class CreateTypeForm : Form
    {
        const string quote = InfoTech_TestExample.Form1.quote;

        string FileString = "";

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

        private void CancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
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
                $"VALUES ({NewID},'''{TypeNameBox.Text}''',{FileString})";

                OdbcCommand FolderInsertCommand = new OdbcCommand(InsertText, connection);

                int i = FolderInsertCommand.ExecuteNonQuery();
            }
            form1.AskRefresh();

            
        }

        private void OpenFileToString(object sender, EventArgs e)
        {
            //if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            //    return;
            //// получаем выбранный файл
            //string filename = openFileDialog1.FileName;
            //// читаем файл в строку

            //using (FileStream fstream = File.OpenRead(filename))
            //{
            //    // преобразуем строку в байты
            //    byte[] FileArray = new byte[fstream.Length];
            //    // считываем данные
            //    fstream.Read(FileArray, 0, FileArray.Length);
            //    FileString = Encoding.Default.GetString(FileArray);
            //}
            FileString = form1.TranslateFileToString();
            FileStringBox.Text = FileString;
        }


    }
}
