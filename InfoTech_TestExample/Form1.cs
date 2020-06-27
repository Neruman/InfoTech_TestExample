using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Odbc;


namespace InfoTech_TestExample
{
    

    public partial class Form1 : Form
    {
        const string quote = "\u0022";
        string ConnectionString = "Dsn=PostgreSQL35W;database=postgres;server=localhost;port=5432;uid=postgres;sslmode=disable;readonly=0;protocol=7.4;fakeoidindex=0;showoidcolumn=0;rowversioning=0;showsystemtables=0;fetch=100;unknownsizes=0;maxvarcharsize=255;maxlongvarcharsize=8190;debug=0;commlog=0;usedeclarefetch=0;textaslongvarchar=1;unknownsaslongvarchar=0;boolsaschar=1;parse=0;lfconversion=1;updatablecursors=1;trueisminus1=0;bi=0;byteaaslongvarbinary=1;useserversideprepare=1;lowercaseidentifier=0;d6=-101;optionalerrors=0;xaopt=1";


        
        public delegate void RefreshEventHandler();
        public event RefreshEventHandler RefreshTreeView;

        public delegate void ChangeEventHandler(string ParentFolderID);

        public Form1()
        {
            InitializeComponent();
            RefreshTreeView += new RefreshEventHandler(LoadTreeFromBegin);
            RefreshTreeView?.Invoke();
        }

        /// <summary>
        /// Метод отрисовки иерархии файлов
        /// </summary>
        private void LoadTreeFromBegin()
        {

            //Очистка содержимого
            treeView1.Nodes.Clear();


            using (OdbcConnection connection = new OdbcConnection(ConnectionString))
            {
                //Подключение к БД
                connection.Open();

                //Отрисовка всех вложенных папок, начиная с Root
                AskFolderByParentFolderID("-1",connection);
            }
        }
        /// <summary>
        /// Последовательный поиск всех вложенных папок для их отображения в иерархии файлов
        /// </summary>
        /// <param name="ParentFolderID">Код родительской папки</param>
        /// <param name="connection">Строка подключения</param>
        private void AskFolderByParentFolderID(string ParentFolderID, OdbcConnection connection)
        {

            string CommandText =
                $"SELECT * FROM public.{quote}Folders{quote}" +
                $"  WHERE {quote}ParentFolderID{quote} = CAST ({ParentFolderID} AS TEXT)" +
                $"  ORDER BY {quote}FolderID{quote} ASC ";
            

            OdbcCommand FolderReaderCommand = new OdbcCommand(CommandText, connection);

            //int RowCounter = AskFolderByParent.ExecuteNonQuery();
           
                OdbcDataReader reader = FolderReaderCommand.ExecuteReader();

                List<string> NewIDs = new List<string>();

                while (reader.Read())
                {


                    string ID = (Convert.ToString((int)reader.GetValue(0)));
                    string Name = (string)reader.GetValue(1);
                    string ParentID = ((string)reader.GetValue(2));


                    AddFolderNode(ParentID, ID, Name);
                    NewIDs.Add(ID);
                }

                for (int i = 0; i < NewIDs.Count(); i++)
                {
                    AskFolderByParentFolderID(NewIDs[i], connection);
                    AskFileByParentFolderID(NewIDs[i], connection);
                }
            reader.Close();

        }

        /// <summary>
        /// Последовательный поиск всех вложенных файлов для их отображения в иерархии файлов
        /// </summary>
        /// <param name="FolderID">Код родительской папки</param>
        /// <param name="connection">Строка подключения</param>
        private void AskFileByParentFolderID(string FolderID, OdbcConnection connection)
        {
            string CommandText = 
                $"SELECT {quote}FileID{quote},{quote}Caption{quote},{quote}Description{quote}"+
                $"  FROM public.{quote}Files{quote}" +
                $"  WHERE {quote}FolderID{quote} = CAST ({FolderID} AS TEXT)" +
                $"  ORDER BY {quote}FileID{quote} ASC ";

            OdbcCommand FolderReaderCommand = new OdbcCommand(CommandText, connection);

            //int RowCounter = AskFolderByParent.ExecuteNonQuery();
            OdbcDataReader reader = FolderReaderCommand.ExecuteReader();

            List<string> NewIDs = new List<string>();

            while (reader.Read())
            {
                string ID = (Convert.ToString((int)reader.GetValue(0)));
                string Name = (string)reader.GetValue(1);
                string Description = (string)reader.GetValue(2);

                AddFileNode(FolderID, ID, Name,Description);
            }
            reader.Close();
        }


