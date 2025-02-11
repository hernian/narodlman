using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace narodlman
{
    internal class Logger: IDisposable
    {
        private readonly Encoding _enc = new UTF8Encoding(false);
        private readonly string _pathLog;
        private readonly StreamWriter _writer;
        private bool disposedValue;

        public Logger(string pathLog)
        {
            var dirLog = Path.GetDirectoryName(pathLog);
            if (!string.IsNullOrEmpty(dirLog))
            {
                Directory.CreateDirectory(dirLog);
            }
            _pathLog = pathLog;
            _writer = new StreamWriter(_pathLog, true, _enc);
        }

        public void WriteHeader()
        {
            var now = DateTime.Now;
            var header = $"[{now}] ";
            _writer.Write(header);
        }

        public void WriteFragment(string fragment)
        {
            _writer.Write(fragment);
        }
        public void WriteFragmentLine(string fragment)
        {
            _writer.WriteLine(fragment);
        }

        public void WriteLine(string line)
        {
            WriteHeader();
            _writer.WriteLine(line);
        }

        public Task WriteLineAsync(string line)
        {
            return Task.Run(() =>
            {
                WriteHeader();
                _writer.WriteLine(line);
                _writer.Flush();
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                    _writer.Dispose();
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~Logger()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
