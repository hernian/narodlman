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
    public class BookInfoMan
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

        public static BookInfoMan Create(string pathBookInfo, string urlTitlePage, string title, string kanaTitle, string author, string kanaAuthor)
        {
            var bookInfo = new BookInfo()
            {
                UrlTitlePage = urlTitlePage,
                Title = title,
                KanaTitle = kanaTitle,
                Author = author,
                KanaAuthor = kanaAuthor,
            };
            var bim = new BookInfoMan(pathBookInfo, bookInfo);
            return bim;
        }

        private readonly JsonSerializerOptions _options = new ()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
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

        public BookInfo BookInfo
        {
            get
            {
                return _bookInfo;
            }
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
                return GetPathWithExtention(".zip");
            }
        }

        public string PathLogFile
        {
            get
            {
                return GetPathWithExtention(".log");
            }
        }

        private string GetPathWithExtention(string extNew)
        {
            var ext = Path.GetExtension(_pathBookInfo);
            var end = _pathBookInfo.Length - ext.Length;
            var pathNew = $"{_pathBookInfo[0..end]}{extNew}";
            return pathNew;
        }
    }
}
