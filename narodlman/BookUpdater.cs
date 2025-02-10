using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public class Error(string msg) : Exception(msg) { }

        public static Task<BookUpdater> CreateAsync(string pathZip, CancellationToken ct)
        {
            return Task.Run<BookUpdater>(() => new BookUpdater(pathZip, ct), ct);
        }

        private readonly string _pathZip;
        private readonly Encoding _enc;
        private readonly Regex _regexEpNum = RegexEpNum();
        private readonly List<Episode> _episodes;

        private BookUpdater(string pathZip, CancellationToken ct)
        {
            _pathZip = pathZip;
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
        public Task<ReadOnlyCollection<EpisodeInfo>> GetEpsodeInfosForUpdateAsync(IEnumerable<EpisodeInfo> episodesNow, CancellationToken ct)
        {
            return Task.Run<ReadOnlyCollection<EpisodeInfo>>(() => GetEpisodeInfosForUpdateInner(episodesNow, ct), ct);
        }

        private ReadOnlyCollection<EpisodeInfo> GetEpisodeInfosForUpdateInner(IEnumerable<EpisodeInfo> epInfosNow, CancellationToken ct)
        {
            var listEpInfoNew = new List<EpisodeInfo>();
            int idx = 0;
            foreach (var epin in epInfosNow)
            {
                ct.ThrowIfCancellationRequested();
                if (idx < _episodes.Count)
                {
                    var idxFound = _episodes.FindIndex(idx, e => e.EpisodeNumber == epin.EpisodeNumber);
                    if (idxFound >= 0)
                    {
                        idx = idxFound + 1;
                        var episode = _episodes[idxFound];
                        if (epin.LastModifiedTime <= episode.LastModifiedTime)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        idx = _episodes.Count + 1;
                    }
                }
                listEpInfoNew.Add(epin);
            }
            return listEpInfoNew.AsReadOnly();
        }

        public Task MergeSaveAsync(IEnumerable<Episode> episodesNew, CancellationToken ct)
        {
            return Task.Run(() => MergeSaveInner(episodesNew, ct), ct);
        }

        private void MergeSaveInner(IEnumerable<Episode> episodesNew, CancellationToken ct)
        {
            var listWrite = new List<Episode>();
            var arrayEpsodesNew = episodesNew.ToArray();
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
            var entryName = $"エピソード{episode.EpisodeNumber}：仮のタイトル.txt";
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

        [GeneratedRegex(@"エピソード\((\d+)\)：")]
        private static partial Regex RegexEpNum();
    }
}
