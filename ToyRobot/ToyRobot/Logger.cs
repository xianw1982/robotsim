namespace ToyRobot
{
    using System;
    using System.IO;

    /// <summary>
    /// very simple logger for logging debug information
    /// </summary>
    public static class Logger
    {
        public enum LogLevel
        {
            debug,
            error,
            info
        }

        private static readonly object _logLock = new object();

        //default stream writer
        private static StreamWriter _logger = new StreamWriter(Console.OpenStandardOutput());

        public static void Init(string outputstream)
        {
            if (!string.IsNullOrEmpty(outputstream))
            {
                lock (_logLock)
                {
                    _logger = new StreamWriter(outputstream);
                }
            }
        }

        public static void Log(Exception ex)
        {
            Log(ex.Message, LogLevel.error);
        }

        public static void Log(string message, LogLevel logLevel = LogLevel.info)
        {
            lock (_logLock)
            {
                _logger.WriteLine($"[{logLevel}] : {message}");
                _logger.Flush();
            }
        }
    }
}
