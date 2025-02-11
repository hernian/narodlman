using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.Core;
using AngleSharp.Html.Parser;
using AngleSharp.Html.Dom;
using System.Text.RegularExpressions;
using System.Security.Cryptography.Xml;

namespace narodlman
{
    internal partial class NaroNavigator(WebView2 webView2, string urlTitlePage)
    {
        public class Error(string msg) : Exception(msg) { }

        public class TitleAuthor(string title, string author)
        {
            public readonly string Title = title;
            public readonly string Author = author;
        }

        private readonly WebView2 _webView2 = webView2;
        private readonly string _utlTitlePage = urlTitlePage;
        private readonly HtmlParser _htmlParser = new ();

        public async Task<IReadOnlyCollection<EpisodeInfo>> GetEpisodeInfoListAsync(int epNumStart, CancellationToken ct)
        {
            using var sem = new SemaphoreSlim(0);
            void OnNavigationCompl(object? sender, CoreWebView2NavigationCompletedEventArgs e)
            {
                Debug.WriteLine($"Navigation Completed. {_webView2.CoreWebView2.Source}");
                sem.Release();
            };
            _webView2.NavigationCompleted += OnNavigationCompl;

            var listEpisodeInfo = new List<EpisodeInfo>();
            try
            {
                Debug.WriteLine("Getting the episode infos from server");
                var sep = _utlTitlePage[^1] != '/' ? "/" : "";
                var pageNum = (epNumStart + 99) / 100;
                var url = $"{_utlTitlePage}{sep}?p={pageNum}";
                Debug.WriteLine($"Navigate To {url}");
                _webView2.CoreWebView2.Navigate(url);
                while (true)
                {
                    await sem.WaitAsync(ct);
                    ct.ThrowIfCancellationRequested();
                    var res = await _webView2.ExecuteScriptAsync("document.documentElement.outerHTML;");
                    ct.ThrowIfCancellationRequested();
                    var urlNextPage = await Task.Run<string>(() =>
                    {
                        var strHtml = System.Text.Json.JsonDocument.Parse(res).RootElement.GetString() ?? throw new Error($"JavaScrip Execution Error");
                        var doc = _htmlParser.ParseDocument(strHtml);
                        var urlNext = ParseEpisodeInfoList(doc, listEpisodeInfo);
                        return urlNext;
                    }, ct);
                    if (urlNextPage == string.Empty)
                    {
                        break;
                    }
                    ct.ThrowIfCancellationRequested();
                    Debug.WriteLine($"Navigate To {urlNextPage}");
                    await _webView2.ExecuteScriptAsync($"location.href = '{urlNextPage}';");
                }
                foreach (var epInfo in listEpisodeInfo)
                {
                    Debug.WriteLine($"#{epInfo.EpisodeNumber}, {epInfo.Url}, {epInfo.Title}, {epInfo.LastModifiedTime}");
                }
                
            }
            finally
            {
                _webView2.NavigationCompleted -= OnNavigationCompl;
            }
            return listEpisodeInfo.AsReadOnly();
        }

        private static string ParseEpisodeInfoList(IHtmlDocument doc, List<EpisodeInfo> listEpisodeInfo)
        {
            var elemNextPage = doc.QuerySelector("a.c-pager__item--next");
            var urlNextPage = elemNextPage?.GetAttribute("href") ?? string.Empty;

            var epNumFirst = GetFirstEpisodeNumber(doc);

            var regexEpDate = RegexEpDate();
            var epNum = epNumFirst;
            var elemEpList = doc.QuerySelectorAll("div.p-eplist__sublist");
            foreach (var elemEp in elemEpList.Cast<IHtmlElement>())
            {
                var elemLink = elemEp.QuerySelector("a.p-eplist__subtitle") ?? throw new Error("No Episode Link");
                var urlEpisode = elemLink.GetAttribute("href") ?? throw new Error("No href in Episode Link.");
                var epTitle = elemLink.TextContent.Trim();

                var elemDate = elemEp.QuerySelector("div.p-eplist__update") ?? throw new Error("No Episode Date");
                var elemDateMod = elemDate.QuerySelector("span[title]");
                var strDate = (elemDateMod != null) ? elemDateMod.GetAttribute("title") ?? "" : elemDate.TextContent;
                var match = regexEpDate.Match(strDate);
                if (!match.Success)
                {
                    throw new Error("Invalid Episode Date");
                }
                var dtTemp = DateTime.Parse(match.Value);
                var dtUtc = DateTime.SpecifyKind(dtTemp, DateTimeKind.Local);
                var epDate = new DateTimeOffset(dtUtc);
                Debug.WriteLine($"#{epNum}, {epTitle}, {epDate.UtcDateTime:s}");
                var epInfo = new EpisodeInfo()
                {
                    EpisodeNumber = epNum,
                    Title = epTitle,
                    Url = urlEpisode,
                    LastModifiedTime = epDate,
                };
                listEpisodeInfo.Add(epInfo);
                epNum++;
            }

            return urlNextPage;
        }

