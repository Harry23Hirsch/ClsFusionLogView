using InoTec;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClsFusionViewer.ViewModels
{
    public class ClsLogFile
    {
        private string _year;
        private string _month;
        private ObservableCollection<ClsLogFileLine> _logFiles;

        public string Year => _year;
        public string Month => _month;
        public ObservableCollection<ClsLogFileLine> LogFiles => _logFiles;

        public ClsLogFile(IEnumerable<ClsLogFileLineType> model)
        {
            _logFiles = new ObservableCollection<ClsLogFileLine>(model.Select(x => new ClsLogFileLine(x)));
            _year = _logFiles[0].Year;
            _month = _logFiles[0].Month;
        }
        public ClsLogFile(string s, IEnumerable<ClsLogFileLine> lines)
        {
            _year = s;
            _month = string.Empty;
            _logFiles = new ObservableCollection<ClsLogFileLine>(lines);
        }
    }
}