        private void TestButtonClick(object sender, EventArgs e)
        {
            //RefreshTreeView?.Invoke();
        }


        /// <summary>
        /// Метод добавления нового узла-папки в иерархию файлов
        /// </summary>
        /// <param name="ParentID">Код родительской папки</param>
        /// <param name="NewID">Код целевой папки</param>
        /// <param name="Name">Название целевой папки</param>
        public void AddFolderNode(string ParentID,string NewID, string Name)
        {
            //Проверка на уникальность ключа (номер папки)
            if (treeView1.Nodes.Find($"Folder_{NewID}", true).Count() > 0)
            {
                MessageBox.Show($"Операция невозможна. Папка с номером {NewID} уже существует");
            }

            else if (treeView1.Nodes.Find($"Folder_{ParentID}", true).Count()>0)
            {
                treeView1.Nodes.Find($"Folder_{ParentID}", true).First().Nodes.Add($"Folder_{NewID}", Name);
            }
            else
            {
                treeView1.Nodes.Add($"Folder_{NewID}", Name);
            }        
        }


        public void AddFileNode(string FolderID, string FileID, string Name, string Description)
        {
            //Проверка на уникальность ключа (номер файла)
            if (treeView1.Nodes.Find($"File_{FileID}", true).Count() > 0)
            {
                MessageBox.Show($"Операция невозможна. Файл с номером {FileID} уже существует");
            }

            else if (treeView1.Nodes.Find($"Folder_{FolderID}", true).Count() > 0)
            {
                treeView1.Nodes.Find($"Folder_{FolderID}", true).First().Nodes.Add($"File_{FileID}", Name);
                treeView1.Nodes.Find($"File_{FileID}", true).First().ToolTipText = Description;
            }
            
        }
        public void CreateFolderRecord (string FolderName, string ParentFolderID)
        {
            using (OdbcConnection connection = new OdbcConnection(ConnectionString))
            {
                //Подключение к БД
                connection.Open();

                //Запрос нового  ID
                string CommandText =
                $"SELECT {quote}FolderID{quote} " +
                $"FROM public.{quote}Folders{quote}" +
                $"  ORDER BY {quote}FolderID{quote} DESC ";

                OdbcCommand FolderReaderCommand = new OdbcCommand(CommandText, connection);

                //int RowCounter = AskFolderByParent.ExecuteNonQuery();
                int NewID = (int)FolderReaderCommand.ExecuteScalar()+1;

                //Размещаем новую запись в БД
                string InsertText =
                $"INSERT INTO {quote}Folders{quote} ({quote}FolderID{quote},{quote}FolderName{quote},{quote}ParentFolderID{quote})" +
                $"VALUES ({NewID},'''{FolderName}''',{ParentFolderID})";

                OdbcCommand FolderInsertCommand = new OdbcCommand(InsertText, connection);

                int i = FolderInsertCommand.ExecuteNonQuery();
            }
            RefreshTreeView?.Invoke();
        }

        public string GetSelectedNode()
        {
            string result = (string)treeView1.SelectedNode.Name;
            if (result == "") { result = "-1"; }
            return result;
        }
        public string GetSelectedFolderID()
        {
            string NodeTag = GetSelectedNode();
            if (NodeTag.Contains("Folder_"))    //если выделен узел с папкой, то выдаём ID папки
            {
                NodeTag = NodeTag.Remove(0, 7);
            }
            else
            {
                NodeTag = NodeTag.Remove(0, 5);  //если выделен узел с файлом, то выдаём папку,в которой он находится
                using (OdbcConnection connection = new OdbcConnection(ConnectionString))
                {
                    //Подключение к БД
                    connection.Open();

                    //Запрос ID папки, к которой принадлежит выделенный файл
                    string CommandText =
                    $"SELECT {quote}FolderID{quote} " +
                    $"FROM public.{quote}Files{quote}" +
                    $"  WHERE {quote}FileID{quote} = {Convert.ToInt32(NodeTag)}";

                    OdbcCommand FolderReaderCommand = new OdbcCommand(CommandText, connection);
                    object FolderID = FolderReaderCommand.ExecuteScalar();

                    NodeTag = Convert.ToString(FolderID);
                }
            }
            return NodeTag;
        }

        private void CreateFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFolderRecord("NewFolder", GetSelectedFolderID());
        }

        private void treeView1_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            try
            {
                toolTip1.SetToolTip(treeView1, treeView1.GetNodeAt(MousePosition).ToolTipText);
                toolTip1.InitialDelay = 0;

            }
            catch (Exception) { }
        }
    }
}
