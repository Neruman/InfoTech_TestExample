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
    public partial class CreateFileForm : Form
    {
        const string quote = InfoTech_TestExample.Form1.quote;

        string FileString = "";

        private string selectedTypeID = "0";

        private string connectionString;
        public string ConnectionString { get => connectionString; set { connectionString = value; } }

        private string folderID;
        public string FolderID { get => folderID; set { folderID = value; } }

        private Form1 form1;
        public Form1 Form1 { set { form1 = value; } }

        public CreateFileForm()
        {
            InitializeComponent();
        }

        public CreateFileForm(string _ConnectionString, string _FolderID, Form1 form)
        {
            InitializeComponent();
            ConnectionString = _ConnectionString;
            FolderID = _FolderID;
            Form1 = form;
            TypeListConfigure();
            this.Show();
        }

        /// <summary>
        /// Выгружает список доступных расширений из БД
        /// </summary>
        private void TypeListConfigure()
        {
            comboBox1.Items.Clear();
            using (OdbcConnection connection = new OdbcConnection(ConnectionString))
            {
                //Подключение к БД
                connection.Open();

                //Запрос списка расширений
                string CommandText =
                $"SELECT {quote}Type{quote} " +
                $"FROM public.{quote}FileTypes{quote}" +
                $"  ORDER BY {quote}TypeID{quote} ASC ";

                OdbcCommand TypeReaderCommand = new OdbcCommand(CommandText, connection);

                //Переносим список известных Типов из БД в выпадающий список
                OdbcDataReader reader = TypeReaderCommand.ExecuteReader();
                while (reader.Read())
                {
                    string Name = (string)reader.GetValue(0);

                    comboBox1.Items.Add(Name);
                }
                reader.Close();
            }
            comboBox1.SelectedIndex = 0;
        }


        private void CancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OkClick(object sender, EventArgs e)
        {
            using (OdbcConnection connection = new OdbcConnection(ConnectionString))
            {
                //Подключение к БД
                connection.Open();

                //Запрос нового  ID
                string CommandText =
                $"SELECT {quote}FileID{quote} " +
                $"FROM public.{quote}Files{quote}" +
                $"  ORDER BY {quote}FileID{quote} DESC ";

                OdbcCommand IDReaderCommand = new OdbcCommand(CommandText, connection);

                int NewID = (int)IDReaderCommand.ExecuteScalar() + 1;

                //Размещаем новую запись в БД
                string InsertText =
                $"INSERT INTO public.{quote}Files{quote} " +
                $"({quote}FileID{quote},{quote}Caption{quote},{quote}Description{quote},{quote}TypeID{quote}," +
                $"{quote}FolderID{quote},{quote}Content{quote})" +
                $"VALUES ({NewID},'{CaptionBox.Text}','{DescriptionBox.Text}','{selectedTypeID}'," +
                $"'{FolderID}','{FileString}')";

                OdbcCommand FileInsertCommand = new OdbcCommand(InsertText, connection);

                int i = FileInsertCommand.ExecuteNonQuery();
            }
            form1.AskRefresh();
            Close();
            
        }

        private void UploadButtonClick(object sender, EventArgs e)
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
            ContentBox.Text = FileString;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (OdbcConnection connection = new OdbcConnection(ConnectionString))
            {
                //Подключение к БД
                connection.Open();

                //Запрос информации о выбранном типе
                string CommandText =
                $"SELECT {quote}TypeID{quote} " +
                $"FROM public.{quote}FileTypes{quote}" +
                $"WHERE {quote}Type{quote} = '{(string)comboBox1.SelectedItem}'" +
                $"ORDER BY {quote}TypeID{quote} ASC ";

                OdbcCommand FolderReaderCommand = new OdbcCommand(CommandText, connection);

                selectedTypeID = Convert.ToString((int)FolderReaderCommand.ExecuteScalar());
            }
        }
    }
}
