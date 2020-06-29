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
    public partial class ChangeTypeForm : Form
    {
        const string quote = InfoTech_TestExample.Form1.quote;

        string FileString = "";

        private string connectionString;
        public string ConnectionString { get => connectionString; set { connectionString = value; } }

        private Form1 form1;
        public Form1 Form1 { set { form1 = value; } }

        public ChangeTypeForm()
        {
            InitializeComponent();
        }

        public ChangeTypeForm(string _ConnectionString, Form1 form)
        {
            InitializeComponent();
            ConnectionString = _ConnectionString;
            Form1 = form;
            TypeListConfigure();
            this.Show();
        }

        /// <summary>
        /// Создаём список всех доступных расширений для изменения
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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (OdbcConnection connection = new OdbcConnection(ConnectionString))
            {
                //Подключение к БД
                connection.Open();

                //Запрос информации о выбранном типе
                string CommandText =
                $"SELECT * " +
                $"FROM public.{quote}FileTypes{quote}" +
                $"WHERE {quote}Type{quote} = '{(string)comboBox1.SelectedItem}'" +
                $"ORDER BY {quote}TypeID{quote} ASC ";

                OdbcCommand TypeReaderCommand = new OdbcCommand(CommandText, connection);

                //Вывод информации о типе в текстовое поле
                OdbcDataReader reader = TypeReaderCommand.ExecuteReader();
                while (reader.Read())
                {
                    //название типа
                    TypeNameBox.Text = (string)reader.GetValue(1);
                    //строковое представление иконки

                    FileStream fstream;
                    try
                    {
                        object[] values1 = new object[3];
                         reader.GetValues(values1);
                        //считываем изображение
                        FileArray = ((byte[])reader.GetValue(2));

                        FileStringBox.Text = Encoding.Default.GetString(FileArray);

                        //сохраняем изображение во временный файл
                         fstream = new FileStream($"{Directory.GetCurrentDirectory()}./{(string)values1[1]}.ico",FileMode.Create,FileAccess.ReadWrite);
                        fstream.Write(FileArray, 0, FileArray.Length);
                        

                        //выгружаем изображение из временного файла
                        Bitmap bm = new Bitmap(fstream);// $"{Directory.GetCurrentDirectory()}./{(string)values1[1]}.ico");
                        pictureBox1.Visible = true;
                        pictureBox1.Image = (Image)bm;
                        fstream.Close();
                    }
                    catch (Exception ex)
                    {
                        try { fstream = new FileStream($"{Directory.GetCurrentDirectory()}./temp.ico", FileMode.Create, FileAccess.ReadWrite); fstream.Close(); } catch { }
                        FileStringBox.Text = "";
                        pictureBox1.Visible = false;
                    }
                }
                reader.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OdbcConnection connection = new OdbcConnection(ConnectionString))
            {
                //Подключение к БД
                connection.Open();

                //Запрос информации о выбранном типе
                string CommandText =
                $"SELECT * " +
                $"FROM public.{quote}FileTypes{quote}" +
                $"WHERE {quote}Type{quote} = '{(string)comboBox1.SelectedItem}'" +
                $"ORDER BY {quote}TypeID{quote} ASC ";

                OdbcCommand IDReaderCommand = new OdbcCommand(CommandText, connection);

                int ChangedTypeID = (int)IDReaderCommand.ExecuteScalar();

                //Записываем изменения
                string ChangeTypeCommand =
                $"UPDATE public.{quote}FileTypes{quote} " +
                $"SET {quote}Type{quote}  = '{TypeNameBox.Text}' , {quote}Icon{quote} =  '{FileString}' "  +
                $"  WHERE {quote}TypeID{quote} = {ChangedTypeID}";

                OdbcCommand TypeReaderCommand = new OdbcCommand(ChangeTypeCommand, connection);
                object i = TypeReaderCommand.ExecuteNonQuery();

                //Всё готово. Закрываем диалоговое окно
                
                
            }
            Close();
        }
        byte[] FileArray;
        private void OpenFileToString(object sender, EventArgs e)
        {
            //if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            //    return;
            //// получаем выбранный файл
            //string filename = openFileDialog1.FileName;
            //// читаем файл в строку
            //FileString = "";
            //using (FileStream fstream = File.OpenRead(filename))
            //{
            //    // преобразуем строку в байты
            //    FileArray = new byte[fstream.Length];

            //    // считываем данные
            //    fstream.Read(FileArray, 0, FileArray.Length);

            //    //формируем строку для выгрузки в БД
            //    FileString = "\\x" + BitConverter.ToString(FileArray).Replace("-", string.Empty);
            //}
            FileString = form1.TranslateFileToString();
            FileStringBox.Text = FileString;
        }
    }

}
