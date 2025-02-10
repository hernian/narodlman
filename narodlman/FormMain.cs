using Microsoft.Web.WebView2.Core;
using System.Diagnostics;

namespace narodlman
{
    public partial class FormMain : Form
    {
        BookInfoMan? _bookInfoMan;

        public FormMain()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
        }
        private void SetBookInfoMan(BookInfoMan bookInfoMan)
        {
            _bookInfoMan = bookInfoMan;
            var pathBookInfo = bookInfoMan.PathBookInfo;
            textBoxPathBookInfo.Text = pathBookInfo;
            textBoxUrlTitlePage.Text = _bookInfoMan.UrlTitlePage;

            Properties.Settings.Default.PathBookInfo = pathBookInfo;
            Properties.Settings.Default.Save();

            buttonDownload.Enabled = true;
        }

        private void CreateNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var form = new FormCreateBookInfo();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                var pathBookInfo = form.PathBookInfo;
                var dirZip = Path.GetDirectoryName(pathBookInfo) ?? string.Empty;
                var fileName = Path.GetFileNameWithoutExtension(pathBookInfo);
                var pathZip = Path.Combine(dirZip, $"{fileName}.zip");
                var bookInfoMan = BookInfoMan.Create(pathBookInfo, form.UrlTitlePage, pathZip);
                bookInfoMan.Save();
                SetBookInfoMan(bookInfoMan);

            }
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialogBookInfo.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var bookInfoMan = BookInfoMan.Load(openFileDialogBookInfo.FileName);
                    SetBookInfoMan(bookInfoMan);
                }
                catch (Exception ex)
                {
                    DebugWriteException(ex);
                }
            }
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private static void DebugWriteException(Exception ex)
        {
            for (var e = ex; e != null; e = e.InnerException)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {

        }

        private async void FormMain_Load(object sender, EventArgs e)
        {
            var dirTemp = Path.GetTempPath();
            var dirCache = Path.Combine(dirTemp, "narodlman.temp");
            Directory.CreateDirectory(dirCache);
            var webView2Environment = await CoreWebView2Environment.CreateAsync(null, dirCache);
            await webView21.EnsureCoreWebView2Async(webView2Environment);

            var pathBookInfo = Properties.Settings.Default.PathBookInfo;
            if (pathBookInfo != string.Empty)
            {
                var bookInfoMah = BookInfoMan.Load(pathBookInfo);
                SetBookInfoMan(bookInfoMah);
            }
        }

        private async void ButtonDownload_Click(object sender, EventArgs e)
        {
            if (_bookInfoMan == null)
            {
                return;
            }
            try
            {
                buttonDownload.Enabled = false;
                buttonCancel.Enabled = true;
                var nn = new NaroNavigator(webView21, textBoxUrlTitlePage.Text);
                using var cts = new CancellationTokenSource();
                var ct = cts.Token;
                var updater = await BookUpdater.CreateAsync(_bookInfoMan.PathZipFile, ct);
                var epNumStart = updater.GetEpisodeNumberForCheck();
                var epInfosNow = await nn.GetEpisodeInfoListAsync(epNumStart, ct);
                var epInfosDL = await updater.GetEpsodeInfosForUpdateAsync(epInfosNow, ct);
                var episodesNew = await nn.GetEpisodesAsync(epInfosDL, ct);
                await updater.MergeSaveAsync(episodesNew, ct);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                buttonCancel.Enabled = false;
                buttonDownload.Enabled = true;
            }
        }
    }
}
