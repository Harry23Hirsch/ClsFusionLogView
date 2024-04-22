using InoTec;
using System;

namespace ClsFusionViewer.ViewModels
{
    public class ClsLogFileLine
    {
        private ClsLogFileLineType _model;

        public DateTime Datum => DateTime.Parse(String.Format($"{Year}/{Month}/{Year} {Time}"));
        public string Year => _model.Year;
        public string Month => _model.Month;
        public string Day => _model.Day;
        public string Time => _model.Time;
        public string Text => _model.Text;

        public ClsLogFileLine(ClsLogFileLineType model)
        {
            _model = model;
        }
    }
}
