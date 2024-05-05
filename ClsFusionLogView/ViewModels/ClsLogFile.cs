using InoTec;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ClsFusionViewer.ViewModels
{
    public class ClsLogFile
    {
        private int _year;
        private int _month;
        private ObservableCollection<ClsLogFileLine> _logFiles;

        public int Year => _year;
        public int Month => _month;
        public ObservableCollection<ClsLogFileLine> LogFiles => _logFiles;

        public ClsLogFile(IEnumerable<ClsLogFileLineType> model)
        {
            _logFiles = new ObservableCollection<ClsLogFileLine>(model.Select(x => new ClsLogFileLine(x)));
            _year = _logFiles[0].Year;
            _month = _logFiles[0].Month;
        }
        public ClsLogFile(string s, IEnumerable<ClsLogFileLine> lines)
        {
            _year = 0;
            _month = 0;
            _logFiles = new ObservableCollection<ClsLogFileLine>(lines);
        }
    }
}
