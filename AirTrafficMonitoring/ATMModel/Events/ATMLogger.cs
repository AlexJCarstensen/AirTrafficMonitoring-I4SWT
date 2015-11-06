using System.IO;

namespace ATMModel.Events
{
    /// <summary>
    /// Class that logs all event 
    /// constructor take file path, default in build directory
    /// </summary>
    public class ATMLogger : IATMLogEvent
    {
        private readonly string _path;

        public ATMLogger(string path = @"ATMLogger.txt")
        {
            _path = path;
        }
        /// <summary>
        /// Logs message in file
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            if (!File.Exists(_path))
            {
                var sr = File.CreateText(_path);
                sr.Close();
            }

            using (var sr = File.AppendText(_path))
            {
                sr.WriteLine(message);
            }
        }
    }
}