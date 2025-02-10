using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace narodlman
{
    internal class BookInfoMan
    {

        public class Error(string msg) : Exception(msg) { }


        public static BookInfoMan Load(string pathBookInfo)
        {
            var enc = new UTF8Encoding(false);
            using var reader = new StreamReader(pathBookInfo, enc);
            var strJson = reader.ReadToEnd();
            var bookInfo = JsonSerializer.Deserialize<BookInfo>(strJson) ?? throw new Error($"Load BookInfo Error. {pathBookInfo}");
            var bim = new BookInfoMan(pathBookInfo, bookInfo);
            return bim;
        }

        public static BookInfoMan Create(string pathBookInfo, string urlTitlePage, string pathZip)
        {
            var bookInfo = new BookInfo()
            {
                UrlTitlePage = urlTitlePage,
                PathZip = pathZip
            };
            if (File.Exists(pathZip))
            {
                File.Delete(pathZip);
            }
            var bim = new BookInfoMan(pathBookInfo, bookInfo);
            return bim;
        }

        private readonly JsonSerializerOptions _options = new ()
        {
            Encoder = JavaScriptEncoder.Default,
            WriteIndented = true,
        };
        private readonly string _pathBookInfo;
        private readonly BookInfo _bookInfo;

        private BookInfoMan(string pathBookInfo, BookInfo bookInfo)
        {
            _pathBookInfo = pathBookInfo;
            _bookInfo = bookInfo;
        }

        public void Save()
        {
            var strJson = JsonSerializer.Serialize(_bookInfo, _options);
            var enc = new UTF8Encoding(false);
            using var writer = new StreamWriter(_pathBookInfo, false, enc);
            writer.Write(strJson);
        }

        public string PathBookInfo
        {
            get
            {
                return _pathBookInfo;
            }
        }
        public string UrlTitlePage
        {
            get
            {
                return _bookInfo.UrlTitlePage;
            }
        }

        public string PathZipFile
        {
            get
            {
                return _bookInfo.PathZip;
            }
        }
    }
}
