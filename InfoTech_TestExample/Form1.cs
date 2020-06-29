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

namespace InfoTech_TestExample
{
    


    public partial class Form1 : Form
    {
        public const string quote = "\u0022"; //Символ кавычки для упрощения написания запросов к БД
        private const string ConnectionString = "Dsn=PostgreSQL35W;database=postgres;server=localhost;port=5432;uid=postgres;sslmode=disable;readonly=0;protocol=7.4;fakeoidindex=0;showoidcolumn=0;rowversioning=0;showsystemtables=0;fetch=100;unknownsizes=0;maxvarcharsize=255;maxlongvarcharsize=8190;debug=0;commlog=0;usedeclarefetch=0;textaslongvarchar=1;unknownsaslongvarchar=0;boolsaschar=1;parse=0;lfconversion=1;updatablecursors=1;trueisminus1=0;bi=0;byteaaslongvarbinary=1;useserversideprepare=1;lowercaseidentifier=0;d6=-101;optionalerrors=0;xaopt=1";

        
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
        /// Внешний запрос на обновление иерархии файлов
        /// </summary>
        public void AskRefresh()
        {
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

                //Запрашиваем иконки для файлов
                treeView1.ImageList = new ImageList();
                treeView1.ImageList.Images.Add("-1",(Image)new Bitmap(2, 2));

                AskIcons(connection);

                //Отрисовка всех вложенных папок, начиная с Root
                AskFolderByParentFolderID("-1",connection);

            }
            treeView1.Nodes[0].Checked = true;
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
                $"SELECT {quote}FileID{quote},{quote}Caption{quote},{quote}Description{quote},{quote}TypeID{quote}"+
                $"  FROM public.{quote}Files{quote}" +
                $"  WHERE {quote}FolderID{quote} = CAST ({FolderID} AS TEXT)" +
                $"  ORDER BY {quote}FileID{quote} ASC ";

            OdbcCommand FolderReaderCommand = new OdbcCommand(CommandText, connection);
            OdbcDataReader reader = FolderReaderCommand.ExecuteReader();
            List<string> NewIDs = new List<string>();
            while (reader.Read())
            {
                string ID = (Convert.ToString((int)reader.GetValue(0)));
                string Name = (string)reader.GetValue(1);
                string Description = (string)reader.GetValue(2);
                int TypeID = Convert.ToInt32((string)reader.GetValue(3));

                AddFileNode(FolderID, ID, Name,Description,TypeID);
            }
            reader.Close();
        }


        private void AskIcons(OdbcConnection connection)
        {
            string CommandText =
                $"SELECT * " +
                $"  FROM public.{quote}FileTypes{quote}" +
                $"  ORDER BY {quote}TypeID{quote} ASC ";
            OdbcCommand ReaderCommand = new OdbcCommand(CommandText, connection);
            OdbcDataReader reader = ReaderCommand.ExecuteReader();
            while (reader.Read())
            {
                int TypeID = (int)reader.GetValue(0);
                string TypeName = (string)reader.GetValue(1);
                treeView1.ImageList.Images.Add($"{TypeID}", UploadTypeIcon(connection,TypeName));
            }
            reader.Close();
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

            else if (treeView1.Nodes.Find($"Folder_{ParentID}", true).Count() > 0)
            {
                treeView1.Nodes.Find($"Folder_{ParentID}", true).First().Nodes.Add($"Folder_{NewID}", Name);
                try
                {
                    treeView1.Nodes.Find($"File_{NewID}", true).First().ImageKey = "-1";
                }
                catch (Exception ex) { }

            }

            //Нижний блок не удалять, иначе пропадёт корневой узел и всё остальное вместе с ним!
            else
            {
                treeView1.Nodes.Add($"Folder_{NewID}", Name);
            }
        }

