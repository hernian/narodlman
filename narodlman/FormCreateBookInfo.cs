using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;


namespace narodlman
{
    public partial class FormCreateBookInfo : Form
    {
        private static readonly string[] URL_DATA_FORMAT = ["UniformResourceLocator", "UniformResourceLocatorW"];

        private CancellationTokenSource _ctsTitleAuthor = new();
        private Task? _taskSetTitleAuthor;
        private readonly CoreWebView2Environment _env;

        public FormCreateBookInfo(CoreWebView2Environment env)
        {
            InitializeComponent();
            _env = env;
        }

        private void FormCreateBookInfo_DragEnter(object sender, DragEventArgs e)
        {
            var dataObj = e.Data;
            if (dataObj == null)
            {
                return;
            }
            foreach (var format in URL_DATA_FORMAT)
            {
                if (dataObj.GetDataPresent(format))
                {
                    e.Effect = DragDropEffects.Copy;
                    break;
                }
            }
        }

        public string UrlTitlePage
        {
            get
            {
                return textBoxUrlTitlePage.Text;
            }
        }

        public string PathBookInfo
        {
            get
            {
                return saveFileDialog.FileName;
            }
        }

        private void FormCreateBookInfo_DragDrop(object sender, DragEventArgs e)
        {
            var dataObj = e.Data;
            if (dataObj == null)
            {
                return;
            }
            textBoxUrlTitlePage.Text = dataObj.GetData(DataFormats.Text)?.ToString() ?? "";
        }

        private void ButtonGetTitleAuthor_Click(object sender, EventArgs e)
        {
            _taskSetTitleAuthor = SetDefaultTitleAuthorAsync();
        }

        private async Task SetDefaultTitleAuthorAsync()
        {
            _ctsTitleAuthor.Cancel();
            _ctsTitleAuthor = new();

            if (webView2.CoreWebView2 == null)
            {
                await webView2.EnsureCoreWebView2Async(_env);
            }
            // VisualStudioの警告が出るので、本来不要だがwebView2.CoreWebView2のnullチェックを行う
            if (webView2.CoreWebView2 == null)
            {
                return;
            }
            try
            {
                var nn = new NaroNavigator(webView2, textBoxUrlTitlePage.Text);
                var titleAuthor = await nn.GetTitleAuthorAsync(_ctsTitleAuthor.Token);
                textBoxTitle.Text = titleAuthor.Title;
                textBoxAuthor.Text = titleAuthor.Author;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public Task Wait()
        {
            if (_taskSetTitleAuthor == null)
            {
                return Task.CompletedTask;
            }
            _ctsTitleAuthor.Cancel();
            return _taskSetTitleAuthor;
        }

        public BookInfoMan GetBookInfoMan()
        {
            var bmi = BookInfoMan.Create(
                saveFileDialog.FileName,
                textBoxUrlTitlePage.Text,
                textBoxTitle.Text,
                textBoxKanaTitle.Text,
                textBoxAuthor.Text,
                textBoxKanaAuthor.Text);
            return bmi;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
