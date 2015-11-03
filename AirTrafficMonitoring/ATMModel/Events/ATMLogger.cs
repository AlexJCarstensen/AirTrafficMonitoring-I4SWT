using System.IO;

namespace ATMModel.Events
{
    class ATMLogger : IATMLogEvent
    {
        private readonly string _path;

        public ATMLogger(string path = @"ATMLogger.txt")
        {
            _path = path;
        }

        public void Log(string message)
        {
            if (!File.Exists(_path))
            {
                StreamWriter sr = File.CreateText(_path);
                sr.Close();
            }

            using (StreamWriter sr = File.AppendText(_path))
            {
                sr.WriteLine(message);
            }
        }
    }
}