        private static int GetFirstEpisodeNumber(IHtmlDocument doc)
        {
            var elemPagerRes = doc.QuerySelector("div.c-pager__result-stats");
            // 1ページに全エピソードが入る場合ページャーは存在しない
            // その場合は開始エピソード番号は1となる
            if (elemPagerRes == null)
            {
                return 1;
            }
            var regexpFirstPage = RegexFirstPage();
            var matchFirstPage = regexpFirstPage.Match(elemPagerRes.TextContent);
            if (!matchFirstPage.Success)
            {
                throw new Error($"Invalid Pager Result");
            }
            var epNumFirst = int.Parse(matchFirstPage.Value);
            return epNumFirst;
        }

        public async Task<IReadOnlyCollection<Episode>> GetEpisodesAsync(IEnumerable<EpisodeInfo> epInfos, Action callback, CancellationToken ct)
        {
            using var sem = new SemaphoreSlim(0);
            void OnNavigationCompl(object? sender, CoreWebView2NavigationCompletedEventArgs e)
            {
                Debug.WriteLine($"Navigation Completed. {_webView2.CoreWebView2.Source}");
                sem.Release();
            };
            _webView2.NavigationCompleted += OnNavigationCompl;

            var listEpisode = new List<Episode>();
            try
            {
                foreach (var epInfo in epInfos)
                {
                    ct.ThrowIfCancellationRequested();
                    var strJs = $"location.href = '{epInfo.Url}';";
                    await _webView2.CoreWebView2.ExecuteScriptAsync(strJs);
                    await sem.WaitAsync(ct);
                    ct.ThrowIfCancellationRequested();
                    var res = await _webView2.ExecuteScriptAsync("document.documentElement.outerHTML;");
                    ct.ThrowIfCancellationRequested();
                    await Task.Run(() =>
                    {
                        var strHtml = System.Text.Json.JsonDocument.Parse(res).RootElement.GetString() ?? throw new Error($"JavaScrip Execution Error");
                        var doc = _htmlParser.ParseDocument(strHtml);
                        var episode = ParseEpisode(doc, epInfo);
                        listEpisode.Add(episode);
                    }, ct);
                    callback();
                }
            }
            finally
            {
                _webView2.NavigationCompleted -= OnNavigationCompl;
            }
            return listEpisode.AsReadOnly();
        }

        private static Episode ParseEpisode(IHtmlDocument doc, EpisodeInfo epInfo)
        {
            var elemNovelBody = doc.QuerySelector("div.p-novel__body") ?? throw new Error("No Novel Body");
            int lineNumber = 1;
            var listPara = new List<string>();
            while (true)
            {
                var selector = $"p#L{lineNumber}";
                var elemPara = elemNovelBody.QuerySelector(selector);
                if (elemPara == null)
                {
                    break;
                }
                var strPara = elemPara.TextContent;
                listPara.Add(strPara);
                lineNumber++;
            }
            var episode = new Episode(epInfo.EpisodeNumber, epInfo.Title, epInfo.LastModifiedTime, listPara.AsReadOnly());
            return episode;
        }

        public async Task<TitleAuthor> GetTitleAuthorAsync(CancellationToken ct)
        {
            using var sem = new SemaphoreSlim(0);
            void OnNavigationCompl(object? sender, CoreWebView2NavigationCompletedEventArgs e)
            {
                Debug.WriteLine("Navigation Completed.");
                sem.Release();
            };
            _webView2.NavigationCompleted += OnNavigationCompl;
            try
            {
                ct.ThrowIfCancellationRequested();
                _webView2.CoreWebView2.Navigate(_utlTitlePage);
                await sem.WaitAsync(ct);
                ct.ThrowIfCancellationRequested();
                var res = await _webView2.ExecuteScriptAsync("document.documentElement.outerHTML;");
                ct.ThrowIfCancellationRequested();
                var titleAuthor = await Task.Run<TitleAuthor>(() =>
                {
                    var strHtml = System.Text.Json.JsonDocument.Parse(res).RootElement.GetString() ?? throw new Error($"JavaScrip Execution Error");
                    var doc = _htmlParser.ParseDocument(strHtml);
                    var titleAuthor = ParseTitleAuthor(doc);
                    return titleAuthor;
                }, ct);
                return titleAuthor;
            }
            finally
            {
                _webView2.NavigationCompleted -= OnNavigationCompl;
            }
        }

        private static TitleAuthor ParseTitleAuthor(IHtmlDocument doc)
        {
            var strTitle = doc.QuerySelector("h1.p-novel__title")?.TextContent ?? throw new Exception("No Title");
            var strAuthor = doc.QuerySelector("div.p-novel__author > a")?.TextContent ?? throw new Exception("No Author");
            return new TitleAuthor(strTitle, strAuthor);
        }

        [GeneratedRegex(@"[0-9\/: ]+")]
        private static partial Regex RegexEpDate();
        [GeneratedRegex(@"\d+")]
        private static partial Regex RegexFirstPage();
    }
}
