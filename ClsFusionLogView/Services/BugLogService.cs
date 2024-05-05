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
        }

        public override void Log(string message)
        {
            lock (base.lockObj)
            {
                using (StreamWriter streamWriter = new StreamWriter(_fileName))
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
            }
        }
    }
}
