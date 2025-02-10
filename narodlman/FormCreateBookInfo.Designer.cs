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
            label2 = new Label();
            textBoxPathBookInfo = new TextBox();
            buttonBrowse = new Button();
            saveFileDialog = new SaveFileDialog();
            SuspendLayout();
            // 
            // buttonOK
            // 
            buttonOK.DialogResult = DialogResult.OK;
            buttonOK.Location = new Point(552, 74);
            buttonOK.Name = "buttonOK";
            buttonOK.Size = new Size(75, 23);
            buttonOK.TabIndex = 5;
            buttonOK.Text = "OK";
            buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            buttonCancel.DialogResult = DialogResult.Cancel;
            buttonCancel.Location = new Point(643, 74);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(75, 23);
            buttonCancel.TabIndex = 6;
            buttonCancel.Text = "キャンセル";
            buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 38);
            label1.Name = "label1";
            label1.Size = new Size(106, 15);
            label1.TabIndex = 3;
            label1.Text = "タイトルページURL(&T)";
            // 
            // textBoxUrlTitlePage
            // 
            textBoxUrlTitlePage.Location = new Point(138, 35);
            textBoxUrlTitlePage.Name = "textBoxUrlTitlePage";
            textBoxUrlTitlePage.Size = new Size(580, 23);
            textBoxUrlTitlePage.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 9);
            label2.Name = "label2";
            label2.Size = new Size(68, 15);
            label2.TabIndex = 0;
            label2.Text = "本の情報(&B)";
            // 
            // textBoxPathBookInfo
            // 
            textBoxPathBookInfo.Location = new Point(138, 6);
            textBoxPathBookInfo.Name = "textBoxPathBookInfo";
            textBoxPathBookInfo.ReadOnly = true;
            textBoxPathBookInfo.Size = new Size(528, 23);
            textBoxPathBookInfo.TabIndex = 1;
            // 
            // buttonBrowse
            // 
            buttonBrowse.Location = new Point(672, 6);
            buttonBrowse.Name = "buttonBrowse";
            buttonBrowse.Size = new Size(46, 23);
            buttonBrowse.TabIndex = 2;
            buttonBrowse.Text = "...";
            buttonBrowse.UseVisualStyleBackColor = true;
            buttonBrowse.Click += ButtonBrowse_Click;
            // 
            // saveFileDialog
            // 
            saveFileDialog.DefaultExt = "bookinfo";
            saveFileDialog.Filter = "bookinfo(*.bookinfo)|*.bookinfo|all(*.*)|*.*";
            saveFileDialog.Title = "本の情報を保存";
            // 
            // FormCreateBookInfo
            // 
            AcceptButton = buttonOK;
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new Size(730, 114);
            Controls.Add(buttonBrowse);
            Controls.Add(textBoxPathBookInfo);
            Controls.Add(label2);
            Controls.Add(textBoxUrlTitlePage);
            Controls.Add(label1);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOK);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormCreateBookInfo";
            ShowInTaskbar = false;
            Text = "本情報の作成";
            DragDrop += FormCreateBookInfo_DragDrop;
            DragEnter += FormCreateBookInfo_DragEnter;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonOK;
        private Button buttonCancel;
        private Label label1;
        private TextBox textBoxUrlTitlePage;
        private Label label2;
        private TextBox textBoxPathBookInfo;
        private Button buttonBrowse;
        private SaveFileDialog saveFileDialog;
    }
}