using System;
using System.IO;

namespace ClsFusionViewer.Services
{
    public class BugLogServiceFactory
    {
        private readonly string _logPath;
        private readonly BugLogService _buglogService;

        public BugLogServiceFactory() 
        {
            _logPath = String.Format("{0}Log\\BugLog.log", AppContext.BaseDirectory);
            _buglogService = new BugLogService(_logPath);
        }

        public void BugLogWrite(Exception ex, string src)
        {
            string bugLog = String.Format(
                "{0} :: {1} -> {2} :: \"{3}\"\r\n",
                DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"),
                src,
                ex.Source,
                ex.Message);

            _buglogService.Log(bugLog);
        }
        public void BugLogWrite(string src, string msg)
        {
            string bugLog = String.Format(
                "{0} :: {1} :: \"{2}\"\r\n",
                DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss"),
                src,
                msg);

            _buglogService.Log(bugLog);
        }
    }
}
