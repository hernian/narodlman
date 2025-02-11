namespace narodlman
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            新規作成ToolStripMenuItem = new ToolStripMenuItem();
            読み込みToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            終了xToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            textBoxUrlTitlePage = new TextBox();
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            buttonDownload = new Button();
            buttonCancel = new Button();
            openFileDialogBookInfo = new OpenFileDialog();
            label3 = new Label();
            textBoxPathBookInfo = new TextBox();
            textBox1 = new TextBox();
            progressBar = new ProgressBar();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(698, 489);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = SystemColors.MenuBar;
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 新規作成ToolStripMenuItem, 読み込みToolStripMenuItem, toolStripSeparator1, 終了xToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(67, 20);
            fileToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // 新規作成ToolStripMenuItem
            // 
            新規作成ToolStripMenuItem.Name = "新規作成ToolStripMenuItem";
            新規作成ToolStripMenuItem.Size = new Size(131, 22);
            新規作成ToolStripMenuItem.Text = "新規作成...";
            新規作成ToolStripMenuItem.Click += CreateNewToolStripMenuItem_Click;
            // 
            // 読み込みToolStripMenuItem
            // 
            読み込みToolStripMenuItem.Name = "読み込みToolStripMenuItem";
            読み込みToolStripMenuItem.Size = new Size(131, 22);
            読み込みToolStripMenuItem.Text = "読み込み...";
            読み込みToolStripMenuItem.Click += LoadToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(128, 6);
            // 
            // 終了xToolStripMenuItem
            // 
            終了xToolStripMenuItem.Name = "終了xToolStripMenuItem";
            終了xToolStripMenuItem.Size = new Size(131, 22);
            終了xToolStripMenuItem.Text = "終了(&Q)";
            終了xToolStripMenuItem.Click += QuitToolStripMenuItem_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 72);
            label1.Name = "label1";
            label1.Size = new Size(92, 15);
            label1.TabIndex = 2;
            label1.Text = "タイトルページURL";
            // 
            // textBoxUrlTitlePage
            // 
            textBoxUrlTitlePage.Location = new Point(120, 69);
            textBoxUrlTitlePage.Name = "textBoxUrlTitlePage";
            textBoxUrlTitlePage.ReadOnly = true;
            textBoxUrlTitlePage.Size = new Size(654, 23);
            textBoxUrlTitlePage.TabIndex = 3;
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Enabled = false;
            webView21.Location = new Point(12, 109);
            webView21.Name = "webView21";
            webView21.Size = new Size(340, 294);
            webView21.TabIndex = 6;
            webView21.ZoomFactor = 1D;
            // 
            // buttonDownload
            // 
            buttonDownload.Enabled = false;
            buttonDownload.Location = new Point(617, 425);
            buttonDownload.Name = "buttonDownload";
            buttonDownload.Size = new Size(75, 23);
            buttonDownload.TabIndex = 7;
            buttonDownload.Text = "ダウンロード(&D)";
            buttonDownload.UseVisualStyleBackColor = true;
            buttonDownload.Click += ButtonDownload_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(698, 425);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 8;
            buttonCancel.Text = "キャンセル";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += ButtonCancel_Click;
            // 
            // openFileDialogBookInfo
            // 
            openFileDialogBookInfo.DefaultExt = "bookinfo";
            openFileDialogBookInfo.Filter = "bookinfo(*.bookinfo)|*.bookinfo|all(*.*)|*.*";
            openFileDialogBookInfo.Title = "本の情報をロード";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 41);
            label3.Name = "label3";
            label3.Size = new Size(43, 15);
            label3.TabIndex = 9;
            label3.Text = "本情報";
            // 
            // textBoxPathBookInfo
            // 
            textBoxPathBookInfo.Location = new Point(120, 38);
            textBoxPathBookInfo.Name = "textBoxPathBookInfo";
            textBoxPathBookInfo.ReadOnly = true;
            textBoxPathBookInfo.Size = new Size(654, 23);
            textBoxPathBookInfo.TabIndex = 10;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(358, 109);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Size = new Size(416, 294);
            textBox1.TabIndex = 11;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(12, 409);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(761, 10);
            progressBar.TabIndex = 12;
            // 
            // FormMain
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(800, 583);
            Controls.Add(progressBar);
            Controls.Add(textBox1);
            Controls.Add(textBoxPathBookInfo);
            Controls.Add(label3);
            Controls.Add(buttonCancel);
            Controls.Add(buttonDownload);
            Controls.Add(webView21);
            Controls.Add(textBoxUrlTitlePage);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "FormMain";
            Text = "Form1";
            Load += FormMain_Load;
            DragDrop += FormMain_DragDrop;
            DragEnter += FormMain_DragEnter;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem 新規作成ToolStripMenuItem;
        private ToolStripMenuItem 読み込みToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem 終了xToolStripMenuItem;
        private Label label1;
        private TextBox textBoxUrlTitlePage;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private Button buttonDownload;
        private Button buttonCancel;
        private OpenFileDialog openFileDialogBookInfo;
        private Label label3;
        private TextBox textBoxPathBookInfo;
        private TextBox textBox1;
        private ProgressBar progressBar;
    }
}
