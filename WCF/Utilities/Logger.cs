namespace Utilities
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Configuration;
    using System.Collections.Specialized;
    using System.Threading;

    /// <summary>
    /// The logger.
    /// </summary>
    public class Logger
    {
        #region Constants and Fields

        /// <summary>
        /// prefix for all messages
        /// </summary>
        private static string Prefix = " yc: "; // default prefix, can be overriten by web.config

        /// <summary>
        /// prefix for all critical error messages
        /// </summary>
        private static string PrefixCriticalError = "(critical error, aborting ...) ";

        /// <summary>
        /// prefix for all error messages
        /// </summary>
        private static string PrefixError = "(error) ";

        /// <summary>
        /// prefix for all information messages
        /// </summary>
        private static string PrefixInformation = "(info) ";

        /// <summary>
        /// prefix for all warning messages
        /// </summary>
        private static string PrefixWarning = "(warning) ";

        /// <summary>
        /// logging level 
        /// </summary>
        public static LoggingLevel LogLevel = LoggingLevel.Warning;

        /// <summary>
        /// prefix for all warning messages
        /// </summary>
        private static Stopwatch timer = null;

        /// <summary>
        /// stream to write log messages to
        /// </summary>
        private static StreamWriter sw = null;

        #endregion

        #region Enums

        /// <summary>
        /// Logging level
        /// </summary>
        public enum LoggingLevel
        {
            /// <summary>
            /// The information.
            /// </summary>
            Information = 0, 

            /// <summary>
            /// The warning.
            /// </summary>
            Warning, 

            /// <summary>
            /// The error.
            /// </summary>
            Error, 

            /// <summary>
            /// The critical error.
            /// </summary>
            CriticalError
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets number of errors logged
        /// </summary>
        public static int ErrorCounter { get; private set; }

        /// <summary>
        /// Gets number of warnings logged
        /// </summary>
        public static int WarningCounter { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// prevent instantiation
        /// </summary>
        private Logger() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logFileName"></param>
        /// <param name="logLevel"></param>
        private static void Init(string logFileName, string prefix, LoggingLevel logLevel)
        {
            Prefix = prefix;
            LogLevel = logLevel;

            if (!String.IsNullOrEmpty(logFileName))
            {
                // let it throw, dont catch exceptions here
                // lets not start unless logger set up properly
                sw = File.CreateText(logFileName);
            }
        }

        /// <summary>
        /// The write critical error.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        public static void WriteCriticalError(string msg)
        {
            ErrorCounter++;
            WriteEntry(msg, LoggingLevel.CriticalError);
            Console.Beep();
            Console.Beep();
            Console.Beep();
            Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="msg">
        /// Error message to log
        /// </param>
        public static void WriteError(string msg)
        {
            ErrorCounter++;
            WriteEntry(msg, LoggingLevel.Error);
        }

        /// <summary>
        /// The write information.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        public static void WriteInformation(string msg)
        {
            WriteEntry(msg, LoggingLevel.Information);
        }

        /// <summary>
        /// The write warning.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        public static void WriteWarning(string msg)
        {
            WarningCounter++;
            WriteEntry(msg, LoggingLevel.Warning);
        }

        #endregion

        #region Methods

        /// <summary>
        /// writes log message to console.
        /// </summary>
        /// <param name="msg">
        /// message to write.
        /// </param>
        /// <param name="level">
        /// logging level.
        /// </param>
        private static void WriteEntry(string msg, LoggingLevel level)
        {
            if (level < LogLevel)
            {
                return;
            }

            if (timer == null)
            {
                timer = Stopwatch.StartNew();
            }

            string label = timer.Elapsed.ToString();
            switch (level)
            {
                case LoggingLevel.Information:
                    label += Prefix + PrefixInformation;
                    break;
                case LoggingLevel.Warning:
                    label += Prefix + PrefixWarning;
                    break;
                case LoggingLevel.Error:
                    label += Prefix + PrefixError;
                    break;
                case LoggingLevel.CriticalError:
                    label += Prefix + PrefixCriticalError;
                    break;
            }

            //
            // init log file from web.config if not done yet
            //
            if (sw == null)
            {
                NameValueCollection tmp = ConfigurationManager.AppSettings;
                LoggingLevel tmpL = LoggingLevel.Information;
                switch (tmp["LogLevel"])
                {
                    case "Information":
                        tmpL = LoggingLevel.Information;
                        break;
                    case "Warning":
                        tmpL = LoggingLevel.Warning;
                        break;
                    case "Error":
                        tmpL = LoggingLevel.Error;
                        break;
                    case "CriticalError":
                        tmpL = LoggingLevel.CriticalError;
                        break;
                }
                Init(tmp["LogFileName"], tmp["LogEntryPrefix"], tmpL);
            }
            //

            if (sw != null)
            {
                Mutex _lock = new Mutex(false, "mymutex");
                _lock.WaitOne();
                sw.WriteLine(label + msg);
                sw.Flush();
                _lock.ReleaseMutex();
            }
            else
            {
                Console.WriteLine(label + msg);
            }
        }

        #endregion
    }
}