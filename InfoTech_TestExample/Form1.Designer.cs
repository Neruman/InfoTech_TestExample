namespace InfoTech_TestExample
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Узел0");
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.CreateFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.создатьТипToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменитьТипToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.postgresDataSet1 = new InfoTech_TestExample.postgresDataSet();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postgresDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateFolderToolStripMenuItem,
            this.DeleteFolderToolStripMenuItem,
            this.RenameToolStripMenuItem,
            this.loadFileToolStripMenuItem,
            this.downloadFileToolStripMenuItem,
            this.создатьТипToolStripMenuItem,
            this.изменитьТипToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(778, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // CreateFolderToolStripMenuItem
            // 
            this.CreateFolderToolStripMenuItem.Name = "CreateFolderToolStripMenuItem";
            this.CreateFolderToolStripMenuItem.Size = new System.Drawing.Size(136, 20);
            this.CreateFolderToolStripMenuItem.Text = "Создать новую папку";
            this.CreateFolderToolStripMenuItem.Click += new System.EventHandler(this.CreateFolderToolStripMenuItem_Click);
            // 
            // DeleteFolderToolStripMenuItem
            // 
            this.DeleteFolderToolStripMenuItem.Name = "DeleteFolderToolStripMenuItem";
            this.DeleteFolderToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.DeleteFolderToolStripMenuItem.Text = "Удалить";
            this.DeleteFolderToolStripMenuItem.Click += new System.EventHandler(this.DeleteFolderToolStripMenuItem_Click);
            // 
            // RenameToolStripMenuItem
            // 
            this.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem";
            this.RenameToolStripMenuItem.Size = new System.Drawing.Size(106, 20);
            this.RenameToolStripMenuItem.Text = "Переименовать";
            this.RenameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // loadFileToolStripMenuItem
            // 
            this.loadFileToolStripMenuItem.Name = "loadFileToolStripMenuItem";
            this.loadFileToolStripMenuItem.Size = new System.Drawing.Size(132, 20);
            this.loadFileToolStripMenuItem.Text = "Загрузить файл в БД";
            this.loadFileToolStripMenuItem.Click += new System.EventHandler(this.loadFileToolStripMenuItem_Click);
            // 
            // downloadFileToolStripMenuItem
            // 
            this.downloadFileToolStripMenuItem.Name = "downloadFileToolStripMenuItem";
            this.downloadFileToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.downloadFileToolStripMenuItem.Text = "Скачать файл";
            // 
            // создатьТипToolStripMenuItem
            // 
            this.создатьТипToolStripMenuItem.Name = "создатьТипToolStripMenuItem";
            this.создатьТипToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.создатьТипToolStripMenuItem.Text = "Создать тип";
            this.создатьТипToolStripMenuItem.Click += new System.EventHandler(this.создатьТипToolStripMenuItem_Click);
            // 
            // изменитьТипToolStripMenuItem
            // 
            this.изменитьТипToolStripMenuItem.Name = "изменитьТипToolStripMenuItem";
            this.изменитьТипToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.изменитьТипToolStripMenuItem.Text = "Изменить тип";
            this.изменитьТипToolStripMenuItem.Click += new System.EventHandler(this.изменитьТипToolStripMenuItem_Click);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Location = new System.Drawing.Point(0, 24);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Узел0";
            treeNode1.Text = "Узел0";
            treeNode1.ToolTipText = "1231";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView1.Size = new System.Drawing.Size(155, 428);
            this.treeView1.TabIndex = 1;
            this.treeView1.MouseHover += new System.EventHandler(this.treeView1_MouseHover);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(246, 30);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OpenFileToString);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(327, 30);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(431, 20);
            this.textBox1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(165, 30);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.SaveStringToFile);
            // 
            // postgresDataSet1
            // 
            this.postgresDataSet1.DataSetName = "postgresDataSet";
            this.postgresDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(165, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "label1";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(168, 77);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox2.Size = new System.Drawing.Size(590, 373);
            this.textBox2.TabIndex = 6;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 452);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "EXPLORER";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postgresDataSet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem CreateFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DeleteFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RenameToolStripMenuItem;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private postgresDataSet postgresDataSet1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem создатьТипToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изменитьТипToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}

