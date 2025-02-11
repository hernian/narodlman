using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Text;

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

        private async void CreateNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using var form = new FormCreateBookInfo(webView21.CoreWebView2.Environment);
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                var bim = form.GetBookInfoMan();
                bim.Save();
                SetBookInfoMan(bim);
            }
            await form.Wait();
            Debug.WriteLine("Wait Compl. FormCreateBookInfo");
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
                webView21.Enabled = false;
                webView21.ZoomFactor = 0.3;
                progressBar.Style = ProgressBarStyle.Marquee;
                var nn = new NaroNavigator(webView21, textBoxUrlTitlePage.Text);
                using var cts = new CancellationTokenSource();
                var ct = cts.Token;
                using var logger = new Logger(_bookInfoMan.PathLogFile);
                var updater = await BookUpdater.CreateAsync(_bookInfoMan.PathZipFile, logger, ct);
                var epNumStart = updater.GetEpisodeNumberForCheck();
                var epInfosNow = await nn.GetEpisodeInfoListAsync(epNumStart, ct);
                var epInfosDL = await updater.GetEpsodeInfosForUpdateAsync(epInfosNow, ct);
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Maximum = epInfosDL.Count;
                progressBar.Value = 0;
                void CallbackEpisode()
                {
                    this.Invoke(() =>
                    {
                        progressBar.Value += 1;
                    });
                }
                var episodesNew = await nn.GetEpisodesAsync(epInfosDL, CallbackEpisode, ct);
                await updater.MergeSaveAsync(episodesNew, ct);
                var strHtml = await GetNewEpisodesMessage(episodesNew);
                await updater.OutputGenEPubBatAsync(_bookInfoMan.BookInfo, ct);
                webView21.NavigateToString(strHtml);
                webView21.ZoomFactor = 1.0;
                webView21.Enabled = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Maximum = 100;
                progressBar.Value = 0;
                buttonCancel.Enabled = false;
                buttonDownload.Enabled = true;
            }
        }

        private static Task<string> GetNewEpisodesMessage(IEnumerable<Episode> episodesNew)
        {
            return Task.Run<string>(() =>
            {
                var sb = new StringBuilder();
                sb.AppendLine("<html>");
                sb.AppendLine("<head>");
                sb.AppendLine("<title>結果</title>");
                sb.AppendLine("<style>table, table th, table td { border: 1px solid black; }</style>");
                sb.AppendLine("</head>");
                sb.AppendLine("<body>");
                if (episodesNew.Any())
                {
                    sb.AppendLine("<p>更新されたエピソード</p>");
                    sb.AppendLine("<table>");
                    sb.AppendLine("<tr><th>#</th><th>タイトル</th><th>更新日時</th></tr>");
                    foreach (var epNew in episodesNew)
                    {
                        sb.Append("<tr>");
                        sb.Append($"<td>{epNew.EpisodeNumber}</td>");
                        sb.Append($"<td>{epNew.Title}</td>");
                        sb.Append($"<td>{DateTimeUtil.GetDateTimeString(epNew.LastModifiedTime.LocalDateTime)}</td>");
                        sb.AppendLine("</tr>");
                    }
                    sb.AppendLine("</table>");
                }
                else
                {
                    sb.AppendLine("<p>更新されたエピソードはありません</p>");
                }
                sb.AppendLine("</body></html>");
                return sb.ToString();
            });
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileNames = (string[]?)e.Data.GetData(DataFormats.FileDrop, false);
                if (fileNames != null && fileNames.Length == 1)
                {
                    var pathBookInfo = fileNames[0];
                    var ext = Path.GetExtension(pathBookInfo).ToLower();
                    if (ext == ".bookinfo")
                    {
                        e.Effect = DragDropEffects.Copy;
                        return;
                    }
                }
            }
            e.Effect = DragDropEffects.None;
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }
            var fileNames = (string[]?)e.Data.GetData(DataFormats.FileDrop, false);
            if (fileNames == null || fileNames.Length < 1)
            {
                return;
            }
            try
            {
                var pathBookInfo = fileNames[0];
                var bookInfoMah = BookInfoMan.Load(pathBookInfo);
                SetBookInfoMan(bookInfoMah);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
