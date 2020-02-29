using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security;

namespace HomeTheater.Helper
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    internal class Logger
    {
        private static Logger _i;
        private readonly StreamWriter streamWriter;
        protected string LOG_PATH = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", ".log");

        public Logger()
        {
            try
            {
                // Open the file
                streamWriter = new StreamWriter(LOG_PATH, true);
            }
            catch (Exception ex)
            {
                if
                (
                    ex is UnauthorizedAccessException
                    || ex is ArgumentNullException
                    || ex is PathTooLongException
                    || ex is DirectoryNotFoundException
                    || ex is NotSupportedException
                    || ex is ArgumentException
                    || ex is SecurityException
                    || ex is IOException
                )
                    throw new Exception("Failed to create log file: " + ex.Message);
                throw;
            }
        }

        public static Logger Instance
        {
            get
            {
                if (_i == null) _i = new Logger();
                return _i;
            }
        }

        public void Close()
        {
            streamWriter.Close();
        }

        #region Error

        public void Error(string format, object arg0 = null, object arg1 = null, object arg2 = null, object arg3 = null)
        {
            WriteLine("error", format, arg0, arg1, arg2, arg3);
        }

        public void Error(string value)
        {
            WriteLine("error", value);
        }

        public void Error(Exception ex)
        {
            WriteLine("error", ex);
        }

        public void Error(object value)
        {
            WriteLine("error", value);
        }

        #endregion

        #region Warn

        public void Warn(string format, object arg0 = null, object arg1 = null, object arg2 = null, object arg3 = null)
        {
            WriteLine("warning", format, arg0, arg1, arg2, arg3);
        }

        public void Warn(string value)
        {
            WriteLine("warning", value);
        }

        public void Warn(Exception ex)
        {
            WriteLine("warning", ex);
        }

        public void Warn(object value)
        {
            WriteLine("warning", value);
        }

        #endregion

        #region Notice

        public void Notice(string format, object arg0 = null, object arg1 = null, object arg2 = null,
            object arg3 = null)
        {
            WriteLine("notice", format, arg0, arg1, arg2, arg3);
        }

        public void Notice(string value)
        {
            WriteLine("notice", value);
        }

        public void Notice(Exception ex)
        {
            WriteLine("notice", ex);
        }

        public void Notice(object value)
        {
            WriteLine("notice", value);
        }

        #endregion

        #region Info

        public void Info(string format, object arg0 = null, object arg1 = null, object arg2 = null, object arg3 = null)
        {
            WriteLine("info", format, arg0, arg1, arg2, arg3);
        }

        public void Info(string value)
        {
            WriteLine("info", value);
        }

        public void Info(Exception ex)
        {
            WriteLine("info", ex);
        }

        public void Info(object value)
        {
            WriteLine("info", value);
        }

        #endregion

        #region WriteLine

        private void WriteLine(string type, string format, object arg0 = null, object arg1 = null, object arg2 = null,
            object arg3 = null)
        {
            var prefix = string.Format("[{0: yyyy-MM-dd HH:mm:ss}] {1}: ", DateTime.UtcNow, type.ToUpper());
            streamWriter.WriteLine(prefix + format, arg0, arg1, arg2, arg3);
#if DEBUG
            Console.WriteLine(type.ToUpper() + format, arg0, arg1, arg2, arg3);
#endif
        }

        private void WriteLine(string type, string value)
        {
            var prefix = string.Format("[{0: yyyy-MM-dd HH:mm:ss}] {1}: ", DateTime.UtcNow, type.ToUpper());
            streamWriter.WriteLine(prefix + value);
#if DEBUG
            Console.WriteLine(type.ToUpper() + ": " + value);
#endif
        }

        private void WriteLine(string type, Exception ex)
        {
            streamWriter.WriteLine("[{0: yyyy-MM-dd HH:mm:ss}] {1}: {2}\n{3}", DateTime.UtcNow, type.ToUpper(),
                ex.Message, ex.StackTrace);
#if DEBUG
            Console.WriteLine("{0}: {1}\n{2}", type.ToUpper(), ex.Message, ex.StackTrace);
#endif
        }

        private void WriteLine(string type, object value)
        {
            streamWriter.WriteLine("[{0: yyyy-MM-dd HH:mm:ss}] {1}: ", DateTime.UtcNow, type.ToUpper());
            streamWriter.WriteLine(value);
#if DEBUG
            Console.WriteLine("{0}: ", type.ToUpper());
            Console.WriteLine(value);
#endif
        }

        #endregion
    }
}