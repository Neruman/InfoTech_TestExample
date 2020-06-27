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
    public partial class RenameForm : Form
    {
        const string quote = InfoTech_TestExample.Form1.quote;

        private string connection;
        public string Connection { get => connection; set { connection = value; } }

        private string id;
        public string ID { get => id; set { id = value; } }

        private Form1 form1;
        public Form1 Form1 { set { form1 = value; } }

        private string inputType;
        public string InputType { get => inputType; set { inputType = value; } }

        public RenameForm()
        {
            InitializeComponent();
        }

        public RenameForm(string _ConnectionString, string _ID, string _InputType, Form1 form)
        {
            InitializeComponent();
            Connection = _ConnectionString;
            ID =_ID;
            InputType = _InputType;
            Form1 = form;
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string CommandString = "" ;
            if (InputType == "Folder")
            {
                CommandString =
                $"UPDATE public.{quote}Folders{quote}" +
                $"SET {quote}FolderName{quote} = '{textBox1.Text}'" +
                $"  WHERE {quote}FolderID{quote} = {Convert.ToInt32(ID)}";
               // $"  WHERE {quote}FolderID{quote} = {Convert.ToInt32(AskNodeFolderID())}";
            }      
            if (InputType == "File")
            {
                CommandString =
                    $"UPDATE public.{quote}Files{quote} " +
                    $"SET {quote}Caption{quote} = '{textBox1.Text}'" +
                    $"WHERE {quote}FileID{quote} = {Convert.ToInt32(ID)}";
            }

            using (OdbcConnection connect = new OdbcConnection(Connection))
            {
                connect.Open();
                OdbcCommand FolderReaderCommand = new OdbcCommand(CommandString, connect);
                object i = FolderReaderCommand.ExecuteNonQuery();
                form1.AskRefresh();
                this.Close();
            }
        }
    }
}