        /// <summary>
        /// Метод добавления нового узла файла
        /// </summary>
        /// <param name="FolderID">Родительская папка</param>
        /// <param name="FileID">Код файла</param>
        /// <param name="Name">Имя файла</param>
        /// <param name="Description">Всплывающая подсказка</param>
        public void AddFileNode(string FolderID, string FileID, string Name, string Description, int TypeID)
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
                treeView1.Nodes.Find($"File_{FileID}", true).First().ImageIndex = TypeID;
            }
            
        }

        /// <summary>
        /// Создание новой папки в БД
        /// </summary>
        /// <param name="FolderName">Имя новой папки</param>
        /// <param name="ParentFolderID">Родительская папка</param>
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


        /// <summary>
        /// Определяем номер выделенного узла в TreeView
        /// </summary>
        /// <returns></returns>
        public string GetSelectedNode()
        {
            try
            {
                return (string)treeView1.SelectedNode.Name;
            }
            catch
            {
                return "-1";
            }
        }

        /// <summary>
        /// Определяем номер папки по выделенному узлу в TreeView. Если выбран файл, выводится ID его папки-контейнера
        /// </summary>
        /// <returns></returns>
        public string AskContainerFolderID()
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
        /// <summary>
        /// Выводит номер папки или пустую строку, если это не папка
        /// </summary>
        /// <returns></returns>
        public string AskNodeFolderID()
        {
            string NodeTag = GetSelectedNode();
            if (NodeTag.Contains("Folder_"))    //если выделен узел с папкой, то выдаём ID папки
            {
                NodeTag = NodeTag.Remove(0, 7);
            }
            return NodeTag;
        }
        /// <summary>
        /// Определяем номер файла или пустую строку, если это не файл
        /// </summary>
        /// <returns></returns>
        public string AskNodeFileID()
        {
            string NodeTag = GetSelectedNode();
            if (NodeTag.Contains("File_"))    //если выделен узел с папкой, то выдаём ID папки
            { 
                NodeTag = NodeTag.Remove(0, 5);  //если выделен узел с файлом, то выдаём папку,в которой он находится
            }
            return NodeTag;
        }

        public string TypeOfSelectedNode()
        {
            if (GetSelectedNode().Contains("Folder_"))  { return "Folder"; }
            if (GetSelectedNode().Contains("File_"))    { return "File"; }
            return "-1";
        }

        private void CreateFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateFolderRecord("NewFolder", AskContainerFolderID());
        }



        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetSelectedNode().Contains("Folder_")   == true)  { DialogForms.RenameForm askForm = new DialogForms.RenameForm(ConnectionString, AskNodeFolderID(), "Folder",  this); }
            if (GetSelectedNode().Contains("File_")     == true)  { DialogForms.RenameForm askForm = new DialogForms.RenameForm(ConnectionString, AskNodeFileID(),   "File",    this); }
        }

        private void DeleteFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GetSelectedNode().Contains("Folder_") == true)
            {
                string CommandText =
                 $"DELETE FROM public.{quote}Folders{quote}" +
                 $"  WHERE {quote}FolderID{quote} = {Convert.ToInt32(AskNodeFolderID())}";

                DialogForms.DeleteForm DeleteThisFolderForm = new DialogForms.DeleteForm(ConnectionString, CommandText, this);                ;
            }
            if (GetSelectedNode().Contains("File_") == true)
            {
                string CommandText =
                    $"DELETE FROM public.{quote}Files{quote} " +
                    $"WHERE {quote}FileID{quote} = {Convert.ToInt32(AskNodeFileID())}";

                DialogForms.DeleteForm DeleteThisFolderForm = new DialogForms.DeleteForm(ConnectionString, CommandText, this);
            }
        }



        private void treeView1_MouseHover(object sender, EventArgs e)
        {
            TreeViewHitTestInfo info = treeView1.HitTest(MousePosition.X-62,MousePosition.Y-109);
            if (info.Node != null)
            {
                toolTip1.SetToolTip(treeView1, info.Node.ToolTipText);
            }
        }

        private void создатьТипToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogForms.CreateTypeForm AskForm = new DialogForms.CreateTypeForm(ConnectionString, this);
        }

        private void изменитьТипToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogForms.ChangeTypeForm AskForm = new DialogForms.ChangeTypeForm(ConnectionString, this);
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogForms.CreateFileForm AskForm = new DialogForms.CreateFileForm(ConnectionString, AskContainerFolderID(), this);
        }

        string filestring;

        private void OpenFileToString(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в строку

            using (FileStream fstream = File.OpenRead(filename))
            {
                // преобразуем строку в байты
                byte[] FileArray = new byte[fstream.Length];
                // считываем данные
                fstream.Read(FileArray, 0, FileArray.Length);
                filestring = Encoding.Default.GetString(FileArray);
            }
            
            textBox2.Text = filestring;
        }
        
        private void SaveStringToFile(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            byte[] TestArray = Encoding.Default.GetBytes(filestring);
            using (FileStream fstream = new FileStream(filename, FileMode.OpenOrCreate))
            {
                fstream.Write(TestArray, 0, TestArray.Length);
            }
        }

        public string TranslateFileToString()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return "\\x00";

            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в строку
            string result= "";
            
            using (FileStream fstream = File.OpenRead(filename))
            {
                // преобразуем строку в байты
                byte[] FileArray = new byte[fstream.Length];

                // считываем данные
                fstream.Read(FileArray, 0, FileArray.Length);

                //формируем строку для выгрузки в БД
                result = "\\x" + BitConverter.ToString(FileArray).Replace("-", string.Empty);
            }
            return result;
        }

        public Image UploadTypeIcon(OdbcConnection connection, string TypeName)
        {

            //Запрос информации о выбранном типе
            string CommandText =
            //$"SELECT * " +
            //$" FROM public.{quote}FileTypes{quote}" +
            //$" WHERE {quote}TypeID{quote} = {TypeID}" +
            //$" ORDER BY {quote}TypeID{quote} ASC ";


            $"SELECT * " +
            $"FROM public.{quote}FileTypes{quote}" +
            $"WHERE {quote}Type{quote} = '{TypeName}'" +
            $"ORDER BY {quote}TypeID{quote} ASC ";

            OdbcCommand TypeReaderCommand = new OdbcCommand(CommandText, connection);

            //Вывод информации о типе в текстовое поле
            OdbcDataReader reader = TypeReaderCommand.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    //считываем изображение
                    byte[] FileArray;
                    FileArray = (byte[])reader.GetValue(2);

                    //сохраняем изображение во временный файл
                    FileStream fstream = new FileStream($"{Directory.GetCurrentDirectory()}./{TypeName}.ico", FileMode.Create);
                    fstream.Write(FileArray, 0, FileArray.Length);


                    //выгружаем изображение из временного файла
                    Bitmap Result = new Bitmap(fstream);//($"{Directory.GetCurrentDirectory()}./{TypeName}.ico");
                    fstream.Close();
                    return (Image)Result;
                }
                catch (Exception)
                {
                    return (Image)new Bitmap(2, 2);
                }
            }
            reader.Close();
            return (Image)new Bitmap(2, 2);
        }
    }
}
