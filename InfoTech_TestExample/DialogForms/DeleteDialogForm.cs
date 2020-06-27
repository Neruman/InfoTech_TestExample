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
    public partial class DeleteForm : Form
    {
        private string connection;
        public string Connection { get => connection; set { connection = value; } }

        private string command;
        public string Command { get => command; set { command = value; } }

        private Form1 form1;
        public Form1 Form1 { set { form1 = value; } }

        public DeleteForm()
        {
            InitializeComponent();
        }
        public DeleteForm(string _Connection, string _Command, Form1 form)
        {
            InitializeComponent();
            Connection = _Connection;
            Command = _Command;
            Form1 = form;
            this.Show();
        }

        private void OKButtonClick(object sender, EventArgs e)
        {
            using (OdbcConnection connect = new OdbcConnection(Connection))
            {
                connect.Open();
                OdbcCommand FolderReaderCommand = new OdbcCommand(Command, connect);
                object i = FolderReaderCommand.ExecuteNonQuery();
                form1.AskRefresh();
                this.Close();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
