using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace narodlman
{
    internal partial class BookUpdater
    {
        private const int COUNT_EPISODES_CHECH_UPDATE = 100;
        private const int MAX_EPISODE_TITLE_LEN = 64;
        public class Error(string msg) : Exception(msg) { }

        public static Task<BookUpdater> CreateAsync(string pathZip, Logger logger, CancellationToken ct)
        {
            return Task.Run<BookUpdater>(() => new BookUpdater(pathZip, logger, ct), ct);
        }

        private readonly string _pathZip;
        private readonly Logger _logger;
        private readonly Encoding _enc;
        private readonly Regex _regexEpNum = RegexEpNum();
        private readonly List<Episode> _episodes;
        private readonly char[] _invalidFileNameChars = Path.GetInvalidFileNameChars();

        private BookUpdater(string pathZip, Logger logger, CancellationToken ct)
        {
            _pathZip = pathZip;
            _logger = logger;
            _enc = new UTF8Encoding(false);

            var listEpisode = new List<Episode>();
            if (File.Exists(pathZip))
            {
                using var zipArch = ZipFile.OpenRead(_pathZip);
                foreach (var entry in zipArch.Entries)
                {
                    ct.ThrowIfCancellationRequested();
                    var episode = LoadEpisode(entry);
                    listEpisode.Add(episode);
                }
            }
            _episodes = listEpisode;
        }

        public int GetEpisodeNumberForCheck()
        {
            var epNum = 1;
            if (_episodes.Count > 0)
            {
                var epNumLast = _episodes[^1].EpisodeNumber;
                epNum = Math.Max(epNumLast - COUNT_EPISODES_CHECH_UPDATE, 1);
            }
            return epNum;
        }
        public Task<ReadOnlyCollection<EpisodeInfo>> GetEpsodeInfosForUpdateAsync(IEnumerable<EpisodeInfo> epInfNews, CancellationToken ct)
        {
            return Task.Run<ReadOnlyCollection<EpisodeInfo>>(() => GetEpisodeInfosForUpdateInner(epInfNews, ct), ct);
        }

        private ReadOnlyCollection<EpisodeInfo> GetEpisodeInfosForUpdateInner(IEnumerable<EpisodeInfo> epInfNews, CancellationToken ct)
        {
            var listEpInfoNew = new List<EpisodeInfo>();
            int idx = 0;
            foreach (var epInfNew in epInfNews)
            {
                ct.ThrowIfCancellationRequested();
                if (idx < _episodes.Count)
                {
                    var idxFound = _episodes.FindIndex(idx, e => e.EpisodeNumber == epInfNew.EpisodeNumber);
                    if (idxFound >= 0)
                    {
                        idx = idxFound + 1;
                        var epNow = _episodes[idxFound];
                        Debug.WriteLine($"#{epInfNew.EpisodeNumber} new: {epInfNew.LastModifiedTime.UtcDateTime:s}, now: {epNow.LastModifiedTime.UtcDateTime:s}");
                        if (epInfNew.LastModifiedTime <= epNow.LastModifiedTime)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"#{epInfNew.EpisodeNumber} now: {epInfNew.LastModifiedTime.ToLocalTime()}");
                        idx = _episodes.Count + 1;
                    }
                }
                else
                {
                    Debug.WriteLine($"#{epInfNew.EpisodeNumber} now: {epInfNew.LastModifiedTime.ToLocalTime()}");
                }
                listEpInfoNew.Add(epInfNew);
            }
            return listEpInfoNew.AsReadOnly();
        }

        public Task MergeSaveAsync(IEnumerable<Episode> episodesNew, CancellationToken ct)
        {
            return Task.Run(() => MergeSaveInner(episodesNew, ct), ct);
        }

        private void MergeSaveInner(IEnumerable<Episode> episodesNew, CancellationToken ct)
        {
            var arrayEpsodesNew = episodesNew.ToArray();
            if (arrayEpsodesNew.Length > 0)
            {
                _logger.WriteLine("更新されたエピソード");
                foreach (var epNew in arrayEpsodesNew)
                {
                    _logger.WriteFragmentLine($"#{epNew.EpisodeNumber}, {epNew.Title}, {DateTimeUtil.GetDateTimeString(epNew.LastModifiedTime)}");
                }
            }
            else
            {
                _logger.WriteLine("更新されたエピソードはありません");
            }

            var listWrite = new List<Episode>();
            var idx = 0;
            foreach (var episode in _episodes)
            {
                ct.ThrowIfCancellationRequested();
                if (idx < arrayEpsodesNew.Length)
                {
                    var idxFound = Array.FindIndex(arrayEpsodesNew, idx, e => e.EpisodeNumber == episode.EpisodeNumber);
                    if (idxFound >= 0)
                    {
                        listWrite.Add(arrayEpsodesNew[idxFound]);
                        idx = idxFound + 1;
                        continue;
                    }
                }
                listWrite.Add(episode);
            }
            if (idx < arrayEpsodesNew.Length)
            {
                listWrite.AddRange(arrayEpsodesNew[idx..arrayEpsodesNew.Length]);
            }

            var pathZipTemp = $"{_pathZip}.temp";
            if (File.Exists(pathZipTemp))
            {
                File.Delete(pathZipTemp);
            }
            // File.Move()の前にZipファイルを閉じる必要があるので、旧い形のusingにしてzipArchのスコープを限定した
            using (var zipArch = ZipFile.Open(pathZipTemp, ZipArchiveMode.Create))
            {
                foreach (var episode in listWrite)
                {
                    ct.ThrowIfCancellationRequested();
                    SaveEpisode(zipArch, episode);
                }
            }
            ct.ThrowIfCancellationRequested();
            File.Move(pathZipTemp, _pathZip, true);
        }

        private Episode LoadEpisode(ZipArchiveEntry entry)
        {
            var match = _regexEpNum.Match(entry.FullName);
            if (!match.Success)
            {
                throw new Error($"Invalid Episode Name In Zip. {entry.FullName}");
            }
            var epNum = int.Parse(match.Groups[1].Value);

            var lastModTime = entry.LastWriteTime;

            using var zipStream = entry.Open();
            using var reader = new StreamReader(zipStream, _enc);
            var title = reader.ReadLine() ?? throw new Exception("Invalid Zip Entry.");
            var listPara = new List<string>();
            while (true)
            {
                var para = reader.ReadLine();
                if (para == null)
                {
                    break;
                }
                listPara.Add(para);
            }
            var episode = new Episode(epNum, title, lastModTime, listPara.AsReadOnly());
            return episode;
        }
        private void SaveEpisode(ZipArchive zipArch, Episode episode)
        {
            var entryName = GetEntryNameFromEpisode(episode);
            var entry = zipArch.CreateEntry(entryName);
            entry.LastWriteTime = episode.LastModifiedTime;
            using var zipStream = entry.Open();
            using var writer = new StreamWriter(zipStream, _enc);
            writer.Write(episode.Title);
            foreach (var para in episode.Paragraphs)
            {
                writer.WriteLine(para);
            }
        }

        private string GetEntryNameFromEpisode(Episode episode)
        {
            var sb = new StringBuilder();
            sb.Append($"エピソード{episode.EpisodeNumber}：");
            var len = Math.Min(episode.Title.Length, MAX_EPISODE_TITLE_LEN);
            foreach (var ch in episode.Title[0..len])
            {
                if (_invalidFileNameChars.Contains(ch))
                {
                    continue;
                }
                sb.Append(ch);
            }
            if (episode.Title.Length > MAX_EPISODE_TITLE_LEN)
            {
                sb.Append('…');
            }
            sb.Append(".txt");
            return sb.ToString();
        }

        public Task OutputGenEPubBatAsync(BookInfo bookInfo, CancellationToken ct)
        {
            return Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();
                var genBat = new genepub_bat(_pathZip, bookInfo);
                var strBat = genBat.TransformText();
                var enc = new UTF8Encoding(false);
                var dirZip = Path.GetDirectoryName(_pathZip) ?? string.Empty;
                var fileName = Path.GetFileNameWithoutExtension(_pathZip);
                var pathBat = Path.Combine(dirZip, $"{fileName}_genepub.bat");
                using var writer = new StreamWriter(pathBat, false, enc);
                writer.WriteLine(strBat);
            }, ct);
        }

        [GeneratedRegex(@"エピソード(\d+)：")]
        private static partial Regex RegexEpNum();
    }
}
