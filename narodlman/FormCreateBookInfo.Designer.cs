namespace narodlman
{
    partial class FormCreateBookInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonOK = new Button();
            buttonCancel = new Button();
            label1 = new Label();
            textBoxUrlTitlePage = new TextBox();
            saveFileDialog = new SaveFileDialog();
            buttonGetTitleAuthor = new Button();
            webView2 = new Microsoft.Web.WebView2.WinForms.WebView2();
            label3 = new Label();
            textBoxTitle = new TextBox();
            label4 = new Label();
            textBoxKanaTitle = new TextBox();
            label5 = new Label();
            textBoxAuthor = new TextBox();
            label6 = new Label();
            textBoxKanaAuthor = new TextBox();
            ((System.ComponentModel.ISupportInitialize)webView2).BeginInit();
            SuspendLayout();
            // 
            // buttonOK
            // 
            buttonOK.DialogResult = DialogResult.OK;
            buttonOK.Location = new Point(545, 299);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 15;
            buttonOK.Text = "保存(&S)";
            buttonOK.UseVisualStyleBackColor = true;
            buttonOK.Click += ButtonOK_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Location = new Point(650, 299);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 16;
            buttonCancel.Text = "キャンセル";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 18);
            label1.Name = "label1";
            label1.Size = new Size(106, 15);
            label1.TabIndex = 3;
            label1.Text = "タイトルページURL(&T)";
            // 
            // textBoxUrlTitlePage
            // 
            textBoxUrlTitlePage.Location = new Point(136, 14);
            textBoxUrlTitlePage.Name = "textBoxUrlTitlePage";
            textBoxUrlTitlePage.Size = new Size(478, 23);
            textBoxUrlTitlePage.TabIndex = 4;
            // 
            // saveFileDialog
            // 
            saveFileDialog.DefaultExt = "bookinfo";
            saveFileDialog.Filter = "bookinfo(*.bookinfo)|*.bookinfo|all(*.*)|*.*";
            saveFileDialog.Title = "本の情報を保存";
            // 
            // buttonGetTitleAuthor
            // 
            buttonGetTitleAuthor.Location = new Point(620, 14);
            buttonGetTitleAuthor.Name = "buttonGetTitleAuthor";
            buttonGetTitleAuthor.Size = new Size(96, 23);
            buttonGetTitleAuthor.TabIndex = 5;
            buttonGetTitleAuthor.Text = "情報取得(&G)";
            buttonGetTitleAuthor.UseVisualStyleBackColor = true;
            buttonGetTitleAuthor.Click += ButtonGetTitleAuthor_Click;
            // 
            // webView2
            // 
            webView2.AllowExternalDrop = true;
            webView2.CreationProperties = null;
            webView2.DefaultBackgroundColor = Color.White;
            webView2.Enabled = false;
            webView2.Location = new Point(10, 191);
            webView2.Name = "webView2";
            webView2.Size = new Size(189, 90);
            webView2.TabIndex = 14;
            webView2.ZoomFactor = 0.3D;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 50);
            label3.Name = "label3";
            label3.Size = new Size(57, 15);
            label3.TabIndex = 6;
            label3.Text = "タイトル(&T)";
            // 
            // textBoxTitle
            // 
            textBoxTitle.Location = new Point(136, 47);
            textBoxTitle.Name = "textBoxTitle";
            textBoxTitle.Size = new Size(580, 23);
            textBoxTitle.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(10, 83);
            label4.Name = "label4";
            label4.Size = new Size(90, 15);
            label4.TabIndex = 8;
            label4.Text = "カタカナタイトル(&I)";
            // 
            // textBoxKanaTitle
            // 
            textBoxKanaTitle.ImeMode = ImeMode.Katakana;
            textBoxKanaTitle.Location = new Point(136, 80);
            textBoxKanaTitle.Name = "textBoxKanaTitle";
            textBoxKanaTitle.Size = new Size(580, 23);
            textBoxKanaTitle.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(10, 116);
            label5.Name = "label5";
            label5.Size = new Size(59, 15);
            label5.TabIndex = 10;
            label5.Text = "著者名(&A)";
            // 
            // textBoxAuthor
            // 
            textBoxAuthor.Location = new Point(136, 113);
            textBoxAuthor.Name = "textBoxAuthor";
            textBoxAuthor.Size = new Size(580, 23);
            textBoxAuthor.TabIndex = 11;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(10, 149);
            label6.Name = "label6";
            label6.Size = new Size(95, 15);
            label6.TabIndex = 12;
            label6.Text = "カタカナ著者名(&U)";
            // 
            // textBoxKanaAuthor
            // 
            textBoxKanaAuthor.ImeMode = ImeMode.Katakana;
            textBoxKanaAuthor.Location = new Point(136, 146);
            textBoxKanaAuthor.Name = "textBoxKanaAuthor";
            textBoxKanaAuthor.Size = new Size(580, 23);
            textBoxKanaAuthor.TabIndex = 13;
            // 
            // FormCreateBookInfo
            // 
            AcceptButton = buttonOK;
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new Size(730, 331);
            Controls.Add(textBoxKanaAuthor);
            Controls.Add(label6);
            Controls.Add(textBoxAuthor);
            Controls.Add(label5);
            Controls.Add(textBoxKanaTitle);
            Controls.Add(label4);
            Controls.Add(textBoxTitle);
            Controls.Add(label3);
            Controls.Add(webView2);
            Controls.Add(buttonGetTitleAuthor);
            Controls.Add(textBoxUrlTitlePage);
            Controls.Add(label1);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOK);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCreateBookInfo";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "本情報の作成";
            DragDrop += FormCreateBookInfo_DragDrop;
            DragEnter += FormCreateBookInfo_DragEnter;
            ((System.ComponentModel.ISupportInitialize)webView2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonOK;
        private Button buttonCancel;
        private Label label1;
        private TextBox textBoxUrlTitlePage;
        private SaveFileDialog saveFileDialog;
        private Button buttonGetTitleAuthor;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView2;
        private Label label3;
        private TextBox textBoxTitle;
        private Label label4;
        private TextBox textBoxKanaTitle;
        private Label label5;
        private TextBox textBoxAuthor;
        private Label label6;
        private TextBox textBoxKanaAuthor;
    }
}