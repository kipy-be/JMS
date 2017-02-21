using System;
using System.IO;
using System.Text;

namespace JMS.Common
{
    public class LogsManager
    {
        private readonly string _logsFileName;
        private readonly string _logsPath;
        private DateTime _logsFileDate;
        private StreamWriter _stream;

        public LogsManager(string path, string fileName)
        {
            _logsPath = path;
            _logsFileName = fileName;
            _logsFileDate = DateTime.Today;

            InitOrResetStream();
        }

        private void CheckIfNeedToRotate()
        {
            if (_logsFileDate.Date != DateTime.Today)
                InitOrResetStream();
        }

        private void InitOrResetStream()
        {
            try
            {
                if (_stream != null)
                {
                    _stream.Dispose();
                    _stream = null;
                }

                DateTime today = DateTime.Today;

                var uri = Path.Combine(_logsPath, today.ToString("yyyyMM"));
                Directory.CreateDirectory(uri);

                uri = Path.Combine(uri, today.ToString("yyMMdd") + "_" + _logsFileName);

                _stream = new StreamWriter(new FileStream(uri, FileMode.Append, FileAccess.Write, FileShare.ReadWrite),
                    Encoding.UTF8);
                _stream.AutoFlush = true;

                _logsFileDate = DateTime.Today;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void Log(object obj)
        {
            if (_stream == null) return;
            CheckIfNeedToRotate();

            _stream.Write("{0} : ", DateTime.Now);
            _stream.WriteLine(obj);
        }

        public void Log(string message, params object[] args)
        {
            if (_stream == null) return;
            CheckIfNeedToRotate();

            if (!string.IsNullOrEmpty(message))
            {
                _stream.Write("{0:HH:mm:ss.fff} : ", DateTime.Now);
                _stream.Write(message, args);
            }
            _stream.WriteLine();
        }
    }
}
