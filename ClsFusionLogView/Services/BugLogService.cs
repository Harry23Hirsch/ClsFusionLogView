using System.IO;

namespace ClsFusionViewer.Services
{
    public abstract class LogBase
    {
        protected readonly object lockObj = new object();
        public abstract void Log(string message);
    }

    public class BugLogService : LogBase
    {
        private readonly string _fileName;

        public BugLogService(string fileName)
        {

            _fileName = fileName;

            if (!File.Exists(_fileName))
            {
                File.Create(_fileName).Dispose();
            }
        }

        public override void Log(string message)
        {
            lock (base.lockObj)
            {
                File.AppendAllText(_fileName, message);
            }
        }
    }
}
